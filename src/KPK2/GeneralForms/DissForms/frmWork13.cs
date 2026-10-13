using Hardware.LfGenerator.Akip._3410.Voltage;
using KPK2.Common;
using KPK2.MyClasse;
using KPK2.MyControl;
using KPK2.Properties;
using LCards;
using NLog;
using Sparc.KPK2.Hardware;
using Sparc.KPK2.Hardware.LTR;
using Sparc.KPK2.Hardware.MyClasseUSB;
using Sparc.KPK2.Hardware.PS115V;
using Sparc.KPK2.Hardware.PS27V;
using Sparc.KPK2.Imitator;
using Sparc.KPK2.NFunctions;
using Sparc.KPK2.NFunctions.Enums;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using UserControlsKPK2;
using ZedGraph;
using CommonGlobals = KPK2.Common.Globals;
using Functions = Sparc.KPK2.NFunctions.Functions;
using Globals = Sparc.KPK2.NFunctions.Globals;
using Res = Sparc.KPK2.Resources.Properties.Resources;
using Timer = System.Windows.Forms.Timer;

namespace KPK2.GeneralForms.DissForms
{
    public partial class frmWork13 : Form
    {
        private static Logger _logger = LogManager.GetLogger("KPK2Error");
        private const int PowerSupply115 = 115;
        private const int PowerSupply115Phase = 90;
        private const int PowerSupply27 = 27;
        private const double MinPowerSupply27 = 24.3;
        private const double MaxPowerSupply27 = 29.7;
        private const int MinPowerSupply115 = 109;
        private const int MaxPowerSupply115 = 121;
        private const int OnOffTimes = 50;
        private int curOnOffTime = 0;
        private const double MaxCurrent115 = 5;
        private const double MaxCurrent27 = 2;
        private int testId = -1;

        private TimeSpan _beforeBIControlAndPower = TimeSpan.FromMinutes(3); //Три минуты после включения перед проверкой встроенного контроля

        private DateTime _dtDissOn;
        private DateTime dtStart;
        private DateTime dtStop;
        //private ManualResetEvent mrStopMeasure;
        //таймер ожидания некоторого времени
        private Timer tWaitTimer = new Timer();
        //таймер измерения времени
        private Timer tMeasureTime = new Timer();
        private int timeLeft = 0; //оставшееся время в секундах                
        private Thread threadState;
        private Thread threadState2;
        //таймер обновления результатов
        private Timer tRefreshResults = new Timer();
        //таймер измерения на платах
        //private System.Timers.Timer measTimer = new System.Timers.Timer();

        private PowerStrip strip;
        private Dictionary<int, double> _cellResult = new Dictionary<int, double>();
        private List<Button> _TkButtons = new List<Button>();
        private Measure _measWorker = new Measure();
        private ctrlTK17 TK17Control;
        private int _memoryChannel;
        //Компенсация медианного фильтра
        private double medFiltrKoef = double.NaN;
        private double[] smes1Result = new double[OnOffTimes];
        private double[] smes2Result = new double[OnOffTimes];
        private bool _currentResultSaved;

        private int _statePower115;
        private int _statePower27;

        private event MethodInvoker TimeOut;
        private event MethodInvoker SaveClick;
        private event Action<PowerSupplyType> SupplyTypeChanged;
        private event MethodInvoker PActive;

        private SecPowerDiss13 spControl = null;

        private const int amplF1Cell = 0;
        private const int amplF2Cell = 1;
        private const int amplF3Cell = 2;

        private const int cur1F1Cell = 3;
        private const int cur2F1Cell = 4;
        private const int cur1F2Cell = 5;
        private const int cur2F2Cell = 6;
        private const int cur1F3Cell = 7;
        private const int cur2F3Cell = 8;

        private const int devF1Cell = 9;
        private const int devF2Cell = 10;
        private const int devF3Cell = 11;
        private const int durF1Cell = 12;
        private const int durF2Cell = 13;
        private const int durF3Cell = 14;

        private const int plus60Vst = 15;
        private const int contrHV = 16;
        private const int Minus27st = 17;
        private const int Plus12Vst = 18;
        private const int Minus12Vst = 19;
        private const int Smes1 = 20;
        private const int Smes2 = 21;
        private const int SignalP = 22;

        private enum PowerSupplyType
        {
            High,
            Low,
            Normal
        }

        private const int releVob = 1;
        private const int releAru = 0;
        private const int releGenSVCh = 7;
        private const int rele1Mod = 3;
        private const int rele2Mod = 4;
        private const int releControl = 2;
        private const int rele1ControlTask = 8;
        private const int rele2ControlTask = 9;

        private bool saveResults
        {
            get { return testId != -1; }
        }

        private List<bool> _pState = new List<bool>();

    private bool PState
    {
      get
      {
          return SignalPState;
        //bool result = false;
        //foreach (bool b in _pState)
        //  result |= b;
        //return result;
      }

            set
            {
                _pState.Add(value);
                if (_pState.Count > 8)
                    _pState.RemoveAt(0);
                //if (value)
                if (SignalPState)
                    try
                    {
                        if (PActive != null)
                            PActive();
                    }
                    catch
                    {
                    }
            }
        }

        public frmWork13()
        {
            InitializeComponent();
            tWaitTimer.Interval = 1000;
            tWaitTimer.Tick += tWaitTimer_Tick;
            _measWorker.ResultsReady += ResultsReady;
            _memoryChannel = DataManager.Instance.GetChannelByName(DataManager.DiscretInputs, "Memory_1");
            if (strip == null)
                strip = new PowerStrip(false, testId, ShowStrip, stripError);
        }

        private void MeasureState()
        {
            while (!_monitorStop.WaitOne(0))
            {
                BitArray states = DO176X.Instance.GetRelaysState();
                if (states == null)
                {
                    MessageBox.Show(string.Format(Resources.ErrorOnDeviceN, Resources.DiscretInputOutputsBoard));
                    return;
                }

                RenewStates(states);
                Thread.Sleep(500);
            }
        }

        private void RenewStates(BitArray adv1760)
        {
            if (InvokeRequired)
            {
                BeginInvoke(new Action<BitArray>(RenewStates), adv1760);
                return;
            }
            siVOB.State = !adv1760[releVob];
            siARU.State = !adv1760[releAru];
            siGenSVCh.State = adv1760[releGenSVCh];
            siMOD.State = adv1760[rele1Mod] && adv1760[rele2Mod];
            siControl.State = adv1760[releControl];
            siControlTask.State = adv1760[rele1ControlTask] && adv1760[rele2ControlTask];
        }

        private void stripError(string error)
        {
            StopStripMeasuring();
            MessageBox.Show(error, Resources.ErrorCaption);
        }

        private void ResultsReady(List<USBBaseNew> usblist)
        {
            foreach (USBBaseNew ub in usblist)
            {
                if (ub.OrderedFunctions != null)
                {
                    int i = ub.IndicatorNumber;
                    foreach (Functions.CalcFunction cf in ub.OrderedFunctions)
                    {
                        AddCellResult(i, ub.GetFunctionResult(cf));
                        i++;
                    }
                }
                else
                    AddCellResult(ub.IndicatorNumber, ub.Result);
            }
        }

        private void AddCellResult(int key, double result)
        {
            if (_cellResult.ContainsKey(key))
                _cellResult[key] = result;
            else
                _cellResult.Add(key, result);
        }

        public void ShowStrip(double[] arResult)
        {
            if (InvokeRequired)
            {
                BeginInvoke(new Action<double[]>(ShowStrip), arResult);
                return;
            }
            for (int i = 0; i < 2; i++)
                piStrip.Bars[i].Value = (float)Math.Round(arResult[i], 1);
            if (!Power115V.Instance.IsAktor) //если ИП другой, то плат-нормализаторов нет, значит измеряем с источника
            {
                for (int i = 2; i < 4; i++)
                    piStrip.Bars[i].Value = (float)Math.Round(arResult[i], 1);

            }
        }

        public frmWork13(int id, string devNum = "")
            : this()
        {
            testId = id;
            _deviceNumber = devNum;
        }

        private void bTK9_Click(object sender, EventArgs e)
        {
            ((Button)sender).Enabled = false;
            if (PressButton((Button)sender))
            {
                if (!StopTK9_1())
                    MessageBox.Show(Resources.ErrorCaption);
            }
            else
            {
                try
                {
                    SetTK17();
                    SetControl(false); //Контроль - ВЫКЛ                        
                    SetControlTask(true); //Контрольная задача 2                        
                    MessageBox.Show(string.Format(Res.SwitchNChToPositionN, Res.Task2) + " " + Resources.PressB1);
                    TimeOut += TK9_TimeOut;
                    WaitSomeTime(3);
                    //TK9_TimeOut();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            ((Button)sender).Enabled = true;
        }

        private void TK9_TimeOut()
        {
            TimeOut -= TK9_TimeOut;
            ShowIndicatorControl(true);
            SaveClick += TK9_EndCheck;
            MeasureTK9_1();
            MessageBox.Show(Res.EnterPSAndUSFromIndicator);
        }

        private void ClearAllReference()
        {
            SaveClick = null;
            TimeOut = null;
        }

        private void TK9_EndCheck()
        {
            SaveClick -= TK9_EndCheck;
            if (saveResults)
            {
                DataManager.Instance.SaveParameter(testId, "TK9_W", Functions.MoveToDouble(tbSpeed.Text));
                DataManager.Instance.SaveParameter(testId, "TK9_A", Functions.MoveToDouble(tbAngle.Text));

                DataManager.Instance.SaveParameter(testId, "Ampl_F1", _cellResult[amplF1Cell]);
                DataManager.Instance.SaveParameter(testId, "Ampl_F2", _cellResult[amplF2Cell]);
                DataManager.Instance.SaveParameter(testId, "Ampl_F3", _cellResult[amplF3Cell]);

                DataManager.Instance.SaveParameter(testId, "Dev_F1", siDevF1.Value);
                DataManager.Instance.SaveParameter(testId, "Dev_F2", siDevF2.Value);
                DataManager.Instance.SaveParameter(testId, "Dev_F3", siDevF3.Value);

                DataManager.Instance.SaveParameter(testId, "Dur_F1", siDurF1.Value);
                DataManager.Instance.SaveParameter(testId, "Dur_F2", siDurF2.Value);
                DataManager.Instance.SaveParameter(testId, "Dur_F3", siDurF3.Value);

                DataManager.Instance.SaveParameter(testId, "I_SVCh1_1", _cellResult[cur2F1Cell] * CurrentMeasureKoef);
                DataManager.Instance.SaveParameter(testId, "I_SVCh2_1", _cellResult[cur2F2Cell] * CurrentMeasureKoef);
                DataManager.Instance.SaveParameter(testId, "I_SVCh3_1", _cellResult[cur2F3Cell] * CurrentMeasureKoef);
            }
            bTK9.PerformClick();
        }

        private bool IsItTKButton(Button btn)
        {
            foreach (Button b in _TkButtons)
            {
                if (b.Name == btn.Name)
                    return true;
            }
            return false;
        }

        private void LockOtherTkButtons(Button btn)
        {
            foreach (Button b in _TkButtons)
                if (b.Name != btn.Name)
                    b.Enabled = false;
        }

        private void UnlockTkButtons()
        {
            foreach (Button b in _TkButtons)
                b.Enabled = true;
        }

        private bool PressButton(Button btn)
        {
            if (btn.FlatStyle == FlatStyle.Flat)
            {
                btn.FlatStyle = FlatStyle.Standard;
                btn.BackColor = Color.Transparent;
                if (IsItTKButton(btn))
                    UnlockTkButtons();
                return true;
            }
            btn.FlatStyle = FlatStyle.Flat;
            btn.BackColor = Globals.PressedButton;
            if (IsItTKButton(btn))
                LockOtherTkButtons(btn);
            return false;
        }

        private void ClearMeasurementList()
        {
            _measList.Clear();
        }

        private bool StopTK9_1()
        {
            ClearAllReference();
            _measWorker.SetupLTR11(0, 0, 0);
            StopMeasuring();
            ClearMeasurementList();
            tWaitTimer.Stop();
            tRefreshResults.Stop();
            //StartDiss(false);
            HideControls();
            return true;
        }

        private void HideControls()
        {
            gbTK9.Visible = false;
            gbTimer.Visible = false;
            gbStopWatch.Visible = false;
            gbIndicator.Visible = false;
            gbOnOffTimes.Visible = false;
            DismissStateLabelControl();
            DismissGraphControl();
            if (gbDevice.Visible)
                tbValue.Leave -= CheckPhaseIndex;
            gbDevice.Visible = false;
            bStartForTwoHours.Visible = false;
            gbPS.Visible = false;
            if (TK17Control != null)
            {
                Controls.Remove(TK17Control);
                TK17Control = null;
            }
        }

        private void DismissGraphControl()
        {
            if (_graphControl != null)
            {
                _graphControl.Parent.Controls.Remove(_graphControl);
                _graphControl = null;
            }
        }

        private double CurrentMeasureKoef = 1.3;

        private void MeasureTK9_1()
        {
            AddMeasurement("Ampl_F1", amplF1Cell, Functions.CalcFunction.Amplitude);
            AddMeasurement("Ampl_F2", amplF2Cell, Functions.CalcFunction.Amplitude);
            AddMeasurement("Ampl_F3", amplF3Cell, Functions.CalcFunction.Amplitude);

            _measWorker.SetupLTR11(13, 38500, 1200); //Частота 400Гц, 1200 точек
            AddMeasurement("I_SVCh1_1", cur1F1Cell, Functions.CalcFunction.PairSubtraction);
            AddMeasurement("I_SVCh1_2", cur2F1Cell, Functions.CalcFunction.PairSubtraction);
            AddMeasurement("I_SVCh2_1", cur1F2Cell, Functions.CalcFunction.PairSubtraction);
            AddMeasurement("I_SVCh2_2", cur2F2Cell, Functions.CalcFunction.PairSubtraction);
            AddMeasurement("I_SVCh3_1", cur1F3Cell, Functions.CalcFunction.PairSubtraction);
            AddMeasurement("I_SVCh3_2", cur2F3Cell, Functions.CalcFunction.PairSubtraction);
            StartWorker();

            tRefreshResults.Interval = 2000;
            tRefreshResults.Tick += TK9_Tick;
            tRefreshResults.Start();
            siCurrent1.LightColor = siCurrent2.LightColor = siCurrent3.LightColor = Color.LawnGreen;
            ShowTK9Control(frmReport.ExcludeDiss13DurationAndDeviation);
        }

        private void ShowTK9Control(bool excludeDiss13DurationAndDeviation)
        {
            gbTK9.Visible = true;

            siDevF1.Visible = siDevF2.Visible =
                siDevF3.Visible = siDurF1.Visible = siDurF2.Visible = siDurF3.Visible = !excludeDiss13DurationAndDeviation;
            lDev1Down.Visible = lDev2Down.Visible =
                lDev3Down.Visible = lDur1Down.Visible = lDur2Down.Visible = lDur3Down.Visible = !excludeDiss13DurationAndDeviation;
            lDurMain.Visible = lDevMain.Visible = !excludeDiss13DurationAndDeviation;
        }

        private void TK9_Meas()
        {
            if (double.IsNaN(medFiltrKoef))
            {
                USBBaseNew usbn = new USBBaseNew(testId, "MedFiltrKoef", 0);
                medFiltrKoef = usbn.Koef;
            }

            int KZFirstChannel = 0;
            int KZSecondChannel = 2; // Было 1 
            int KZThirdChannel = 4; // Было 2

            while (!_monitorStop.WaitOne(0))
            {
                List<double> durationResults = new List<double>();
                if (!ControlTask.Instance().GetDuration(ref durationResults))
                {
                    ShowMessage(Res.KZBoardNoResponse);
                    tRefreshResults.Stop();
                    return;
                }
                _cellResult[durF1Cell] = durationResults[KZFirstChannel];
                _cellResult[durF2Cell] = durationResults[KZSecondChannel];
                _cellResult[durF3Cell] = durationResults[KZThirdChannel];

                double[] deviations;
                GetDeviations(out deviations);

                _cellResult[devF1Cell] = deviations[KZFirstChannel];
                _cellResult[devF2Cell] = deviations[KZSecondChannel];
                _cellResult[devF3Cell] = deviations[KZThirdChannel];
            }
        }

        private void TK9_Tick(object sender, EventArgs e)
        {
            try
            {
                siAmplF1.Value = _cellResult[amplF1Cell];
                siAmplF2.Value = _cellResult[amplF2Cell];
                siAmplF3.Value = _cellResult[amplF3Cell];
            }
            catch
            {
            }

            try
            {
                siDurF1.Value = _cellResult[durF1Cell];
                siDurF2.Value = _cellResult[durF2Cell];
                siDurF3.Value = _cellResult[durF3Cell];
            }
            catch
            {
            }

            try
            {

                siDevF1.Value = _cellResult[devF1Cell];
                siDevF2.Value = _cellResult[devF2Cell];
                siDevF3.Value = _cellResult[devF3Cell];
            }
            catch
            {
            }

            try
            {
                siCurrent1.Value = _cellResult[cur2F1Cell] * CurrentMeasureKoef;
                siCurrent2.Value = _cellResult[cur2F2Cell] * CurrentMeasureKoef;
                siCurrent3.Value = _cellResult[cur2F3Cell] * CurrentMeasureKoef;
            }
            catch
            {
            }
        }

        private void GetDeviations(out double[] deviations)
        {
            const int countOfMeasure = 17;
            const int channelCount = 6;
            double[] Tmin = new double[channelCount];
            double[] Tmax = new double[channelCount];
            double Tcurrent = 0;
            double[] channels = new double[channelCount];
            List<List<double>> _fullFreqResult = new List<List<double>>(channelCount);
            for (int i = 0; i < channelCount; i++)
                _fullFreqResult.Add(new List<double>());
            for (int i = 0; i < countOfMeasure; i++)
            {
                List<double> freqResult = new List<double>();
                if (!ControlTask.Instance().GetFrequency(ref freqResult))
                {
                    MessageBox.Show(string.Format(Resources.ErrorOnDevice, Resources.ControlTaskBoard, Resources.GetValue));
                    continue;
                }
                Thread.Sleep(100); //пауза, плата не справляется     
                for (int channel = 0; channel < channelCount; channel++)
                    _fullFreqResult[channel].Add(freqResult[channel]);
                ///LogOpen.WriteLogOpen(string.Format(" Частота: {0}, {1}, {2}, {3}, {4}, {5}", freqResult[0], freqResult[1], freqResult[2], freqResult[3], freqResult[4], freqResult[5]));
            }

            List<double> _freqAverage = new List<double>(channelCount);
            for (int i = 0; i < channelCount; i++)
                _freqAverage.Add(0);

            for (int i = 0; i < channelCount; i++)
            {
                _freqAverage[i] = _fullFreqResult[i].Average();
            }

            for (int i = 0; i < channelCount; i++)
            {
                _fullFreqResult[i].Sort((val1, val2) => Math.Abs(val1 - _freqAverage[i]) > Math.Abs(val2 - _freqAverage[i]) ? -1 : Math.Abs(val1 - _freqAverage[i]) < Math.Abs(val2 - _freqAverage[i]) ? 1 : 0);
            }

            for (int i = 0; i < channelCount; i++)
            {
                for (int c = 0; c < countOfMeasure / 10; c++)
                    _fullFreqResult[i].RemoveAt(0);
            }

            for (int channel = 0; channel < channelCount; channel++)
            {
                for (int i = 0; i < _fullFreqResult[channel].Count; i++)
                {

                    channels[channel] = _fullFreqResult[channel][i];
                    //((Label)Controls.Find("label"+(24+i+channel*5),false)[0]).Text = freqResult[channel*2].ToString();
                    Tcurrent = 1d / _fullFreqResult[channel][i];
                    if ((i == 0) || (Tcurrent < Tmin[channel]))
                        Tmin[channel] = Tcurrent;
                    if ((i == 0) || (Tcurrent > Tmax[channel]))
                        Tmax[channel] = Tcurrent;
                }
            }
            //LogOpen.WriteLogOpen(string.Format(" Частоты: {0}, {1}, {2}", channels[0], channels[1], channels[2]));


            deviations = new double[channelCount];
            for (int channel = 0; channel < channelCount; channel++)
            {

                deviations[channel] =
                  Math.Round((Tmax[channel] - Tmin[channel]) / (Tmax[channel] + Tmin[channel]) * 100 * medFiltrKoef, 1);
                channels[channel] = deviations[channel];

            }
            //var log = LogManager.GetLogger("KPK2Info");
            _logger.Trace(" Девиация: {0}, {1}, {2}, {3}, {4}, {5}", channels[0], channels[1], channels[2], channels[3], channels[4], channels[5]);
        }

        private List<USBBaseNew> _measList = new List<USBBaseNew>();

        private void AddMeasurement(string paramName, int cellNumber)
        {
            if (cellNumber < 0)
                return;
            USBBaseNew ub = new USBBaseNew(testId, paramName, cellNumber);
            _measList.Add(ub);
        }

        private void AddMeasurement(string paramName, int cellNumber, params Functions.CalcFunction[] functions)
        {
            if (cellNumber < 0)
                return;
            USBBaseNew ub = new USBBaseNew(testId, paramName, cellNumber, functions);
            _measList.Add(ub);
        }

        private void StartWorker()
        {
            _measWorker.StopMeasuring(false);
            _measWorker.StartWorkerWithList(_measList);
        }

        private string GetTime(int seconds)
        {
            int min = seconds / 60;
            int sec = seconds - min * 60;
            StringBuilder sb = new StringBuilder();
            sb.Append(min);
            sb.Append(":");
            sb.Append(sec < 10 ? "0" + sec : sec.ToString());
            return sb.ToString();
        }

        ManualResetEvent mreWaitTimerStop = new ManualResetEvent(false);

        private void WaitSomeTime(double minutes)
        {
           
            if (InvokeRequired)
            {
                BeginInvoke(new Action<double>(WaitSomeTime), minutes);
                return;
            }
            mreWaitTimerStop.Reset();
            tWaitTimer.Stop();
            lState.Text = Res.Waiting;
            if (Globals.Emulation)
                timeLeft = (int)(minutes);
            else
                timeLeft = (int)(minutes * 60);

            //if (cbShortTime.Checked)
            //  timeLeft = 5;
            siTimer.ValueString = GetTime(timeLeft);
            tWaitTimer.Start();
            gbTimer.Visible = true;
        }

        private Imitator.Device _device;
        private DevicesID _deviceId = DevicesID.Diss13;
        private Modification _modification = Modification.None;
        private string _deviceNumber;
        private Thread threadStrip;
        private const int AutoMeasureTimeout = 500;
        ManualResetEvent _mreMeasureStrip = new ManualResetEvent(false);


        private void frmWork13_Load(object sender, EventArgs e)
        {

            if (Globals.Emulation)
                _device = new Imitator.Diss13();
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
                _TkButtons.Add(bTK5);
                //_TkButtons.Add(bTK6);
                _TkButtons.Add(bTK7);
                _TkButtons.Add(bTK8);
                _TkButtons.Add(bTK9);
                _TkButtons.Add(bTK11);
                _TkButtons.Add(bTK13);
                _TkButtons.Add(bTK17);
                _TkButtons.Add(bSecPower);
                if (!KPK2.Common.Globals.InitPower27V(PowerSupply27, MaxCurrent27))
                    MessageBox.Show(string.Format(Resources.ErrorOnDeviceN, string.Format(Resources.PowerSupplyN, 40)));
                if (!KPK2.Common.Globals.InitPower115(PowerSupply115Phase, MaxCurrent115))
                    MessageBox.Show(string.Format(Resources.ErrorOnDeviceN,
                                                  string.Format(Resources.PowerSupplyN, 115)));
                else if (Power115V.Instance.GetOutputState())
                    Power115V.Instance.SetVoltage(PowerSupply115);
                if (!Globals.Emulation)
                    if (!LTRManager.CheckCrate())
                        MessageBox.Show(Resources.LTRCrateFailed);
                StartStripMeasuring();
                rbDissOn.Text = Resources.ButtonDissReadyToOn;
                rbDissOff.Text = Resources.ButtonDissOffAlready;
                siAmplF1.LightColor = Color.Lime;
                siAmplF2.LightColor = Color.Lime;
                siAmplF3.LightColor = Color.Lime;
                KPK2.Common.Globals.SetKZBeginParams(true);
                AddMeasurement(Plus_6VParam, SignalP, Functions.CalcFunction.RMS);
                _measWorker.HoldSignals = 1;
                StartWorker();
                //SAConnect = new Task(SpectrumAnalyzerConnect);
                //SAConnect.Start();
                //SetVCh(true);
            }
            catch
            {
                MessageBox.Show(Resources.CommandError + Environment.NewLine + Resources.ApplicationMustBeClosed,
                                Resources.ErrorCaption);
            }
        }

        public void StartStripMeasuring()
        {
            _mreMeasureStrip.Reset();
            threadStrip = new Thread(MeasureStrip);
            threadStrip.IsBackground = true;
            threadStrip.Start();
        }

        private bool errorPS27VAppear = false;

        private void MeasureStrip()
        {
            while (!_mreMeasureStrip.WaitOne(0))
            {
                try
                {
                    strip.Execute(_mreMeasureStrip);
                    if (!errorPS27VAppear)
                        if (Common.Globals.CheckPS27Protection(Power27V.Instance, ShowError))
                            errorPS27VAppear = true;
                        
                }
                catch
                {
                    break;
                }
                _mreMeasureStrip.WaitOne(AutoMeasureTimeout);
            }
        }

        private void ShowError(string text)
        {
            if (InvokeRequired)
            {
                Invoke(new Action<string>(ShowError), text);
                return;
            }

            MessageBox.Show(text, Resources.ErrorCaption);
        }

        public void StopStripMeasuring()
        {
            _mreMeasureStrip.Set();
            Application.DoEvents();
            Thread.Sleep(200);
            if (threadStrip != null)
                if (threadStrip.IsAlive)
                    threadStrip.Abort();
        }

        private string Plus_6VParam = "Plus_6V";

        private bool SignalPState
        {
            get { return _cellResult[SignalP] > 4.5; }
        }

        private void tWaitTimer_Tick(object sender, EventArgs e)
        {
            timeLeft--;
            siTimer.ValueString = GetTime(timeLeft);
            if (timeLeft <= 0 || mreWaitTimerStop.WaitOne(0))
            {
                tWaitTimer.Stop();
                BeginInvoke(TimeOut);
                gbTimer.Visible = false;
                lState.Text = Resources.Done;
            }
        }

        private void SetTK17()
        {
            BitArray reles = new BitArray(10, false);
            reles[releVob] = false; //Вобуляция - ВКЛ
            reles[releAru] = false; //АРУ - ВКЛ
            reles[releGenSVCh] = true; //ГЕН.СВЧ - ВКЛ
            reles[rele1Mod] = true;
            reles[rele2Mod] = true; //МОД - ВКЛ
            reles[releControl] = true; //КОНТРОЛЬ - ВКЛ
            reles[rele1ControlTask] = false;
            reles[rele2ControlTask] = false; //КОНТР. ЗАДАЧА - ВЫКЛ
            if (!DO176X.Instance.ChangeStateAllRelays(reles))
            {
                MessageBox.Show(Res.ReleError);
            }
            //StartDiss(true);
            Thread.Sleep(500); //На установку напряжения
            //CheckVoltage();
            SavePSVoltages();

        }

        private void SavePSVoltages()
        {
            if (saveResults)
            {
                DataManager.Instance.SaveParameter(testId, "Voltage115V", Power115V.Instance.GetVoltage());
                DataManager.Instance.SaveParameter(testId, "Voltage27V", Power27V.Instance.GetVoltage());
                //DataManager.Instance.SaveParameter(testId, "Voltage27V", _lambda27V.GetVoltage());
            }
        }

        private void CheckVoltage()
        {
            double voltage = Power27V.Instance.GetVoltage(); // _lambda27V.GetVoltage()
            if ((voltage < MinPowerSupply27) || (voltage > MaxPowerSupply27))
                throw new Exception(Resources.PowerSupply27VFailed);
            voltage = Power115V.Instance.GetVoltage();
            if ((voltage < MinPowerSupply115) || (voltage > MaxPowerSupply115))
                throw new Exception(Resources.PowerSupply115VFailed);
        }

        private void bStartDiss_Click(object sender, EventArgs e)
        {
            if (PressButton((Button)sender))
            {
                foreach (Button btn in _TkButtons)
                {
                    if (btn.FlatStyle == FlatStyle.Flat)
                        btn.PerformClick();
                }
                //StartDiss(false);
                EnableButtons(false);
            }
            else
            {
                //StartDiss(true);
                EnableButtons(true);
            }
        }

        private void EnableButtons(bool state)
        {
            bTK5.Enabled = state;
            //            bTK6.Enabled = state;
            bTK7.Enabled = state;
            bTK8.Enabled = state;
            bTK9.Enabled = state;
            bTK11.Enabled = state;
            bTK17.Enabled = state;
            bTK13.Enabled = state;
        }

        private void StartDiss(bool state)
        {
            if (InvokeRequired)
            {
                BeginInvoke(new Action<bool>(StartDiss), state);
                return;
            }
            if (state)
                StartMeasureStates();
            else
                StopMeasureStates();

            lState.Text = state ? Resources.TurnOnDiss : Resources.TurnOffDiss;
            Application.DoEvents();
            if (state)
            {
                Power115V.Instance.SetPhase(90);
                if (!Power115V.Instance.SetVoltageGradually(PowerSupply115))
                    MessageBox.Show(string.Format(Resources.ErrorCommand, "Включение питания"), Resources.ErrorCaption);
                //var logger = LogManager.GetCurrentClassLogger();
                _logger.Info(string.Format("ИП 115В включен!"));
            }
            else
            {
                Power115V.Instance.SetOnOff(false);
                ResetTextIndicators();
                if (_statePower115 <1)
                {
                    //var logger = LogManager.GetCurrentClassLogger();
                    _logger.Info(string.Format("ИП 115В выключен!"));
                    _statePower115++;
                }
            }
            if (state)
                errorPS27VAppear = false;
            Power27V.Instance.SetOnOff(state); // _lambda27V.OnOffState(state)
            if (state)
            {
                //var logger = LogManager.GetCurrentClassLogger();
                _logger.Info(string.Format("ИП постоянного тока 27В включен!"));
            }
            else
            {
                if (_statePower27 < 1)
                {
                    //var logger = LogManager.GetCurrentClassLogger();
                    _logger.Info(string.Format("ИП постоянного тока 27В выключен!"));
                    _statePower27++;
                }
            }
            lState.Text = state ? Resources.DissIsOn : Resources.DissIsOff;
            if (state)
                _dtDissOn = DateTime.Now;
        }

        private void StartMeasureStates()
        {
            if (threadState == null)
                threadState = new Thread(MeasureState);
            if (!threadState.IsAlive)
            {
                _monitorStop.Reset();
                threadState = new Thread(MeasureState);
                threadState.Start();
                threadState2 = new Thread(TK9_Meas);
                threadState2.Start();
                DiscreteInputs.Instance().StartMeasuring(DiscreteInputs_Measured);
            }
        }

        private void StopMeasureStates()
        {
            AbortThread(threadState);
            AbortThread(threadState2);
            DiscreteInputs.Instance().StopMeasuring();
        }

        ManualResetEvent _monitorStop = new ManualResetEvent(false);

        private void AbortThread(Thread threadState)
        {
            _monitorStop.Set();
            Thread.Sleep(200);
            if (threadState != null)
                if (threadState.IsAlive)
                    threadState.Abort();
        }


        private void DiscreteInputs_Measured(BitArray states)
        {
            if (InvokeRequired)
            {
                BeginInvoke(new Action<BitArray>(DiscreteInputs_Measured), states);
                return;
            }
            PState = states[_memoryChannel];
            siP.State = PState;
            label43.Text = String.Empty;
            foreach (bool state in states)
                label43.Text += " " + (state ? 1 : 0);
        }

        private void ResetTextIndicators()
        {
            siVOB.State = false;
            siARU.State = false;
            siGenSVCh.State = false;
            siMOD.State = false;
            siControl.State = false;
            siControlTask.State = false;
            _pState.Clear();
            siP.State = false;
        }

        private void bTimer_Click(object sender, EventArgs e)
        {
            bTimer.Enabled = false;
            if (PressButton(bTimer))
            {
                tMeasureTime.Stop();
            }
            else
            {
                if (PState)
                {
                    ShowMessage(Res.CantMeasureTimeWithP);
                    PressButton(bTimer);
                    bTimer.Enabled = true;
                    return;
                }
                dtStart = DateTime.Now;
                dtStop = DateTime.Now;
                DiscreteOutputs.Instance().SetOutput(2, false);
                tMeasureTime.Interval = 200;
                tMeasureTime.Tick += tMeasureTime_Tick;
                PActive += delegate
                  {
                      PActive = null;
                      bTimer.PerformClick();
                      dtStop = DateTime.Now;
                      if (saveResults)
                          DataManager.Instance.SaveParameter(testId, "PTime", siTime.Value);
                      SetIndicatorValue(siTime, Math.Round((DateTime.Now - dtStart).TotalSeconds, 3));
                      ShowMessage("Сигнал П появился, время зафиксировано, проверка завершена!");
                      bTK11.PerformClick();
                  };
                tMeasureTime.Start();
            }
            bTimer.Enabled = true;
        }

        private delegate void SetIndicatorValueDelegate(SegmentIndication si, double value);

        private void SetIndicatorValue(SegmentIndication si, double value)
        {
            if (InvokeRequired)
            {
                Invoke(new SetIndicatorValueDelegate(SetIndicatorValue), si, value);
                return;
            }
            si.Value = value;
        }

        private void tMeasureTime_Tick(object sender, EventArgs e)
        {
            if (!PState)
                siTime.Value = Math.Round((DateTime.Now - dtStart).TotalSeconds, 3);
        }

        private Thread tWork;

        private bool _firstTimeStart;

        private void StartWork()
        {
            try
            {
                KPK2.Common.Globals.SetThreadCulture(Thread.CurrentThread);
                SaveClick = null;
                CreateTK17Control(false);
                bool startFromStop = Power115V.Instance.GetOutputState();
                for (; curOnOffTime < OnOffTimes; curOnOffTime++)
                {
                    int curTime = 4;
                    if ((!startFromStop) || _firstTimeStart)
                    {
                        _firstTimeStart = false;
                        StartDiss(true);
                        siOn.Value = curOnOffTime + 1;
                        _currentResultSaved = false;
                        ShowTK17Control(false);

                        if ((curOnOffTime == 14) || (curOnOffTime == 29) || (curOnOffTime == OnOffTimes - 1))
                        {
                            SetControl(false);
                            SetControlTask(true); //Контрольная задача
                            ShowIndicatorControl(true);
                            ShowMessage(string.Format(Res.NTurnOn, (curOnOffTime + 1)));
                            SaveClick = null;
                            SaveClick += TK13_SpeedSave;
                        }
                        else
                        {
                            SaveClick = null;
                            SaveClick += TK13_Save;
                        }

                        WaitSomeTime(curTime);
                        if (mreStopOnOffWork.WaitOne(0))
                            return;
                        if (Globals.Emulation)
                            mreStopOnOffWork.WaitOne(TimeSpan.FromSeconds(curTime)); //4 секунды во вкл. состоянии
                        else
                            mreStopOnOffWork.WaitOne(TimeSpan.FromMinutes(curTime));  //4 минуты во вкл. состоянии                        

                        while (!_currentResultSaved && !mreStopOnOffWork.WaitOne(0))
                            Thread.Sleep(200);
                    }

                    ShowIndicatorControl(false);
                    if (mreStopOnOffWork.WaitOne(0))
                        return;
                    StartDiss(false);
                    if (curOnOffTime == OnOffTimes - 1)
                        break;
                    curTime = 2;
                    if (mreStopOnOffWork.WaitOne(0))
                        return;
                    WaitSomeTime(curTime);
                    if (Globals.Emulation)
                        mreStopOnOffWork.WaitOne(TimeSpan.FromSeconds(curTime)); //2 секунды в выкл. состоянии
                    else                    
                        mreStopOnOffWork.WaitOne(TimeSpan.FromMinutes(curTime)); //2 минуты в выкл. состоянии                    

                    startFromStop = false;
                    if (mreStopOnOffWork.WaitOne(0))
                        return;
                }

                RemoveTK17Control(SaveFromOutside);
                HideGbOnOffTimes();
                StartDiss(true);
                EndOfRunWork();
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                MessageBox.Show(Resources.Error);
            }
        }

        private void EndOfRunWork()
        {
            if (InvokeRequired)
            {
                BeginInvoke(new MethodInvoker(EndOfRunWork));
                return;
            }
            bTK13.PerformClick();
        }

        private void HideGbOnOffTimes()
        {
            if (InvokeRequired)
            {
                Invoke(new MethodInvoker(HideGbOnOffTimes));
                return;
            }
            gbOnOffTimes.Visible = false;
        }

        private void CreateTK17Control(bool showAll)
        {
            if (InvokeRequired)
            {
                Invoke(new Action<bool>(CreateTK17Control), showAll);
                return;
            }
            TK17Control = new ctrlTK17(showAll);
            Controls.Add(TK17Control);
            TK17Control.Location = showAll
                                     ? gbOnOffTimes.Location
                                     : new Point(gbOnOffTimes.Location.X, gbOnOffTimes.Location.Y + 120);
            TK17Control.SaveClick += SaveFromOutside;
            TK17Control.Visible = false;
        }

        private void TK13_SpeedSave()
        {
            //SaveClick = TK13_Save;
            if (!CheckHighSpeedFields() || !TK17Control.CheckFields())
                return;
            if (saveResults)
            {
                string speedStr = "Speed_15_On";
                string angleStr = "Angle_15_On";
                switch (curOnOffTime)
                {
                    case 29:
                        speedStr = "Speed_30_On";
                        angleStr = "Angle_30_On";
                        break;
                    case (OnOffTimes - 1):
                        speedStr = "Speed_50_On";
                        angleStr = "Angle_50_On";
                        break;
                }
                DataManager.Instance.SaveParameter(testId, speedStr, Convert.ToInt32(tbSpeed.Text));
                DataManager.Instance.SaveParameter(testId, angleStr, Functions.MoveToDouble(tbAngle.Text));
                ShowIndicatorControl(false);
            }
            TK13_Save();
        }

        private void ShowMessage(string text)
        {
            if (InvokeRequired)
            {
                Invoke(new Action<string>(ShowMessage), text);
                return;
            }
            MessageBox.Show(text);
        }

        private void SaveSmesResult()
        {
            USBBaseNew smes1 = new USBBaseNew(testId, "TK17_Smes1", 0);
            USBBaseNew smes2 = new USBBaseNew(testId, "TK17_Smes2", 0);
            DataManager.Instance.SaveParameter(testId, "Smes1_Done", CheckSmesValues(smes1, smes1Result) ? 1 : 0);
            DataManager.Instance.SaveParameter(testId, "Smes2_Done", CheckSmesValues(smes2, smes2Result) ? 1 : 0);
        }

        private static bool CheckSmesValues(USBBaseNew smes, IEnumerable<double> values)
        {
            foreach (double d in values)
                if ((d < smes.Min) || (d > smes.Max))
                    return false;
            return true;
        }

        private void TK13_Save()
        {
            //lock (smes1Result)
            //{
            TK17Control.DisableSave();
            smes1Result[curOnOffTime] = TK17Control.Smes1;
            smes2Result[curOnOffTime] = TK17Control.Smes2;
            _currentResultSaved = true;
            HideTK17Control();

            if (curOnOffTime >= OnOffTimes - 1)
                SaveSmesResult();
            //}

        }

        private void HideTK17Control()
        {
            TK17Control.Visible = false;
        }


        private void RemoveTK17Control(MethodInvoker SaveFunc)
        {
            if (InvokeRequired)
            {
                Invoke(new Action<MethodInvoker>(RemoveTK17Control), SaveFunc);
                return;
            }
            TK17Control.SaveClick -= SaveFunc;
            Controls.Remove(TK17Control);
            TK17Control = null;
        }

        private const string ParamFreqSVCh = "FreqSVCh";
        private const string ParamPowerSVCh = "PowerSVCh";
        private double MeasuredFrequency = -1;

        private void bTK5_Click(object sender, EventArgs e)
        {
            if (PressButton(bTK5))
            {
                try
                {
                    if (fpControl != null)
                    {
                        fpControl.StopSearchPMonitor();
                        MeasuredFrequency = fpControl.Frequency;
                        if (saveResults)
                        {
                            if (!double.IsNaN(fpControl.Temperature))
                                DataManager.Instance.SetAddParam(testId, DataManager.CheckTemperatureParam,
                                    fpControl.Temperature.ToString());
                            if (fpControl.Frequency != -1)
                                DataManager.Instance.SaveParameter(testId, ParamFreqSVCh, fpControl.Frequency);
                            if (fpControl.Power != -1)
                                DataManager.Instance.SaveParameter(testId, ParamPowerSVCh, fpControl.Power * 1000d);
                        }

                        Controls.Remove(fpControl);
                        fpControl = null;
                    }
                }
                catch
                {
                    MessageBox.Show(Resources.Error, Resources.Error);
                }

                ClearAllReference();
                //StartDiss(false);
                HideControls();
            }
            else
            {
                try
                {
                    //MessageBox.Show(string.Format(Resources.ConnectUsingRE, Resources.FrequencyMeter + " и измеритель мощности"));
                    SetTK17();
                    SetMOD(false);
                    //TimeOut += TK5_Timeout;
                    //WaitSomeTime(15);
                    WaitIfEarly(_beforeBIControlAndPower, TK5_Timeout);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void SpectrumAnalyzerConnect()
        {
            try
            {
                if (!CommonSettings.Default.SpectrumAnalyzerActive)
                {
                    lSA_State.Text = "";
                    return;
                }

                if (!SpectrumAnalyzer.IsConnected)
                    SpectrumAnalyzer.Instance.Disconnect();
                SpectrumAnalyzer.Instance.Init();
                if (SpectrumAnalyzer.IsConnected)
                    lSA_State.Text = "Успешное подключение к анализатору спектра";
                else
                    lSA_State.Text = "";
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
            }
            //lSA_State.Text = "Ошибка подключения к анализатору спектра";
        }

        private void TK5_Timeout()
        {
            //TimeOut -= TK5_Timeout;
            ShowFreqAndPowerControl(true);
            //ShowDeviceControl(Resources.Frequency, Resources.Hz);            
            MessageBox.Show(string.Format(Res.FillAllFieldsAndPressN, fpControl.bCalculatePower.Text));
            //SaveClick += TK5_Save;
        }

        private SVCHFreqAndPowerDiss13 fpControl = null;

        private void ShowFreqAndPowerControl(bool state)
        {
            if (state)
            {
                fpControl = new SVCHFreqAndPowerDiss13();
                int xLocation = gbTimer.Location.X + gbTimer.Width / 2 - fpControl.Width / 2;
                fpControl.Location = new Point(xLocation, gbTimer.Location.Y - 10);
                fpControl.SetModChanged += SetMOD;
                //fpControl.Location = gbTimer.Location;
                Controls.Add(fpControl);
            }
        }

        private void bTK7_Click(object sender, EventArgs e)
        {
            ((Button)sender).Enabled = false;
            if (PressButton((Button)sender))
            {
                ClearAllReference();
                StopContinousSpectrumMeas();
                //StartDiss(false);
                HideControls();
                if (SpectrumAnalyzer.IsConnected)
                    SpectrumAnalyzer.Instance.Disconnect();
            }
            else
            {
                try
                {
                    SetTK17();
                    SetVOB(false); //Вобуляция - ВЫКЛ
                    
                    SpectrumAnalyzerConnect();
                    if (!CommonSettings.Default.SpectrumAnalyzerActive)
                        WaitIfEarly(_beforeBIControlAndPower, TK7_Step2);
                    else
                    {
                        double freqMeasured = DataManager.Instance.GetParameterValue(testId, ParamFreqSVCh) * 1e6;
                        if (double.IsNaN(freqMeasured))
                        {
                            if (Math.Abs(MeasuredFrequency - (-1)) <= double.Epsilon)
                            {
                                MessageBox.Show(string.Format(Res.DoFirstNCheck, bTK5.Text));
                                ((Button)sender).Enabled = true;
                                bTK7.PerformClick();
                                return;
                            }
                        }
                        WaitIfEarly(_beforeBIControlAndPower, TK7_StepSA2);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            ((Button)sender).Enabled = true;
        }

        private void TK7_StepSA2()
        {
            const double MainFreqDev = 2.5e6;
            double freqMeasured = DataManager.Instance.GetParameterValue(testId, ParamFreqSVCh) * 1e6;
            if (double.IsNaN(freqMeasured))
            {
                if (Math.Abs(MeasuredFrequency - (-1)) <= double.Epsilon)
                {
                    MessageBox.Show(string.Format(Res.DoFirstNCheck, bTK5.Text));
                    return;
                }

                freqMeasured = KPK2.Common.Globals.ConvertMHzToHz(MeasuredFrequency);
            }
            else
                MeasuredFrequency = KPK2.Common.Globals.ConvertHzToMHz(freqMeasured);

            ShowStateLabelControl();

            SetStateLabelControlMessage(Res.SpectrumAnalyzerConnection);
            Application.DoEvents();

            double[] result = null;
            try
            {
                SpectrumAnalyzer.Instance.Init();
                SpectrumAnalyzer.Instance.Center = freqMeasured;
                SpectrumAnalyzer.Instance.Span = 20e6; //20MHz
                SpectrumAnalyzer.Instance.RefLevel = 20; //20Дбм
                SpectrumAnalyzer.Instance.RBW = 100e3; //100kHz
                SpectrumAnalyzer.Instance.SetCommonParams();
                SetStateLabelControlMessage(Res.Connected);
                Application.DoEvents();
                ShowGraph();
                SpectrumAnalyzer.Instance.SetWorkModeSweep();
                var preResult = CollectSweep(TimeSpan.FromSeconds(2));
                result = preResult[preResult.Count - 1];
                RenewGraphValues(result);
                Application.DoEvents();
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                SetStateLabelControlMessage(Res.ConnectionSpectrumAnalyzerError);
                SpectrumAnalyzer.Instance.Disconnect();
                return;
            }

            if (result != null)
            {
                SetStateLabelControlMessage(Res.CheckSpectrum);
                Application.DoEvents();
                double startFreq = SpectrumAnalyzer.Instance.SweepStartFreq;
                double binSize = SpectrumAnalyzer.Instance.SweepBinSize;
                SpectrumCalculation.PeakResult pResult = SpectrumCalculation.GetPeaksAroundFreq(result, freqMeasured, MainFreqDev, startFreq, binSize);
                if (pResult == null)
                {
                    MessageBox.Show(Res.CenterFreqNotFound);
                    return;
                }

                string innerMessage = string.Empty;
                bool rightData = true;
                if (pResult.MainFreqPlus.Count >= 2)
                {
                    const int minDbm = -50;
                    bool inRes = pResult.MainFreq.Value > minDbm; //Больше -50 Dbm т.е. есть сигнал
                    if (!inRes)
                        innerMessage += string.Format(Res.SignalLevelOnNLessThenNDbm, MeasuredFrequency, minDbm);
                    rightData &= inRes;
                    inRes = pResult.MainFreqPlus[0].Value > pResult.MainFreqPlus[1].Value;
                    if (!inRes)
                        innerMessage += string.Format(Res.PeaksAfterNFollowIncrease, MeasuredFrequency);
                    rightData &= inRes;
                    inRes = pResult.MainFreq.Value < pResult.MainFreqPlus[0].Value;
                    if (!inRes)
                        innerMessage += string.Format(Res.PeakFollowAfterFreqNHaveLessAmpl, MeasuredFrequency);
                    rightData &= inRes;
                }
                else
                {
                    rightData = false;
                    innerMessage = string.Format(Res.PeaksOnFreqNNotFound, MeasuredFrequency);
                }

                if (!rightData)
                {
                    MessageBox.Show(string.Format(Res.SpectrumNotAnalyzableN, innerMessage));
                    StartContinousSpectrumMeas();
                    return;
                }
                SetStateLabelControlMessage(Res.MeasureForCalculation);
                Application.DoEvents();
                List<double[]> fullValues = null;
                try
                {
                    fullValues = CollectSweep(TimeSpan.FromSeconds(14));
                }
                catch (Exception e)
                {
                    _logger.Error(e);
                    MessageBox.Show(Res.SAMeasureError);
                    return;
                }

                double[] minHold = SpectrumCalculation.ToMinArray(fullValues);
                double[] maxHold = SpectrumCalculation.ToMaxArray(fullValues);
                double[] averageHold = SpectrumCalculation.ToAverageArray(minHold, maxHold);
                RenewGraphValues(averageHold);
                pResult = SpectrumCalculation.GetPeaksAroundFreq(averageHold, freqMeasured, MainFreqDev, startFreq, binSize);
                if (pResult == null || pResult.MainFreqPlus.Count < 2 || pResult.MainFreqMinus.Count < 1)
                {
                    string innerMessage2 = string.Empty;
                    if (pResult == null)
                        innerMessage2 = Res.NoMeasures;
                    else
                        if (pResult.MainFreqPlus.Count < 2 || pResult.MainFreqMinus.Count < 1)
                            innerMessage2 = Res.PeaksOnFreqNNotFound;
                    MessageBox.Show(Res.SpectrumNotAnalyzableN, innerMessage2);
                    StartContinousSpectrumMeas();
                    return;
                }

                double m10 = SpectrumCalculation.CalculatePhaseIndex(pResult);
                DismissStateLabelControl();
                ShowDeviceControl(Resources.PhaseIndex, string.Empty);
                tbValue.Text = Math.Round(m10, 2).ToString();
                bManualParamSave.Visible = false;
                tbValue.ReadOnly = true;
                if (saveResults)
                    DataManager.Instance.SaveParameter(testId, PhaseIndexParam, m10);
            }
            else
                SetStateLabelControlMessage(Res.NoDataToAnalyze);
        }

        private Thread _measSpectrum;
        private ManualResetEvent _mreStopSpectrumMeas = new ManualResetEvent(false);

        private void StopContinousSpectrumMeas()
        {
            _mreStopSpectrumMeas.Set();
            if (_measSpectrum != null && _measSpectrum.IsAlive)
            {
                Application.DoEvents();
                _measSpectrum.Join(500);
            }
        }

        private void StartContinousSpectrumMeas()
        {
            _mreStopSpectrumMeas.Reset();
            SetStateLabelControlMessage(Res.ContinousMeas);
            _measSpectrum = new Thread(SpectrumMeas);
            _measSpectrum.IsBackground = true;
            _measSpectrum.Start();
        }

        private void RenewGraphValues(double[] result)
        {
            if (InvokeRequired)
            {
                Invoke(new Action<double[]>(RenewGraphValues), result);
                return;
            }

            if (_graphControl == null)
                return;

            PointPairList pplist = new PointPairList();

            double startFreq = SpectrumAnalyzer.Instance.SweepStartFreq;
            double binSize = SpectrumAnalyzer.Instance.SweepBinSize;
            for (int i = 0; i < result.Length; i++)
                pplist.Add(startFreq + i * binSize, result[i]);
            _graphControl.RenewValues(pplist);
            Application.DoEvents();
        }

        private GraphControl _graphControl;

        private void ShowGraph()
        {
            _graphControl = new GraphControl();
            _graphControl.SpectrumGraphPrepare();
            _graphControl.Location = gbTK9.Location;
            gbDevice.Parent.Controls.Add(_graphControl);
        }

        private void SetStateLabelControlMessage(string message)
        {
            if (_stateControl != null)
                _stateControl.lState.Text = message;
        }

        private List<double[]> CollectSweep(TimeSpan timeSpan)
        {
            List<double[]> result = new List<double[]>();
            DateTime dtStart = DateTime.Now;
            TimeSpan noCollect = TimeSpan.FromSeconds(3); //Время настройки анализатора (без сбора)
            timeSpan += noCollect;
            while (DateTime.Now - dtStart < timeSpan)
            {
                var sweep = SpectrumAnalyzer.Instance.GetSweep();
                if (DateTime.Now - dtStart > noCollect)
                {
                    result.Add(sweep);
                    RenewGraphValues(result[result.Count - 1]);
                }
                else
                    RenewGraphValues(sweep);
            }
            return result;
        }

        private void TK7_Step2()
        {
            //TimeOut -= TK7_Step2;
            ShowDeviceControl(Resources.PhaseIndex, String.Empty);
            MessageBox.Show(Res.MeasureFreqIndex);
            SaveClick += TK7_Save;
        }

        private void ShowDeviceControl(string paramName, string units)
        {
            tbValue.Text = String.Empty;
            lParam.Text = paramName + ":";
            lUnit.Text = units;
            gbDevice.Visible = true;
            tbValue.Leave += CheckPhaseIndex;
        }

        private void CheckPhaseIndex(object sender, EventArgs ea)
        {
            double min = 0;
            double max = 0;
            DataManager.Instance.GetParamLimitAndId(PhaseIndexParam, ref min, ref max);
            TextBox tb = (TextBox)sender;
            double value = Functions.MoveToDouble(tb.Text);
            if (double.IsNaN(value))
            {
                tb.ForeColor = DefaultForeColor;
                return;
            }

            bool goodValue = value >= min && value <= max;
            tb.ForeColor = goodValue ? Color.Green : Color.Red;
        }

        private StateControl _stateControl;

        private void ShowStateLabelControl()
        {
            _stateControl = new StateControl();
            _stateControl.Location = gbDevice.Location;
            gbDevice.Parent.Controls.Add(_stateControl);
        }

        private void DismissStateLabelControl()
        {
            if (_stateControl != null)
            {
                gbDevice.Parent.Controls.Remove(_stateControl);
                _stateControl = null;
            }
        }

        private string PhaseIndexParam = "PhaseIndex";

        private void TK7_Save()
        {
            SaveClick -= TK7_Save;
            if (saveResults)
                DataManager.Instance.SaveParameter(testId, PhaseIndexParam, Functions.MoveToDouble(tbValue.Text));
            bTK7.PerformClick();
        }

        private void bTK8_Click(object sender, EventArgs e)
        {
            ((Button)sender).Enabled = false;
            if (PressButton((Button)sender))
            {
                SupplyTypeChanged -= TK8_SupplyTypeChanged;
                bNormal115.PerformClick();
                ClearAllReference();
                //StartDiss(false);
                HideControls();
                gbPowerManagement.Enabled = false;
            }
            else
            {
                try
                {
                    SetTK17();
                    SetMOD(false);
                    //TimeOut += TK8_Step1;
                    //WaitSomeTime(1.5);
                    TK8_Step1();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            ((Button)sender).Enabled = true;
        }

        private void TK8_Step1()
        {
            //TimeOut -= TK8_Step1;
            gbPowerManagement.Enabled = true;

            SupplyTypeChanged += TK8_SupplyTypeChanged;
            ShowVoltageControl(485);
            gbPS.Text = "Напряжение источника 500 В при установке напряжения 485 В";
            //MessageBox.Show($"HV485= {DataManager.Instance.GetParameterValue(testId, "HV485V").ToString()}");
            if (!double.IsNaN(DataManager.Instance.GetParameterValue(testId, "HV485V"))) 
                nudHigh.Value = Convert.ToDecimal(DataManager.Instance.GetParameterValue(testId, "HV485V"));
            if (!double.IsNaN(DataManager.Instance.GetParameterValue(testId, "LV485V")))
                nudLow.Value = Convert.ToDecimal(DataManager.Instance.GetParameterValue(testId, "LV485V"));
            MessageBox.Show(Res.SetPotentAndFixValues485V);
            SaveClick += TK8_Step2;
        }

        private void TK8_HighVoltage()
        {
            if (InvokeRequired)
            {
                Invoke(new MethodInvoker(TK8_HighVoltage));
                return;
            }
            nudHigh.Enabled = true;
            nudLow.Enabled = false;
            nudNominal.Enabled = false;
            HighVoltagePassed = true;
            UnlockSaveButton();
        }

        private void TK8_LowVoltage()
        {
            if (InvokeRequired)
            {
                Invoke(new MethodInvoker(TK8_LowVoltage));
                return;
            }
            nudHigh.Enabled = false;
            nudLow.Enabled = true;
            nudNominal.Enabled = false;
            LowVoltagePassed = true;
            UnlockSaveButton();
        }

        private void UnlockSaveButton()
        {
            if (LowVoltagePassed && HighVoltagePassed)
                bSaveVoltage.Enabled = true;
        }

        private void TK8_NormaVoltage()
        {
            if (InvokeRequired)
            {
                Invoke(new MethodInvoker(TK8_NormaVoltage));
                return;
            }
            nudLow.Enabled = false;
            nudHigh.Enabled = false;
            nudNominal.Enabled = true;
        }

        private void TK8_Step2()
        {
            SaveClick -= TK8_Step2;
            bNormal115.PerformClick();
            if (saveResults)
            {
                DataManager.Instance.SaveParameter(testId, "HV485V", (double)nudHigh.Value);
                DataManager.Instance.SaveParameter(testId, "LV485V", (double)nudLow.Value);
            }

            ShowVoltageControl(515);
            gbPS.Text = "Напряжение источника 500 В при установке напряжения 515 В";
            if (!double.IsNaN(DataManager.Instance.GetParameterValue(testId, "HV515V")))
                nudHigh.Value = (decimal)DataManager.Instance.GetParameterValue(testId, "HV515V");
            if (!double.IsNaN(DataManager.Instance.GetParameterValue(testId, "LV515V")))
                nudLow.Value = (decimal)DataManager.Instance.GetParameterValue(testId, "LV515V");
            MessageBox.Show(Res.SetPotentAndFixValues515V);
            SaveClick += TK8_Step3;
        }

        private void TK8_SupplyTypeChanged(PowerSupplyType type)
        {
            switch (type)
            {
                case PowerSupplyType.High:
                    TK8_HighVoltage();
                    break;
                case PowerSupplyType.Low:
                    TK8_LowVoltage();
                    break;
                case PowerSupplyType.Normal:
                    TK8_NormaVoltage();
                    break;
            }
        }

        private void TK8_Step3()
        {
            MessageBox.Show(Res.SetPotent505V);
            SaveClick -= TK8_Step3;
            bNormal115.PerformClick();
            if (saveResults)
            {
                DataManager.Instance.SaveParameter(testId, "HV515V", (double)nudHigh.Value);
                DataManager.Instance.SaveParameter(testId, "LV515V", (double)nudLow.Value);
            }
            ShowVoltageControl(400);
            if (DataManager.Instance.GetAddParam(testId, "NV400V") != "")
                nudNominal.Value = decimal.Parse(DataManager.Instance.GetAddParam(testId, "NV400V"));
            if (!double.IsNaN(DataManager.Instance.GetParameterValue(testId, "HV400V")))
                nudHigh.Value = (decimal)DataManager.Instance.GetParameterValue(testId, "HV400V");
            if (!double.IsNaN(DataManager.Instance.GetParameterValue(testId, "LV400V")))
                nudLow.Value = (decimal)DataManager.Instance.GetParameterValue(testId, "LV400V");
            lblNominalVolt.Visible = true;
            nudNominal.Visible = true;
            lblVolt.Visible = true;
            nudNominal.Enabled = true;
            //MessageBox.Show(Res.SetPotent505V);
            MessageBox.Show(string.Format(Res.MeasureVoltageBetweenStandNAndN, "9(+)", "12(-)"));
            SaveClick += TK8_Step4;
        }

        private void TK8_Step4()
        {
            SaveClick -= TK8_Step4;
            bNormal115.PerformClick();
            if (saveResults)
            {
                DataManager.Instance.SaveParameter(testId, "HV400V", (double)nudHigh.Value);
                DataManager.Instance.SaveParameter(testId, "LV400V", (double)nudLow.Value);
                DataManager.Instance.SetAddParam(testId, "NV400V", nudNominal.Value.ToString());
            }
            ShowVoltageControl(6.3, 2);
            if (!double.IsNaN(DataManager.Instance.GetParameterValue(testId, "HV6p3V")))
                nudHigh.Value = (decimal)DataManager.Instance.GetParameterValue(testId, "HV6p3V");
            if (!double.IsNaN(DataManager.Instance.GetParameterValue(testId, "LV6p3V")))
                nudLow.Value = (decimal)DataManager.Instance.GetParameterValue(testId, "LV6p3V");
            lblNominalVolt.Visible = false;
            nudNominal.Visible = false;
            lblVolt.Visible = false;
            nudNominal.Enabled = false;
            MessageBox.Show(string.Format(Res.MeasureVoltageBetweenStandNAndNfor6V, "9(-)", "11(+)"));
            SaveClick += TK8_End;
        }

        private void TK8_End()
        {
            SaveClick -= TK8_End;
            if (saveResults)
            {
                DataManager.Instance.SaveParameter(testId, "HV6p3V", (double)nudHigh.Value);
                DataManager.Instance.SaveParameter(testId, "LV6p3V", (double)nudLow.Value);
            }
            bTK8.PerformClick();
        }

        private bool HighVoltagePassed = false, LowVoltagePassed = false;

        private void ShowVoltageControl(double voltage)
        {
            gbPS.Text = string.Format(Res.ByVoltageN, voltage);
            nudHigh.Enabled = false;
            nudLow.Enabled = false;
            nudHigh.Value = (decimal)voltage;
            nudLow.Value = (decimal)voltage;
            bSaveVoltage.Enabled = false;
            HighVoltagePassed = LowVoltagePassed = false;
            gbPS.Visible = true;
        }

        private void ShowVoltageControl(double voltage, int decimalPlaces)
        {
            nudHigh.DecimalPlaces = decimalPlaces;
            nudHigh.Increment = (decimal)1 / (10 * decimalPlaces);
            nudLow.DecimalPlaces = decimalPlaces;
            nudLow.Increment = (decimal)1 / (10 * decimalPlaces);
            ShowVoltageControl(voltage);
        }

        //МОД - ВКЛ/ВЫКЛ
        private static void SetMOD(bool state)
        {
            bool result = true;
            result &= DO176X.Instance.SetOutput(rele1Mod, state);
            result &= DO176X.Instance.SetOutput(rele2Mod, state);
            if (!result)
                throw new Exception("Неудалось переключить реле на плате Advantech");
        }

        //Вобуляция - ВКЛ/ВЫКЛ
        private static void SetVOB(bool state)
        {
            if (!DO176X.Instance.SetOutput(releVob, !state))
                throw new Exception("Неудалось переключить реле на плате Advantech");
        }

        //Контроль ВКЛ/ВЫКЛ
        private static void SetControl(bool state)
        {
            if (!DO176X.Instance.SetOutput(releControl, state))
                throw new Exception("Неудалось переключить реле на плате Advantech");
        }

        private static void SetControlTask(bool state)
        {
            bool result = true;
            result &= DO176X.Instance.SetOutput(rele1ControlTask, state);
            result &= DO176X.Instance.SetOutput(rele2ControlTask, state); //Контрольная задача 2 
            if (!result)
                throw new Exception("Неудалось переключить реле на плате Advantech");
        }

        private void bMaxVoltage_Click(object sender, EventArgs e)
        {
            ((Button)sender).Enabled = false;
            if (!Power115V.Instance.SetVoltage(MaxPowerSupply115))
                MessageBox.Show(string.Format(Resources.ErrorOnDeviceN, string.Format(Resources.PowerSupplyN, 115)));
            else
                PowerButtonPressed((Button)sender);
            SupplyTypeChanged.BeginInvoke(PowerSupplyType.High, null, null);
            ((Button)sender).Enabled = true;
        }

        private void bMinVoltage115_Click(object sender, EventArgs e)
        {
            ((Button)sender).Enabled = false;
            if (!Power115V.Instance.SetVoltage(MinPowerSupply115))
                MessageBox.Show(string.Format(Resources.ErrorOnDeviceN, string.Format(Resources.PowerSupplyN, 115)));
            else
                PowerButtonPressed((Button)sender);
            if (SupplyTypeChanged != null)
                SupplyTypeChanged.BeginInvoke(PowerSupplyType.Low, null, null);
            ((Button)sender).Enabled = true;
        }

        private void PowerButtonPressed(Button button)
        {
            bMinVoltage115.FlatStyle = FlatStyle.Standard;
            bMinVoltage115.BackColor = Color.Transparent;
            bMaxVoltage115.FlatStyle = FlatStyle.Standard;
            bMaxVoltage115.BackColor = Color.Transparent;
            bNormal115.FlatStyle = FlatStyle.Standard;
            bNormal115.BackColor = Color.Transparent;

            button.FlatStyle = FlatStyle.Flat;
            button.BackColor = Globals.PressedButton;
        }

        private void bNormal115_Click(object sender, EventArgs e)
        {
            ((Button)sender).Enabled = false;
            if (!Power115V.Instance.SetVoltage(PowerSupply115))
                MessageBox.Show(string.Format(Resources.ErrorOnDeviceN, string.Format(Resources.PowerSupplyN, 115)));
            else
                PowerButtonPressed((Button)sender);
            if (SupplyTypeChanged != null)
                SupplyTypeChanged.BeginInvoke(PowerSupplyType.Normal, null, null);
            ((Button)sender).Enabled = true;
        }

        private void bTK11_Click(object sender, EventArgs e)
        {
            ((Button)sender).Enabled = false;
            if (PressButton((Button)sender))
            {
                ClearAllReference();
                //StartDiss(false);
                HideControls();
                PActive = null;
            }
            else
            {
                try
                {
                    SetTK17();
                    MessageBox.Show(string.Format(Res.SwitchNChToPositionN, Res.Off) + " " + Res.SwitchKROnIndicatorToK);
                    SetControl(false); //Контроль - ВЫКЛ                    
                    ShowStopWatch();
                    DialogResult dr = MessageBox.Show(Res.WaitWhileDissCapture + " " + Res.IsPOn, Resources.Question, MessageBoxButtons.YesNo);
                    if (saveResults)
                        DataManager.Instance.SaveParameter(testId, "PLightCatch", dr == DialogResult.Yes ? 0 : 1);
                    if (dr == DialogResult.Yes)
                    {
                        MessageBox.Show(Res.DissNotInCaptureMode + " " + Res.CantContinueCheck);
                        bTK11.Enabled = true;
                        bTK11.PerformClick();
                        return;
                    }
                    MessageBox.Show(string.Format(Res.WaitWhileTaskNCompleted, 1) + " " + Res.SwitchKROnIndicatorToR + " " + string.Format(Res.ImmidiatelyTurnStopwatchByButtonN, bTimer.Text));
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            ((Button)sender).Enabled = true;
        }

        private void ShowStopWatch()
        {
            siTime.Value = 0;
            siTime.LightColor = Color.LawnGreen;
            gbStopWatch.Visible = true;
        }

        private void bTK17_Click(object sender, EventArgs e)
        {
            ((Button)sender).Enabled = false;
            if (PressButton((Button)sender))
            {
                ClearAllReference();
                //StartDiss(false);
                HideControls();
            }
            else
            {
                try
                {
                    SetTK17();
                    //WaitSomeTime(3);
                    //TimeOut += TK17_Step2;
                    TK17_Step2();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            ((Button)sender).Enabled = true;
        }

        private void TK17_Step2()
        {
            WaitIfEarly(_beforeBIControlAndPower, TK17_Step2_1);
        }

        private void TK17_Step2_1()
        {
            SaveClick += TK17_Step3;
            CreateTK17Control(true);
            ShowTK17Control(true);
            if (!double.IsNaN(_currentTK17Params.Smes1))
            {
                TK17Control.SetValues(_currentTK17Params.Smes1, _currentTK17Params.Smes2, _currentTK17Params.Geter,
                                      _currentTK17Params.UnChOut, _currentTK17Params.Klistr);
            }
            MessageBox.Show(Res.FixAllNChParams);
        }

        private void WaitIfEarly(TimeSpan needTime, MethodInvoker timeOut = null)
        {
            TimeSpan timeAfterOn = DateTime.Now - _dtDissOn;
            if (timeAfterOn < needTime)
            {
                if (timeOut != null)
                    TimeOut = timeOut;
                WaitSomeTime((needTime - timeAfterOn).TotalMinutes);
            }
            else if (timeOut != null)
                BeginInvoke(timeOut);            
        }

        private void ShowTK17Control(bool showAll)
        {
            if (InvokeRequired)
            {
                Invoke(new Action<bool>(ShowTK17Control), showAll);
                return;
            }
            if (TK17Control == null)
                return;
            TK17Control.Clear();
            TK17Control.Visible = true;
            TK17Control.SetFocus();
        }

        private void SaveFromOutside()
        {
            if (SaveClick != null)
                SaveClick();
        }

        public class TK17Params
        {
            public double Smes1 = double.NaN;
            public double Smes2 = double.NaN;
            public double Geter = double.NaN;
            public double UnChOut = double.NaN;
            public double Klistr = double.NaN;
        }

        TK17Params _currentTK17Params = new TK17Params();

        private void TK17_Step3()
        {
            SaveClick -= TK17_Step3;
            if (saveResults)
            {
                _currentTK17Params.Smes1 = TK17Control.Smes1;
                _currentTK17Params.Smes2 = TK17Control.Smes2;
                _currentTK17Params.Geter = TK17Control.Geter;
                _currentTK17Params.UnChOut = TK17Control.UNChOut;
                _currentTK17Params.Klistr = TK17Control.Clistr ?? -1;
                DataManager.Instance.SaveParameter(testId, "TK17_Smes1", TK17Control.Smes1);
                DataManager.Instance.SaveParameter(testId, "TK17_Smes2", TK17Control.Smes2);
                DataManager.Instance.SaveParameter(testId, "TK17_Geter", TK17Control.Geter);
                DataManager.Instance.SaveParameter(testId, "TK17_UNChOut", TK17Control.UNChOut);
                DataManager.Instance.SaveParameter(testId, "TK17_Klistr", TK17Control.Clistr ?? -1);
            }
            Controls.Remove(TK17Control);
            SetControl(false);
            SetControlTask(true);
            MessageBox.Show(Res.TK17Command);
            WaitSomeTime(3);
            TimeOut = TK17_Step4;
        }

        private void TK17_Step4()
        {
            //TimeOut -= TK17_Step4;
            ShowIndicatorControl(true);
            if (!double.IsNaN(_tk17A2))
            {
                tbSpeed.Text = _tk17W2.ToString();
                tbAngle.Text = _tk17A2.ToString();
            }
            MessageBox.Show(Res.EnterPSAndUSFromIndicator);
            SaveClick += TK17_Step5;
        }

        private int _tk17W2;
        private double _tk17A2 = double.NaN;

        private void TK17_Step5()
        {
            SaveClick -= TK17_Step5;
            if (saveResults)
            {
                _tk17W2 = Convert.ToInt32(tbSpeed.Text);
                _tk17A2 = Functions.MoveToDouble(tbAngle.Text);
                DataManager.Instance.SaveParameter(testId, "TK17_W2", Convert.ToInt32(tbSpeed.Text));
                DataManager.Instance.SaveParameter(testId, "TK17_A2", _tk17A2);
            }
            ShowIndicatorControl(false);
            MessageBox.Show(Res.TK17_2Command);
            WaitSomeTime(3);
            TimeOut = TK17_Step5_1;
        }

        private void TK17_Step5_1()
        {
            TimeOut = null;
            ShowIndicatorControl(true, true);
            if (_tk17W2M != -1)
                tbSpeed.Text = _tk17W2M.ToString();
            MessageBox.Show(Res.EnterPSFromIndicator);
            SaveClick += TK17_Step6;
        }

        private int _tk17W2M = -1;
        //private double _tk17A2M = double.NaN;

        private void TK17_Step6()
        {
            SaveClick -= TK17_Step6;
            if (saveResults)
            {
                _tk17W2M = Convert.ToInt32(tbSpeed.Text);
                double percents = (_tk17W2M - _tk17W2) / (double)_tk17W2 * 100d;
                //_tk17A2M = Convert.ToDouble(tbAngle.Text);
                DataManager.Instance.SaveParameter(testId, "TK17_W2M", percents);
                //DataManager.Instance.SaveParameter(testId, "TK17_A2M", Convert.ToDouble(tbAngle.Text));
            }
            SetControlTask(false);
            ShowIndicatorControl(false);
            MessageBox.Show(Res.TK17_3Command);
            WaitSomeTime(3);
            TimeOut = TK17_Step6_1;
        }

        private void TK17_Step6_1()
        {
            TimeOut = null;
            ShowIndicatorControl(true);
            if (!double.IsNaN(_tk17A1))
            {
                tbSpeed.Text = _tk17W1.ToString();
                tbAngle.Text = _tk17A1.ToString();
            }
            MessageBox.Show(Res.EnterPSAndUSFromIndicator);
            SaveClick += TK17_Step7;
        }

        private int _tk17W1;
        private double _tk17A1 = double.NaN;

        private void TK17_Step7()
        {
            SaveClick -= TK17_Step7;
            if (saveResults)
            {
                _tk17W1 = Convert.ToInt32(tbSpeed.Text);
                _tk17A1 = Functions.MoveToDouble(tbAngle.Text);
                DataManager.Instance.SaveParameter(testId, "TK17_W1", Convert.ToInt32(tbSpeed.Text));
                DataManager.Instance.SaveParameter(testId, "TK17_A1", _tk17A1);
            }
            ShowIndicatorControl(false);
            DialogResult dResult = MessageBox.Show(string.Format(Res.AskPositionOnNchN, Res.Task3), Resources.Question,
              MessageBoxButtons.YesNoCancel);
            if (dResult == DialogResult.Yes)
            {
                MessageBox.Show(string.Format(Res.SwitchNChToPositionN, Res.Task3));
                WaitSomeTime(3);
                TimeOut = TK17_Step7_1;
            }
            else if (dResult == DialogResult.No)
                TK17_Step8_1();
            else
                bTK17.PerformClick();

        }

        private void TK17_Step7_1()
        {
            TimeOut = null;
            ShowIndicatorControl(true);
            if (!double.IsNaN(_tk17A3))
            {
                tbSpeed.Text = _tk17W3.ToString();
                tbAngle.Text = _tk17A3.ToString();
            }
            MessageBox.Show(Res.EnterPSAndUSFromIndicator);
            SaveClick += TK17_Step8;
        }

        private int _tk17W3;
        private double _tk17A3 = double.NaN;

        private void TK17_Step8()
        {
            SaveClick -= TK17_Step8;
            if (saveResults)
            {
                _tk17W3 = Convert.ToInt32(tbSpeed.Text);
                _tk17A3 = Functions.MoveToDouble(tbAngle.Text);
                DataManager.Instance.SaveParameter(testId, "TK17_W3", Convert.ToInt32(tbSpeed.Text));
                DataManager.Instance.SaveParameter(testId, "TK17_A3", Functions.MoveToDouble(tbAngle.Text));
            }
            TK17_Step8_1();
        }

        private void TK17_Step8_1()
        {
            MessageBox.Show(Res.Task17_4Command);
            DialogResult dr = MessageBox.Show(Res.IsPOn, Resources.Question, MessageBoxButtons.YesNo);
            if (saveResults)
                DataManager.Instance.SaveParameter(testId, "TK17_PLight", dr == DialogResult.Yes ? 1 : 0);
            bTK17.PerformClick();
        }

        private void bSaveSpeed_Click(object sender, EventArgs e)
        {
            ((Button)sender).Enabled = false;
            if (CheckHighSpeedFields())
                if (SaveClick != null)
                    SaveClick();
            ((Button)sender).Enabled = true;
        }

        private bool CheckHighSpeedFields()
        {
            return CheckIntTextBox(tbSpeed) && (!tbAngle.Visible || CheckDoubleTextBox(tbAngle));
        }

        private bool CheckIntTextBox(TextBox tb)
        {
            int result;
            if (!int.TryParse(tb.Text, out result))
            {
                ep.SetError(tb, string.Format(Resources.ErrorInputNValue, tb.Text));
                tb.Focus();
                return false;
            }
            else
                ep.SetError(tb, null);
            return true;
        }

        private bool CheckDoubleTextBox(TextBox tb)
        {
            double result;
            result = Functions.MoveToDouble(tb.Text);
            if (double.IsNaN(result))
            {
                ep.SetError(tb, string.Format(Resources.ErrorInputNValue, tb.Text));
                tb.Focus();
                return false;
            }
            else
                ep.SetError(tb, null);
            return true;
        }

        private void bStartForTwoHours_Click(object sender, EventArgs e)
        {
            if (PressButton((Button)sender))
            {
                tWaitTimer.Stop();
                ClearAllReference();
                //StartDiss(false);
                HideControls();
            }
            else
            {
                //StartDiss(true);
                TimeOut += TwoHoursTimeout;
                WaitSomeTime(120);
            }
        }

        private void TwoHoursTimeout()
        {
            TimeOut -= TwoHoursTimeout;
            bStartForTwoHours.PerformClick();
        }

        private void bTK13_Click(object sender, EventArgs e)
        {
            bTK13.Enabled = false;
            if (PressButton(bTK13))
            {
                mreStopOnOffWork.Set();
                for (int i = 0; i < 20; i++)
                {
                    Thread.Sleep(500);
                    Application.DoEvents();
                    if (!tWork.IsAlive)
                        break;
                }
                if (tWork != null)
                    if (tWork.IsAlive)
                        tWork.Abort();
                ClearAllReference();
                HideControls();
                lState.Text = Resources.Ready;
            }
            else
            {
                try
                {
                    SetTK17();
                    _firstTimeStart = true;
                    gbOnOffTimes.Visible = true;
                    curOnOffTime = 0;
                    RunOnOffWork();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            bTK13.Enabled = true;
        }

        ManualResetEvent mreStopOnOffWork = new ManualResetEvent(false);

        private void RunOnOffWork()
        {
            mreStopOnOffWork.Reset();
            tWork = new Thread(StartWork);
            tWork.IsBackground = true;
            tWork.Start();
        }

        private void bManualParamSave_Click(object sender, EventArgs e)
        {
            ((Button)sender).Enabled = false;
            double result = Functions.MoveToDouble(tbValue.Text);
            if (Double.IsNaN(result))
            {
                MessageBox.Show(string.Format(Resources.ErrorInputNValue, tbValue.Text));
                tbValue.Focus();
            }
            if (SaveClick != null)
                SaveClick();
            ((Button)sender).Enabled = true;
        }

        private delegate void ShowIndicatorControlDelegate(bool state, bool psOnly);

        private void ShowIndicatorControl(bool state, bool psOnly = false)
        {
            if (InvokeRequired)
            {
                Invoke(new ShowIndicatorControlDelegate(ShowIndicatorControl), state, psOnly);
                return;
            }
            tbSpeed.Text = "";
            tbAngle.Text = "";
            gbIndicator.Visible = state;
            tbAngle.Visible = lAngle.Visible = !psOnly;
            bSaveSpeed.Enabled = true;
        }

        private void frmWork13_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                if (threadState != null)
                    if (threadState.IsAlive)
                        threadState.Abort();
                StopStripMeasuring();
                if (ParentForm != null)
                    ((frmMain) ParentForm).EnableMenu(true);
                StopMeasureStates();
                KPK2.Common.Globals.SetKZBeginParams(false);
                StartDiss(false);
                _measWorker.StopMeasuring(false);
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                MessageBox.Show(Resources.CloseFormError);
            }
        }

        private void bSaveVoltage_Click(object sender, EventArgs e)
        {
            ((Button)sender).Enabled = false;
            if (SaveClick != null)
                SaveClick();
            //((Button)sender).Enabled = true;
        }

        private void rbDissOn_CheckedChanged(object sender, EventArgs e)
        {
            _statePower115 = 0;
            _statePower27 = 0;
            if (rbDissOn.FlatStyle != FlatStyle.Flat)
            {
                rbDissOn.FlatStyle = FlatStyle.Flat;
                rbDissOn.BackColor = Globals.PressedButton;
                SetTK17();
                StartDiss(true);
                UnlockTkButtons();
                rbDissOff.Text = Resources.ButtonDissReadyToOff;
                rbDissOn.Text = Resources.ButtonDissOnAlready;
                rbDissOff.FlatStyle = FlatStyle.Standard;
                rbDissOff.BackColor = Color.Transparent;

            }
        }

        private void rbDissOff_CheckedChanged(object sender, EventArgs e)
        {
            if (rbDissOff.FlatStyle != FlatStyle.Flat)
            {
                rbDissOff.FlatStyle = FlatStyle.Flat;
                rbDissOff.BackColor = Globals.PressedButton;
                StartDiss(false);
                UnpressTkButtons();
                LockTkButtons();
                rbDissOff.Text = Resources.ButtonDissOffAlready;
                rbDissOn.Text = Resources.ButtonDissReadyToOn;
                rbDissOn.FlatStyle = FlatStyle.Standard;
                rbDissOn.BackColor = Color.Transparent;
            }
        }

        private void LockTkButtons()
        {
            foreach (Button b in _TkButtons)
                b.Enabled = false;
        }

        private void UnpressTkButtons()
        {
            foreach (Button b in _TkButtons)
                if (b.FlatStyle == FlatStyle.Flat)
                {
                    b.PerformClick();
                    return;
                }
        }

        private void tbValue_Leave(object sender, EventArgs e)
        {
            if (!tbValue.Visible)
                return;
            double result;
            tbValue.Text = tbValue.Text.Trim();
            result = Functions.MoveToDouble(tbValue.Text);
            if (double.IsNaN(result))
                MessageBox.Show(string.Format(Resources.ErrorInputNValue, tbValue.Text));
            tbValue.Focus();
        }

        private void bSecPower_Click(object sender, EventArgs e)
        {
            if (PressButton(bSecPower))
            {
                StopMeasuring();
                if (spControl != null)
                {
                    Controls.Remove(spControl);
                    spControl = null;
                }
            }
            else
            {
                try
                {
                    SetTK17();
                    ShowSecPowerControl();
                    MeasureSecPower();
                    tRefreshResults.Interval = 2000;
                    tRefreshResults.Tick += SecPower_Tick;
                    tRefreshResults.Start();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void StopMeasuring()
        {
            _measWorker.StopMeasuring(false);
            _measWorker.StartMeasuring();
        }

        private void SecPower_Tick(object sender, EventArgs e)
        {
            if (spControl == null)
                return;
            try
            {
                spControl.si60V.Value = _cellResult[plus60Vst];
                //spControl.siContrHV.Value = _cellResult[contrHV];
                spControl.siMinus27.Value = _cellResult[Minus27st];
                spControl.siPlus12.Value = _cellResult[Plus12Vst];
                spControl.siMinus12.Value = _cellResult[Minus12Vst];
                //spControl.siSmes1.Value = _cellResult[Smes1];
                //spControl.siSmes2.Value = _cellResult[Smes2];
            }
            catch
            {
            }
        }

        private void MeasureSecPower()
        {
            AddMeasurement("Plus60Vst", plus60Vst);
            //AddMeasurement("ContrHV", contrHV);
            AddMeasurement("Minus27Vst", Minus27st);
            AddMeasurement("Plus12Vst", Plus12Vst);
            AddMeasurement("Minus12Vst", Minus12Vst);
            //AddMeasurement("Smes1", Smes1);
            //AddMeasurement("Smes2", Smes2);
            StartWorker();
        }

        private void ShowSecPowerControl()
        {
            spControl = new SecPowerDiss13();
            Controls.Add(spControl);
            spControl.Location = gbTK9.Location;
        }

        private void bWaitTimerStop_Click(object sender, EventArgs e)
        {
            mreWaitTimerStop.Set();
        }

        private void nudNominal_TextChanged(object sender, EventArgs e)
        {
            decimal nominalVoltage = nudNominal.Value;
            if (nominalVoltage < 100 || nominalVoltage > 500)
            {
                MessageBox.Show("ОШИБКА ВВОДА ДАННЫХ!\nЗначение напряжения должно быть в диапазоне 100...500 В!");
                nudNominal.Focus();
            }
        }

        private void gbPS_Enter(object sender, EventArgs e)
        {

        }

        public void SpectrumMeas()
        {
            while (!_mreStopSpectrumMeas.WaitOne(0))
            {
                double[] result = SpectrumAnalyzer.Instance.GetSweep();
                RenewGraphValues(result);
            }
        }

    }
}