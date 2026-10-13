using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Threading;
using System.Windows.Forms;
using KPK2.MyClasse;
using KPK2.MyControl;
using NLog;
using Sparc.KPK2.Algorithms.Diss3228M;
using Sparc.KPK2.Hardware.PS27V;
using Sparc.KPK2.Imitator;
using Sparc.KPK2.NFunctions;
using Sparc.KPK2.NFunctions.Enums;
using Sparc.KPK2.Resources.Properties;
using UserControlsKPK2;
using Timer = System.Windows.Forms.Timer;

namespace KPK2.GeneralForms.DissForms
{
    public partial class frmWork3228_Monoblock : Form, IVisual
    {
        private static Logger _logger = LogManager.GetLogger("KPK2Error");
        private readonly int _testId;
        private AlgorithmDiss3228M _algorithm;
        private Timer _timerCollectParams = new Timer();
        private readonly int _collectInterval = 250;
        private readonly List<TextIndicator> _indDiOrderList = new List<TextIndicator>();
        private readonly List<MethodInvoker> _renewDelegateList = new List<MethodInvoker>();
        private readonly Dictionary<Checks, TwoDelegates> _actionDelegates = new Dictionary<Checks, TwoDelegates>();
        private Thread _tLoad;
        private readonly ManualResetEvent _mreEndOfLoad = new ManualResetEvent(false);
        private Control _activeControl;
        readonly Color _pressBackColor = Color.FromArgb(153, 249, 164);
        private readonly Imitator.Diss3228M _device = new Imitator.Diss3228M();
        private Timer _tWorkTimer;
        private DateTime _startDissTime;
        private readonly List<RadioButton> _chButtons = new List<RadioButton>();
        private bool _buttonActivated;
        private const double PowerIndicatorActivateVoltage = 24;

        private int _statePower27;

        private readonly DevicesID _deviceId = DevicesID.Diss3228_Monoblock;
        private readonly Modification _modification = Modification.None;
        private readonly string _deviceNumber;

        private readonly Dictionary<TextIndicator, bool> _needIndWithCtrlVCh = new Dictionary<TextIndicator, bool>();
        private readonly Dictionary<TextIndicator, bool> _needIndWithoutCtrlVCh = new Dictionary<TextIndicator, bool>();


        private enum Checks
        {
            Stop,
            FirstPower,
            SecPower,
            Indication,
            Ulf,
            MeasFreq,
            Ac,
            ContrBs,
            ContrVCh,
            Clear
        }

        private enum SecPowerIndicators
        {
            Plus12V,
            Minus12V,
            Plus15V = 3,
            Minus15V = 4,
            Plus9V = 6,
            Plus5V = 8
        }

        private enum UlfIndicators
        {
            Ulf1Channel1,
            Ulf2Channel1,
            Ulf1Channel2 = 3,
            Ulf2Channel2,
            Ulf1Channel3 = 6,
            Ulf2Channel3
        }

        private class TwoDelegates
        {
            public readonly MethodInvoker Activate;
            public readonly MethodInvoker Deactivate;

            public TwoDelegates(MethodInvoker activate, MethodInvoker deactivate)
            {
                Activate = activate;
                Deactivate = deactivate;
            }
        }

        private class SiSetting
        {
            public int FractPart;
            public int WholePart;
            public string UpString;
            public string DownString;

            public SiSetting()
            {
                UpString = "";
            }
        }

        public frmWork3228_Monoblock(int id, string devNum = "")
            : this()
        {
            _testId = id;
            _deviceNumber = devNum;
        }

        public frmWork3228_Monoblock()
        {
            InitializeComponent();
            FillDictionaries();
            FillIndication();
        }

        #region Implementation of IVisual

        public void ErrorAppear(string errText)
        {
            ShowError(errText);
        }

        void IVisual.ShowMessage(string pressButtonContrBs)
        {
            ShowMessage(pressButtonContrBs);
        }

        private void ShowError(string message)
        {
            if (InvokeRequired)
            {
                BeginInvoke(new Action<string>(ShowError), message);
                return;
            }
            MessageBox.Show(message, Resources.ErrorCaption);
        }

        private void ShowMessage(string message)
        {
            if (InvokeRequired)
            {
                Invoke(new Action<string>(ShowMessage), message);
                return;
            }
            MessageBox.Show(message);
        }

        #endregion

        private void FillIndication()
        {
            _needIndWithCtrlVCh.Add(tiMemory2, false);

            _needIndWithoutCtrlVCh.Add(tiMemory2, true);
        }

        private void FillDictionaries()
        {
            FillDictionaryUlf();
            FillDictionarySp();
            FillDictionaryFp();
        }

        private void InitIndOrderList()
        {
            _indDiOrderList.Add(tiGood);
            _indDiOrderList.Add(tiMemory2);
        }

        private void StartCollectTimer()
        {
            if (InvokeRequired)
            {
                Invoke(new MethodInvoker(StartCollectTimer));
                return;
            }
            _timerCollectParams = new Timer();
            _timerCollectParams.Tick += CollectParamsOnTick;
            _timerCollectParams.Interval = _collectInterval;
            _timerCollectParams.Start();
        }

        private bool _powerOff;

        private void CollectParamsOnTick(object sender, EventArgs eventArgs)
        {
            _timerCollectParams.Stop();
            try
            {
                RenewIndicators();
                if (_powerOff)
                {
                    tiDissOn.State = false;
                    tiGood.State = false;
                    tiMemory2.State = false;
                }
                else
                {
                    tiDissOn.State = _algorithm.GetIndicatorValue(AlgorithmDiss3228M.Indicators.Power27V) > PowerIndicatorActivateVoltage;
                    tiRefuseMCO.State = IsState(_algorithm.GetIndicatorValue(AlgorithmDiss3228M.Indicators.RefuseMco));
                    tiRefuseMAO.State = IsState(_algorithm.GetIndicatorValue(AlgorithmDiss3228M.Indicators.RefuseMao));
                    tiRefuseMP.State = IsState(_algorithm.GetIndicatorValue(AlgorithmDiss3228M.Indicators.RefuseMp));
                    tiRefuseUSK.State = IsState(_algorithm.GetIndicatorValue(AlgorithmDiss3228M.Indicators.RefuseUsk));
                }

                RenewPower();
                foreach (MethodInvoker inv in _renewDelegateList)
                    inv();
                _timerCollectParams.Start();
            }
            catch(Exception ex)
            {
                _logger.Error(ex);
            }            
        }

        private bool IsState(double value)
        {
            const double stateLimit = 5.9;
            return Math.Abs(value) > stateLimit;
        }

        private bool errorPS27Appear = false;

        private void RenewPower()
        {
            if (piStrip.Bars.Count == 0)
                return;
            piStrip.Bars[0].Value = (float)Math.Round(_algorithm.Voltage27, 2);
            piStrip.Bars[1].Value = (float)Math.Round(_algorithm.Current27, 2);

            if (!errorPS27Appear)
                if (Common.Globals.CheckPS27Protection(Power27V.Instance, ShowError))
                    errorPS27Appear = true;
        }

        private void RenewIndicators()
        {
            BitArray indState = _algorithm.GetDiIndicators();
            List<TextIndicator> curList = _indDiOrderList;
            for (int i = 0; i < indState.Length; i++)
                if (_powerOff)
                    SetIndicatorState(curList, i, false);
                else
                    SetIndicatorState(curList, i, indState[i]);
        }

        private void SetIndicatorState(List<TextIndicator> indOrderList, int numIndicator, bool state)
        {
            if (indOrderList.Count > numIndicator)
                indOrderList[numIndicator].State = state;
        }

        private void frmWork3228_Load(object sender, EventArgs e)
        {
            var caption = Globals.GetDissCaption(_deviceId, _modification);

            if (string.IsNullOrEmpty(_deviceNumber))
            {
               Text = string.Format(Resources.CheckDeviceN, caption);
               //var logger = LogManager.GetCurrentClassLogger();
               _logger.Info(Text);
            }
            else
            {
                Text = string.Format(Resources.CheckDeviceNN, caption, _deviceNumber);
                //var logger = LogManager.GetCurrentClassLogger();
                _logger.Info(Text);
            }
            CreateIndicators();
            CreateButtons();
            siWorkTime.Inactive = true;
        }

        private void frmWork3228_Shown(object sender, EventArgs e)
        {
            _tLoad = new Thread(ThreadLoad) { Priority = ThreadPriority.Lowest };
            _tLoad.Start();
        }

        private void ThreadLoad()
        {
            try
            {
                Common.Globals.SetThreadCulture(Thread.CurrentThread);
                InitIndOrderList();
                FillVisibility();
                FillSiSettings();
                FillActionDelegates();
                _algorithm = new AlgorithmDiss3228M(this, _testId, (str) => {_logger.Error(str); } );
            }
            catch
            {
                ShowError(Resources.LoadFormError);
            }
            _mreEndOfLoad.Set();
        }

        private void FillActionDelegates()
        {
            _actionDelegates.Add(Checks.FirstPower, new TwoDelegates(ActivateFirstPower, DeactivateFirstPower));
            _actionDelegates.Add(Checks.SecPower, new TwoDelegates(ActivateSecondPower, DeactivateSecondPower));
            _actionDelegates.Add(Checks.Indication, new TwoDelegates(ActivateIndication, DeactivateIndication));
            _actionDelegates.Add(Checks.Ulf, new TwoDelegates(ActivateUlf, DeactivateUlf));
            _actionDelegates.Add(Checks.MeasFreq, new TwoDelegates(ActivateMeasFrequency, DeactivateMeasFrequency));
            _actionDelegates.Add(Checks.Ac, new TwoDelegates(ActivateAc, DeactivateAc));
            _actionDelegates.Add(Checks.ContrBs, new TwoDelegates(ActivateContrBs, DeactivateContrBs));
            _actionDelegates.Add(Checks.ContrVCh, new TwoDelegates(ActivateContrVCh, DeactivateContrVCh));
        }

        private void DeactivateMeasFrequency()
        {
            if (((MeasFreqDiss3228M)_activeControl).Freq1_3_Measured)
                DataManager.Instance.SaveParameter(_testId, AlgorithmDiss3228M.Parameters.UhfFreqChannels1And3,
                    ((MeasFreqDiss3228M)_activeControl).Freq1_3);
            if (((MeasFreqDiss3228M)_activeControl).Freq2_Measured)
                DataManager.Instance.SaveParameter(_testId, AlgorithmDiss3228M.Parameters.UhfFreqChannel2,
                    ((MeasFreqDiss3228M)_activeControl).Freq2);

            _algorithm.SetMeasChannelOff();
        }

        private void ActivateMeasFrequency()
        {
            _activeControl = new MeasFreqDiss3228M(_algorithm);
            gbAll.Controls.Add(_activeControl);
        }

        private void DeactivateIndication()
        {
            ((IndicationDiss3228M)_activeControl).StopTimer();
            var indicationWithCtrlVCh = ((IndicationDiss3228M)_activeControl).IndicationWithCtrlVCh;
            var indicationWithoutCtrlVCh = ((IndicationDiss3228M)_activeControl).IndicationWithoutCtrlVCh;
            if (indicationWithCtrlVCh != null && indicationWithoutCtrlVCh != null)
                DataManager.Instance.SaveParameter(_testId, AlgorithmDiss3228M.Parameters.IndicationParam, (bool)indicationWithCtrlVCh && (bool)indicationWithoutCtrlVCh);
            _algorithm.Stop();
            _renewDelegateList.Remove(RenewIndication);
        }

        private void ActivateIndication()
        {
            _activeControl = new IndicationDiss3228M();
            gbAll.Controls.Add(_activeControl);
            _activeControl.Location = new Point(4, 4);
            _algorithm.SetContrVCh(true);
            ((IndicationDiss3228M)_activeControl).StartTimer(TimeSpan.FromSeconds(20));
            _renewDelegateList.Add(RenewIndication);
        }

        private void RenewIndication()
        {
            if (_algorithm.ContrVChState)
            {
                bool result = true;
                foreach (var item in _needIndWithCtrlVCh)
                    result &= item.Key.State == item.Value;
                if (((IndicationDiss3228M)_activeControl).IsTimerOver())
                {
                    ((IndicationDiss3228M)_activeControl).StopTimer();
                    ((IndicationDiss3228M)_activeControl).IndicationWithCtrlVCh = result;
                    _algorithm.SetContrVCh(false);
                    ((IndicationDiss3228M)_activeControl).StartTimer(TimeSpan.FromSeconds(13));
                }
            }
            else //!StateWithContrVCh
            {
                bool result = true;
                foreach (var item in _needIndWithoutCtrlVCh)
                    result &= item.Key.State == item.Value;
                if (((IndicationDiss3228M)_activeControl).IsTimerOver())
                {
                    ((IndicationDiss3228M)_activeControl).StopTimer();
                    ((IndicationDiss3228M)_activeControl).IndicationWithoutCtrlVCh = result;
                }
            }
        }

        private void CreateButtons()
        {
            int baseY = 5;
            int baseWidth = 104;
            const int baseHeight = 40;
            int space = 11;
            int stepX = baseWidth + space * 2;
            baseWidth *= 2;
            baseWidth += space;
            stepX += baseWidth;

            for (int i = 0; i < 3; i++)
            {
                var button = new RadioButton
                {
                    Name = "bChannel" + (i + 1),
                    Location = new Point(
                        gbAll.Location.X + baseWidth / 2 + baseWidth / 20 + i * (stepX - baseWidth / 4),
                        gbAll.Location.Y + gbAll.Height + baseY),
                    TextAlign = ContentAlignment.MiddleCenter,
                    Text = (i + 1).ToString(),
                    Appearance = Appearance.Button,
                    Size = new Size(baseWidth, baseHeight),
                    Visible = false,
					Anchor = (AnchorStyles.Bottom | AnchorStyles.Left)
                };
                button.CheckedChanged += BOnCheckedChanged;
                tpWork.Controls.Add(button);
                _chButtons.Add(button);
            }
        }

        private int _channelState;
        private double _channelValue1;
        private double _channelValue2;

        private void BOnCheckedChanged(object sender, EventArgs eventArgs)
        {
            RadioButton rb = (RadioButton)sender;
            rb.BackColor = rb.Checked ? Globals.SuccessColor : Color.Transparent;
            if (rb.Checked)
            {
                SaveOneChannelParams(_algorithm.CurrentParameter, _channelState);
                SetChannelIndicatorsVisible(0, false);
                SetChannelIndicatorsVisible(1, false);
                SetChannelIndicatorsVisible(2, false);
                int numButton = Convert.ToInt32(rb.Name.Substring(rb.Name.Length - 1, 1));
                _channelState = numButton;

                var isComplete = _algorithm.SetMeasChannel(_channelState - 1);
                if (!isComplete)
                    ErrorAppear(string.Format(Resources.ErrorOnDeviceN, Resources.DiscreteOutputsBoard));

                SetChannelIndicatorsVisible(_channelState - 1, true);
            }
        }

        private void SetChannelIndicatorsVisible(int channel, bool state)
        {
            if (channel < 0)
                return;
            int ind1 = channel * 3;
            int ind2 = channel * 3 + 1;
            _arInd[ind1].Visible = state;
            _arInd[ind2].Visible = state;
            _arUpLabel[ind1].Visible = state;
            _arUpLabel[ind2].Visible = state;
            _arDownLabel[ind1].Visible = state;
            _arDownLabel[ind2].Visible = state;
        }

        private void SaveOneChannelParams(string paramName, int channelState)
        {
            DataManager.Instance.SaveParameter(_testId, string.Format(paramName, channelState, 1), _channelValue1);
            DataManager.Instance.SaveParameter(_testId, string.Format(paramName, channelState, 2), _channelValue2);
        }

        private const string IndName = "siInd";
        private const string LUpName = "lUp";
        private const string LDownName = "lDown";
        private const int NumIndInRow = 8;
        private readonly Label[] _arUpLabel = new Label[16];
        private readonly Label[] _arDownLabel = new Label[16];
        private readonly SegmentIndication[] _arInd = new SegmentIndication[16];

        private void CreateIndicators()
        {
            const int baseX = 5;
            const int baseY = 45;
            const int baseWidth = 104;
            const int baseHeight = 40;
            const int stepX = baseWidth + 11;
            const int stepY = baseHeight + 40;

            for (int i = 0; i < 2; i++)
                for (int j = 0; j < NumIndInRow; j++)
                {
                    var si = new SegmentIndication
                    {
                        Name = IndName + "_" + i + "_" + j,
                        Size = new Size(baseWidth, baseHeight),
                        Location = new Point(baseX + j * stepX, baseY + i * stepY)
                    };
                    _arInd[j + i * NumIndInRow] = si;
                    SegmentIndicatorDefault(ref si);
                    gbAll.Controls.Add(si);

                    //Верхний лейбл
                    var lbl = new Label
                    {
                        Name = LUpName + "_" + i + "_" + j,
                        Text = string.Format(Resources.UV, Resources.Volt),
                        AutoSize = true,
                        Location = new Point(baseX + j * stepX, baseY + i * stepY - Size.Height - 3)
                    };
                    //arUpLabel.Add(lbl);
                    _arUpLabel[j + i * NumIndInRow] = lbl;
                    LabelDefault(ref lbl);
                    gbAll.Controls.Add(lbl);

                    //Нижний лейбл
                    lbl = new Label
                    {
                        Name = LDownName + "_" + i + "_" + j,
                        Text = String.Empty,
                        TextAlign = ContentAlignment.MiddleCenter,
                        Size = new Size(baseWidth, (int)(Font.Size * 2)),
                        Location = new Point(baseX + j * stepX, baseY + i * stepY + baseHeight)
                    };
                    _arDownLabel[j + i * NumIndInRow] = lbl;
                    LabelDefault(ref lbl);
                    gbAll.Controls.Add(lbl);
                }
        }

        private static void SegmentIndicatorDefault(ref SegmentIndication si)
        {
            si.Font = new Font("Microsoft Sans Serif", 21.75f);
            si.DarkColor = Color.Transparent;
            si.Unit = "";
            si.Value = 0;
            si.Visible = false;
            si.TabStop = false;
        }

        private static void LabelDefault(ref Label lbl)
        {
            lbl.Font = new Font("Verdana", 8.25f);
            lbl.Visible = false;
        }

        /// <summary>
        /// Блокируем все кнопки на TabPage "Проведение измерений",
        /// запрещаем переход на другие вкладки
        /// </summary>
        /// <param name="btn">кнопка, которую нажали</param>
        private void PressButton(Button btn)
        {
            //countButton = 1;
            btn.FlatStyle = FlatStyle.Flat;
            btn.BackColor = _pressBackColor;
            Control.ControlCollection collButton = btn.Parent.Controls;
            for (int i = 0; i < collButton.Count; i++)
                if (collButton[i].GetType() == typeof(Button) &&
                    collButton[i].Name != btn.Name)
                    collButton[i].Enabled = false;
        }

        #region Activate/Deactivate Actions

        private void ActivateFirstPower()
        {
            gbPower.Enabled = true;
            _algorithm.ActivateFirstPower(SetIndicatorLimitsFp);
            _algorithm.SaveResults = true;
            _renewDelegateList.Add(FirstPowerMonitor);
        }

        private readonly Dictionary<AlgorithmDiss3228M.Indicators, int> _fpIndicators = new Dictionary<AlgorithmDiss3228M.Indicators, int>();

        private void FillDictionaryFp()
        {
            _fpIndicators.Add(AlgorithmDiss3228M.Indicators.Power27V, 0);
            _fpIndicators.Add(AlgorithmDiss3228M.Indicators.Current27V, 4);
        }

        private void SetIndicatorLimitsFp(int indNum, double min, double max)
        {
            SetIndicatorLimits(_fpIndicators, indNum, min, max);
        }

        private void DeactivateFirstPower()
        {
            if (!rbNorma.Checked)
                rbNorma.Checked = true;
            _algorithm.Stop();
            _renewDelegateList.Remove(FirstPowerMonitor);
            _algorithm.SaveFirstPower();
            gbPower.Enabled = false;
        }

        private void ActivateSecondPower()
        {
            _algorithm.ActivateSecondPower(SetIndicatorLimitsSp);
            _algorithm.SaveResults = true;
            _renewDelegateList.Add(SecondPowerMonitor);
        }

        private void SetIndicatorLimitsSp(int indNum, double min, double max)
        {
            SetIndicatorLimits(_spIndicators, indNum, min, max);
        }

        private readonly Dictionary<AlgorithmDiss3228M.Indicators, int> _spIndicators = new Dictionary<AlgorithmDiss3228M.Indicators, int>();

        private void FillDictionarySp()
        {
            _spIndicators.Add(AlgorithmDiss3228M.Indicators.Plus12V, (int)SecPowerIndicators.Plus12V);
            _spIndicators.Add(AlgorithmDiss3228M.Indicators.Minus12V, (int)SecPowerIndicators.Minus12V);
            _spIndicators.Add(AlgorithmDiss3228M.Indicators.Plus15V, (int)SecPowerIndicators.Plus15V);
            _spIndicators.Add(AlgorithmDiss3228M.Indicators.Minus15V, (int)SecPowerIndicators.Minus15V);
            _spIndicators.Add(AlgorithmDiss3228M.Indicators.Plus9V, (int)SecPowerIndicators.Plus9V);
            _spIndicators.Add(AlgorithmDiss3228M.Indicators.Plus5V, (int)SecPowerIndicators.Plus5V);
        }

        private void SecondPowerMonitor()
        {
            SetIndicatorFromAlgorithm((int)SecPowerIndicators.Plus12V, AlgorithmDiss3228M.Indicators.Plus12V);
            SetIndicatorFromAlgorithm((int)SecPowerIndicators.Minus12V, AlgorithmDiss3228M.Indicators.Minus12V);
            SetIndicatorFromAlgorithm((int)SecPowerIndicators.Plus15V, AlgorithmDiss3228M.Indicators.Plus15V);
            SetIndicatorFromAlgorithm((int)SecPowerIndicators.Minus15V, AlgorithmDiss3228M.Indicators.Minus15V);
            SetIndicatorFromAlgorithm((int)SecPowerIndicators.Plus9V, AlgorithmDiss3228M.Indicators.Plus9V);
            SetIndicatorFromAlgorithm((int)SecPowerIndicators.Plus5V, AlgorithmDiss3228M.Indicators.Plus5V);
        }

        private void DeactivateSecondPower()
        {
            _renewDelegateList.Remove(SecondPowerMonitor);
            _algorithm.Stop();
        }

        private void ActivateUlf()
        {
            var isComplete = _algorithm.SetUlfState(true);
            if (!isComplete)
                ErrorAppear(string.Format(Resources.ErrorOnDeviceN, Resources.DiscreteOutputsBoard));

            _algorithm.ActivateUlf(SetIndicatorLimitsUlf);
            _algorithm.SaveResults = false;
            _renewDelegateList.Add(UlfMonitor);
        }

        private void SetIndicatorLimitsUlf(int indNum, double min, double max)
        {
            SetIndicatorLimits(_ulfIndicators, indNum, min, max);
        }

        private readonly Dictionary<AlgorithmDiss3228M.Indicators, int> _ulfIndicators = new Dictionary<AlgorithmDiss3228M.Indicators, int>();

        private void FillDictionaryUlf()
        {
            _ulfIndicators.Add(AlgorithmDiss3228M.Indicators.Ulf1Channel1, (int)UlfIndicators.Ulf1Channel1);
            _ulfIndicators.Add(AlgorithmDiss3228M.Indicators.Ulf2Channel1, (int)UlfIndicators.Ulf2Channel1);
            _ulfIndicators.Add(AlgorithmDiss3228M.Indicators.Ulf1Channel2, (int)UlfIndicators.Ulf1Channel2);
            _ulfIndicators.Add(AlgorithmDiss3228M.Indicators.Ulf2Channel2, (int)UlfIndicators.Ulf2Channel2);
            _ulfIndicators.Add(AlgorithmDiss3228M.Indicators.Ulf1Channel3, (int)UlfIndicators.Ulf1Channel3);
            _ulfIndicators.Add(AlgorithmDiss3228M.Indicators.Ulf2Channel3, (int)UlfIndicators.Ulf2Channel3);
        }

        private void UlfMonitor()
        {
            _channelValue1 = _algorithm.GetIndicatorValue(AlgorithmDiss3228M.Indicators.Ulf1Channel1);
            _channelValue2 = _algorithm.GetIndicatorValue(AlgorithmDiss3228M.Indicators.Ulf2Channel1);
            SetIndicatorFromAlgorithm((int)UlfIndicators.Ulf1Channel1, AlgorithmDiss3228M.Indicators.Ulf1Channel1);
            SetIndicatorFromAlgorithm((int)UlfIndicators.Ulf2Channel1, AlgorithmDiss3228M.Indicators.Ulf2Channel1);
            SetIndicatorFromAlgorithm((int)UlfIndicators.Ulf1Channel2, AlgorithmDiss3228M.Indicators.Ulf1Channel1);
            SetIndicatorFromAlgorithm((int)UlfIndicators.Ulf2Channel2, AlgorithmDiss3228M.Indicators.Ulf2Channel1);
            SetIndicatorFromAlgorithm((int)UlfIndicators.Ulf1Channel3, AlgorithmDiss3228M.Indicators.Ulf1Channel1);
            SetIndicatorFromAlgorithm((int)UlfIndicators.Ulf2Channel3, AlgorithmDiss3228M.Indicators.Ulf2Channel1);
        }

        private void DeactivateUlf()
        {
            SaveOneChannelParams(_algorithm.CurrentParameter, _channelState);
            _algorithm.Stop();
            _algorithm.SetUlfState(false);

            var isComplete = _algorithm.SetMeasChannelOff();
            if (!isComplete)
                ErrorAppear(string.Format(Resources.ErrorOnDeviceN, Resources.DiscreteOutputsBoard));

            _renewDelegateList.Remove(UlfMonitor);
        }

        private void ActivateAc()
        {
            _renewDelegateList.Add(AcMonitor);
        }

        private Dictionary<int, BitIndication.BitState> GetMask(int addr, int correct)
        {
            var reversedCode = correct.ReverseBits(0, 4);

            Dictionary<int, BitIndication.BitState> mask = new Dictionary<int, BitIndication.BitState>();
            for (int i = 0; i < 31; i++)
            {
                var state = new BitIndication.BitState { CurrentState = BitIndication.BitState.State.Inactive };
                if (i < 8)
                {
                    int checkNumber = (int)Math.Pow(2, i);
                    state.CurrentState = (addr & checkNumber) == checkNumber ? BitIndication.BitState.State.One : BitIndication.BitState.State.Zero;
                }
                if (i >= 9 && i < 13)
                {
                    int checkNumber = (int)Math.Pow(2, (i - 9));
                    state.CurrentState = (reversedCode & checkNumber) == checkNumber ? BitIndication.BitState.State.One : BitIndication.BitState.State.Zero;
                }
                mask.Add(i, state);
            }
            var fState = new BitIndication.BitState { CurrentState = BitIndication.BitState.State.Formula };
            fState.Formula += delegate (BitArray ba)
                {
                    bool result = true;
                    for (int i = 0; i < ba.Length - 1; i++)
                        if (ba[i])
                            result = !result;
                    return result;
                };
            mask.Add(31, fState);
            return mask;
        }

        private void AcMonitor()
        {
            List<int> words = _algorithm.GetWords();
            if (words == null)
                return;
            if (words.Count <= 2)
                return;
            SetWord1(words[0]);
            SetWord2(words[1]);
            SetWord3(words[2]);
        }

        private void SetWord1(int word)
        {
            biAddr1.Value = biContr1.Value = biP1.Value = biF1.Value = biService1.Value = biCheck1.Value = word;
        }

        private void SetWord2(int word)
        {
            biAddr2.Value = biContr2.Value = biP2.Value = biF2.Value = biService2.Value = biCheck2.Value = word;
        }

        private void SetWord3(int word)
        {
            biAddr3.Value = biContr3.Value = biP3.Value = biF3.Value = biService3.Value = biCheck3.Value = word;
        }

        private void DeactivateAc()
        {
            _renewDelegateList.Remove(AcMonitor);
            DataManager.Instance.SaveParameter(_testId, AlgorithmDiss3228M.Parameters.ModeAcStr, GetWord1OkState() && GetWord2OkState() && GetWord3OkState());
        }


        private void ActivateContrBs()
        {
            SetWord1Mask(GetContrBsMask(0, _algorithm.GetWordAddr(0)));
            SetWord2Mask(GetContrBsMask(1, _algorithm.GetWordAddr(1)));
            SetWord3Mask(GetContrBsMask(2, _algorithm.GetWordAddr(2)));
            _algorithm.SetContrBs(true);
            _device.SetContrBSState(true);
            _renewDelegateList.Add(FrequencyMonitor);
        }

        private void SetWord1Mask(Dictionary<int, BitIndication.BitState> dictionary)
        {
            biAddr1.SetMask(dictionary);
            biContr1.SetMask(dictionary);
            biP1.SetMask(dictionary);
            biF1.SetMask(dictionary);
            biService1.SetMask(dictionary);
            biCheck1.SetMask(dictionary);
        }

        private void SetWord2Mask(Dictionary<int, BitIndication.BitState> dictionary)
        {
            biAddr2.SetMask(dictionary);
            biContr2.SetMask(dictionary);
            biP2.SetMask(dictionary);
            biF2.SetMask(dictionary);
            biService2.SetMask(dictionary);
            biCheck2.SetMask(dictionary);
        }

        private void SetWord3Mask(Dictionary<int, BitIndication.BitState> dictionary)
        {
            biAddr3.SetMask(dictionary);
            biContr3.SetMask(dictionary);
            biP3.SetMask(dictionary);
            biF3.SetMask(dictionary);
            biService3.SetMask(dictionary);
            biCheck3.SetMask(dictionary);
        }

        private Dictionary<int, BitIndication.BitState> GetContrBsMask(int index, int addr)
        {
            Dictionary<int, BitIndication.BitState> mask = new Dictionary<int, BitIndication.BitState>();
            for (int i = 0; i < 31; i++)
            {
                var state = new BitIndication.BitState { CurrentState = BitIndication.BitState.State.Inactive };
                if (i < 8)
                {
                    int checkNumber = (int)Math.Pow(2, i);
                    state.CurrentState = (addr & checkNumber) == checkNumber ? BitIndication.BitState.State.One : BitIndication.BitState.State.Zero;
                }
                if (i == 8)
                    state.CurrentState = BitIndication.BitState.State.One;

                if (i >= 27 && i < 31)
                    state.CurrentState = index == 0 ? BitIndication.BitState.State.One : BitIndication.BitState.State.Zero;

                mask.Add(i, state);
            }
            var fState = new BitIndication.BitState { CurrentState = BitIndication.BitState.State.Formula };
            fState.Formula += CsFormula;
            mask.Add(31, fState);
            return mask;
        }

        private void FrequencyMonitor()
        {
            List<int> words = _algorithm.GetWords();
            if (words == null)
                return;
            if (words.Count <= 2)
                return;

            SetWord1(words[0]);
            SetWord2(words[1]);
            SetWord3(words[2]);
            siFrequency1.Value = GetFrequencyFromWord(words[0]);
            siFrequency2.Value = GetFrequencyFromWord(words[1]);
            siFrequency3.Value = GetFrequencyFromWord(words[2]);
        }

        private double GetFrequencyFromWord(int word)
        {
            // Частоту берём из бит с 16го по 27й (итого 12 бит)
            const int mask = (1 << 12) - 1;
            var result = (word >> 15) & mask;

            // Если биты 30 и 31 (биты служебных сигналов) равны 11, то необходимо использовать двоичное дополнение
            var isComplementNeeded = ((word >> 29) & 3) == 3;
            if (isComplementNeeded)
                result |= ~mask;

            return result;
        }

        private void DeactivateContrBs()
        {
            _algorithm.SetContrBs(false);
            _device.SetContrBSState(false);
            _renewDelegateList.Remove(FrequencyMonitor);
            bool result = GetWord1OkState() && GetWord2OkState() && GetWord3OkState();

            result &= siFrequency1.Value <= AlgorithmDiss3228M.ContrBsFrequency.Frequency1Max &&
                      siFrequency1.Value >= AlgorithmDiss3228M.ContrBsFrequency.Frequency1Min;
            result &= siFrequency2.Value <= AlgorithmDiss3228M.ContrBsFrequency.Frequency2Max &&
                      siFrequency2.Value >= AlgorithmDiss3228M.ContrBsFrequency.Frequency2Min;
            result &= siFrequency3.Value <= AlgorithmDiss3228M.ContrBsFrequency.Frequency3Max &&
                      siFrequency3.Value >= AlgorithmDiss3228M.ContrBsFrequency.Frequency3Min;

            DataManager.Instance.SaveParameter(_testId, AlgorithmDiss3228M.Parameters.ModeContrBsStr, result);
        }

        private bool GetWord1OkState()
        {
            return biAddr1.OKState && biContr1.OKState && biP1.OKState && biF1.OKState && biService1.OKState &&
                   biCheck1.OKState;
        }

        private bool GetWord2OkState()
        {
            return biAddr2.OKState && biContr2.OKState && biP2.OKState && biF2.OKState && biService2.OKState &&
                   biCheck2.OKState;
        }

        private bool GetWord3OkState()
        {
            return biAddr3.OKState && biContr3.OKState && biP3.OKState && biF3.OKState && biService3.OKState &&
                   biCheck3.OKState;
        }

        private void ActivateContrVCh()
        {
            SetWord1Mask(GetContrVChMask(_algorithm.GetWordAddr(0)));
            SetWord2Mask(GetContrVChMask(_algorithm.GetWordAddr(1)));
            SetWord3Mask(GetContrVChMask(_algorithm.GetWordAddr(2)));
            _algorithm.SetContrVCh(true);
            _device.SetContrVChState(true);
            _renewDelegateList.Add(FrequencyMonitor);
        }

        private Dictionary<int, BitIndication.BitState> GetContrVChMask(int addr)
        {
            Dictionary<int, BitIndication.BitState> mask = new Dictionary<int, BitIndication.BitState>();
            for (int i = 0; i < 31; i++)
            {
                var state = new BitIndication.BitState { CurrentState = BitIndication.BitState.State.Inactive };
                if (i < 8)
                {
                    int checkNumber = (int)Math.Pow(2, i);
                    state.CurrentState = (addr & checkNumber) == checkNumber ? BitIndication.BitState.State.One : BitIndication.BitState.State.Zero;
                }
                if (i == 8)
                    state.CurrentState = BitIndication.BitState.State.One;
                mask.Add(i, state);
            }
            var fState = new BitIndication.BitState { CurrentState = BitIndication.BitState.State.Formula };
            fState.Formula += CsFormula;
            mask.Add(31, fState);
            return mask;
        }

        private bool CsFormula(BitArray ba)
        {
            bool result = true;
            for (int i = 0; i < ba.Length - 1; i++)
                if (ba[i])
                    result = !result;
            return result;
        }

        private void DeactivateContrVCh()
        {
            _algorithm.SetContrVCh(false);
            _device.SetContrVChState(false);
            _renewDelegateList.Remove(FrequencyMonitor);
            bool result = GetWord1OkState() && GetWord2OkState() && GetWord3OkState();
            result &= siFrequency1.Value <= AlgorithmDiss3228M.ContrVChFrequency.Frequency1Max &&
                      siFrequency1.Value >= AlgorithmDiss3228M.ContrVChFrequency.Frequency1Min;
            result &= siFrequency2.Value <= AlgorithmDiss3228M.ContrVChFrequency.Frequency2Max &&
                      siFrequency2.Value >= AlgorithmDiss3228M.ContrVChFrequency.Frequency2Min;
            result &= siFrequency3.Value <= AlgorithmDiss3228M.ContrVChFrequency.Frequency3Max &&
                      siFrequency3.Value >= AlgorithmDiss3228M.ContrVChFrequency.Frequency3Min;
            DataManager.Instance.SaveParameter(_testId, AlgorithmDiss3228M.Parameters.ModeContrVChStr, result);
        }

        #endregion Activate/Deactivate Actions

        private void FirstPowerMonitor()
        {
            SetIndicatorFromAlgorithm(0, AlgorithmDiss3228M.Indicators.Power27V);
            SetIndicatorFromAlgorithm(4, AlgorithmDiss3228M.Indicators.Current27V);
        }

        /// <summary>
        /// Настройка вида индикаторов в GroupBox
        /// </summary>
        /// <param name="modeMeasurements"></param>
        private void ImageDesigner(Checks modeMeasurements)
        {
            try
            {

                if (modeMeasurements != Checks.Stop)
                    if (_activeControl != null)
                        _activeControl.Dispose();

                pDefaultValues.Visible = false;

                switch (modeMeasurements)
                {
                    case Checks.Stop: // Stop - остановили измерения
                        ImageStop();
                        break;
                    case Checks.FirstPower:
                        ImageFirstPower();
                        break;
                    case Checks.SecPower:
                        ImageSecPower();
                        break;
                    case Checks.Ulf:
                        ImageUlf();
                        break;
                    case Checks.Indication:
                        ImageIndication();
                        break;
                    case Checks.MeasFreq:
                        SetIndicatorsVisibility(Checks.Clear);
                        break;
                    case Checks.Ac:
                        ImageAc();
                        break;
                    case Checks.ContrBs:
                        ImageContrBs();
                        break;
                    case Checks.ContrVCh:
                        ImageContrVCh();
                        break;
                    default:
                        ImageOpenForm();
                        break;
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
            }
        }

        private void ImageIndication()
        {
            SetIndicatorsVisibility(Checks.Clear);
            _activeControl.Visible = true;
            _activeControl.BringToFront();
        }

        private void ImageContrVCh()
        {
            TaskVisibility(true, true);
            siFrequency1.MinValue = AlgorithmDiss3228M.ContrVChFrequency.Frequency1Min;
            siFrequency1.MaxValue = AlgorithmDiss3228M.ContrVChFrequency.Frequency1Max;
            siFrequency2.MinValue = AlgorithmDiss3228M.ContrVChFrequency.Frequency2Min;
            siFrequency2.MaxValue = AlgorithmDiss3228M.ContrVChFrequency.Frequency2Max;
            siFrequency3.MinValue = AlgorithmDiss3228M.ContrVChFrequency.Frequency3Min;
            siFrequency3.MaxValue = AlgorithmDiss3228M.ContrVChFrequency.Frequency3Max;
        }

        private void ImageContrBs()
        {
            TaskVisibility(true, true);
            siFrequency1.MinValue = AlgorithmDiss3228M.ContrBsFrequency.Frequency1Min;
            siFrequency1.MaxValue = AlgorithmDiss3228M.ContrBsFrequency.Frequency1Max;
            siFrequency2.MinValue = AlgorithmDiss3228M.ContrBsFrequency.Frequency2Min;
            siFrequency2.MaxValue = AlgorithmDiss3228M.ContrBsFrequency.Frequency2Max;
            siFrequency3.MinValue = AlgorithmDiss3228M.ContrBsFrequency.Frequency3Min;
            siFrequency3.MaxValue = AlgorithmDiss3228M.ContrBsFrequency.Frequency3Max;
        }

        private void ImageAc()
        {
            TaskVisibility(true, false);
            _activeControl = new CorrectionCode();
            tpControlTask.Controls.Add(_activeControl);
            _activeControl.Location = gbFrequencies.Location;

            try
            {
                string value = DataManager.Instance.GetAddParam(_testId, AlgorithmDiss3228M.Parameters.CorrectionCode1);
                int intValue;
                if (!String.IsNullOrEmpty(value))
                {
                    intValue = Convert.ToInt32(value);
                    ((CorrectionCode)_activeControl).nudCode1.Value = intValue;
                    SetWord1Mask(GetMask(_algorithm.GetWordAddr(0), intValue));
                }
                else
                    SetWord1Mask(GetMask(_algorithm.GetWordAddr(0), 0));
                value = DataManager.Instance.GetAddParam(_testId, AlgorithmDiss3228M.Parameters.CorrectionCode2);
                if (!String.IsNullOrEmpty(value))
                {
                    intValue = Convert.ToInt32(value);
                    ((CorrectionCode)_activeControl).nudCode2.Value = intValue;
                    SetWord2Mask(GetMask(_algorithm.GetWordAddr(1), intValue));
                }
                else
                    SetWord2Mask(GetMask(_algorithm.GetWordAddr(1), 0));

                value = DataManager.Instance.GetAddParam(_testId, AlgorithmDiss3228M.Parameters.CorrectionCode3);
                if (!String.IsNullOrEmpty(value))
                {
                    intValue = Convert.ToInt32(value);
                    ((CorrectionCode)_activeControl).nudCode3.Value = intValue;
                    SetWord3Mask(GetMask(_algorithm.GetWordAddr(2), intValue));
                }
                else
                    SetWord3Mask(GetMask(_algorithm.GetWordAddr(2), 0));
            }
            catch
            {
                MessageBox.Show(Resources.ErrorLoadParameters, Resources.ErrorCaption);
            }

            ((CorrectionCode)_activeControl).nudCode1.TextChanged += CorrectionCode1Updated;
            ((CorrectionCode)_activeControl).nudCode2.TextChanged += CorrectionCode2Updated;
            ((CorrectionCode)_activeControl).nudCode3.TextChanged += CorrectionCode3Updated;
        }

        void CorrectionCode1Updated(object sender, EventArgs args)
        {
            SetWord1Mask(GetMask(_algorithm.GetWordAddr(0), (int)((NumericUpDown)sender).Value));
            DataManager.Instance.SetAddParam(_testId, AlgorithmDiss3228M.Parameters.CorrectionCode1,
                                             ((NumericUpDown)sender).Value.ToString(CultureInfo.InvariantCulture));
        }

        void CorrectionCode2Updated(object sender, EventArgs args)
        {
            SetWord2Mask(GetMask(_algorithm.GetWordAddr(1), (int)((NumericUpDown)sender).Value));
            DataManager.Instance.SetAddParam(_testId, AlgorithmDiss3228M.Parameters.CorrectionCode2,
                                               ((NumericUpDown)sender).Value.ToString(CultureInfo.InvariantCulture));
        }

        void CorrectionCode3Updated(object sender, EventArgs args)
        {
            SetWord3Mask(GetMask(_algorithm.GetWordAddr(2), (int)((NumericUpDown)sender).Value));
            DataManager.Instance.SetAddParam(_testId, AlgorithmDiss3228M.Parameters.CorrectionCode3,
                                               ((NumericUpDown)sender).Value.ToString(CultureInfo.InvariantCulture));
        }

        private void TaskVisibility(bool array, bool frequency)
        {
            lArray1.Visible = lArray2.Visible = lArray3.Visible = array;
            SetArray1Visibility(array);
            SetArray2Visibility(array);
            SetArray3Visibility(array);
            gbFrequencies.Visible = frequency;
            if (frequency)
                siFrequency1.Inactive = siFrequency2.Inactive = siFrequency3.Inactive = true;
        }

        private void SetArray1Visibility(bool state)
        {
            gAddr1.Visible = gContr1.Visible = gP1.Visible = gF1.Visible = gService1.Visible = state;
            biAddr1.Visible = biContr1.Visible = biP1.Visible = biF1.Visible = biService1.Visible = biCheck1.Visible = state;
        }

        private void SetArray2Visibility(bool state)
        {
            gAddr2.Visible = gContr2.Visible = gP2.Visible = gF2.Visible = gService2.Visible = state;
            biAddr2.Visible = biContr2.Visible = biP2.Visible = biF2.Visible = biService2.Visible = biCheck2.Visible = state;
        }

        private void SetArray3Visibility(bool state)
        {
            gAddr3.Visible = gContr3.Visible = gP3.Visible = gF3.Visible = gService3.Visible = state;
            biAddr3.Visible = biContr3.Visible = biP3.Visible = biF3.Visible = biService3.Visible = biCheck3.Visible = state;
        }

        private void ImageUlf()
        {
            gbAll.Text = Resources.UNChNoiseParams;
            SetIndicatorsVisibility(Checks.Ulf);
        }

        private void ImageSecPower()
        {
            gbAll.Text = Resources.SecondaryPSVoltage;
            SetIndicatorsVisibility(Checks.SecPower);
        }

        private void ImageFirstPower()
        {
            gbAll.Text = Resources.PSParams;
            SetIndicatorsVisibility(Checks.FirstPower);
            // регулировка БП на 27В
            gbTune27V.Visible = true;
        }

        private void ImageStop()
        {
            foreach (var si in _arInd)
                si.Inactive = true;
            gbTune27V.Visible = false;
            if (_activeControl != null)
                _activeControl.Enabled = false;
            siFrequency1.Inactive = siFrequency2.Inactive = siFrequency3.Inactive = true;
        }

        private void ImageOpenForm()
        {
            gbAll.Text = "";
        }

        /// <summary>
        /// Разблокируем все кнопки TabPage "Проведение измерений",
        /// разрешаем переход на другие вкладки
        /// </summary>
        /// <param name="btn">кнопка, которую отжали</param>
        private void WrungButton(Button btn)
        {
            btn.FlatStyle = FlatStyle.Standard;
            btn.BackColor = Color.Transparent;
            EnableAllButtons(btn, true);
        }

        private void EnableAllButtons(Button baseB, bool state)
        {
            Control.ControlCollection collButton = baseB.Parent.Controls;
            for (int i = 0; i < collButton.Count; i++)
                if (collButton[i].GetType() == typeof(Button))
                    collButton[i].Enabled = state;
        }

        private void UnpressPressedButton(Button baseB)
        {
            Control.ControlCollection collButton = baseB.Parent.Controls;
            for (int i = 0; i < collButton.Count; i++)
                if (collButton[i].GetType() == typeof(Button))
                    if (((Button)collButton[i]).FlatStyle == FlatStyle.Flat)
                        ((Button)collButton[i]).PerformClick();
        }

        private void SetIndicatorsVisibility(Checks checkType)
        {
            int iCheckType = (int)checkType;
            if (!_visibility.ContainsKey(iCheckType))
                return;

            for (int i = 0; i < _arInd.Length; i++)
            {
                bool visibility = _visibility[iCheckType].Contains(i);
                if (_channelState != 0)
                    visibility &= !CheckIndNumber_OneChannel(i);
                _arInd[i].Visible = _arUpLabel[i].Visible = _arDownLabel[i].Visible = visibility;
                if (visibility)
                    if (_siSettings.ContainsKey(iCheckType))
                        if (_siSettings[iCheckType].ContainsKey(i))
                        {
                            SiSetting siCurrent = _siSettings[iCheckType][i];
                            _arInd[i].FractPart = siCurrent.FractPart;
                            _arInd[i].WholePart = siCurrent.WholePart;
                            if (!String.IsNullOrEmpty(siCurrent.UpString))
                                _arUpLabel[i].Text = siCurrent.UpString;
                            if (!String.IsNullOrEmpty(siCurrent.DownString))
                                _arDownLabel[i].Text = siCurrent.DownString;
                        }
                _arInd[i].Inactive = true;
            }
        }

        private readonly Dictionary<int, List<int>> _visibility = new Dictionary<int, List<int>>();
        private readonly Dictionary<int, Dictionary<int, SiSetting>> _siSettings = new Dictionary<int, Dictionary<int, SiSetting>>();

        private void FillVisibility()
        {
            _visibility.Add((int)Checks.Clear, new List<int>(new int[0])); //Первичные ИП
            _visibility.Add((int)Checks.FirstPower, new List<int>(new[] { 0, 4 })); //Первичные ИП
            _visibility.Add((int)Checks.SecPower, new List<int>(new[] { 0, 1, 3, 4, 6, 8 })); //Вторичные ИП
            _visibility.Add((int)Checks.Ulf, new List<int>(new[] { 0, 1, 3, 4, 6, 7 })); //УНЧ
        }

        /// <summary>
        /// Проверяет относится ли индикатор к текущему проверяемому каналу одноканального ДИСС
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        private bool CheckIndNumber_OneChannel(int i)
        {
            if (_channelState == 0)
                return true;
            if ((_channelState == 1) && (i != 0) && (i != 1) && (i != 8) && (i != 9))
                return true;
            if ((_channelState == 2) && (i != 3) && (i != 4) && (i != 11) && (i != 12))
                return true;
            if ((_channelState == 3) && (i != 6) && (i != 7) && (i != 14) && (i != 15))
                return true;
            return false;
        }

        private void FillSiSettings()
        {
            FirstPowerSiSettings();
            SecPowerSiSettings();
            UlfSettings();
        }

        private void UlfSettings()
        {
            Dictionary<int, SiSetting> dict = new Dictionary<int, SiSetting>();
            SiSetting si = new SiSetting
            {
                FractPart = 1,
                WholePart = 4,
                UpString = string.Format(Resources.UV, Resources.Volt),
                DownString = string.Format(Resources.ChannelN_UnchN, 1, 1)
            };
            dict.Add((int)UlfIndicators.Ulf1Channel1, si);
            si = new SiSetting
            {
                FractPart = 1,
                WholePart = 4,
                UpString = string.Format(Resources.UV, Resources.Volt),
                DownString = string.Format(Resources.ChannelN_UnchN, 1, 2)
            };
            dict.Add((int)UlfIndicators.Ulf2Channel1, si);
            si = new SiSetting
            {
                FractPart = 1,
                WholePart = 4,
                UpString = string.Format(Resources.UV, Resources.Volt),
                DownString = string.Format(Resources.ChannelN_UnchN, 2, 1)
            };
            dict.Add((int)UlfIndicators.Ulf1Channel2, si);
            si = new SiSetting
            {
                FractPart = 1,
                WholePart = 4,
                UpString = string.Format(Resources.UV, Resources.Volt),
                DownString = string.Format(Resources.ChannelN_UnchN, 2, 2)
            };
            dict.Add((int)UlfIndicators.Ulf2Channel2, si);
            si = new SiSetting
            {
                FractPart = 1,
                WholePart = 4,
                UpString = string.Format(Resources.UV, Resources.Volt),
                DownString = string.Format(Resources.ChannelN_UnchN, 3, 1)
            };
            dict.Add((int)UlfIndicators.Ulf1Channel3, si);
            si = new SiSetting
            {
                FractPart = 1,
                WholePart = 4,
                UpString = string.Format(Resources.UV, Resources.Volt),
                DownString = string.Format(Resources.ChannelN_UnchN, 3, 2)
            };
            dict.Add((int)UlfIndicators.Ulf2Channel3, si);
            _siSettings.Add((int)Checks.Ulf, dict);
        }

        private void FirstPowerSiSettings()
        {
            Dictionary<int, SiSetting> dict = new Dictionary<int, SiSetting>();
            SiSetting si = new SiSetting
            {
                FractPart = 2,
                WholePart = 3,
                UpString = string.Format(Resources.UV, Resources.Volt),
                DownString = string.Format(Resources.Positive27V, Resources.Volt)
            };
            dict.Add(0, si);
            si = new SiSetting
            {
                FractPart = 2,
                WholePart = 3,
                UpString = string.Format(Resources.IA, Resources.Amper),
                DownString = string.Format(Resources.Current27V, Resources.Amper)
            };
            dict.Add(4, si);
            _siSettings.Add((int)Checks.FirstPower, dict);
        }

        private void SecPowerSiSettings()
        {
            Dictionary<int, SiSetting> dict = new Dictionary<int, SiSetting>();
            SiSetting si = new SiSetting
            {
                FractPart = 2,
                WholePart = 3,
                UpString = string.Format(Resources.UV, Resources.Volt),
                DownString = string.Format(12.ToString("+0"), Resources.Volt)
            };
            dict.Add((int)SecPowerIndicators.Plus12V, si);
            si = new SiSetting
            {
                FractPart = 2,
                WholePart = 2,
                UpString = string.Format(Resources.UV, Resources.Volt),
                DownString = -12 + Resources.Volt
            };
            dict.Add((int)SecPowerIndicators.Minus12V, si);
            si = new SiSetting
            {
                FractPart = 2,
                WholePart = 2,
                UpString = string.Format(Resources.UV, Resources.Volt),
                DownString = string.Format(15.ToString("+0"), Resources.Volt)
            };
            dict.Add((int)SecPowerIndicators.Plus15V, si);
            si = new SiSetting
            {
                FractPart = 2,
                WholePart = 2,
                UpString = string.Format(Resources.UV, Resources.Volt),
                DownString = -15 + Resources.Volt
            };
            dict.Add((int)SecPowerIndicators.Minus15V, si);
            si = new SiSetting
            {
                FractPart = 3,
                WholePart = 1,
                UpString = string.Format(Resources.UV, Resources.Volt),
                DownString = string.Format(9.ToString("+0"), Resources.Volt)
            };
            dict.Add((int)SecPowerIndicators.Plus9V, si);
            si = new SiSetting
            {
                FractPart = 3,
                WholePart = 1,
                UpString = string.Format(Resources.UV, Resources.Volt),
                DownString = 5.ToString("+0") + Resources.Volt
            };
            dict.Add((int)SecPowerIndicators.Plus5V, si);
            _siSettings.Add((int)Checks.SecPower, dict);
        }

        private void b27Minus_Click(object sender, EventArgs e)
        {
            EnablePowerControls(false);
            const double shift = -0.1;
            _algorithm.ShiftPower27(shift);
            EnablePowerControls(true);
        }

        private void rbNorma_CheckedChanged(object sender, EventArgs e)
        {
            gbPower.Enabled = false;
            if (rbNorma.FlatStyle == FlatStyle.Standard)
            {
                rbNorma.FlatStyle = FlatStyle.Flat;
                _algorithm.SetVoltageRegime(AlgorithmDiss3228M.VoltageRegime.Standard);
                rbPlus.FlatStyle = rbMinus.FlatStyle = FlatStyle.Standard;
            }
            gbPower.Enabled = true;
        }

        private void rbMinus_CheckedChanged(object sender, EventArgs e)
        {
            gbPower.Enabled = false;
            if (rbMinus.FlatStyle == FlatStyle.Standard)
            {
                rbMinus.FlatStyle = FlatStyle.Flat;
                _algorithm.SetVoltageRegime(AlgorithmDiss3228M.VoltageRegime.Min);
                rbPlus.FlatStyle = rbNorma.FlatStyle = FlatStyle.Standard;
            }
            gbPower.Enabled = true;
        }

        private void RunWorkTimer()
        {
            _startDissTime = DateTime.Now;
            _tWorkTimer = new Timer { Interval = 1000 };
            _tWorkTimer.Tick += WorkTimerOnTick;
            _tWorkTimer.Start();
        }

        private void WorkTimerOnTick(object sender, EventArgs eventArgs)
        {
            TimeSpan ts = DateTime.Now - _startDissTime;
            siWorkTime.ValueString = string.Format("{0:00}:{1:00}:{2:00}", ts.Hours, ts.Minutes, ts.Seconds);
        }

        private void rbPlus_CheckedChanged(object sender, EventArgs e)
        {
            gbPower.Enabled = false;
            if (rbPlus.FlatStyle == FlatStyle.Standard)
            {
                rbPlus.FlatStyle = FlatStyle.Flat;
                _algorithm.SetVoltageRegime(AlgorithmDiss3228M.VoltageRegime.High);
                rbMinus.FlatStyle = rbNorma.FlatStyle = FlatStyle.Standard;
            }
            gbPower.Enabled = true;
        }

        private void rbOn_CheckedChanged(object sender, EventArgs e)
        {
            gbMode.Enabled = false;
            _statePower27 = 0;
            try
            {
                if (rbOn.Checked)
                {
                    _powerOff = false;
                    errorPS27Appear = false;
                    rbOn.FlatStyle = FlatStyle.Flat;
                    rbOff.FlatStyle = FlatStyle.Standard;
                    rbOff.Text = Resources.DissCanOff;
                    rbOn.Text = Resources.DissOn;
                    if (_timerCollectParams.Enabled)
                        _timerCollectParams.Stop();
                    if (_algorithm == null)
                        _mreEndOfLoad.WaitOne();
                    if (_algorithm == null)
                    {
                        MessageBox.Show(Resources.ErrorAlgorithmInitialization);
                        return;
                    }                    
                    if (_algorithm != null && !_algorithm.DissOn(true))
                        MessageBox.Show(Resources.Error);
                    else
                    {
                        EnableAllButtons(bFirstPower, true);
                        EnableAllButtons(bAC, true);
                        //var logger = LogManager.GetCurrentClassLogger();
                        _logger.Info(string.Format("ИП постоянного тока 27В включен!"));
                    }
                    StartCollectTimer();
                    RunWorkTimer();
                }
            }
            catch(Exception ex)
            {
                _logger.Error(ex);
                MessageBox.Show(string.Format(Properties.Resources.ErrorOnDeviceN, string.Format(Properties.Resources.PowerSupplyN, 27)));
            }
            gbMode.Enabled = true;
        }


        private void rbOff_CheckedChanged(object sender, EventArgs e)
        {
            gbMode.Enabled = false;
            try
            {
                if (rbOff.Checked)
                {
                    _powerOff = true;
                    rbOff.FlatStyle = FlatStyle.Flat;
                    rbOn.FlatStyle = FlatStyle.Standard;
                    rbOff.Text = Resources.DissOff;
                    rbOn.Text = Resources.DissCanOn;
                    ImageDesigner(Checks.Stop);
                    if (!_algorithm.DissOn(false))
                        MessageBox.Show(Resources.Error);
                    if (_statePower27 < 1)
                    {
                        //var logger = LogManager.GetCurrentClassLogger();
                        _logger.Info(string.Format("ИП постоянного тока 27В выключен!"));
                        _statePower27++;
                    }
                    UnpressPressedButton(bFirstPower);
                    UnpressPressedButton(bAC);
                    EnableAllButtons(bFirstPower, false);
                    EnableAllButtons(bAC, false);
                    if (_timerCollectParams.Enabled)
                        _timerCollectParams.Stop();
                    _tWorkTimer.Stop();
                    siWorkTime.Inactive = true;
                }
            }
            catch(Exception ex)
            {
                _logger.Error(ex);
                MessageBox.Show(string.Format(Properties.Resources.ErrorOnDeviceN, string.Format(Properties.Resources.PowerSupplyN, 27)));
            }
            gbMode.Enabled = true;
        }

        private void btn_27_Plus_Click(object sender, EventArgs e)
        {
            EnablePowerControls(false);
            const double shift = 0.1;
            _algorithm.ShiftPower27(shift);
            EnablePowerControls(true);
        }

        private void EnablePowerControls(bool state)
        {
            gbTune27V.Enabled = gbPower.Enabled = state;
        }

        private void bCheck_Click(object sender, EventArgs e)
        {
            Button bCurrent = (Button)sender;
            bCurrent.Enabled = false;
            Checks check = (Checks)Convert.ToInt32(bCurrent.Tag);
            if (bCurrent.FlatStyle == FlatStyle.Standard)
            {
                _buttonActivated = true;
                PressButton(bCurrent);
                ImageDesigner(check);
                if (check == Checks.Ulf)
                    ShowChannelButtons(true);
                _actionDelegates[check].Activate();
            }
            else
            {
                _actionDelegates[check].Deactivate();
                ImageDesigner(Checks.Stop);
                WrungButton(bCurrent);
                ShowChannelButtons(false);
                _buttonActivated = false;
            }
            bCurrent.Enabled = true;
        }

        private void ShowChannelButtons(bool state)
        {
            foreach (RadioButton rb in _chButtons)
            {
                rb.Enabled = state;
                rb.Visible = state;
                if (!state)
                {
                    rb.FlatStyle = FlatStyle.Standard;
                    rb.Checked = false;
                }
            }
            if (state)
                _chButtons[0].PerformClick();
            else
                _channelState = 0;
        }

        private void SetIndicatorLimits(Dictionary<AlgorithmDiss3228M.Indicators, int> eqDict, int indNum, double min, double max)
        {
            var indicator = eqDict[(AlgorithmDiss3228M.Indicators)indNum];
            if (_arInd.Length <= indicator)
                return;

            _arInd[indicator].MinValue = min;
            _arInd[indicator].MaxValue = max;
            _arInd[indicator].Value = 0; //Сброс индикатора до того как он получит значение.
        }

        private void SetIndicatorFromAlgorithm(int indNum, AlgorithmDiss3228M.Indicators indicator)
        {
            double value = _algorithm.GetIndicatorValue(indicator);
            if (!double.IsNaN(value))
                _arInd[indNum].Value = value;
        }

        private void tabControl1_Selecting(object sender, TabControlCancelEventArgs e)
        {
            // При нажатых кнопках на вкладке запрещаем переход на другую вкладку
            if (_buttonActivated)
            {
                e.Cancel = true;
                MessageBox.Show(Resources.CantChangeTab, Resources.ApplicationName, MessageBoxButtons.OK,
                                MessageBoxIcon.Information);
            }
        }

        private void frmWork3228_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                if (_algorithm != null)
                    _algorithm.Finish();
                Power27V.Instance.SetOnOff(false);
                if (_timerCollectParams.Enabled)
                    _timerCollectParams.Stop();
                if (_tWorkTimer.Enabled)
                    _tWorkTimer.Stop();
                if (_statePower27 < 1)
                {
                    //var logger = LogManager.GetCurrentClassLogger();
                    _logger.Info(string.Format("ИП постоянного тока 27В выключен!"));
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                MessageBox.Show(Properties.Resources.CloseFormError);
            }
        }
    }
}
