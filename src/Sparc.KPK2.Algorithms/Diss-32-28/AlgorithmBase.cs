using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Sparc.KPK2.Hardware;
using Sparc.KPK2.Hardware.LTR;
using Sparc.KPK2.Hardware.MyClasseUSB;
using Sparc.KPK2.Hardware.PS115V;
using Sparc.KPK2.Hardware.PS27V;
using Sparc.KPK2.NFunctions;

namespace Sparc.KPK2.Algorithms
{
    public class AlgorithmBase
    {
        #region Переменные

        protected int _testId;

        public static TimeSpan TimeForFrequency = new TimeSpan(0, 0, 3, 0);

        public static string CorrectionCodeParam1 = "CorrectionCode1";
        public static string CorrectionCodeParam2 = "CorrectionCode2";
        public static string CorrectionCodeParam3 = "CorrectionCode3";

        BitArray _indicatorDIState = new BitArray(13);
        List<int> _indicatorDILink = new List<int>(13);

        BitArray _indicatorAdvState = new BitArray(1);
        List<int> _indicatorAdvLink = new List<int>(1);

        private const double MaxCurrent115 = 5;
        private const double MaxCurrent27 = 15;

        protected double _voltage27 = double.NaN;
        protected double _current27 = double.NaN;
        private double _voltage115 = double.NaN;
        private double _current115 = double.NaN;

        private Dictionary<int, double> _indicatorValue = new Dictionary<int, double>();
        
        private int[] _arrAddr = new[] { 139, 75, 203 };

        protected Measure _measWorker = new Measure();
        protected IVisual _iVisual;
        private VoltageRegime _voltRegime = VoltageRegime.Standard;

        public string ModeMemoryStr = "ModeMemory";
        public string ModeACStr = "ModeAC";
        public string ModeContrBSStr = "ModeContrBS";
        public string ModeContrVChStr = "ModeContrVCh";
        protected string _currentParam = "";

        public string CurrentParameter
        {
            get { return _currentParam; }
        }

        public static class TKParams
        {
            public static string TK1_1Delta = "I_K_1_1_Delta";
            public static string TK2_1Delta = "I_K_1_2_Delta";
            public static string TK1_2Delta = "I_K_2_1_Delta";
            public static string TK2_2Delta = "I_K_2_2_Delta";
            public static string TK1_3Delta = "I_K_3_1_Delta";
            public static string TK2_3Delta = "I_K_3_2_Delta";

            public static string TK1Abs = "I_K_1_Abs";
            public static string TK2Abs = "I_K_2_Abs";
            public static string TK3Abs = "I_K_3_Abs";
        }

        public static class ContrBSFrequency
        {
            public static int Frequency1 = -500;
            public static int Frequency2 = 500;
            public static int Frequency3 = 250;

            public static int Frequency1Min = -501;
            public static int Frequency1Max = -499;
            public static int Frequency2Min = 499;
            public static int Frequency2Max = 501;
            public static int Frequency3Min = 249;
            public static int Frequency3Max = 251;
        }

        public static class ContrVChFrequency
        {
            public static int Frequency1Min = 242;
            public static int Frequency1Max = 258;
            public static int Frequency2Min = 492;
            public static int Frequency2Max = 508;
            public static int Frequency3Min = 242;
            public static int Frequency3Max = 258;
        }

        private static class Power27
        {
            public static double Min = 24.3;
            public static double Standard = 27;
            public static double Max = 29.7;
        }

        private static class Power115
        {
            public static double Min = 109;
            public static double Standard = 115;
            public static double Max = 121;
        }

        public static class Parameters
        {
            public const string Power27 = "Plus_27V_32";
            public const string Power115 = "115V_Read_U";
            public const string Plus12V = "Plus_12V_32";
            public const string Minus12V = "Minus_12V_32";
            public const string Plus15V = "Plus_15V";
            public const string Minus15V = "Minus_15V";
            public const string Minus20V = "Minus_20V";
            public const string Plus6V = "Plus_6V";
            public const string Minus6V = "Minus_6V_32";
            public const string Plus9V = "U9V_Diss3228M";
            public const string Plus5V = "Plus_5V_32_28";
            public const string PM1 = "GLPD_1";
            public const string PM2 = "GLPD_2";
            public const string IPM = "I_GD";
            public const string MOD1 = "MOD1_3228";
            public const string MOD2 = "MOD2_3228";
            public const string UHFFrequency = "UHFFrequency";
            public const string Current27 = "Current_U27V_Diss3228M";
            public const string IndicationParam = "Indication_Diss3228M";
            public const string UHFFreq2 = "HFFrequency_Ch2_Diss3228M";
            public const string UHFFreq1_3 = "HFFrequency_Ch1_3_Diss3228M";
        }

        protected bool _dissOn;
        private bool _oneChannelDiss = false;

       private Action<string> _errorAppear;

        #endregion Переменные

        #region Перечисления

        public enum Indicators
        {
            Power27V,
            Current27V,
            Power115V,
            Plus12V,
            Minus12V,
            Minus20V,
            Plus5V,
            Plus6V,
            Minus6V,
            PM1,
            PM2,
            PMC,
            MOD1,
            MOD2,
            SD1_1,
            SD2_1,
            SD1_2,
            SD2_2,
            SD1_3,
            SD2_3,
            UNCh1_1,
            UNCh2_1,
            UNCh1_2,
            UNCh2_2,
            UNCh1_3,
            UNCh2_3,
            TK1_1,
            TK2_1,
            TK1_2,
            TK2_2,
            TK1_3,
            TK2_3,
            TK1_1_1,
            TK2_1_1,
            TK1_2_1,
            TK2_2_1,
            TK1_3_1,
            TK2_3_1,
            ChannelMeas1,
            ChannelMeas2,
            Flight,
            HighVoltage,
            Capture,
            Plus15V,
            Minus15V,
            Plus9V,
            RefuseMCO,
            RefuseMAO,
            RefuseMP,
            RefuseUSK
        }

        public enum VoltageRegime
        {
            Min,
            Standard,
            High
        }

        #endregion

        #region Свойства

        public bool SaveResults { get; set; }

        public double Voltage27
        {
            get { return _voltage27; }
        }

        public double Current27
        {
            get { return _current27; }
        }

        public double Voltage115
        {
            get { return _voltage115; }
        }

        public double Current115
        {
            get { return _current115; }
        }

        public BitArray DIIndicators
        {
            get { return (BitArray)_indicatorDIState.Clone(); }
        }

        public BitArray AdvIndicators
        {
            get { return (BitArray)_indicatorAdvState.Clone(); }
        }

        public VoltageRegime CurrentVoltageRegime
        {
            set { _voltRegime = value; }
        }

        #endregion Свойства

        #region Потоки и события

        private Thread PowerMonitorThread;
        private Thread StateMonitorThread;
        private ManualResetEvent mrePower = new ManualResetEvent(false);
        private ManualResetEvent mreState = new ManualResetEvent(false);

        #endregion Потоки и события

        public AlgorithmBase(IVisual inIVisual, int testId, Action<string> errorAppear)
        {
            _testId = testId;
            _iVisual = inIVisual;
            _errorAppear = errorAppear;
            LinkIndicatorToChannel();
            CreateMonitorThreads();
            SettingLTRWorker();
        }

        public void SetOneChannelDiss(bool state)
        {
            _oneChannelDiss = state;
        }

        private void SettingLTRWorker()
        {
            _measWorker.ResultsReady += MeasWorkerOnResultsReady;
            _measWorker.ErrorOnLTR += MeasWorkerOnErrorOnLtr;
            _measWorker.AddMeasure(new USBBaseNew("Flight_Diss3228", (int)Indicators.Flight));
            //_measWorker.AddMeasure(new USBBaseNew("Wy_Tester", (int)Indicators.HighVoltage));
            _measWorker.AddMeasure(new USBBaseNew("Capture_Diss3228", (int)Indicators.Capture)); //Общий сигнал захвата временно не работает, поэтому он включается логически по остальным захватам
            _measWorker.AddMeasure(new USBBaseNew("RefuseMCO_Diss3228M", (int)Indicators.RefuseMCO));
            _measWorker.AddMeasure(new USBBaseNew("RefuseMAO_Diss3228M", (int)Indicators.RefuseMAO));
            _measWorker.AddMeasure(new USBBaseNew("RefuseUSK_Diss3228M", (int)Indicators.RefuseUSK));
            _measWorker.AddMeasure(new USBBaseNew("RefuseMP_Diss3228M", (int)Indicators.RefuseMP));
            _measWorker.HoldCurrentSignals();
            _measWorker.Init();
            _measWorker.StartMeasuring();

        }

        private void MeasWorkerOnErrorOnLtr()
        {
            _iVisual.ErrorAppear(null);
        }

        private void MeasWorkerOnResultsReady(List<USBBaseNew> usbList)
        {
            foreach (USBBaseNew ub in usbList)
                SetIndicatorValue(ub.IndicatorNumber, ub.Result);
        }

        public double GetIndicatorValue(Indicators indicator)
        {
            if (_indicatorValue.ContainsKey((int)indicator))
                return _indicatorValue[(int)indicator];
            return double.NaN;
        }

        private void CreateMonitorThreads()
        {
            StartPowerMonitor();
            StateMonitorThread = new Thread(StateMonitor);
            StateMonitorThread.IsBackground = true;
            StateMonitorThread.Start();
        }

        protected void StopPowerMonitorWait()
        {
            mrePower.Set();
            PowerMonitorThread.Join(6000);
        }

        protected void StartPowerMonitor()
        {
            if (PowerMonitorThread != null)
                if (PowerMonitorThread.IsAlive)
                    return;
            mrePower.Reset();
            PowerMonitorThread = new Thread(PowerMonitor);
            PowerMonitorThread.IsBackground = true;
            PowerMonitorThread.Start();
        }

        protected void LinkIndicatorToChannel()
        {
            AddIndicatorDILink("DissOn");
            //AddIndicatorDILink("Flight");
            //AddIndicatorAdvLink("Navigation");
            AddIndicatorDILink("High");
            AddIndicatorDILink("Memory_1");
            AddIndicatorDILink("Memory_2");
            //AddIndicatorDILink("Capture");
            AddIndicatorDILink("Capture_1");
            AddIndicatorDILink("Capture_2");
            AddIndicatorDILink("Capture_3");
            AddIndicatorDILink("GoodCondition");
        }

        protected void ClearAllDILinks()
        {
            _indicatorDILink.Clear();
        }

        protected void AddIndicatorDILink(string channelName)
        {
            _indicatorDILink.Add(DataManager.Instance.GetChannelByName(DataManager.DiscretInputs, channelName));
        }

        private void AddIndicatorAdvLink(string channelName)
        {
            _indicatorAdvLink.Add(DataManager.Instance.GetChannelByName(DataManager.Advantech1760, channelName));
        }

        private void StateMonitor()
        {
            try
            {
                while (!mreState.WaitOne(0))
                {
                    BitArray ba = DiscreteInputs.Instance().GetInputsStates();
                    for (int i = 0; i < _indicatorDILink.Count; i++)
                        _indicatorDIState[i] = ba[_indicatorDILink[i]];
                    ba = DO176X.Instance.ReadDI32(true);
                    for (int i = 0; i < _indicatorAdvLink.Count; i++)
                        _indicatorAdvState[i] = ba[_indicatorAdvLink[i]];
                    if (Globals.Emulation)
                        Thread.Sleep(150);
                }
            }
            catch(Exception ex)
            {
                _errorAppear(ex.ToString());
            }
        }

        private void PowerMonitor()
        {
            try
            {
                while (!mrePower.WaitOne(0))
                {
                    if (Power27V.Instance.GetOutputState() == null)
                        _voltage27 = _current27 = double.NaN;
                    _voltage27 = Globals.Process27Voltage(Power27V.Instance.GetVoltage());
                    _current27 = Power27V.Instance.GetCurrent();
                    SetIndicatorValue((int)AlgorithmBase.Indicators.Current27V, _current27);
                    if (Power115V.Instance.IsAktor)
                        continue;
                    _voltage115 = Globals.Process115Voltage(Power115V.Instance.GetVoltage());
                    _current115 = Power115V.Instance.GetCurrent();
                    if (Globals.Emulation)
                        Thread.Sleep(150);
                }
            }
            catch(Exception ex)
            {
                _errorAppear(ex.ToString());
            }
        }

        public void ActivateFirstPower(Action<int, double, double> SetIndLimit)
        {
            _measWorker.StopMeasuring(false);
            AddMeasure(new USBBaseNew(_testId, Parameters.Power27, (int)Indicators.Power27V), SetIndLimit);
            AddMeasure(new USBBaseNew(_testId, Parameters.Current27, (int)Indicators.Current27V), SetIndLimit, false);
            AddMeasure(new USBBaseNew(_testId, Parameters.Power115, (int) Indicators.Power115V), SetIndLimit, false);
            _measWorker.Init();
            _measWorker.StartMeasuring();
        }

        public void Stop()
        {
            _measWorker.StopMeasuring(SaveResults);
            SaveResults = false;
            _measWorker.Init();
            _measWorker.StartMeasuring();
        }

        public bool ShiftPower27(double shift)
        {
            bool result = true;
            StopPowerMonitorWait();
            try
            {
                switch (_voltRegime)
                {
                    case VoltageRegime.Min:
                        Power27.Min += shift;
                        Power27V.Instance.SetVoltage(Power27.Min);
                        break;
                    case VoltageRegime.High:
                        Power27.Max += shift;
                        Power27V.Instance.SetVoltage(Power27.Max);
                        break;
                    case VoltageRegime.Standard:
                        Power27.Standard += shift;
                        Power27V.Instance.SetVoltage(Power27.Standard);
                        break;
                }
            }
            catch
            {
                result = false;
            }

            StartPowerMonitor();
            return result;
        }

        public bool ShiftPower115(double shift)
        {
            bool result = true;
            StopPowerMonitorWait();
            switch (_voltRegime)
            {
                case VoltageRegime.Min:
                    Power115.Min += shift;
                    if (_dissOn)
                        result &= Power115V.Instance.SetVoltage(Power115.Min);
                    else
                        result &= Power115V.Instance.SetVoltageSimple(Power115.Min);
                    break;
                case VoltageRegime.High:
                    Power115.Max += shift;
                    if (_dissOn)
                        result &= Power115V.Instance.SetVoltage(Power115.Max);
                    else
                        result &= Power115V.Instance.SetVoltageSimple(Power115.Max);
                    break;
                case VoltageRegime.Standard:
                    Power115.Standard += shift;
                    if (_dissOn)
                        result &= Power115V.Instance.SetVoltage(Power115.Standard);
                    else
                        result &= Power115V.Instance.SetVoltageSimple(Power115.Standard);
                    break;
            }
            StartPowerMonitor();
            return result;
        }

        public void SetVoltageRegime(VoltageRegime voltageRegime)
        {
            _voltRegime = voltageRegime;
            ShiftPower115(0);
            ShiftPower27(0);
        }

        //public bool DissOn(bool state)
        //{
        //    bool result = true;
        //    StopPowerMonitorWait();
        //    if (state)
        //    {
        //        result &= ShiftPower27(0);
        //        Power27V.Instance.SetVoltage(MaxCurrent27);

        //    }
        //    result &= Power27V.Instance.SetOnOff(state);
        //    if (state)
        //    {
        //        result &= ShiftPower115(0);
        //        result &= Power115V.Instance.SetCurrentLimit(MaxCurrent115);
        //    }
        //    else
        //        result &= Power115V.Instance.SetOnOff(false);
        //    _dissOn = true;

        //    if (state)
        //    {
        //        result &= ShiftPower115(0);
        //        if (result)
        //            DiscreteOutputs.Instance().SetOutput("TK_15", true);
        //    }
        //    StartPowerMonitor();
        //    return result;
        //}

        public bool Power115VOn(bool state)
        {
            bool result = true;
            if (state)
            {
                result &= ShiftPower115(0);
                result &= Power115V.Instance.SetMaxCurrent(MaxCurrent115);
            }
            else
                result &= Power115V.Instance.SetOnOff(false);
            return result;
        }

        public bool Power27VOn(bool state)
        {
            bool result = true;
            if (state)
            {
                result &= ShiftPower27(0);
                Power27V.Instance.SetCurrent(MaxCurrent27);
            }
            result &= Power27V.Instance.SetOnOff(state);
            return result;
        }

        protected const int DelayPower3228 = 4000;

        public bool DissOn3228(bool state)
        {
            bool result = true;
            StopPowerMonitorWait();
            if (state)
            {
                result &= Power115VOn(state);
                _dissOn = true;
                if (state)
                    result &= ShiftPower115(0);
                Power27VOn(state);
            }
            else
            {
                Power27VOn(state);
                Thread.Sleep(DelayPower3228);
                result &= Power115VOn(state);
            }
            if (state)
            {
                if (result)
                    DiscreteOutputs.Instance().SetOutput("TK_15", true);
            }
            StartPowerMonitor();
            return result;
        }



        #region Commands

        public bool SetHVState(bool state)
        {
            return DiscreteOutputs.Instance().SetOutput("High_32", !state);
        }

        public bool SetTKState(bool state)
        {
            return DiscreteOutputs.Instance().SetOutput("TK_32", state);
        }

        public bool SetUNChState(bool state)
        {
            return DiscreteOutputs.Instance().SetOutput("UNCh_32", state);
        }

        public bool SetContrBS(bool state)
        {
            if (state)
                _iVisual.ShowMessage(Resources.Properties.Resources.PressButtonContrBS);
            ContrBSState = state;
            return true;
            //return DiscreteOutputs.Instance().SetOutput(3, state);
        }

        public bool SetContrBSLong(bool state)
        {
            if (state)
                _iVisual.ShowMessage(Resources.Properties.Resources.PressButtonContrBSLong);
            ContrBSState = state;
            return true;
            //return DiscreteOutputs.Instance().SetOutput(3, state);
        }

        public bool ContrVChState { get; protected set; }
        public bool ContrBSState { get; protected set; }

        public bool SetContrVCh(bool state)
        {
            if (state)
                _iVisual.ShowMessage(Resources.Properties.Resources.PressButtonContrVCh);
            ContrVChState = state;
            return true;
            //return DiscreteOutputs.Instance().SetOutput(6, state);
        }

        #endregion Commands

        public void SaveFirstPower()
        {
            if (_testId <= 0)
                return;
            DataManager.Instance.SaveParameter(_testId, Parameters.Power115, _voltage115);
        }

        public void ActivateSecondPower(Action<int, double, double> SetIndLimit)
        {
            _measWorker.StopMeasuring(false);
            AddMeasure(new USBBaseNew(_testId, Parameters.Plus12V, (int)Indicators.Plus12V), SetIndLimit);
            AddMeasure(new USBBaseNew(_testId, Parameters.Minus12V, (int)Indicators.Minus12V), SetIndLimit);
            AddMeasure(new USBBaseNew(_testId, Parameters.Minus20V, (int)Indicators.Minus20V), SetIndLimit);
            AddMeasure(new USBBaseNew(_testId, Parameters.Plus6V, (int)Indicators.Plus6V), SetIndLimit);
            AddMeasure(new USBBaseNew(_testId, Parameters.Minus6V, (int)Indicators.Minus6V), SetIndLimit);
            AddMeasure(new USBBaseNew(_testId, Parameters.Plus5V, (int)Indicators.Plus5V), SetIndLimit);
            _measWorker.Init();
            _measWorker.StartMeasuring();
        }

        public void ActivatePM(Action<int, double, double> SetIndLimit)
        {
            _measWorker.StopMeasuring(false);
            AddMeasure(new USBBaseNew(_testId, Parameters.PM1, (int)Indicators.PM1), SetIndLimit);
            AddMeasure(new USBBaseNew(_testId, Parameters.PM2, (int)Indicators.PM2), SetIndLimit);
            AddMeasure(new USBBaseNew(_testId, Parameters.IPM, (int)Indicators.PMC), SetIndLimit);
            AddMeasure(new USBBaseNew(_testId, Parameters.MOD1, (int)Indicators.MOD1), SetIndLimit);
            AddMeasure(new USBBaseNew(_testId, Parameters.MOD2, (int)Indicators.MOD2), SetIndLimit);
            _measWorker.Init();
            _measWorker.StartMeasuring();
        }

        public void ActivateSD(string curParam, Action<int, double, double> SetIndLimit)
        {
            _measWorker.StopMeasuring(false);
            _currentParam = curParam;
            AddSameMeasuring(_testId, _currentParam, (int)Indicators.SD1_1, SetIndLimit);
            _measWorker.Init();
            _measWorker.StartMeasuring();
        }

        protected void AddSameMeasuring(int testId, string currentParam, int startIndicator, Action<int, double, double> SetIndLimit = null)
        {

            const int numGroup1 = 3;
            const int numGroup2 = 2;
            //if (_oneChannelDiss)
            //  numGroup1 = 1;
            for (int group1 = 1; group1 <= numGroup1; group1++)
                for (int group2 = 1; group2 <= numGroup2; group2++)
                {
                    string param = string.Format(currentParam, group1, group2);
                    _measWorker.AddMeasure(
                        new USBBaseNew(testId, param, startIndicator++));
                    if (SetIndLimit == null)
                        continue;
                    double min = 0, max = 0;
                    DataManager.Instance.GetParamLimitAndId(param, ref min, ref max);
                    SetIndLimit(startIndicator - 1, min, max);
                }

        }

        public void ActivateUNCh(Action<int, double, double> SetIndLimit)
        {
            _measWorker.StopMeasuring(false);
            _currentParam = "UNCh_{0}_{1}_Noise32";
            AddSameMeasuring(_testId, _currentParam, (int)Indicators.UNCh1_1, SetIndLimit);
            _measWorker.Init();
            _measWorker.StartMeasuring();
        }

        public void ActivateTKFirst(Action<int, double, double> SetIndLimit)
        {
            _measWorker.StopMeasuring(false);
            AddMeasure(new USBBaseNew(_testId, "I_K_1_1_32", (int)Indicators.TK1_1), SetIndLimit);
            AddMeasure(new USBBaseNew(_testId, "I_K_1_2_32", (int)Indicators.TK2_1), SetIndLimit);
            AddMeasure(new USBBaseNew(_testId, "I_K_2_1_32", (int)Indicators.TK1_2), SetIndLimit);
            AddMeasure(new USBBaseNew(_testId, "I_K_2_2_32", (int)Indicators.TK2_2), SetIndLimit);
            AddMeasure(new USBBaseNew(_testId, "I_K_3_1_32", (int)Indicators.TK1_3), SetIndLimit);
            AddMeasure(new USBBaseNew(_testId, "I_K_3_2_32", (int)Indicators.TK2_3), SetIndLimit);
            _measWorker.Init();
            _measWorker.StartMeasuring();
        }

        /// <summary>
        /// Добавляет сигнал на измерение а также устанавливает пределы на индикаторе через callback
        /// </summary>
        /// <param name="uSBBaseNew">Сигнал для добавления </param>
        /// <param name="setIndLimit">Функция для установки пределов на индикаторе</param>
        /// <param name="addToMeasurer">True - добавлять в очередь на измерение</param>
        private void AddMeasure(USBBaseNew uSBBaseNew, Action<int, double, double> setIndLimit, bool addToMeasurer = true)
        {
            setIndLimit(uSBBaseNew.IndicatorNumber, uSBBaseNew.Min, uSBBaseNew.Max);
            if (addToMeasurer)
                _measWorker.AddMeasure(uSBBaseNew);
        }

        public void ActivateTKFirst(int numChannel, Action<int, double, double> SetIndicatorLimit)
        {
            _measWorker.StopMeasuring(false);
            Indicators ind1 = Indicators.TK1_1;
            Indicators ind2 = Indicators.TK2_1;
            switch (numChannel)
            {
                case 2:
                    ind1 = Indicators.TK1_2;
                    ind2 = Indicators.TK2_2;
                    break;
                case 3:
                    ind1 = Indicators.TK1_3;
                    ind2 = Indicators.TK2_3;
                    break;
            }
            USBBaseNew newSignal = new USBBaseNew(_testId, "I_K_1_1_32", (int)ind1);
            _measWorker.AddMeasure(newSignal);
            SetIndicatorLimit((int)ind1, newSignal.Max, newSignal.Min);
            newSignal = new USBBaseNew(_testId, "I_K_1_2_32", (int)ind2);
            _measWorker.AddMeasure(newSignal);
            SetIndicatorLimit((int)ind2, newSignal.Max, newSignal.Min);
            _measWorker.Init();
            _measWorker.StartMeasuring();
        }

        public void RememberTKFirst(int numChannel = 0)
        {
            if (!_oneChannelDiss || (_oneChannelDiss && numChannel == 1))
            {
                CopyIndicatorValue((int)Indicators.TK1_1_1, (int)Indicators.TK1_1);
                CopyIndicatorValue((int)Indicators.TK2_1_1, (int)Indicators.TK2_1);
            }
            if (!_oneChannelDiss || (_oneChannelDiss && numChannel == 2))
            {
                CopyIndicatorValue((int)Indicators.TK1_2_1, (int)Indicators.TK1_2);
                CopyIndicatorValue((int)Indicators.TK2_2_1, (int)Indicators.TK2_2);
            }
            if (!_oneChannelDiss || (_oneChannelDiss && numChannel == 3))
            {
                CopyIndicatorValue((int)Indicators.TK1_3_1, (int)Indicators.TK1_3);
                CopyIndicatorValue((int)Indicators.TK2_3_1, (int)Indicators.TK2_3);
            }
        }

        private void CopyIndicatorValue(int dest, int source)
        {
            SetIndicatorValue(dest, _indicatorValue[source]);
        }

        private void SetIndicatorValue(int indNum, double value)
        {
            if (!_indicatorValue.ContainsKey(indNum))
                _indicatorValue.Add(indNum, value);
            else
                _indicatorValue[indNum] = value;
        }

        public List<int> GetArrays()
        {

            List<int> inArrays = ControlTask.Instance().GetArrays();
            if (inArrays == null)
                return null;

            //разворачиваем для удобства отображения
            for (int i = 0; i < inArrays.Count; i++)
            {
                int reverseInt = 0;
                const int bitCount = 32;
                for (int bit = 0; bit < bitCount; bit++)
                {
                    reverseInt <<= 1;
                    int check = (int)Math.Pow(2, bit);
                    reverseInt |= (inArrays[i] & check) == check ? 1 : 0;
                }
                inArrays[i] = reverseInt;
            }
            return inArrays;
        }

        private void AddArray(List<int> _arrays, int i, int p)
        {
            while (_arrays.Count <= i)
                _arrays.Add(0);
            _arrays[i] = p;
        }

        public int GetArrayAddr(int index)
        {
            if (_arrAddr.Length > index)
                return _arrAddr[index];
            return 0;
        }

        public void Finish()
        {
            _measWorker.HoldSignals = 0;
            _measWorker.StopMeasuring(false);
            mrePower.Set(); //останавливаем мониторинг напряжений
            mreState.Set(); //останавливаем мониторинг табло
        }

        public void ResetLimits(Action<int, double, double> SetIndLimit)
        {
            SetIndLimit((int)Indicators.TK1_1, 0, 0);
            SetIndLimit((int)Indicators.TK2_1, 0, 0);
            SetIndLimit((int)Indicators.TK1_2, 0, 0);
            SetIndLimit((int)Indicators.TK2_2, 0, 0);
            SetIndLimit((int)Indicators.TK1_3, 0, 0);
            SetIndLimit((int)Indicators.TK2_3, 0, 0);
        }

        public void ResetLimitsSD(Action<int, double, double> SetIndLimit)
        {
            SetIndLimit((int)Indicators.SD1_1, 0, 0);
            SetIndLimit((int)Indicators.SD2_1, 0, 0);
            SetIndLimit((int)Indicators.SD1_2, 0, 0);
            SetIndLimit((int)Indicators.SD2_2, 0, 0);
            SetIndLimit((int)Indicators.SD1_3, 0, 0);
            SetIndLimit((int)Indicators.SD2_3, 0, 0);
        }
    }
}
