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
using Res = Sparc.KPK2.Resources.Properties.Resources;

namespace Sparc.KPK2.Algorithms.Diss3228M
{
    public class AlgorithmDiss3228M
    {
        Lambda27V _lambda27V = new Lambda27V();
        private Action<string> _errorAppear;

        public AlgorithmDiss3228M(IVisual inIVisual, int testId, Action<string> errorAppear)
        {
            _testId = testId;
            _iVisual = inIVisual;
            _errorAppear = errorAppear;

            Voltage27 = double.NaN;
            Current27 = double.NaN;

            CurrentParameter = string.Empty;

            var result = SetSignalsOff();
            if (!result)
                _iVisual.ErrorAppear(Res.ErrorAlgorithmInitialization);

            LinkIndicatorToChannel();
            CreateMonitorThreads();
            SettingLtrWorker();
        }

        private bool SetSignalsOff()
        {
            var result = SetContrBs(false);
            result &= SetContrVCh(false);
            result &= SetNavigation(false);
            result &= SetGammaMore10(false);
            result &= SetSeaS(false);
            result &= SetMeasChannelOff();
            return result;
        }

        #region Переменные

        private readonly IVisual _iVisual;
        private readonly int _testId;

        public bool SaveResults;

        private readonly BitArray _indicatorDiState = new BitArray(13);
        private readonly List<int> _indicatorDiLink = new List<int>(13);

        private const double MaxCurrent27 = 15;

        private readonly Dictionary<int, double> _indicatorValue = new Dictionary<int, double>();

        private readonly int[] _arrAddr = new[] { 139, 75, 203 };

        private readonly Measure _measWorker = new Measure();
        private VoltageRegime _voltRegime = VoltageRegime.Standard;

        public static class ContrBsFrequency
        {
            public const int Frequency1Min = -501;
            public const int Frequency1Max = -499;
            public const int Frequency2Min = 499;
            public const int Frequency2Max = 501;
            public const int Frequency3Min = 249;
            public const int Frequency3Max = 251;
        }

        public static class ContrVChFrequency
        {
            public const int Frequency1Min = 236;
            public const int Frequency1Max = 264;
            public const int Frequency2Min = 484;
            public const int Frequency2Max = 516;
            public const int Frequency3Min = 236;
            public const int Frequency3Max = 264;
        }

        private static class Power27
        {
            public static double Min = 24.3;
            public static double Standard = 27;
            public static double Max = 29.7;
        }

        public static class Parameters
        {
            public const string Plus27V = "Plus_27V_32";
            public const string Plus12V = "U12V_Diss3228M";
            public const string Minus12V = "UMinus_12V_Diss3228M";
            public const string Plus15V = "Plus_15V";
            public const string Minus15V = "Minus_15V";
            public const string Plus9V = "U9V_Diss3228M";
            public const string Plus5V = "Plus_5V_Diss3228M";
            public const string Current27 = "Current_U27V_Diss3228M";
            public const string IndicationParam = "Indication_Diss3228M";
            public const string UhfFreqChannel2 = "HFFrequency_Ch2_Diss3228M";
            public const string UhfFreqChannels1And3 = "HFFrequency_Ch1_3_Diss3228M";
            public const string ModeAcStr = "ModeAC";
            public const string ModeContrBsStr = "ModeContrBS";
            public const string ModeContrVChStr = "ModeContrVCh";
            public const string CorrectionCode1 = "CorrectionCode1";
            public const string CorrectionCode2 = "CorrectionCode2";
            public const string CorrectionCode3 = "CorrectionCode3";
            public const string RefuseMco = "RefuseMCO_Diss3228M";
            public const string RefuseMao = "RefuseMAO_Diss3228M";
            public const string RefuseUsk = "RefuseUSK_Diss3228M";
            public const string RefuseMp = "RefuseMP_Diss3228M";
                
                
                }
                
        #endregion Переменные

        #region Перечисления

        public enum Indicators
        {
            RefuseMco,
            RefuseMao,
            RefuseMp,
            RefuseUsk,
            Power27V,
            Current27V,
            Plus12V,
            Minus12V,
            Plus15V,
            Minus15V,
            Plus9V,
            Plus5V,
            Ulf1Channel1,
            Ulf2Channel1,
            Ulf1Channel2,
            Ulf2Channel2,
            Ulf1Channel3,
            Ulf2Channel3
        }

        public enum VoltageRegime
        {
            Min,
            Standard,
            High
        }

        #endregion

        #region Свойства

        public double Voltage27 { get; private set; }
        public double Current27 { get; private set; }

        public bool ContrVChState { get; private set; }
        public string CurrentParameter { get; private set; }

        #endregion Свойства

        #region Потоки и события

        private Thread _powerMonitorThread;
        private Thread _stateMonitorThread;
        private readonly ManualResetEvent _mrePower = new ManualResetEvent(false);
        private readonly ManualResetEvent _mreState = new ManualResetEvent(false);
        private readonly ManualResetEvent _mrePowerStop = new ManualResetEvent(false);

        #endregion Потоки и события

        private void SettingLtrWorker()
        {
            _measWorker.ResultsReady += MeasWorkerOnResultsReady;
            _measWorker.ErrorOnLTR += MeasWorkerOnErrorOnLtr;
            _measWorker.AddMeasure(new USBBaseNew(Parameters.Plus27V, (int)Indicators.Power27V));
            _measWorker.AddMeasure(new USBBaseNew(Parameters.RefuseMco, (int)Indicators.RefuseMco));
            _measWorker.AddMeasure(new USBBaseNew(Parameters.RefuseMao, (int)Indicators.RefuseMao));
            _measWorker.AddMeasure(new USBBaseNew(Parameters.RefuseUsk, (int)Indicators.RefuseUsk));
            _measWorker.AddMeasure(new USBBaseNew(Parameters.RefuseMp, (int)Indicators.RefuseMp));
            _measWorker.HoldCurrentSignals();
            _measWorker.Init();
            _measWorker.StartMeasuring();
        }

        private void MeasWorkerOnErrorOnLtr()
        {
            _iVisual.ErrorAppear(string.Format(Res.ErrorOnDeviceN, Res.LTRBoards));
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
            _stateMonitorThread = new Thread(StateMonitor) { IsBackground = true };
            _stateMonitorThread.Start();
        }

        private void StopPowerMonitorWait()
        {
            _mrePowerStop.Reset();
            _mrePower.Set();
            _mrePowerStop.WaitOne(6000);
        }

        private void StartPowerMonitor()
        {
            if (_powerMonitorThread != null)
                if (_powerMonitorThread.IsAlive)
                    _powerMonitorThread.Abort();
            _mrePower.Reset();
            _powerMonitorThread = new Thread(PowerMonitor) { IsBackground = true };
            _powerMonitorThread.Start();
        }

        private void LinkIndicatorToChannel()
        {
            ClearAllDiLinks();
            AddIndicatorDiLink("GoodCondition");
            AddIndicatorDiLink("Memory_2");
        }

        private void ClearAllDiLinks()
        {
            _indicatorDiLink.Clear();
        }

        private void AddIndicatorDiLink(string channelName)
        {
            _indicatorDiLink.Add(DataManager.Instance.GetChannelByName(DataManager.DiscretInputs, channelName));
        }

        public BitArray GetDiIndicators()
        {
            return (BitArray)_indicatorDiState.Clone();
        }

        private void StateMonitor()
        {
            try
            {
                while (!_mreState.WaitOne(0))
                {
                    BitArray ba = DiscreteInputs.Instance().GetInputsStates();
                    for (int i = 0; i < _indicatorDiLink.Count; i++)
                        _indicatorDiState[i] = ba[_indicatorDiLink[i]];
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
                while (!_mrePower.WaitOne(0))
                {
                    if (Power27V.Instance.GetOutputState() == null) // _lambda27V.GetOutputState() == null
                        Voltage27 = Current27 = double.NaN;
                    Voltage27 = Globals.Process27Voltage(Power27V.Instance.GetVoltage()); // _lambda27V.GetVoltage()
                    Current27 = Power27V.Instance.GetCurrent(); // Current27 = _lambda27V.GetCurrent()
                    SetIndicatorValue((int)Indicators.Current27V, Current27);
                    if (Power115V.Instance.IsAktor)
                        continue;
                    if (Globals.Emulation)
                        Thread.Sleep(150);
                }
                _mrePowerStop.Set();
            }
            catch(Exception ex)
            {
                _errorAppear(ex.ToString());
            }
        }

        public void ActivateFirstPower(Action<int, double, double> setIndLimit)
        {
            _measWorker.StopMeasuring(false);
            AddMeasure(new USBBaseNew(_testId, Parameters.Plus27V, (int)Indicators.Power27V), setIndLimit);
            AddMeasure(new USBBaseNew(_testId, Parameters.Current27, (int)Indicators.Current27V), setIndLimit, false);
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
            switch (_voltRegime)
            {
                case VoltageRegime.Min:
                    Power27.Min += shift;
                    Power27V.Instance.SetVoltage(Power27.Min); //_lambda27V.SetVoltage 
                    //result &= _lambda27V.SetVoltage(Power27.Min);
                    break;
                case VoltageRegime.High:
                    Power27.Max += shift;
                    Power27V.Instance.SetVoltage(Power27.Max); //  _lambda27V.SetVoltage
                    // result &= _lambda27V.SetVoltage(Power27.Max);
                    break;
                case VoltageRegime.Standard:
                    Power27.Standard += shift;
                    Power27V.Instance.SetVoltage(Power27.Standard); //  _lambda27V.SetVoltage
                    //result &= _lambda27V.SetVoltage(Power27.Standard);
                    break;
            }
            StartPowerMonitor();
            return result;
        }

        public void SetVoltageRegime(VoltageRegime voltageRegime)
        {
            _voltRegime = voltageRegime;
            ShiftPower27(0);
        }

        private bool Power27VOn(bool state)
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

        public bool DissOn(bool state)
        {
            bool result = true;
            StopPowerMonitorWait();
            result &= Power27VOn(state);
            SetSeaS(state);
            StartPowerMonitor();
            return result;
        }

        #region Commands

        private bool SetSeaS(bool state)
        {
            bool result = DiscreteOutputs.Instance().SetOutput(0, state);
            return result;
        }

        private bool SetGammaMore10(bool state)
        {
            bool result = DiscreteOutputs.Instance().SetOutput(2, state);
            return result;
        }

        private bool SetNavigation(bool state)
        {
            bool result = DiscreteOutputs.Instance().SetOutput(5, state);
            return result;
        }

        public bool SetUlfState(bool state)
        {
            return DiscreteOutputs.Instance().SetOutput("UNCh_32", state);
        }

        public bool SetContrBs(bool state)
        {
            var result = DO176X.Instance.SetOutput(3, state);
            return result;
        }

        public bool SetContrVCh(bool state)
        {
            var result = DO176X.Instance.SetOutput(4, state);
            if (result) ContrVChState = state;
            return result;
        }

        #endregion Commands

        public void SaveFirstPower()
        {
            if (_testId <= 0)
                return;
            DataManager.Instance.SaveParameter(_testId, Parameters.Current27, Current27);
        }

        public void ActivateSecondPower(Action<int, double, double> setIndLimit)
        {
            _measWorker.StopMeasuring(false);
            AddMeasure(new USBBaseNew(_testId, Parameters.Plus12V, (int)Indicators.Plus12V), setIndLimit);
            AddMeasure(new USBBaseNew(_testId, Parameters.Minus12V, (int)Indicators.Minus12V), setIndLimit);
            AddMeasure(new USBBaseNew(_testId, Parameters.Minus15V, (int)Indicators.Minus15V), setIndLimit);
            AddMeasure(new USBBaseNew(_testId, Parameters.Plus15V, (int)Indicators.Plus15V), setIndLimit);
            AddMeasure(new USBBaseNew(_testId, Parameters.Plus9V, (int)Indicators.Plus9V), setIndLimit);
            AddMeasure(new USBBaseNew(_testId, Parameters.Plus5V, (int)Indicators.Plus5V), setIndLimit);
            _measWorker.Init();
            _measWorker.StartMeasuring();
        }

        private void AddSameMeasuring(int testId, string currentParam, int startIndicator, Action<int, double, double> setIndLimit = null)
        {
            const int numGroup1 = 3;
            const int numGroup2 = 2;
            for (int group1 = 1; group1 <= numGroup1; group1++)
                for (int group2 = 1; group2 <= numGroup2; group2++)
                {
                    string param = string.Format(currentParam, group1, group2);
                    _measWorker.AddMeasure( new USBBaseNew(testId, param, startIndicator++));
                    if (setIndLimit == null)
                        continue;
                    double min = 0, max = 0;
                    DataManager.Instance.GetParamLimitAndId(param, ref min, ref max);
                    setIndLimit(startIndicator - 1, min, max);
                }
        }

        public void ActivateUlf(Action<int, double, double> setIndLimit)
        {
            _measWorker.StopMeasuring(false);
            CurrentParameter = "UNCh_{0}_{1}_Diss3228M";
            AddSameMeasuring(_testId, CurrentParameter, (int)Indicators.Ulf1Channel1, setIndLimit);
            _measWorker.Init();
            _measWorker.StartMeasuring();
        }

        /// <summary>
        /// Добавляет сигнал на измерение а также устанавливает пределы на индикаторе через callback
        /// </summary>
        /// <param name="usbBaseNew">Сигнал для добавления </param>
        /// <param name="setIndLimit">Функция для установки пределов на индикаторе</param>
        /// <param name="addToMeasurer">True - добавлять в очередь на измерение</param>
        private void AddMeasure(USBBaseNew usbBaseNew, Action<int, double, double> setIndLimit, bool addToMeasurer = true)
        {
            setIndLimit(usbBaseNew.IndicatorNumber, usbBaseNew.Min, usbBaseNew.Max);
            if (addToMeasurer)
                _measWorker.AddMeasure(usbBaseNew);
        }

        private void SetIndicatorValue(int indNum, double value)
        {
            _indicatorValue[indNum] = value;
        }

        public List<int> GetWords()
        {
            List<int> words = ControlTask.Instance().GetArrays();
            if (words == null)
                return null;

            // Реверсирование бит в словах
            for (var i = 0; i < words.Count; i++)
                words[i] = words[i].ReverseBits();

            return words;
        }

        public int GetWordAddr(int index)
        {
            if (_arrAddr.Length > index)
                return _arrAddr[index];
            return 0;
        }

        public void Finish()
        {
            _measWorker.HoldSignals = 0;
            _measWorker.StopMeasuring(false);
            _mrePower.Set(); //останавливаем мониторинг напряжений
            _mreState.Set(); //останавливаем мониторинг табло
        }

        #region Адаптированные функции из Diss32Functions

        /// <summary>
        /// Устанавливает канал для одноканального ДИСС-32-28-M
        /// </summary>
        /// <param name="channel">Устанавливаемый канал</param>
        /// <returns>Признак успеха установки канала</returns>
        public bool SetMeasChannel(int channel)
        {
            var result = DO176X.Instance.SetOutput(7, channel == 1);
            result &= DO176X.Instance.SetOutput(8, channel == 0 || channel == 2);
            result &= DO176X.Instance.SetOutput(9, channel == 0 || channel == 2);
            return result;
        }

        public bool SetMeasChannelOff()
        {
            var result = DO176X.Instance.SetOutput(7, false);
            result &= DO176X.Instance.SetOutput(8, false);
            result &= DO176X.Instance.SetOutput(9, false);
            return result;
        }

        #endregion
    }
}
