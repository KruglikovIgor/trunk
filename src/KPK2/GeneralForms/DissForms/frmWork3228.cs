using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;
using KPK2.Common.DissFunctions;
using KPK2.MyClasse;
using KPK2.MyControl;
using Sparc.KPK2.Algorithms;
using Sparc.KPK2.Hardware;
using Sparc.KPK2.Hardware.PS115V;
using Sparc.KPK2.Imitator;
using Sparc.KPK2.NFunctions;
using Sparc.KPK2.Resources.Properties;
using UserControlsKPK2;
using LogOpen = Sparc.KPK2.NFunctions.LogOpen;
using Timer = System.Windows.Forms.Timer;
using Res = Sparc.KPK2.Resources.Properties.Resources;
using KPK2.GeneralForms.Enums;
using NLog;
using Sparc.KPK2.Hardware.PS27V;
using Sparc.KPK2.NFunctions.Enums;

namespace KPK2.GeneralForms.DissForms
{
    public partial class frmWork3228 : Form, IVisual
    {
        private static Logger _logger = LogManager.GetLogger("KPK2Error");
        private int _testId;
        private AlgorithmBase _algorithm;
        private Timer tCollectParams = new Timer();
        private int _collectInterval = 250;
        private List<TextIndicator> _indDIOrderList = new List<TextIndicator>();
        private List<TextIndicator> _indAdvOrderList = new List<TextIndicator>();
        private List<MethodInvoker> _renewDelegateList = new List<MethodInvoker>();
        private Dictionary<Checks, TwoDelegates> _actionDelegates = new Dictionary<Checks, TwoDelegates>();
        private Thread _tLoad;
        private ManualResetEvent _mreEndOfLoad = new ManualResetEvent(false);
        private Control _activeControl;
        readonly Color _pressBackColor = Color.FromArgb(153, 249, 164);
        public Imitator.Diss3228 _device = new Imitator.Diss3228();
        private Timer _tWorkTimer;
        private DateTime _startDissTime;
        private List<RadioButton> chButtons = new List<RadioButton>();
        private bool _oneChannelDiss;
        private bool _buttonActivated = false;

        private int _statePower115;

        private DevicesID _deviceId = DevicesID.Diss3228;
        private Modification _modification = Modification.None;
        private string _deviceNumber;

        public frmWork3228()
        {
            InitializeComponent();
            FillDictionaries();
        }

        private void FillDictionaries()
        {
            FillDictionaryTK();
            FillDictionarySD();
            FillDictionaryUnCh();
            FillDictionaryPM();
            FillDictionarySP();
            FillDictionaryFP();
        }

        //private void InitAdvOrderList()
        //{
        // //_indAdvOrderList.Add(tiNavigation);
        //}

        private void InitIndOrderList()
        {
            _indDIOrderList.Add(tiDissOn);
            //_indDIOrderList.Add(tiNavigation);
           // _indDIOrderList.Add(tiFlight);
            _indDIOrderList.Add(tiHigh);
            _indDIOrderList.Add(tiMemory1);
            _indDIOrderList.Add(tiMemory2);
            //_indDIOrderList.Add(tiCapture);
            _indDIOrderList.Add(tiCapture1);
            _indDIOrderList.Add(tiCapture2);
            _indDIOrderList.Add(tiCapture3);
            _indDIOrderList.Add(tiGood);
            _indDIOrderList.Add(tiGood1);
            _indDIOrderList.Add(tiGood2);
            _indDIOrderList.Add(tiGood3);
        }

        private void StartCollectTimer()
        {
            if (InvokeRequired)
            {
                Invoke(new MethodInvoker(StartCollectTimer));
                return;
            }
            tCollectParams = new Timer();
            tCollectParams.Tick += TCollectParamsOnTick;
            tCollectParams.Interval = _collectInterval;
            tCollectParams.Start();
        }

        private enum IndType
        {
            DI,
            Adv
        }

        private bool _powerOff = false;

        private void TCollectParamsOnTick(object sender, EventArgs eventArgs)
        {
            tCollectParams.Stop();
            RenewIndicators(IndType.DI);
            RenewIndicators(IndType.Adv);
            //tiHigh.State = IsStateHigh(_algorithm.GetIndicatorValue(AlgorithmBase.Indicators.HighVoltage));
            //label3.Text = Math.Round(_algorithm.GetIndicatorValue(AlgorithmBase.Indicators.Flight), 3).ToString();
            //label4.Text = Math.Round(_algorithm.GetIndicatorValue(AlgorithmBase.Indicators.Capture), 3).ToString();
            if (_powerOff)
            {
                tiFlight.State = false;
                tiCapture.State = false;
            }
            else
            {
                tiFlight.State = IsState(_algorithm.GetIndicatorValue(AlgorithmBase.Indicators.Flight));
                tiCapture.State = IsState(_algorithm.GetIndicatorValue(AlgorithmBase.Indicators.Capture));
            }
            //var log = LogManager.GetLogger("KPK2Info");
            _logger.Trace("High: {0}", _algorithm.GetIndicatorValue(AlgorithmBase.Indicators.HighVoltage));
            _logger.Trace("Flight: {0}", _algorithm.GetIndicatorValue(AlgorithmBase.Indicators.Flight));
            _logger.Trace("Capture: {0}", _algorithm.GetIndicatorValue(AlgorithmBase.Indicators.Capture));
            RenewPower();
            foreach (MethodInvoker inv in _renewDelegateList)
                inv();
            tCollectParams.Start();
        }

        private bool IsState(double value)
        {
            const double stateLimit = 5.9;
            return Math.Abs(value) > stateLimit;
        }

        //private bool IsStateHigh(double value)
        //{
        //    return value > 1;
        //}
        private bool errorPS27Appear = false;

        private void RenewPower()
        {
            if (piStrip.Bars.Count == 0)
                return;
            piStrip.Bars[0].Value = (float)Math.Round(_algorithm.Voltage27, 2);
            piStrip.Bars[1].Value = (float)Math.Round(_algorithm.Current27, 2);
            piStrip.Bars[2].Value = (float)Math.Round(_algorithm.Voltage115, 1);
            piStrip.Bars[3].Value = (float)Math.Round(_algorithm.Current115, 2);

            if (!errorPS27Appear)
                if (Common.Globals.CheckPS27Protection(Power27V.Instance, ShowError))
                    errorPS27Appear = true;
        }

        private void RenewIndicators(IndType indType)
        {
            BitArray indState = indType == IndType.DI ? _algorithm.DIIndicators : _algorithm.AdvIndicators;
            List<TextIndicator> curList = indType == IndType.DI ? _indDIOrderList : _indAdvOrderList;
            for (int i = 0; i < indState.Length; i++)
                if (_powerOff)
                    SetIndicatorState(curList, i, false);
                else
                    SetIndicatorState(curList, i, indState[i]);
            //if (cbOneChannelDiss.Checked)
            //    tiCapture.State = tiCapture1.State;
            //else
            //    tiCapture.State = tiCapture1.State && tiCapture2.State && tiCapture3.State;
        }

        private void SetIndicatorState(List<TextIndicator> indOrderList, int numIndicator, bool state)
        {
            if (indOrderList.Count > numIndicator)
                indOrderList[numIndicator].State = state;
        }

        public frmWork3228(int id, string devNum = "")
            : this()
        {
            _testId = id;
            _deviceNumber = devNum;
        }

        private void frmWork3228_Load(object sender, EventArgs e)
        {
            try
            {
                if (String.IsNullOrEmpty(_deviceNumber))
                {
                    Text = string.Format(Res.CheckDeviceN, Globals.GetDissCaption(_deviceId, _modification));
                    //var logger = LogManager.GetCurrentClassLogger();
                    _logger.Info(Text);
                }
                else
                {
                    Text = string.Format(Res.CheckDeviceNN, Globals.GetDissCaption(_deviceId, _modification),
                        _deviceNumber);
                    //var logger = LogManager.GetCurrentClassLogger();
                    _logger.Info(Text);
                }

                CreateIndicators();
                CreateButtons();
                siWorkTime.Inactive = true;
            }
            catch(Exception ex)
            {
                ShowError(Resources.LoadFormError);
                _logger.Error(ex);
            }
        }

        private void frmWork3228_Shown(object sender, EventArgs e)
        {
            try
            {
                _tLoad = new Thread(ThreadLoad);
                _tLoad.Priority = ThreadPriority.Lowest;
                _tLoad.Start();
            }
            catch(Exception ex)
            {
                ShowError(Resources.LoadFormError);
                _logger.Error(ex);
            }
        }

        private void ThreadLoad()
        {
            try
            {
                Common.Globals.SetThreadCulture(Thread.CurrentThread);
                InitIndOrderList();
                //InitAdvOrderList();
                FillVisibility();
                FillSISettings();
                FillActionDelegates();
                _algorithm = new AlgorithmBase(this, _testId, (str) => _logger.Error(str));
            }
            catch
            {
                ShowError(Resources.LoadFormError);
            }
            _mreEndOfLoad.Set();
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

        public void ErrorAppear(string errText)
        {
            ShowError(errText);
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

        private void FillActionDelegates()
        {
            TwoDelegates td = new TwoDelegates();
            td.Activate = ActivateFirstPower;
            td.Deactivate = DeactivateFirstPower;
            _actionDelegates.Add(Checks.FirstPower, td);
            td = new TwoDelegates();
            td.Activate = ActivateSecondPower;
            td.Deactivate = DeactivateSecondPower;
            _actionDelegates.Add(Checks.SecPower, td);
            td = new TwoDelegates();
            td.Activate = ActivatePM;
            td.Deactivate = DeactivatePM;
            _actionDelegates.Add(Checks.PM_MOD, td);
            td = new TwoDelegates();
            td.Activate = ActivateSD;
            td.Deactivate = DeactivateSD;
            _actionDelegates.Add(Checks.SD, td);
            td = new TwoDelegates();
            td.Activate = ActivateUNCh;
            td.Deactivate = DeactivateUNCh;
            _actionDelegates.Add(Checks.UNCh, td);
            td = new TwoDelegates();
            td.Activate = ActivateAC;
            td.Deactivate = DeactivateAC;
            _actionDelegates.Add(Checks.AC, td);
            td = new TwoDelegates();
            td.Activate = ActivateContrBS;
            td.Deactivate = DeactivateContrBS;
            _actionDelegates.Add(Checks.ContrBS, td);
            td = new TwoDelegates();
            td.Activate = ActivateContrVCh;
            td.Deactivate = DeactivateContrVCh;
            _actionDelegates.Add(Checks.ContrVCh, td);
        }

        private void CreateButtons()
        {
            int baseY = 5;
            int baseWidth = 104;
            const int baseHeight = 40;
            int space = 11;
            int stepX = baseWidth + space * 2;
            //baseY += stepY;
            baseWidth *= 2;
            baseWidth += space;
            stepX += baseWidth;

            RadioButton b;

            for (int i = 0; i < 3; i++)
            {
                b = new RadioButton();
                b.Name = "bChannel" + (i + 1);
                tpWork.Controls.Add(b);
                b.Location = new Point(gbAll.Location.X + baseWidth / 2 + baseWidth / 20 + i * (stepX - baseWidth / 4), gbAll.Location.Y + gbAll.Height + baseY);
                b.TextAlign = ContentAlignment.MiddleCenter;
                b.Text = (i + 1).ToString();
                b.Appearance = Appearance.Button;
                b.Size = new Size(baseWidth, baseHeight);
                b.Visible = false;
                b.CheckedChanged += BOnCheckedChanged;
                b.Click += BOnClick;
                chButtons.Add(b);
            }
        }

        private int _channelState = 0;
        private double _channelValue1 = 0;
        private double _channelValue2 = 0;

        private void BOnCheckedChanged(object sender, EventArgs eventArgs)
        {
            RadioButton rb = (RadioButton)sender;
            rb.BackColor = rb.Checked ? Globals.SuccessColor : Color.Transparent;
            if (rb.Checked)
            {
                int numButton = Convert.ToInt32(rb.Name.Substring(rb.Name.Length - 1, 1));
                SaveOneChannelParams(_algorithm.CurrentParameter, _channelState);
                SetChannelIndicatorsVisible(0, false);
                SetChannelIndicatorsVisible(1, false);
                SetChannelIndicatorsVisible(2, false);
                _channelState = numButton;
                Diss32Functions.SetMeasChannel(_channelState - 1);
                SetChannelIndicatorsVisible(_channelState - 1, true);
                if (bTK.FlatStyle == FlatStyle.Flat)
                {
                    TKState = 0;
                    _renewDelegateList.Clear();
                }
            }
        }

        private void SetChannelIndicatorsVisible(int channel, bool state)
        {
            if (channel < 0)
                return;
            int ind1 = channel * 3;
            int ind2 = channel * 3 + 1;
            arInd[ind1].Visible = state;
            arInd[ind2].Visible = state;
            arUpLabel[ind1].Visible = state;
            arUpLabel[ind2].Visible = state;
            arDownLabel[ind1].Visible = state;
            arDownLabel[ind2].Visible = state;
        }

        private void SaveOneChannelParams(string paramName, int channelState)
        {
            DataManager.Instance.SaveParameter(_testId, string.Format(paramName, channelState, 1), _channelValue1);
            DataManager.Instance.SaveParameter(_testId, string.Format(paramName, channelState, 2), _channelValue2);
        }

        private bool _stopCheck;

        private void BOnClick(object sender, EventArgs eventArgs)
        {
            if (_stopCheck)
                return;

            RadioButton rb = (RadioButton)sender;
            if (bTK.FlatStyle != FlatStyle.Flat)
                return;

            //Только для проверки тока
            if (TKState == 0)
            {
                ImageDesigner(Checks.TK);
                // выключаем "Высокое"
                if (cbHigh.Checked)
                    cbHigh.Checked = false;
                cbHigh.Enabled = false;
                _algorithm.SetTKState(true);
                _algorithm.ActivateTKFirst(_channelState, SetIndicatorLimitCode);
                _algorithm.ResetLimits(SetIndicatorLimitsTK1);
                _renewDelegateList.Add(TKMonitor1);
                //SetChannelIndicatorsVisible(CurTok - 1, true);
                TKState++;
            }
            else if (TKState == 1)
                ToK_K_Step2_OneChannel();
            else if (TKState == 2)
            {
                ToK_K_Step3_OneChannel();
                ((Control) sender).Enabled = false;
            }
            else if (TKState == 3)
                ToK_K_Step4_OneChannel();
        }

        private void SetIndicatorLimitCode(int indNumber, double min, double max)
        {
            foreach (var item in _TK1Indicators)
                if (item.Value == indNumber)
                    SetIndicatorLimit(arInd[(int)item.Key], min, max);
        }

        private void ToK_K_Step4_OneChannel()
        {
            _algorithm.SetTKState(false);
            ImageDesigner(Checks.Stop);
            cbHigh.Enabled = true;
            TKState = 0;
        }

        private void ToK_K_Step3_OneChannel()
        {

            TKState++;
            _algorithm.Stop();
            _renewDelegateList.Remove(TKMonitor2);
            ImageDesigner(Checks.TK3);
            ShowStaticTKValuesAndSave(_channelState);

        }

        private void ToK_K_Step2_OneChannel()
        {
            _algorithm.RememberTKFirst(_channelState);
            _algorithm.ResetLimits(SetIndicatorLimitsTK2);
            _renewDelegateList.Remove(TKMonitor1);
            ImageDesigner(Checks.TK2);
            // включаем "Высокое"
            cbHigh.Checked = true;
            _renewDelegateList.Add(TKMonitor2);
            TKState++;
        }

        private const string indName = "siInd";
        private const string lUpName = "lUp";
        private const string lDownName = "lDown";
        private const int NumIndInRow = 8;
        private Label[] arUpLabel = new Label[16];
        private Label[] arDownLabel = new Label[16];
        private SegmentIndication[] arInd = new SegmentIndication[16];

        private void CreateIndicators()
        {
            const int baseX = 5;
            const int baseY = 45;
            const int baseWidth = 104;
            const int baseHeight = 40;
            const int stepX = baseWidth + 11;
            const int stepY = baseHeight + 40;

            SegmentIndication si;
            Label lbl;
            for (int i = 0; i < 2; i++)
                for (int j = 0; j < NumIndInRow; j++)
                {
                    si = new SegmentIndication();
                    si.Name = indName + "_" + i + "_" + j;
                    arInd[j + i * NumIndInRow] = si;
                    SegmentIndicatorDefault(ref si);
                    si.Size = new Size(baseWidth, baseHeight);
                    gbAll.Controls.Add(si);
                    si.Location = new Point(baseX + j * stepX, baseY + i * stepY);
                    //Верхний лейбл
                    lbl = new Label();
                    lbl.Name = lUpName + "_" + i + "_" + j;
                    lbl.Text = string.Format(Resources.UV, Resources.Volt);
                    //arUpLabel.Add(lbl);
                    arUpLabel[j + i * NumIndInRow] = lbl;
                    LabelDefault(ref lbl);
                    lbl.AutoSize = true;
                    gbAll.Controls.Add(lbl);
                    lbl.Location = new Point(baseX + j * stepX, baseY + i * stepY - lbl.Size.Height - 3);
                    //Нижний лейбл
                    lbl = new Label();
                    lbl.Name = lDownName + "_" + i + "_" + j;
                    lbl.Text = String.Empty;
                    lbl.TextAlign = ContentAlignment.MiddleCenter;
                    arDownLabel[j + i * NumIndInRow] = lbl;
                    LabelDefault(ref lbl);
                    lbl.Size = new Size(baseWidth, (int)(lbl.Font.Size * 2));
                    gbAll.Controls.Add(lbl);
                    lbl.Location = new Point(baseX + j * stepX, baseY + i * stepY + baseHeight);
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
            _algorithm.ActivateFirstPower(SetIndicatorLimitsFP);
            _algorithm.SaveResults = true;
            _renewDelegateList.Add(FirstPowerMonitor);
        }

        Dictionary<AlgorithmBase.Indicators, int> _fpIndicators = new Dictionary<AlgorithmBase.Indicators, int>();

        private void FillDictionaryFP()
        {
            _fpIndicators.Add(AlgorithmBase.Indicators.Power27V, 0);
            _fpIndicators.Add(AlgorithmBase.Indicators.Current27V, 1);
            _fpIndicators.Add(AlgorithmBase.Indicators.Power115V, 4);
        }

        private void SetIndicatorLimitsFP(int indNum, double min, double max)
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
            _algorithm.ActivateSecondPower(SetIndicatorLimitsSP);
            _algorithm.SaveResults = true;
            _renewDelegateList.Add(SecondPowerMonitor);
        }

        private void SetIndicatorLimitsSP(int indNum, double min, double max)
        {
            SetIndicatorLimits(_spIndicators, indNum, min, max);
        }

        Dictionary<AlgorithmBase.Indicators, int> _spIndicators = new Dictionary<AlgorithmBase.Indicators, int>();

        public void FillDictionarySP()
        {
            _spIndicators.Add(AlgorithmBase.Indicators.Plus12V, (int)SecPowerIndicators.Plus12V);
            _spIndicators.Add(AlgorithmBase.Indicators.Minus12V, (int)SecPowerIndicators.Minus12V);
            _spIndicators.Add(AlgorithmBase.Indicators.Minus20V, (int)SecPowerIndicators.Minus20V);
            _spIndicators.Add(AlgorithmBase.Indicators.Plus6V, (int)SecPowerIndicators.Plus6V);
            _spIndicators.Add(AlgorithmBase.Indicators.Minus6V, (int)SecPowerIndicators.Minus6V);
            _spIndicators.Add(AlgorithmBase.Indicators.Plus5V, (int)SecPowerIndicators.Plus5V);
        }

        private void SecondPowerMonitor()
        {
            SetIndicatorFromAlgorithm((int)SecPowerIndicators.Plus12V, AlgorithmBase.Indicators.Plus12V);
            SetIndicatorFromAlgorithm((int)SecPowerIndicators.Minus12V, AlgorithmBase.Indicators.Minus12V);
            SetIndicatorFromAlgorithm((int)SecPowerIndicators.Minus20V, AlgorithmBase.Indicators.Minus20V);
            SetIndicatorFromAlgorithm((int)SecPowerIndicators.Plus6V, AlgorithmBase.Indicators.Plus6V);
            SetIndicatorFromAlgorithm((int)SecPowerIndicators.Minus6V, AlgorithmBase.Indicators.Minus6V);
            SetIndicatorFromAlgorithm((int)SecPowerIndicators.Plus5V, AlgorithmBase.Indicators.Plus5V);
        }

        private void DeactivateSecondPower()
        {
            _renewDelegateList.Remove(SecondPowerMonitor);
            _algorithm.Stop();
        }

        private void ActivatePM()
        {
            _algorithm.SetTKState(true);
            _algorithm.ActivatePM(SetIndicatorLimitsPM);
            _algorithm.SaveResults = true;
            _renewDelegateList.Add(PMMonitor);
        }

        Dictionary<AlgorithmBase.Indicators, int> _pmIndicators = new Dictionary<AlgorithmBase.Indicators, int>();

        private void FillDictionaryPM()
        {
            _pmIndicators.Add(AlgorithmBase.Indicators.PM1, (int)PMIndicators.PM1);
            _pmIndicators.Add(AlgorithmBase.Indicators.PM2, (int)PMIndicators.PM2);
            _pmIndicators.Add(AlgorithmBase.Indicators.PMC, (int)PMIndicators.PMC);
            _pmIndicators.Add(AlgorithmBase.Indicators.MOD1, (int)PMIndicators.MOD1);
            _pmIndicators.Add(AlgorithmBase.Indicators.MOD2, (int)PMIndicators.MOD2);
        }

        private void SetIndicatorLimitsPM(int indNum, double min, double max)
        {
            SetIndicatorLimits(_pmIndicators, indNum, min, max);
        }

        private void PMMonitor()
        {
            SetIndicatorFromAlgorithm((int)PMIndicators.PM1, AlgorithmBase.Indicators.PM1);
            SetIndicatorFromAlgorithm((int)PMIndicators.PM2, AlgorithmBase.Indicators.PM2);
            SetIndicatorFromAlgorithm((int)PMIndicators.PMC, AlgorithmBase.Indicators.PMC);
            SetIndicatorFromAlgorithm((int)PMIndicators.MOD1, AlgorithmBase.Indicators.MOD1);
            SetIndicatorFromAlgorithm((int)PMIndicators.MOD2, AlgorithmBase.Indicators.MOD2);
        }

        private void DeactivatePM()
        {
            _algorithm.SetTKState(false);
            _algorithm.Stop();
            _renewDelegateList.Remove(PMMonitor);
        }

        const string SDParam = "SD_{0}_{1}";

        private void ActivateSD()
        {
            _algorithm.SetTKState(true);
            _algorithm.ActivateSD(SDParam, SetIndicatorLimitsSD);
            _algorithm.SaveResults = !_oneChannelDiss;
            _renewDelegateList.Add(SDMonitor);
        }

        Dictionary<AlgorithmBase.Indicators, int> _sdIndicators = new Dictionary<AlgorithmBase.Indicators, int>();

        void FillDictionarySD()
        {
            _sdIndicators.Add(AlgorithmBase.Indicators.SD1_1, (int)SDIndicators.SD1_1);
            _sdIndicators.Add(AlgorithmBase.Indicators.SD2_1, (int)SDIndicators.SD2_1);
            _sdIndicators.Add(AlgorithmBase.Indicators.SD1_2, (int)SDIndicators.SD1_2);
            _sdIndicators.Add(AlgorithmBase.Indicators.SD2_2, (int)SDIndicators.SD2_2);
            _sdIndicators.Add(AlgorithmBase.Indicators.SD1_3, (int)SDIndicators.SD1_3);
            _sdIndicators.Add(AlgorithmBase.Indicators.SD2_3, (int)SDIndicators.SD2_3);
        }

        private void SDMonitor()
        {

            SetIndicatorFromAlgorithm((int)SDIndicators.SD1_1, AlgorithmBase.Indicators.SD1_1);
            SetIndicatorFromAlgorithm((int)SDIndicators.SD2_1, AlgorithmBase.Indicators.SD2_1);
            if (_oneChannelDiss)
            {
                _channelValue1 = _algorithm.GetIndicatorValue(AlgorithmBase.Indicators.SD1_1);
                _channelValue2 = _algorithm.GetIndicatorValue(AlgorithmBase.Indicators.SD2_1);
                SetIndicatorFromAlgorithm((int)SDIndicators.SD1_2, AlgorithmBase.Indicators.SD1_1);
                SetIndicatorFromAlgorithm((int)SDIndicators.SD2_2, AlgorithmBase.Indicators.SD2_1);
                SetIndicatorFromAlgorithm((int)SDIndicators.SD1_3, AlgorithmBase.Indicators.SD1_1);
                SetIndicatorFromAlgorithm((int)SDIndicators.SD2_3, AlgorithmBase.Indicators.SD2_1);
            }
            else
            {
                SetIndicatorFromAlgorithm((int)SDIndicators.SD1_2, AlgorithmBase.Indicators.SD1_2);
                SetIndicatorFromAlgorithm((int)SDIndicators.SD2_2, AlgorithmBase.Indicators.SD2_2);
                SetIndicatorFromAlgorithm((int)SDIndicators.SD1_3, AlgorithmBase.Indicators.SD1_3);
                SetIndicatorFromAlgorithm((int)SDIndicators.SD2_3, AlgorithmBase.Indicators.SD2_3);
            }
        }

        private void DeactivateSD()
        {
            if (_oneChannelDiss)
                SaveOneChannelParams(_algorithm.CurrentParameter, _channelState);
            _algorithm.ResetLimitsSD(SetIndicatorLimitsSD);
            _algorithm.Stop();
            _algorithm.SetTKState(false);
            Diss32Functions.SetMeasChannelOff();
            _renewDelegateList.Remove(SDMonitor);
        }

        private void ActivateUNCh()
        {
            _algorithm.SetUNChState(true);
            _algorithm.ActivateUNCh(SetIndicatorLimitsUnCh);
            _algorithm.SaveResults = !_oneChannelDiss;
            _renewDelegateList.Add(UNChMonitor);
        }

        private void SetIndicatorLimitsUnCh(int indNum, double min, double max)
        {
            SetIndicatorLimits(_unchIndicators, indNum, min, max);
        }

        Dictionary<AlgorithmBase.Indicators, int> _unchIndicators = new Dictionary<AlgorithmBase.Indicators, int>();

        private void FillDictionaryUnCh()
        {
            _unchIndicators.Add(AlgorithmBase.Indicators.UNCh1_1, (int)UNChIndicators.UNCh1_1);
            _unchIndicators.Add(AlgorithmBase.Indicators.UNCh2_1, (int)UNChIndicators.UNCh2_1);
            _unchIndicators.Add(AlgorithmBase.Indicators.UNCh1_2, (int)UNChIndicators.UNCh1_2);
            _unchIndicators.Add(AlgorithmBase.Indicators.UNCh2_2, (int)UNChIndicators.UNCh2_2);
            _unchIndicators.Add(AlgorithmBase.Indicators.UNCh1_3, (int)UNChIndicators.UNCh1_3);
            _unchIndicators.Add(AlgorithmBase.Indicators.UNCh2_3, (int)UNChIndicators.UNCh2_3);
        }

        

        private void UNChMonitor()
        {
            SetIndicatorFromAlgorithm((int)UNChIndicators.UNCh1_1, AlgorithmBase.Indicators.UNCh1_1);
            SetIndicatorFromAlgorithm((int)UNChIndicators.UNCh2_1, AlgorithmBase.Indicators.UNCh2_1);
            if (_oneChannelDiss)
            {
                _channelValue1 = _algorithm.GetIndicatorValue(AlgorithmBase.Indicators.UNCh1_1);
                _channelValue2 = _algorithm.GetIndicatorValue(AlgorithmBase.Indicators.UNCh2_1);
                SetIndicatorFromAlgorithm((int)UNChIndicators.UNCh1_2, AlgorithmBase.Indicators.UNCh1_1);
                SetIndicatorFromAlgorithm((int)UNChIndicators.UNCh2_2, AlgorithmBase.Indicators.UNCh2_1);
                SetIndicatorFromAlgorithm((int)UNChIndicators.UNCh1_3, AlgorithmBase.Indicators.UNCh1_1);
                SetIndicatorFromAlgorithm((int)UNChIndicators.UNCh2_3, AlgorithmBase.Indicators.UNCh2_1);
            }
            else
            {
                SetIndicatorFromAlgorithm((int)UNChIndicators.UNCh1_2, AlgorithmBase.Indicators.UNCh1_2);
                SetIndicatorFromAlgorithm((int)UNChIndicators.UNCh2_2, AlgorithmBase.Indicators.UNCh2_2);
                SetIndicatorFromAlgorithm((int)UNChIndicators.UNCh1_3, AlgorithmBase.Indicators.UNCh1_3);
                SetIndicatorFromAlgorithm((int)UNChIndicators.UNCh2_3, AlgorithmBase.Indicators.UNCh2_3);
            }
        }

        private void DeactivateUNCh()
        {
            if (_oneChannelDiss)
                SaveOneChannelParams(_algorithm.CurrentParameter, _channelState);
            _algorithm.Stop();
            _algorithm.SetUNChState(false);
            Diss32Functions.SetMeasChannelOff();
            _renewDelegateList.Remove(UNChMonitor);
        }

        private void ActivateAC()
        {
            _renewDelegateList.Add(ACMonitor);
        }

        private Dictionary<int, BitIndication.BitState> GetMask(int addr, int correct)
        {
            Dictionary<int, BitIndication.BitState> mask = new Dictionary<int, BitIndication.BitState>();
            for (int i = 0; i < 31; i++)
            {
                BitIndication.BitState state = new BitIndication.BitState();
                state.CurrentState = BitIndication.BitState.State.Inactive;
                if (i < 8)
                {
                    int checkNumber = (int)Math.Pow(2, i);
                    state.CurrentState = (addr & checkNumber) == checkNumber ? BitIndication.BitState.State.One : BitIndication.BitState.State.Zero;
                }
                if (i >= 9 && i < 13)
                {
                    int checkNumber = (int)Math.Pow(2, (i - 9));
                    state.CurrentState = (correct & checkNumber) == checkNumber ? BitIndication.BitState.State.One : BitIndication.BitState.State.Zero;
                }
                mask.Add(i, state);
            }
            BitIndication.BitState fState = new BitIndication.BitState();
            fState.CurrentState = BitIndication.BitState.State.Formula;
            fState.Formula += delegate(BitArray ba)
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

        private void ACMonitor()
        {
            List<int> arrays = _algorithm.GetArrays();
            if (arrays == null)
                return;
            if (arrays.Count <= 2)
                return;
            SetArray1(arrays[0]);
            SetArray2(arrays[1]);
            SetArray3(arrays[2]);
            //tiGood1.State = (arrays[0] & _algorithm.GetArrayAddr(0)) == _algorithm.GetArrayAddr(0); Убираем, Юра М. сказал что это ненужно
            //tiGood2.State = (arrays[1] & _algorithm.GetArrayAddr(1)) == _algorithm.GetArrayAddr(1);
            //tiGood3.State = (arrays[2] & _algorithm.GetArrayAddr(2)) == _algorithm.GetArrayAddr(2);
        }

        private void SetArray1(int array)
        {
            biAddr1.Value = biContr1.Value = biP1.Value = biF1.Value = biService1.Value = biCheck1.Value = array;
            //biCheck1.ChangeBitMask(31, IsEven(array));
        }

        //private bool IsEven(int array)
        //{
        //  int sum = 0;
        //  for (int i = 0; i < 32; i++)
        //  {
        //    int checkNum = (int)Math.Pow(2, i);
        //    if ((checkNum & array) == checkNum)
        //      sum++;
        //  }
        //  return sum%2 == 0;
        //}

        private void SetArray2(int array)
        {
            biAddr2.Value = biContr2.Value = biP2.Value = biF2.Value = biService2.Value = biCheck2.Value = array;
            //biCheck2.ChangeBitMask(31, IsEven(array));
        }

        private void SetArray3(int array)
        {
            biAddr3.Value = biContr3.Value = biP3.Value = biF3.Value = biService3.Value = biCheck3.Value = array;
            //biCheck3.ChangeBitMask(31, IsEven(array));
        }

        private void DeactivateAC()
        {
            _renewDelegateList.Remove(ACMonitor);
            DataManager.Instance.SaveParameter(_testId, _algorithm.ModeACStr, GetArray1OKState() && GetArray2OKState() && GetArray3OKState());
        }


        private void ActivateContrBS()
        {
            SetArray1Mask(GetContrBSMask(0, _algorithm.GetArrayAddr(0)));
            SetArray2Mask(GetContrBSMask(1, _algorithm.GetArrayAddr(1)));
            SetArray3Mask(GetContrBSMask(2, _algorithm.GetArrayAddr(2)));
            _algorithm.SetContrBSLong(true);
            _device.SetContrBSState(true);
            _renewDelegateList.Add(ContrBSMonitor);
        }

        private void SetArray1Mask(Dictionary<int, BitIndication.BitState> dictionary)
        {
            biAddr1.SetMask(dictionary);
            biContr1.SetMask(dictionary);
            biP1.SetMask(dictionary);
            biF1.SetMask(dictionary);
            biService1.SetMask(dictionary);
            biCheck1.SetMask(dictionary);
        }

        private void SetArray2Mask(Dictionary<int, BitIndication.BitState> dictionary)
        {
            biAddr2.SetMask(dictionary);
            biContr2.SetMask(dictionary);
            biP2.SetMask(dictionary);
            biF2.SetMask(dictionary);
            biService2.SetMask(dictionary);
            biCheck2.SetMask(dictionary);
        }

        private void SetArray3Mask(Dictionary<int, BitIndication.BitState> dictionary)
        {
            biAddr3.SetMask(dictionary);
            biContr3.SetMask(dictionary);
            biP3.SetMask(dictionary);
            biF3.SetMask(dictionary);
            biService3.SetMask(dictionary);
            biCheck3.SetMask(dictionary);
        }

        private Dictionary<int, BitIndication.BitState> GetContrBSMask(int index, int addr)
        {
            Dictionary<int, BitIndication.BitState> mask = new Dictionary<int, BitIndication.BitState>();
            for (int i = 0; i < 31; i++)
            {
                BitIndication.BitState state = new BitIndication.BitState();
                state.CurrentState = BitIndication.BitState.State.Inactive;
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
            BitIndication.BitState fState = new BitIndication.BitState();
            fState.CurrentState = BitIndication.BitState.State.Formula;
            fState.Formula += CSFormula;
            mask.Add(31, fState);
            return mask;
        }

        private void ContrBSMonitor()
        {
            List<int> arrays = _algorithm.GetArrays();
            if (arrays == null)
                return;
            if (arrays.Count <= 2)
                return;
            //for (int i = 0; i < 3; i++)
            //{
            //    _contrBSIntegrator.AddValue(i, arrays[i]);
            //    arrays[i] = (int)_contrBSIntegrator.GetValue(i);
            //}

            SetArray1(arrays[0]);
            SetArray2(arrays[1]);
            SetArray3(arrays[2]);
            siFrequency1.Value = GetFrequencyFromArray(arrays[0]);
            siFrequency2.Value = GetFrequencyFromArray(arrays[1]);
            siFrequency3.Value = GetFrequencyFromArray(arrays[2]);
            tiGood1.State = (arrays[0] & _algorithm.GetArrayAddr(0)) == _algorithm.GetArrayAddr(0);
            tiGood2.State = (arrays[1] & _algorithm.GetArrayAddr(1)) == _algorithm.GetArrayAddr(1);
            tiGood3.State = (arrays[2] & _algorithm.GetArrayAddr(2)) == _algorithm.GetArrayAddr(2);
        }

        private double GetFrequencyFromArray(int array)
        {
            array = array >> 15;
            bool signBit = (array >> 14 & 1) == 1;
            if (signBit)
            {
                int add = ~0;
                add = add >> 15;
                add = add << 15;
                array |= add;
            }
            else
                array &= 16383;
            return array;
        }

        private void DeactivateContrBS()
        {
            _algorithm.SetContrBSLong(false);
            _device.SetContrBSState(false);
            _renewDelegateList.Remove(ContrBSMonitor);
            bool result = GetArray1OKState() && GetArray2OKState() && GetArray3OKState();
            result &= (int)siFrequency1.Value == AlgorithmBase.ContrBSFrequency.Frequency1;
            result &= (int)siFrequency2.Value == AlgorithmBase.ContrBSFrequency.Frequency2;
            result &= (int)siFrequency3.Value == AlgorithmBase.ContrBSFrequency.Frequency3;
            DataManager.Instance.SaveParameter(_testId, _algorithm.ModeContrBSStr, result);
        }

        private bool GetArray1OKState()
        {
            return biAddr1.OKState && biContr1.OKState && biP1.OKState && biF1.OKState && biService1.OKState &&
                   biCheck1.OKState;
        }

        private bool GetArray2OKState()
        {
            return biAddr2.OKState && biContr2.OKState && biP2.OKState && biF2.OKState && biService2.OKState &&
                   biCheck2.OKState;
        }

        private bool GetArray3OKState()
        {
            return biAddr3.OKState && biContr3.OKState && biP3.OKState && biF3.OKState && biService3.OKState &&
                   biCheck3.OKState;
        }

        private void ActivateContrVCh()
        {
            SetArray1Mask(GetContrVChMask(_algorithm.GetArrayAddr(0)));
            SetArray2Mask(GetContrVChMask(_algorithm.GetArrayAddr(1)));
            SetArray3Mask(GetContrVChMask(_algorithm.GetArrayAddr(2)));
            _algorithm.SetContrVCh(true);
            _device.SetContrVChState(true);
            _renewDelegateList.Add(ContrBSMonitor);
        }

        private Dictionary<int, BitIndication.BitState> GetContrVChMask(int addr)
        {
            Dictionary<int, BitIndication.BitState> mask = new Dictionary<int, BitIndication.BitState>();
            for (int i = 0; i < 31; i++)
            {
                BitIndication.BitState state = new BitIndication.BitState();
                state.CurrentState = BitIndication.BitState.State.Inactive;
                if (i < 8)
                {
                    int checkNumber = (int)Math.Pow(2, i);
                    state.CurrentState = (addr & checkNumber) == checkNumber ? BitIndication.BitState.State.One : BitIndication.BitState.State.Zero;
                }
                if (i == 8)
                    state.CurrentState = BitIndication.BitState.State.One;
                mask.Add(i, state);
            }
            BitIndication.BitState fState = new BitIndication.BitState();
            fState.CurrentState = BitIndication.BitState.State.Formula;
            fState.Formula += CSFormula;
            mask.Add(31, fState);
            return mask;
        }

        private bool CSFormula(BitArray ba)
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
            _renewDelegateList.Remove(ContrBSMonitor);
            bool result = GetArray1OKState() && GetArray2OKState() && GetArray3OKState();
            result &= siFrequency1.Value <= AlgorithmBase.ContrVChFrequency.Frequency1Max &&
                      siFrequency1.Value >= AlgorithmBase.ContrVChFrequency.Frequency1Min;
            result &= siFrequency2.Value <= AlgorithmBase.ContrVChFrequency.Frequency2Max &&
                      siFrequency2.Value >= AlgorithmBase.ContrVChFrequency.Frequency2Min;
            result &= siFrequency3.Value <= AlgorithmBase.ContrVChFrequency.Frequency3Max &&
                      siFrequency3.Value >= AlgorithmBase.ContrVChFrequency.Frequency3Min;
            DataManager.Instance.SaveParameter(_testId, _algorithm.ModeContrVChStr, result);
        }

        #endregion Activate/Deactivate Actions

        private void FirstPowerMonitor()
        {
            SetIndicatorFromAlgorithm(0, AlgorithmBase.Indicators.Power27V);
            if (!double.IsNaN(_algorithm.Voltage115))
                arInd[4].Value = _algorithm.Voltage115;
        }

        /// <summary>
        /// Настройка вида индикаторов в GroupBox
        /// </summary>
        /// <param name="ModeMeasurements"></param>
        private void ImageDesigner(Checks modeMeasurements)
        {
            try
            {

                if (modeMeasurements != Checks.Stop)
                    if (_activeControl != null)
                        _activeControl.Dispose();

                //pDefaultValues.Visible = modeMeasurements == ModeMeasurements.OpenForm;
                pDefaultValues.Visible = false;

                switch (modeMeasurements)
                {
                    //case Checks.OpenForm: // OpenForm - открываем форму-все скрываем 
                    //  ImageOpenForm();
                    //  break;
                    case Checks.Stop: // Stop - остановили измерения
                        ImageStop();
                        break;
                    case Checks.FirstPower: // FirstPitanie - 27В и 115В
                        ImageFirstPower();
                        break;
                    case Checks.SecPower:
                        ImageSecPower();
                        break;
                    case Checks.PM_MOD:
                        ImagePM();
                        break;
                    case Checks.SD:
                        ImageSD();
                        break;
                    case Checks.UNCh:
                        ImageUNCh();
                        break;
                    case Checks.TK:
                        ImageTK();
                        break;
                    case Checks.TK2:
                        ImageTK2();
                        break;
                    case Checks.TK3:
                        ImageTK3();
                        break;
                    case Checks.Memory:
                        ImageMemory();
                        break;
                    case Checks.AC:
                        ImageAC();
                        break;
                    case Checks.ContrBS:
                        ImageContrBS();
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

        private void ImageContrVCh()
        {
            TaskVisibility(true, true);
            siFrequency1.MinValue = AlgorithmBase.ContrVChFrequency.Frequency1Min;
            siFrequency1.MaxValue = AlgorithmBase.ContrVChFrequency.Frequency1Max;
            siFrequency2.MinValue = AlgorithmBase.ContrVChFrequency.Frequency2Min;
            siFrequency2.MaxValue = AlgorithmBase.ContrVChFrequency.Frequency2Max;
            siFrequency3.MinValue = AlgorithmBase.ContrVChFrequency.Frequency3Min;
            siFrequency3.MaxValue = AlgorithmBase.ContrVChFrequency.Frequency3Max;
        }

        private void ImageContrBS()
        {
            TaskVisibility(true, true);
            siFrequency1.MinValue = siFrequency1.MaxValue = AlgorithmBase.ContrBSFrequency.Frequency1;
            siFrequency2.MinValue = siFrequency2.MaxValue = AlgorithmBase.ContrBSFrequency.Frequency2;
            siFrequency3.MinValue = siFrequency3.MaxValue = AlgorithmBase.ContrBSFrequency.Frequency3;
        }

        private void ImageAC()
        {
            TaskVisibility(true, false);
            _activeControl = new CorrectionCode();
            tpControlTask.Controls.Add(_activeControl);
            _activeControl.Location = gbFrequencies.Location;
            LoadCorrectionCodes();
            ((CorrectionCode)_activeControl).nudCode1.TextChanged += CorrectionCode1Updated;
            ((CorrectionCode)_activeControl).nudCode2.TextChanged += CorrectionCode2Updated;
            ((CorrectionCode)_activeControl).nudCode3.TextChanged += CorrectionCode3Updated;
        }

        private void LoadCorrectionCodes()
        {
            try
            {
                string value = DataManager.Instance.GetAddParam(_testId, AlgorithmBase.CorrectionCodeParam1);
                int intValue = 0;
                if (!String.IsNullOrEmpty(value))
                    intValue = Convert.ToInt32(value);
                ((CorrectionCode)_activeControl).nudCode1.Value = intValue;
                RenewCorrection(0, intValue);
                

                value = DataManager.Instance.GetAddParam(_testId, AlgorithmBase.CorrectionCodeParam2);
                intValue = 0;
                if (!String.IsNullOrEmpty(value))
                    intValue = Convert.ToInt32(value);
                ((CorrectionCode)_activeControl).nudCode2.Value = intValue;
                RenewCorrection(1, intValue);

                value = DataManager.Instance.GetAddParam(_testId, AlgorithmBase.CorrectionCodeParam3);
                intValue = 0;
                if (!String.IsNullOrEmpty(value))
                    intValue = Convert.ToInt32(value);
                ((CorrectionCode)_activeControl).nudCode3.Value = intValue;
                RenewCorrection(2, intValue);
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                MessageBox.Show(Resources.ErrorLoadParameters, Resources.ErrorCaption);
            }
        }

        private void RenewCorrection(int i, int intValue)
        {
            var mask = GetMask(_algorithm.GetArrayAddr(i), BitWork.Revert4Bits(intValue));
            switch (i)
            {
                case 0:
                    SetArray1Mask(mask);
                    return;
                case 1:
                    SetArray2Mask(mask);
                    return;
                default:
                    SetArray3Mask(mask);
                    return;
            }
        }

        void CorrectionCode1Updated(object sender, EventArgs args)
        {
            int value = (int) ((NumericUpDown) sender).Value;
            RenewCorrection(0, value);
            DataManager.Instance.SetAddParam(_testId, AlgorithmBase.CorrectionCodeParam1, value.ToString());
        }

        void CorrectionCode2Updated(object sender, EventArgs args)
        {
            int value = (int)((NumericUpDown)sender).Value;
            RenewCorrection(1, value);
            DataManager.Instance.SetAddParam(_testId, AlgorithmBase.CorrectionCodeParam2, value.ToString());
        }

        void CorrectionCode3Updated(object sender, EventArgs args)
        {
            int value = (int)((NumericUpDown)sender).Value;
            RenewCorrection(2, value);
            DataManager.Instance.SetAddParam(_testId, AlgorithmBase.CorrectionCodeParam3, value.ToString());
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

        private void ImageMemory()
        {
            TaskVisibility(false, false);
            _activeControl = new Memory3228(_oneChannelDiss);
            tpControlTask.Controls.Add(_activeControl);
            _activeControl.Location = new Point(4, 4);
        }

        private void ImageTK3()
        {
            gbAll.Text = Resources.TKResults;
            SetIndicatorsVisibility(Checks.TK3);
        }

        private void ImageTK2()
        {
            gbAll.Text = Resources.TKHighOn;
            SetIndicatorsVisibility(Checks.TK2);
        }

        private void ImageTK()
        {
            gbAll.Text = Resources.TKHighOff;
            SetIndicatorsVisibility(Checks.TK);
        }

        private void ImageUNCh()
        {
            gbAll.Text = Resources.UNChNoiseParams;
            SetIndicatorsVisibility(Checks.UNCh);
        }

        private void ImageSD()
        {
            gbAll.Text = Resources.SDParams;
            SetIndicatorsVisibility(Checks.SD);
        }

        private void ImagePM()
        {
            gbAll.Text = Resources.PMParams;
            SetIndicatorsVisibility(Checks.PM_MOD);
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
            // регулировка новых БП на 27В и 115В 
            gbTune27V.Visible = gbTune115V.Visible = true;
        }

        private void ImageStop()
        {
            for (int i = 0; i < arInd.Length; i++)
                arInd[i].Inactive = true;
            gbTune115V.Visible = gbTune27V.Visible = false;
            if (_activeControl != null)
                _activeControl.Enabled = false;
            siFrequency1.Inactive = siFrequency2.Inactive = siFrequency3.Inactive = true;
        }

        private void ImageOpenForm()
        {
            gbAll.Text = "";
            ivFrequency.Visible = false;
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
            if (!_Visibility.ContainsKey(iCheckType))
                return;

            for (int i = 0; i < arInd.Length; i++)
            {
                bool visibility = _Visibility[iCheckType].Contains(i);
                if (_oneChannelDiss && _channelState != 0)
                    visibility &= !CheckIndNumber_OneChannel(i);
                arInd[i].Visible = arUpLabel[i].Visible = arDownLabel[i].Visible = visibility;
                if (visibility)
                    if (_siSettings.ContainsKey(iCheckType))
                        if (_siSettings[iCheckType].ContainsKey(i))
                        {
                            SISetting siCurrent = _siSettings[iCheckType][i];
                            arInd[i].FractPart = siCurrent.FractPart;
                            arInd[i].WholePart = siCurrent.WholePart;
                            if (!String.IsNullOrEmpty(siCurrent.UpString))
                                arUpLabel[i].Text = siCurrent.UpString;
                            if (!String.IsNullOrEmpty(siCurrent.DownString))
                                arDownLabel[i].Text = siCurrent.DownString;
                        }
                arInd[i].Inactive = true;
            }
        }

        private Dictionary<int, List<int>> _Visibility = new Dictionary<int, List<int>>();
        private Dictionary<int, Dictionary<int, SISetting>> _siSettings = new Dictionary<int, Dictionary<int, SISetting>>();

        private void FillVisibility()
        {
            _Visibility.Add((int)Checks.FirstPower, new List<int>(new[] { 0, 4 })); //Первичные ИП
            _Visibility.Add((int)Checks.SecPower, new List<int>(new[] { 0, 1, 3, 5, 6, 8 })); //Вторичные ИП
            _Visibility.Add((int)Checks.PM_MOD, new List<int>(new[] { 0, 1, 2, 4, 5 })); // ПМ МОД
            _Visibility.Add((int)Checks.TK, new List<int>(new[] { 0, 1, 3, 4, 6, 7 })); // ТОК К
            _Visibility.Add((int)Checks.SD, new List<int>(new[] { 0, 1, 3, 4, 6, 7 })); //СД
            _Visibility.Add((int)Checks.UNCh, new List<int>(new[] { 0, 1, 3, 4, 6, 7 })); //УНЧ
            _Visibility.Add((int)Checks.TK2, new List<int>(new[] { 0, 1, 3, 4, 6, 7, 8, 9, 11, 12, 14, 15 })); // ТОК К2
            _Visibility.Add((int)Checks.TK3, new List<int>(new[] { 0, 1, 3, 4, 6, 7, 8, 11, 14 })); // ТОК К3
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

        private void FillSISettings()
        {
            FirstPowerSISettings();
            SecPowerSISettings();
            PMSettings();
            TKSettings();
            SDSettings();
            UNChSettings();
        }

        private void UNChSettings()
        {
            Dictionary<int, SISetting> dict = new Dictionary<int, SISetting>();
            SISetting si = new SISetting();
            si.FractPart = 1;
            si.WholePart = 4;
            si.UpString = string.Format(Resources.UV, Resources.miliVolt);
            si.DownString = string.Format(Resources.NoiseNN, 1, 1);
            dict.Add((int)UNChIndicators.UNCh1_1, si);
            si = new SISetting();
            si.FractPart = 1;
            si.WholePart = 4;
            si.UpString = string.Format(Resources.UV, Resources.miliVolt);
            si.DownString = string.Format(Resources.NoiseNN, 2, 1);
            dict.Add((int)UNChIndicators.UNCh2_1, si);
            si = new SISetting();
            si.FractPart = 1;
            si.WholePart = 4;
            si.UpString = string.Format(Resources.UV, Resources.miliVolt);
            si.DownString = string.Format(Resources.NoiseNN, 1, 2);
            dict.Add((int)UNChIndicators.UNCh1_2, si);
            si = new SISetting();
            si.FractPart = 1;
            si.WholePart = 4;
            si.UpString = string.Format(Resources.UV, Resources.miliVolt);
            si.DownString = string.Format(Resources.NoiseNN, 2, 2);
            dict.Add((int)UNChIndicators.UNCh2_2, si);
            si = new SISetting();
            si.FractPart = 1;
            si.WholePart = 4;
            si.UpString = string.Format(Resources.UV, Resources.miliVolt);
            si.DownString = string.Format(Resources.NoiseNN, 1, 3);
            dict.Add((int)UNChIndicators.UNCh1_3, si);
            si = new SISetting();
            si.FractPart = 1;
            si.WholePart = 4;
            si.UpString = string.Format(Resources.UV, Resources.miliVolt);
            si.DownString = string.Format(Resources.NoiseNN, 2, 3);
            dict.Add((int)UNChIndicators.UNCh2_3, si);
            _siSettings.Add((int)Checks.UNCh, dict);
        }

        private void SDSettings()
        {
            Dictionary<int, SISetting> dict = new Dictionary<int, SISetting>();
            SISetting si = new SISetting();
            si.FractPart = 3;
            si.WholePart = 2;
            si.UpString = string.Format(Resources.UV, Resources.Volt);
            si.DownString = string.Format(Resources.SDNN, 1, 1);
            dict.Add((int)SDIndicators.SD1_1, si);
            si = new SISetting();
            si.FractPart = 3;
            si.WholePart = 2;
            si.UpString = string.Format(Resources.UV, Resources.Volt);
            si.DownString = string.Format(Resources.SDNN, 2, 1);
            dict.Add((int)SDIndicators.SD2_1, si);
            si = new SISetting();
            si.FractPart = 3;
            si.WholePart = 2;
            si.UpString = string.Format(Resources.UV, Resources.Volt);
            si.DownString = string.Format(Resources.SDNN, 1, 2);
            dict.Add((int)SDIndicators.SD1_2, si);
            si = new SISetting();
            si.FractPart = 3;
            si.WholePart = 2;
            si.UpString = string.Format(Resources.UV, Resources.Volt);
            si.DownString = string.Format(Resources.SDNN, 2, 2);
            dict.Add((int)SDIndicators.SD2_2, si);
            si = new SISetting();
            si.FractPart = 3;
            si.WholePart = 2;
            si.UpString = string.Format(Resources.UV, Resources.Volt);
            si.DownString = string.Format(Resources.SDNN, 1, 3);
            dict.Add((int)SDIndicators.SD1_3, si);
            si = new SISetting();
            si.FractPart = 3;
            si.WholePart = 2;
            si.UpString = string.Format(Resources.UV, Resources.Volt);
            si.DownString = string.Format(Resources.SDNN, 2, 3);
            dict.Add((int)SDIndicators.SD2_3, si);
            _siSettings.Add((int)Checks.SD, dict);
        }

        private void TKSettings()
        {
            Dictionary<int, SISetting> dict = new Dictionary<int, SISetting>();
            SISetting si = new SISetting();
            si.FractPart = 1;
            si.WholePart = 3;
            si.UpString = string.Format(Resources.UV, Resources.miliVolt);
            si.DownString = string.Format(Resources.TKNN, 1, 1);
            dict.Add((int)TKIndicators.TK1_1, si);
            si = new SISetting();
            si.FractPart = 1;
            si.WholePart = 3;
            si.UpString = string.Format(Resources.UV, Resources.miliVolt);
            si.DownString = string.Format(Resources.TKNN, 2, 1);
            dict.Add((int)TKIndicators.TK2_1, si);
            si = new SISetting();
            si.FractPart = 1;
            si.WholePart = 3;
            si.UpString = string.Format(Resources.UV, Resources.miliVolt);
            si.DownString = string.Format(Resources.TKNN, 1, 2);
            dict.Add((int)TKIndicators.TK1_2, si);
            si = new SISetting();
            si.FractPart = 1;
            si.WholePart = 3;
            si.UpString = string.Format(Resources.UV, Resources.miliVolt);
            si.DownString = string.Format(Resources.TKNN, 2, 2);
            dict.Add((int)TKIndicators.TK2_2, si);
            si = new SISetting();
            si.FractPart = 1;
            si.WholePart = 3;
            si.UpString = string.Format(Resources.UV, Resources.miliVolt);
            si.DownString = string.Format(Resources.TKNN, 1, 3);
            dict.Add((int)TKIndicators.TK1_3, si);
            si = new SISetting();
            si.FractPart = 1;
            si.WholePart = 3;
            si.UpString = string.Format(Resources.UV, Resources.miliVolt);
            si.DownString = string.Format(Resources.TKNN, 2, 3);
            dict.Add((int)TKIndicators.TK2_3, si);
            si = new SISetting();
            si.FractPart = 1;
            si.WholePart = 3;
            si.UpString = string.Format(Resources.UV, Resources.miliVolt);
            si.DownString = string.Format(Resources.TKNN, 1, 1);
            dict.Add((int)TKIndicators.TK1_1_2, si);
            si = new SISetting();
            si.FractPart = 1;
            si.WholePart = 3;
            si.UpString = string.Format(Resources.UV, Resources.miliVolt);
            si.DownString = string.Format(Resources.TKNN, 2, 1);
            dict.Add((int)TKIndicators.TK2_1_2, si);
            si = new SISetting();
            si.FractPart = 1;
            si.WholePart = 3;
            si.UpString = string.Format(Resources.UV, Resources.miliVolt);
            si.DownString = string.Format(Resources.TKNN, 1, 2);
            dict.Add((int)TKIndicators.TK1_2_2, si);
            si = new SISetting();
            si.FractPart = 1;
            si.WholePart = 3;
            si.UpString = string.Format(Resources.UV, Resources.miliVolt);
            si.DownString = string.Format(Resources.TKNN, 2, 2);
            dict.Add((int)TKIndicators.TK2_2_2, si);
            si = new SISetting();
            si.FractPart = 1;
            si.WholePart = 3;
            si.UpString = string.Format(Resources.UV, Resources.miliVolt);
            si.DownString = string.Format(Resources.TKNN, 1, 3);
            dict.Add((int)TKIndicators.TK1_3_2, si);
            si = new SISetting();
            si.FractPart = 1;
            si.WholePart = 3;
            si.UpString = string.Format(Resources.UV, Resources.miliVolt);
            si.DownString = string.Format(Resources.TKNN, 3, 2);
            dict.Add((int)TKIndicators.TK2_3_2, si);
            _siSettings.Add((int)Checks.TK, dict);
            _siSettings.Add((int)Checks.TK2, dict);

            dict = new Dictionary<int, SISetting>();
            si = new SISetting();
            si.FractPart = 1;
            si.WholePart = 3;
            si.UpString = string.Format(Resources.UV, Resources.miliVolt);
            si.DownString = string.Format(Resources.DeltaTKNN, 1, 1);
            dict.Add((int)TKIndicators.TK1_1, si);
            si = new SISetting();
            si.FractPart = 1;
            si.WholePart = 3;
            si.UpString = string.Format(Resources.UV, Resources.miliVolt);
            si.DownString = string.Format(Resources.DeltaTKNN, 2, 1);
            dict.Add((int)TKIndicators.TK2_1, si);
            si = new SISetting();
            si.FractPart = 1;
            si.WholePart = 3;
            si.UpString = string.Format(Resources.UV, Resources.miliVolt);
            si.DownString = string.Format(Resources.DeltaTKNN, 1, 2);
            dict.Add((int)TKIndicators.TK1_2, si);
            si = new SISetting();
            si.FractPart = 1;
            si.WholePart = 3;
            si.UpString = string.Format(Resources.UV, Resources.miliVolt);
            si.DownString = string.Format(Resources.DeltaTKNN, 2, 2);
            dict.Add((int)TKIndicators.TK2_2, si);
            si = new SISetting();
            si.FractPart = 1;
            si.WholePart = 3;
            si.UpString = string.Format(Resources.UV, Resources.miliVolt);
            si.DownString = string.Format(Resources.DeltaTKNN, 1, 3);
            dict.Add((int)TKIndicators.TK1_3, si);
            si = new SISetting();
            si.FractPart = 1;
            si.WholePart = 3;
            si.UpString = string.Format(Resources.UV, Resources.miliVolt);
            si.DownString = string.Format(Resources.DeltaTKNN, 2, 3);
            dict.Add((int)TKIndicators.TK2_3, si);
            si = new SISetting();
            si.FractPart = 1;
            si.WholePart = 3;
            si.UpString = string.Format(Resources.UV, Resources.miliVolt);
            si.DownString = string.Format(Resources.DeltaUNAbs, 1);
            dict.Add((int)TKIndicators.TK1_1_2, si);
            si = new SISetting();
            si.FractPart = 1;
            si.WholePart = 3;
            si.UpString = string.Format(Resources.UV, Resources.miliVolt);
            si.DownString = string.Format(Resources.DeltaUNAbs, 2);
            dict.Add((int)TKIndicators.TK1_2_2, si);
            si = new SISetting();
            si.FractPart = 1;
            si.WholePart = 3;
            si.UpString = string.Format(Resources.UV, Resources.miliVolt);
            si.DownString = string.Format(Resources.DeltaUNAbs, 3);
            dict.Add((int)TKIndicators.TK1_3_2, si);
            _siSettings.Add((int)Checks.TK3, dict);
        }

        private void PMSettings()
        {
            Dictionary<int, SISetting> dict = new Dictionary<int, SISetting>();
            SISetting si = new SISetting();
            si.FractPart = 2;
            si.WholePart = 2;
            si.UpString = string.Format(Resources.UV, Resources.Volt);
            si.DownString = string.Format(Resources.PMN, 1);
            dict.Add((int)PMIndicators.PM1, si);
            si = new SISetting();
            si.FractPart = 2;
            si.WholePart = 2;
            si.UpString = string.Format(Resources.UV, Resources.Volt);
            si.DownString = string.Format(Resources.PMN, 2);
            dict.Add((int)PMIndicators.PM2, si);
            si = new SISetting();
            si.FractPart = 2;
            si.WholePart = 1;
            si.UpString = string.Format(Resources.UV, Resources.Volt);
            si.DownString = Resources.CurrentPM;
            dict.Add((int)PMIndicators.PMC, si);
            si = new SISetting();
            si.FractPart = 2;
            si.WholePart = 2;
            si.UpString = string.Format(Resources.UV, Resources.Volt);
            si.DownString = string.Format(Resources.MODN, 1);
            dict.Add((int)PMIndicators.MOD1, si);
            si = new SISetting();
            si.FractPart = 2;
            si.WholePart = 1;
            si.UpString = string.Format(Resources.UV, Resources.Volt);
            si.DownString = string.Format(Resources.MODN, 2);
            dict.Add((int)PMIndicators.MOD2, si);
            _siSettings.Add((int)Checks.PM_MOD, dict);
        }

        private void FirstPowerSISettings()
        {
            Dictionary<int, SISetting> dict = new Dictionary<int, SISetting>();
            SISetting si = new SISetting();
            si.FractPart = 2;
            si.WholePart = 3;
            si.UpString = string.Format(Resources.UV, Resources.Volt);
            si.DownString = string.Format(Resources.Positive27V, Resources.Volt);
            dict.Add(0, si);
            si = new SISetting();
            si.FractPart = 2;
            si.WholePart = 3;
            si.UpString = string.Format(Resources.UV, Resources.Volt);
            si.DownString = string.Format(Resources.Alternate115V, Resources.Volt);
            dict.Add(4, si);
            _siSettings.Add((int)Checks.FirstPower, dict);
        }

        private void SecPowerSISettings()
        {
            Dictionary<int, SISetting> dict = new Dictionary<int, SISetting>();
            SISetting si = new SISetting();
            si.FractPart = 2;
            si.WholePart = 3;
            si.UpString = string.Format(Resources.UV, Resources.Volt);
            si.DownString = string.Format(12.6.ToString("+0.0{0}"), Resources.Volt);
            dict.Add((int)SecPowerIndicators.Plus12V, si);
            si = new SISetting();
            si.FractPart = 2;
            si.WholePart = 2;
            si.UpString = string.Format(Resources.UV, Resources.Volt);
            si.DownString = -12.6 + Resources.Volt;
            dict.Add((int)SecPowerIndicators.Minus12V, si);
            si = new SISetting();
            si.FractPart = 1;
            si.WholePart = 2;
            si.UpString = string.Format(Resources.UV, Resources.Volt);
            si.DownString = -20 + Resources.Volt;
            dict.Add((int)SecPowerIndicators.Minus20V, si);
            si = new SISetting();
            si.FractPart = 3;
            si.WholePart = 2;
            si.UpString = string.Format(Resources.UV, Resources.Volt);
            si.DownString = 6.3 + Resources.Volt;
            dict.Add((int)SecPowerIndicators.Plus6V, si);
            si = new SISetting();
            si.FractPart = 3;
            si.WholePart = 1;
            si.UpString = string.Format(Resources.UV, Resources.Volt);
            si.DownString = -6.3 + Resources.Volt;
            dict.Add((int)SecPowerIndicators.Minus6V, si);
            si = new SISetting();
            si.FractPart = 3;
            si.WholePart = 2;
            si.UpString = string.Format(Resources.UV, Resources.Volt);
            si.DownString = 5.ToString("+0") + Resources.Volt;
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
                _algorithm.SetVoltageRegime(AlgorithmBase.VoltageRegime.Standard);
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
                _algorithm.SetVoltageRegime(AlgorithmBase.VoltageRegime.Min);
                rbPlus.FlatStyle = rbNorma.FlatStyle = FlatStyle.Standard;
            }
            gbPower.Enabled = true;
        }

        private void RunWorkTimer()
        {
            _startDissTime = DateTime.Now;
            _tWorkTimer = new Timer();
            _tWorkTimer.Interval = 1000;
            _tWorkTimer.Tick += TWorkTimerOnTick;
            _tWorkTimer.Start();
        }

        private void TWorkTimerOnTick(object sender, EventArgs eventArgs)
        {
            TimeSpan ts = DateTime.Now - _startDissTime;
            CheckFrequencyTime(ts);
            siWorkTime.ValueString = string.Format("{0:00}:{1:00}:{2:00}", ts.Hours, ts.Minutes, ts.Seconds);
        }

        private void CheckFrequencyTime(TimeSpan ts)
        {
            if (ts > AlgorithmBase.TimeForFrequency)
                if (!ivFrequency.Visible)
                    ivFrequency.Visible = true;
        }

        private void rbPlus_CheckedChanged(object sender, EventArgs e)
        {
            gbPower.Enabled = false;
            if (rbPlus.FlatStyle == FlatStyle.Standard)
            {
                rbPlus.FlatStyle = FlatStyle.Flat;
                _algorithm.SetVoltageRegime(AlgorithmBase.VoltageRegime.High);
                rbMinus.FlatStyle = rbNorma.FlatStyle = FlatStyle.Standard;
            }
            gbPower.Enabled = true;
        }

        private void rbOn_CheckedChanged(object sender, EventArgs e)
        {            
            gbMode.Enabled = false;
            _statePower115 = 0;
            try
            {
                if (rbOn.Checked)
                {
                    _powerOff = false;
                    errorPS27Appear = false;
                    cbOneChannelDiss.Enabled = false;
                    rbOn.FlatStyle = FlatStyle.Flat;
                    rbOff.FlatStyle = FlatStyle.Standard;
                    rbOff.Text = Resources.DissCanOff;
                    rbOn.Text = Resources.DissOn;
                    if (tCollectParams.Enabled)
                        tCollectParams.Stop();
                    if (_algorithm == null)
                        _mreEndOfLoad.WaitOne();
                    if (!_algorithm.DissOn3228(true))
                        MessageBox.Show(Resources.Error);
                    else
                    {
                        cbHigh.Enabled = true;
                        EnableAllButtons(bFirstPower, true);
                        EnableAllButtons(bMemory, true);
                        //var logger = LogManager.GetCurrentClassLogger();
                        _logger.Info(string.Format("ИП постоянного тока 27В включен!"));
                        _logger.Info(string.Format("ИП 115В включен!"));
                    }
                    StartCollectTimer();
                    cbHigh.Checked = true;
                    //if (!_mreEndOfLoad.WaitOne(0))
                    //  _tLoad.Join();
                    RunWorkTimer();
                }
            }
            catch(Exception ex)
            {
                _logger.Error(ex.ToString());
                MessageBox.Show(string.Format(Resources.ErrorOnDeviceN, string.Format(Properties.Resources.PowerSupplyN, 115)));
            }
            gbMode.Enabled = true;
        }

        
        private void rbOff_CheckedChanged(object sender, EventArgs e)
        {
            gbMode.Enabled = false;
            if (rbOff.Checked)
            {
                _powerOff = true;
                cbOneChannelDiss.Enabled = true;
                rbOff.FlatStyle = FlatStyle.Flat;
                rbOn.FlatStyle = FlatStyle.Standard;
                rbOff.Text = Resources.DissOff;
                rbOn.Text = Resources.DissCanOn;
                cbHigh.Checked = false;
                ImageDesigner(Checks.Stop);
                if (!_algorithm.DissOn3228(false))
                    MessageBox.Show(Resources.Error);
                if (_statePower115 < 1)
                {
                    //var logger = LogManager.GetCurrentClassLogger();
                    _logger.Info(string.Format("ИП постоянного тока 27В выключен!"));
                    _logger.Info(string.Format("ИП 115В выключен!"));
                    _statePower115++;
                }
                UnpressPressedButton(bFirstPower);
                UnpressPressedButton(bMemory);
                EnableAllButtons(bFirstPower, false);
                EnableAllButtons(bMemory, false);
                _tWorkTimer.Stop();
                ivFrequency.Visible = false;
                cbHigh.Enabled = false;
                siWorkTime.Inactive = true;
            }
            gbMode.Enabled = true;
        }

        private void cbHigh_CheckedChanged(object sender, EventArgs e)
        {
            cbHigh.Enabled = false;
            if (!_algorithm.SetHVState(cbHigh.Checked))
                MessageBox.Show(Resources.Error);
            else
            {
                cbHigh.Text = cbHigh.Checked ? Resources.HVOn : Resources.HVCanOn;
                cbHigh.FlatStyle = cbHigh.Checked ? FlatStyle.Flat : FlatStyle.Standard;
            }
            cbHigh.Enabled = true;
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
            gbTune27V.Enabled = gbTune115V.Enabled = gbPower.Enabled = state;
        }

        private void b115Minus_Click(object sender, EventArgs e)
        {
            EnablePowerControls(false);
            const double shift = -0.25;
            _algorithm.ShiftPower115(shift);
            EnablePowerControls(true);
        }

        private void b115Plus_Click(object sender, EventArgs e)
        {
            EnablePowerControls(false);
            const double shift = 0.25;
            _algorithm.ShiftPower115(shift);
            EnablePowerControls(true);
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
                if ((check == Checks.TK || check == Checks.SD || check == Checks.UNCh) && _oneChannelDiss)
                    ShowChannelButtons(true);
                _actionDelegates[check].Activate();
                //if (check == Checks.ContrBS)
                //  MessageBox.Show(Resources.PressButtonContrBS);
                //if (check == Checks.ContrVCh)
                //  MessageBox.Show(Resources.PressButtonContrVCh);
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

        public void ShowChannelButtons(bool state)
        {
            foreach (RadioButton rb in chButtons)
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
                chButtons[0].PerformClick();
            else
                _channelState = 0;
        }

        private int TKState = 0;

        private void bTK_Click(object sender, EventArgs e)
        {
            Button bCurrent = (Button)sender;
            bCurrent.Enabled = false;
            Checks check = (Checks)Convert.ToInt32(bCurrent.Tag);
            if (bCurrent.FlatStyle == FlatStyle.Standard)
            {
                _buttonActivated = true;
                PressButton(bCurrent);
                ImageDesigner(check);
                if (_oneChannelDiss)
                    ShowChannelButtons(true);
                // выключаем "Высокое"
                if (cbHigh.Checked)
                    cbHigh.Checked = false;
                cbHigh.Enabled = false;
                _algorithm.SetTKState(true);
                if (!_oneChannelDiss)
                {
                    _algorithm.ActivateTKFirst(SetIndicatorLimitsTK1);
                    _renewDelegateList.Add(TKMonitor1);
                }
            }
            else
            {
                if (_oneChannelDiss)
                    TKState = 2;

                if (TKState == 0)
                {
                    TKState++;
                    _algorithm.RememberTKFirst();
                    _algorithm.ResetLimits(SetIndicatorLimitsTK2);
                    _renewDelegateList.Remove(TKMonitor1);
                    ImageDesigner(Checks.TK2);
                    // включаем "Высокое"
                    cbHigh.Checked = true;
                    _renewDelegateList.Add(TKMonitor2);
                }
                else if (TKState == 1)
                {
                    TKState++;
                    _algorithm.Stop();
                    Application.DoEvents();
                    _renewDelegateList.Remove(TKMonitor2);
                    ImageDesigner(Checks.TK3);
                    ShowStaticTKValuesAndSave();
                }
                else
                {
                    _stopCheck = true;
                    _algorithm.Stop();
                    Application.DoEvents();
                    TKState = 0;
                    _algorithm.SetTKState(false);
                    ImageDesigner(Checks.Stop);
                    WrungButton(bCurrent);
                    ShowChannelButtons(false);
                    cbHigh.Enabled = true;
                    if (!cbHigh.Checked)
                    cbHigh.Checked = true;
                    _buttonActivated = false;
                    _algorithm.ResetLimits(SetIndicatorLimitsTK2);
                    _algorithm.ResetLimits(SetIndicatorLimitsTK1);
                }
            }
            _stopCheck = false;
            bCurrent.Enabled = true;
        }

        private void SetIndicatorLimits(Dictionary<AlgorithmBase.Indicators, int> eqDict, int indNum, double min, double max)
        {
            var indicator = eqDict[(AlgorithmBase.Indicators)indNum];
            if (arInd.Length <= indicator)
                return;

            arInd[indicator].MinValue = min;
            arInd[indicator].MaxValue = max;
        }

        private void SetIndicatorLimitsTK1(int indNum, double min, double max)
        {
            SetIndicatorLimits(_TK1Indicators, indNum, min, max);
        }

        private void SetIndicatorLimitsSD(int indNum, double min, double max)
        {
            SetIndicatorLimits(_sdIndicators, indNum, min, max);
        }

        private void SetIndicatorLimitsTK2(int indNum, double min, double max)
        {
            SetIndicatorLimits(_TK2Indicators, indNum, min, max);
        }

        private void ShowStaticTKValuesAndSave()
        {
            CalculateAndSaveTK(AlgorithmBase.Indicators.TK1_1, AlgorithmBase.Indicators.TK1_1_1, AlgorithmBase.TKParams.TK1_1Delta, (int)TKIndicators.TK1_1);
            CalculateAndSaveTK(AlgorithmBase.Indicators.TK2_1, AlgorithmBase.Indicators.TK2_1_1, AlgorithmBase.TKParams.TK2_1Delta, (int)TKIndicators.TK2_1);
            CalculateAndSaveTK(AlgorithmBase.Indicators.TK1_1, AlgorithmBase.Indicators.TK1_2_1, AlgorithmBase.TKParams.TK1_2Delta, (int)TKIndicators.TK1_2);
            CalculateAndSaveTK(AlgorithmBase.Indicators.TK2_2, AlgorithmBase.Indicators.TK2_2_1, AlgorithmBase.TKParams.TK2_2Delta, (int)TKIndicators.TK2_2);
            CalculateAndSaveTK(AlgorithmBase.Indicators.TK1_3, AlgorithmBase.Indicators.TK1_3_1, AlgorithmBase.TKParams.TK1_3Delta, (int)TKIndicators.TK1_3);
            CalculateAndSaveTK(AlgorithmBase.Indicators.TK2_3, AlgorithmBase.Indicators.TK2_3_1, AlgorithmBase.TKParams.TK2_3Delta, (int)TKIndicators.TK2_3);

            CalculateSubstructAndSaveTK((int) TKIndicators.TK1_1, (int) TKIndicators.TK2_1, AlgorithmBase.TKParams.TK1Abs, (int) TKIndicators.TK1_1_2);
            CalculateSubstructAndSaveTK((int)TKIndicators.TK1_2, (int)TKIndicators.TK2_2, AlgorithmBase.TKParams.TK2Abs, (int)TKIndicators.TK1_2_2);
            CalculateSubstructAndSaveTK((int)TKIndicators.TK1_3, (int)TKIndicators.TK2_3, AlgorithmBase.TKParams.TK3Abs, (int)TKIndicators.TK1_3_2);}

        private void CalculateSubstructAndSaveTK(int indicator1, int indicator2, string saveParam, int showIndicator)
        {
            double value = Math.Abs(arInd[indicator1].Value - arInd[indicator2].Value);
            DataManager.Instance.SaveParameter(_testId, saveParam, value);
            double min = 0, max = 0;
            DataManager.Instance.GetParamLimitAndId(saveParam, ref min, ref max);
            arInd[showIndicator].MinValue = min;
            arInd[showIndicator].MaxValue = max;
            arInd[showIndicator].Value = value;
        }

        private void CalculateAndSaveTK(AlgorithmBase.Indicators indicator1, AlgorithmBase.Indicators indicator2, string saveParam, int showIndicator)
        {
            double value = Math.Abs(_algorithm.GetIndicatorValue(indicator1) - _algorithm.GetIndicatorValue(indicator2));
            DataManager.Instance.SaveParameter(_testId, saveParam, value);
            double min = 0, max = 0;
            DataManager.Instance.GetParamLimitAndId(AlgorithmBase.TKParams.TK1_1Delta, ref min, ref max);
            arInd[showIndicator].MinValue = min;
            arInd[showIndicator].MaxValue = max;
            arInd[showIndicator].Value = value;
            SetIndicatorLimit(arInd[showIndicator], min, max);
        }

        private void SetIndicatorLimit(SegmentIndication segmentIndication, double min, double max)
        {
            segmentIndication.MaxValue = max;
            segmentIndication.MinValue = min;
        }

        private void ShowStaticTKValuesAndSave(int numChannel)
        {
            double value, min = 0, max = 0;
            switch (numChannel)
            {
                case 1:
                    value =
                      Math.Abs(_algorithm.GetIndicatorValue(AlgorithmBase.Indicators.TK1_1) -
                               _algorithm.GetIndicatorValue(AlgorithmBase.Indicators.TK1_1_1));
                    DataManager.Instance.SaveParameter(_testId, AlgorithmBase.TKParams.TK1_1Delta, value);
                    DataManager.Instance.GetParamLimitAndId(AlgorithmBase.TKParams.TK1_1Delta, ref min, ref max);
                    SetIndicatorLimit(arInd[(int)TKIndicators.TK1_1], min, max);
                    arInd[(int)TKIndicators.TK1_1].Value = value;
                    value =
                      Math.Abs(_algorithm.GetIndicatorValue(AlgorithmBase.Indicators.TK2_1) -
                               _algorithm.GetIndicatorValue(AlgorithmBase.Indicators.TK2_1_1));
                    DataManager.Instance.SaveParameter(_testId, AlgorithmBase.TKParams.TK2_1Delta, value);
                    DataManager.Instance.GetParamLimitAndId(AlgorithmBase.TKParams.TK2_1Delta, ref min, ref max);
                    SetIndicatorLimit(arInd[(int)TKIndicators.TK2_1], min, max);
                    arInd[(int)TKIndicators.TK2_1].Value = value;

                    value = Math.Abs(arInd[(int)TKIndicators.TK1_1].Value - arInd[(int)TKIndicators.TK2_1].Value);
                    DataManager.Instance.SaveParameter(_testId, AlgorithmBase.TKParams.TK1Abs, value);
                    DataManager.Instance.GetParamLimitAndId(AlgorithmBase.TKParams.TK1Abs, ref min, ref max);
                    SetIndicatorLimit(arInd[(int)TKIndicators.TK1_1_2], min, max);
                    arInd[(int)TKIndicators.TK1_1_2].Value = value;
                    
                    break;

                case 2:
                    value =
                      Math.Abs(_algorithm.GetIndicatorValue(AlgorithmBase.Indicators.TK1_2) -
                               _algorithm.GetIndicatorValue(AlgorithmBase.Indicators.TK1_2_1));
                    DataManager.Instance.SaveParameter(_testId, AlgorithmBase.TKParams.TK1_2Delta, value);
                    DataManager.Instance.GetParamLimitAndId(AlgorithmBase.TKParams.TK1_2Delta, ref min, ref max);
                    SetIndicatorLimit(arInd[(int)TKIndicators.TK1_2], min, max);
                    arInd[(int)TKIndicators.TK1_2].Value = value;
                    value =
                      Math.Abs(_algorithm.GetIndicatorValue(AlgorithmBase.Indicators.TK2_2) -
                               _algorithm.GetIndicatorValue(AlgorithmBase.Indicators.TK2_2_1));
                    DataManager.Instance.SaveParameter(_testId, AlgorithmBase.TKParams.TK2_2Delta, value);
                    DataManager.Instance.GetParamLimitAndId(AlgorithmBase.TKParams.TK2_2Delta, ref min, ref max);
                    SetIndicatorLimit(arInd[(int)TKIndicators.TK2_2], min, max);
                    arInd[(int)TKIndicators.TK2_2].Value = value;

                    value = Math.Abs(arInd[(int)TKIndicators.TK1_2].Value - arInd[(int)TKIndicators.TK2_2].Value);
                    DataManager.Instance.SaveParameter(_testId, AlgorithmBase.TKParams.TK2Abs, value);
                    DataManager.Instance.GetParamLimitAndId(AlgorithmBase.TKParams.TK2Abs, ref min, ref max);
                    SetIndicatorLimit(arInd[(int)TKIndicators.TK1_2_2], min, max);
                    arInd[(int)TKIndicators.TK1_2_2].Value = value;
                    break;
                default:
                    value =
                      Math.Abs(_algorithm.GetIndicatorValue(AlgorithmBase.Indicators.TK1_3) -
                               _algorithm.GetIndicatorValue(AlgorithmBase.Indicators.TK1_3_1));
                    DataManager.Instance.SaveParameter(_testId, AlgorithmBase.TKParams.TK1_3Delta, value);
                    DataManager.Instance.GetParamLimitAndId(AlgorithmBase.TKParams.TK1_3Delta, ref min, ref max);
                    SetIndicatorLimit(arInd[(int)TKIndicators.TK1_3], min, max);
                    arInd[(int)TKIndicators.TK1_3].Value = value;
                    value =
                      Math.Abs(_algorithm.GetIndicatorValue(AlgorithmBase.Indicators.TK2_3) -
                               _algorithm.GetIndicatorValue(AlgorithmBase.Indicators.TK2_3_1));
                    DataManager.Instance.SaveParameter(_testId, AlgorithmBase.TKParams.TK2_3Delta, value);
                    DataManager.Instance.GetParamLimitAndId(AlgorithmBase.TKParams.TK2_3Delta, ref min, ref max);
                    SetIndicatorLimit(arInd[(int)TKIndicators.TK2_3], min, max);
                    arInd[(int)TKIndicators.TK2_3].Value = value;

                    value = Math.Abs(arInd[(int)TKIndicators.TK1_3].Value - arInd[(int)TKIndicators.TK2_3].Value);
                    DataManager.Instance.SaveParameter(_testId, AlgorithmBase.TKParams.TK3Abs, value);
                    DataManager.Instance.GetParamLimitAndId(AlgorithmBase.TKParams.TK3Abs, ref min, ref max);
                    SetIndicatorLimit(arInd[(int)TKIndicators.TK1_3_2], min, max);
                    arInd[(int)TKIndicators.TK1_3_2].Value = value;
                    break;
            }
        }

        Dictionary<AlgorithmBase.Indicators, int> _TK1Indicators = new Dictionary<AlgorithmBase.Indicators, int>();
        Dictionary<AlgorithmBase.Indicators, int> _TK2Indicators = new Dictionary<AlgorithmBase.Indicators, int>();

        public void FillDictionaryTK()
        {
            _TK1Indicators.Add(AlgorithmBase.Indicators.TK1_1, (int)TKIndicators.TK1_1);
            _TK1Indicators.Add(AlgorithmBase.Indicators.TK2_1, (int)TKIndicators.TK2_1);
            _TK1Indicators.Add(AlgorithmBase.Indicators.TK1_2, (int)TKIndicators.TK1_2);
            _TK1Indicators.Add(AlgorithmBase.Indicators.TK2_2, (int)TKIndicators.TK2_2);
            _TK1Indicators.Add(AlgorithmBase.Indicators.TK1_3, (int)TKIndicators.TK1_3);
            _TK1Indicators.Add(AlgorithmBase.Indicators.TK2_3, (int)TKIndicators.TK2_3);

            _TK2Indicators.Add(AlgorithmBase.Indicators.TK1_1, (int)TKIndicators.TK1_1_2);
            _TK2Indicators.Add(AlgorithmBase.Indicators.TK2_1, (int)TKIndicators.TK2_1_2);
            _TK2Indicators.Add(AlgorithmBase.Indicators.TK1_2, (int)TKIndicators.TK1_2_2);
            _TK2Indicators.Add(AlgorithmBase.Indicators.TK2_2, (int)TKIndicators.TK2_2_2);
            _TK2Indicators.Add(AlgorithmBase.Indicators.TK1_3, (int)TKIndicators.TK1_3_2);
            _TK2Indicators.Add(AlgorithmBase.Indicators.TK2_3, (int)TKIndicators.TK2_3_2);
        }

        private void TKMonitor1()
        {
            foreach (var item in _TK1Indicators)
                SetIndicatorFromAlgorithm(item.Value, item.Key);
        }

        private void SetIndicatorFromAlgorithm(int indNum, AlgorithmBase.Indicators indicator)
        {
            double value = _algorithm.GetIndicatorValue(indicator);
            if (!double.IsNaN(value))
                arInd[indNum].Value = value;
        }

        private void TKMonitor2()
        {
            SetIndicatorFromAlgorithm((int)TKIndicators.TK1_1_2, AlgorithmBase.Indicators.TK1_1);
            SetIndicatorFromAlgorithm((int)TKIndicators.TK2_1_2, AlgorithmBase.Indicators.TK2_1);
            SetIndicatorFromAlgorithm((int)TKIndicators.TK1_2_2, AlgorithmBase.Indicators.TK1_2);
            SetIndicatorFromAlgorithm((int)TKIndicators.TK2_2_2, AlgorithmBase.Indicators.TK2_2);
            SetIndicatorFromAlgorithm((int)TKIndicators.TK1_3_2, AlgorithmBase.Indicators.TK1_3);
            SetIndicatorFromAlgorithm((int)TKIndicators.TK2_3_2, AlgorithmBase.Indicators.TK2_3);
        }

        private void bMemory_Click(object sender, EventArgs e)
        {
            bMemory.Enabled = false;
            if (bMemory.FlatStyle == FlatStyle.Standard)
            {
                _buttonActivated = true;
                PressButton(bMemory);
                ImageDesigner(Checks.Memory);
                _algorithm.SetContrBS(true);
                Application.DoEvents();
                Thread.Sleep(1500);
                bool result = true;
                result &= ApplyIndicatorValue(tiDissOn.State, ((Memory3228)_activeControl).cb1);
                result &= ApplyIndicatorValue(tiHigh.State, ((Memory3228)_activeControl).cb2);
                result &= ApplyIndicatorValue(tiFlight.State, ((Memory3228)_activeControl).cb3);
                result &= ApplyIndicatorValue(tiCapture.State, ((Memory3228)_activeControl).cb4);
                result &= ApplyIndicatorValue(tiCapture1.State, ((Memory3228)_activeControl).cb5);
                if (!_oneChannelDiss)
                {
                    result &= ApplyIndicatorValue(tiCapture2.State, ((Memory3228)_activeControl).cb6);
                    result &= ApplyIndicatorValue(tiCapture3.State, ((Memory3228)_activeControl).cb7);
                }
                result &= ApplyIndicatorValue(tiGood.State, ((Memory3228)_activeControl).cb8);
                DataManager.Instance.SaveParameter(_testId, _algorithm.ModeMemoryStr, result);
                // Thread.Sleep(500);
                _algorithm.SetContrBS(false);
                ImageDesigner(Checks.Stop);
                WrungButton(bMemory);
                _buttonActivated = false;
            }
            bMemory.Enabled = true;
        }

        private bool ApplyIndicatorValue(bool value, CheckBox cb)
        {
            cb.Text = value ? Resources.Yes : Resources.No;
            cb.Checked = value;
            Application.DoEvents();
            return value;
        }

        private void ivFrequency_Leave(object sender, EventArgs e)
        {
            if (!double.IsNaN(ivFrequency.Value))
                DataManager.Instance.SaveParameter(_testId, AlgorithmBase.Parameters.UHFFrequency, ivFrequency.Value);
        }

        private void cbOneChannelDiss_CheckedChanged(object sender, EventArgs e)
        {
            _oneChannelDiss = cbOneChannelDiss.Checked;
            tiCapture2.Visible = tiCapture3.Visible = !_oneChannelDiss;
            string note = DataManager.Instance.GetTestingNote(_testId);
            if (String.IsNullOrEmpty(note) || !note.Contains(Resources.OneChannelDiss))
                if (String.IsNullOrEmpty(note))
                    DataManager.Instance.SaveTestingNote(_testId, Resources.OneChannelDiss);
                else
                {
                    note += " " + Resources.OneChannelDiss;
                    DataManager.Instance.SaveTestingNote(_testId, note);
                }
            _algorithm.SetOneChannelDiss(_oneChannelDiss);
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

        #region Implementation of IVisual

        void IVisual.ShowMessage(string pressButtonContrBs)
        {
            ShowMessage(pressButtonContrBs);
        }

        #endregion

        private void frmWork3228_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                if (_algorithm != null)
                    _algorithm.Finish();
                Power115V.Instance.SetOnOff(false);
                if (_statePower115 < 1)
                {
                    //var logger = LogManager.GetCurrentClassLogger();
                    _logger.Info(string.Format("ИП 115В выключен!"));
                    Power27V.Instance.SetOnOff(false);
                    _logger.Info(string.Format("ИП постоянного тока 27В выключен!"));
                    //_lambda27V.OnOffState(false);
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
