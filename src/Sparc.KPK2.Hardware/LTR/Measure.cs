using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using LCards;
using NLog;
using Sparc.KPK2.Hardware.MyClasseUSB;
using Sparc.KPK2.NFunctions;

namespace Sparc.KPK2.Hardware.LTR
{
    public class Measure
    {
        #region Поля
        private int _waitTimeout = 1500;
        private int _holdSignals;
        private bool _isWorking;
        private ManualResetEvent _cancelEvent = new ManualResetEvent(true);
        private readonly List<USBBaseNew> _usbList = new List<USBBaseNew>();
        private readonly List<int> _slots = new List<int>();

        public Action<double> freqCalculated;
        public string freqParam = string.Empty;

        private Logger _logger = LogManager.GetLogger("KPK2Info");

        /// <summary>
        /// Ошибка при получении или установке значений на платах LTR
        /// </summary>
        private bool _ltrError;

        /// <summary>
        /// Наш процесс для измерений в фоновом режиме
        /// </summary>
        private Thread _worker;

        /// <summary>
        /// Писать в лог или нет об ошибках
        /// </summary>
        private bool _writeLog = true;

        /// <summary>
        /// Список всех измеряемых сигналов поплатно 
        /// </summary>
        public List<List<UsbBase>> lstUsb = new List<List<UsbBase>>();

        /// <summary>
        /// Класс хранения 2-х списков каналы, которые можно измерять вместе и которые только раздельно
        /// </summary>
        public class ChannelLists
        {
            public List<USBBaseNew> Common = new List<USBBaseNew>();
            public List<USBBaseNew> Separate = new List<USBBaseNew>();
        }

        /// <summary>
        /// список каналов, которые разделены на те, что отдельно измеряются и те что могут измеряться вместе и разделены по слотам
        /// </summary>
        private readonly Dictionary<int, ChannelLists> _configList = new Dictionary<int, ChannelLists>();

        #endregion

        #region Свойства

        public bool IsWorking
        {
            get { return _isWorking; }
        }

        public int HoldSignals
        {
            set
            {
                if (value >= 0)
                    _holdSignals = value;
            }
        }

        public List<List<UsbBase>> LstUsb
        {
            get { return lstUsb; }
        }

        public bool WriteLog
        {
            get { return _writeLog; }
            set { _writeLog = value; }
        }

        public bool LTRError
        {
            get { return _ltrError; }
        }

        #endregion

        #region События

        public delegate void ResultsReadyDelegate(List<USBBaseNew> usbList);

        public delegate void EmptyDelegate();

        /// <summary>
        /// Событие сообщающее о том что результаты измерений готовы
        /// </summary>        
        public event ResultsReadyDelegate ResultsReady;

        public event EmptyDelegate ErrorOnLTR;

        #endregion

        #region Функции

        public void HoldCurrentSignals()
        {
            HoldSignals = _usbList.Count;
        }

        /// <summary>
        /// начальная инициализация и сортировка по слотам и каналам
        /// </summary>
        /// <returns></returns>
        public bool Init()
        {
            if (_slots.Count <= 0)
                return false;
            _configList.Clear();
            foreach (int slot in _slots)
            {
                SortChannels(slot);
                switch (USBBaseNew.GetTypeModule((uint)slot))
                {
                    case USBBaseNew.LTRType.LTR114:
                        InitLTR114New((uint)slot);
                        break;
                    case USBBaseNew.LTRType.LTR22:
                        InitLTR22New((uint)slot);
                        break;
                }
            }
            return true;
        }

        public double GetValue(string signal)
        {
            foreach (USBBaseNew ub in _usbList)
                if (ub.ParamName == signal)
                    return ub.Result;
            return double.NaN;
        }

        /// <summary>
        /// сортирует каналы по тем, которые измеряются отдельно, и тем, которые измеряются вместе
        /// </summary>
        /// <param name="slot"></param>
        private void SortChannels(int slot)
        {
            ChannelLists cList = new ChannelLists();
            foreach (USBBaseNew usb in _usbList)
            {
                if (usb.Slot == slot)
                {
                    if (usb.IsAlone)
                        cList.Separate.Add(usb);
                    else
                        cList.Common.Add(usb);
                }
            }
            _configList.Add(slot, cList);
        }

        private static void InitLTR114New(uint slot)
        {
            if (Globals.Emulation)
                return;
            LTRManager.LTR114SetInterframeInterval(slot, 0);
            LTRManager.LTR114SetFreqDiv(slot, 2);
        }

        private static void InitLTR22New(uint slot)
        {
            if (Globals.Emulation)
                return;
            LTRManager.LTR22SetACDCState(slot, false);
            LTRManager.LTR22SetMeasADCZero(slot, false);
            LTRManager.LTR22SetFreq(slot, 1);
        }

        public void AddMeasure(USBBaseNew usb)
        {
            if (!_slots.Contains(usb.Slot))
                _slots.Add(usb.Slot);
            _usbList.Add(usb);
        }

        public void AddMeasureList(IList<USBBaseNew> lst)
        {
            lock (_syncObject)
            {
                foreach (USBBaseNew usb in lst)
                    AddMeasure(usb);
            }
        }

        public void StartWorkerWithList(IList<USBBaseNew> lst)
        {
            AddMeasureList(lst);
            Init();
            StartMeasuring();
        }

        private void ClearAllLists(bool writeDb)
        {
            if (_usbList.Count <= 0)
                return;
            List<USBBaseNew> tList = new List<USBBaseNew>();
            //временный список для хранения сигналов, которые не нужно удалять (зафиксированы)
            int min = Math.Min(_holdSignals, _usbList.Count);
            for (int i = 0; i < min; i++)
                tList.Add(_usbList[i]);
            if (writeDb)
                for (int i = min; i < _usbList.Count; i++)
                {
                    _usbList[i].Save();
                }
            _usbList.Clear();
            _slots.Clear();
            AddMeasureList(tList);
            Init();
        }

        private object _syncObject = 1;
        /// <summary>
        /// Запуск измерений
        /// </summary>
        public void StartMeasuring()
        {
            lock (_syncObject)
            {
                _ltrError = false;
                _worker = new Thread(MeasureCycle);
                _worker.IsBackground = true;
                _worker.Start();
                _isWorking = true;
            }
        }

        /// <summary>
        /// Остановка измерений
        /// </summary>
        public void StopMeasuring(bool writeDb)
        {
            lock (_syncObject)
            {
                if (!_isWorking)
                    return;
                _cancelEvent.Reset();
                _configList.Clear();
                if (!_cancelEvent.WaitOne(_waitTimeout, false))
                    if (_worker != null)
                    {
                        _worker.Abort();
                        _worker.Join();
                        _cancelEvent.Set();
                    }
                ClearAllLists(writeDb);
                _isWorking = false;
            }
        }

        private const int NumPointsLTR114 = 512;
        private const int NumPointsLTR11 = 1024;
        public bool ExpandPointsLTR11 = false;
        public int KoefLtr11 = 10;
        //private const int NumPointsLTR22 = 50000;        
        private const int NumPointsLTR22 = 16384;
        public bool ExpandPointsLTR22 = false;
        public int KoefLtr22 = 4;
        public const int freqSamples22 = 78125;

        private void MeasureCycle()
        {
            bool isFirstLoop = true;
            try
            {
                while (_configList.Count > 0) //всегда true должно быть, выход = принудительное завершение процесса
                {
                    foreach (KeyValuePair<int, ChannelLists> slotList in _configList)
                    {
                        bool confNeed = AnyInSlotListNeedConfigure(slotList);
                        if ((isFirstLoop) || (slotList.Value.Separate.Count != 0) || confNeed) //TODO если CheckCorrectRange - false, то isFirstLoop - true
                        {

                            if (!ProcessSeparateChannels(slotList))
                                return;

                            if (!Globals.Emulation)
                                if (slotList.Value.Common.Count > 0)
                                    switch (USBBaseNew.GetTypeModule((uint)slotList.Key))
                                    {
                                        case USBBaseNew.LTRType.LTR114:
                                            if (!SetupChannelsLTR114(slotList.Value.Common))
                                                continue;
                                            break;
                                        case USBBaseNew.LTRType.LTR22:
                                            if (!SetupChannelsLTR22(slotList.Value.Common))
                                                continue;
                                            break;
                                        case USBBaseNew.LTRType.LTR11:
                                            if (!SetupChannelsLTR11(slotList.Value.Common))
                                                continue;
                                            break;
                                    }
                        }

                        if (slotList.Value.Common.Count > 0)
                        {
                            if (!ProcessCommonChannels(slotList))
                                return;
                        }
                    }
                    if (ResultsReady != null)
                        ResultsReady(_usbList);
                    isFirstLoop = false;
                }
            }
            catch
            {
            }
            _cancelEvent.Set();
        }

        private bool AnyInSlotListNeedConfigure(KeyValuePair<int, ChannelLists> slotList)
        {
            foreach (var item in slotList.Value.Common)
                if (confDictionary.ContainsKey((uint)item.Slot))
                    if (confDictionary[(uint)item.Slot].ContainsKey(item.Channel))
                        if (confDictionary[(uint)item.Slot][item.Channel].ConfigureNeed)
                            return true;
            return false;
        }

        private bool ProcessCommonChannels(KeyValuePair<int, ChannelLists> slotList)
        {
            uint numPoints = 512;
            if (Globals.Emulation)
            {
                FillEmulationResult(slotList.Value.Common);
                return true;
            }

            switch (USBBaseNew.GetTypeModule((uint)slotList.Key))
            {
                case USBBaseNew.LTRType.LTR114:
                    numPoints = NumPointsLTR114;
                    break;
                case USBBaseNew.LTRType.LTR22:
                    numPoints = ExpandPointsLTR22 ? (uint)(NumPointsLTR22 * KoefLtr22) : NumPointsLTR22;
                    break;
                case USBBaseNew.LTRType.LTR11:
                    numPoints = _ltr11CustomSetup
                                  ? _ltr11Points
                                  : (uint)(ExpandPointsLTR11 ? NumPointsLTR11 * KoefLtr11 : NumPointsLTR11);
                    break;
            }
            try
            {
                List<double[]> result = LTRManager.GetData((uint)slotList.Key, numPoints);
                for (int i = 0; i < slotList.Value.Common.Count; i++)
                {
                    double[] points = result[i];
                    RenewRange((uint)slotList.Value.Common[i].Slot, slotList.Value.Common[i].Channel, points);
                }
                CalcResult(result, slotList.Value.Common);
            }
            catch (Exception ex)
            {
                // если возникает ошибка на платах, то завершаем измерения, отправляем событие и выходим из функции
                if (ex is ThreadAbortException)
                {
                    _cancelEvent.Set();
                    return false;
                }
                _ltrError = true;
                if (ErrorOnLTR != null)
                    ErrorOnLTR();
                StopMeasuring(false);
                return false;
            }

            return true;
        }

        private bool ProcessSeparateChannels(KeyValuePair<int, ChannelLists> slotList)
        {
            List<USBBaseNew> current = new List<USBBaseNew>();
            for (int ch = 0; ch < slotList.Value.Separate.Count; ch++)
            {
                current.Add(slotList.Value.Separate[ch]);
                if (Globals.Emulation)
                    FillEmulationResult(current);
                else
                {
                    try
                    {
                        List<double[]> result = null;
                        try
                        {
                            result = SetupChannelsAndGetData((uint)slotList.Key, current);
                        }
                        catch
                        {
                            continue;
                        }

                        if (result != null)
                        {
                            for (int i = 0; i < current.Count; i++)
                            {
                                double[] points = result[i];
                                RenewRange((uint)current[i].Slot, current[i].Channel, points);
                            }
                            CalcResult(result, current);
                        }

                    }
                    catch (Exception ex)
                    {
                        // если возникает ошибка на платах, то завершаем измерения, отправляем событие и выходим из функции
                        if (ex is ThreadAbortException)
                        //если это принудительное завершение процесса, то все ок
                        {
                            _cancelEvent.Set();
                            return false;
                        }
                        _ltrError = true;
                        if (ErrorOnLTR != null)
                            ErrorOnLTR();
                        StopMeasuring(false);
                        return false;
                    }
                }
                current.Clear();
            }

            return true;
        }

        private List<double[]> SetupChannelsAndGetData(uint slotListKey, List<USBBaseNew> current)
        {
            List<double[]> result = null;

            switch (USBBaseNew.GetTypeModule(slotListKey))
            {
                case USBBaseNew.LTRType.LTR114:
                    if (!SetupChannelsLTR114(current))
                        throw new Exception("Setup channels LTR114 failed");
                    result = LTRManager.GetData((uint)current[0].Slot, NumPointsLTR114);
                    break;
                case USBBaseNew.LTRType.LTR22:
                    if (!SetupChannelsLTR22(current))
                        throw new Exception("Setup channels LTR22 failed");
                    result = LTRManager.GetData((uint)current[0].Slot,
                    ExpandPointsLTR22
                        ? (uint)(NumPointsLTR22 * KoefLtr22)
                        : NumPointsLTR22);
                    break;
                case USBBaseNew.LTRType.LTR11:
                    if (!SetupChannelsLTR11(current))
                        throw new Exception("Setup channels LTR11 failed");
                    result = LTRManager.GetData((uint)current[0].Slot,
                    _ltr11CustomSetup
                        ? _ltr11Points
                        : (uint)
                        (ExpandPointsLTR11
                            ? NumPointsLTR11 * KoefLtr11
                            : NumPointsLTR11));
                    break;
            }

            return result;
        }

        private void FillEmulationResult(List<USBBaseNew> current)
        {
            foreach (USBBaseNew usbBase in current)
            {
                usbBase.Result = Imitator.Hardware.HImitator.LTRCards.GetVoltage(usbBase.Slot, usbBase.Channel);
                if (usbBase.OrderedFunctions != null && usbBase.Slot != 11 && usbBase.Channel != 8)
                {
                    foreach (Functions.CalcFunction cf in usbBase.OrderedFunctions)
                        if ((cf == Functions.CalcFunction.PairSubtraction))
                            usbBase.AddFunctionResult(cf, usbBase.Result);
                        else
                            usbBase.AddFunctionResult(cf, usbBase.Result);
                }
                Thread.Sleep(30);
            }
        }

        private void CalcResult(IList<double[]> lstArDouble, List<USBBaseNew> chList)
        {
            if (!_cancelEvent.WaitOne(0, false))
                return;
            if (lstArDouble == null)
                return;
            bool flg = USBBaseNew.GetTypeModule((uint)chList[0].Slot) != USBBaseNew.LTRType.LTR22;
            for (int i = 0; i < chList.Count; i++)
            {
                if (chList[i].OrderedFunctions != null)
                {
                    foreach (Functions.CalcFunction cf in chList[i].OrderedFunctions)
                        if ((cf == Functions.CalcFunction.PairSubtraction) && (i % 2 == 0))
                            continue;
                        else if ((cf == Functions.CalcFunction.PairSubtraction))
                            chList[i].AddFunctionResult(cf, Functions.CalcFunc(cf, lstArDouble[i], lstArDouble[i - 1]));
                        else
                            chList[i].AddFunctionResult(cf, Functions.CalcFunc(cf, lstArDouble[i]));
                }
                else
                    chList[i].Result =
                      flg
                        ? Functions.CalcMean(lstArDouble[i])
                        : Functions.CalcRMS(
                          lstArDouble[
                            i]); //flg ? Functions.CalcMean(lstArDouble[i]) : Functions.CalcRMS(lstArDouble[i]); Делаем только RMS т.к. оно заменяет среднее.
                                 //if (chList[i].ParamName == "Plus_6V" && _isDiss13)
                                 //    chList[i].Result = Functions.CalcRMS(lstArDouble[i]); 

                if (freqCalculated != null)
                {
                    if (chList[i].ParamName == freqParam)
                        freqCalculated(Functions.CalcFrequency(lstArDouble[i], freqSamples22));
                }
            }
        }

        private uint _ltr11Slot = 0;
        private uint _ltr11Divider = 0;
        private uint _ltr11Points = 0;
        private bool _ltr11CustomSetup = false;

        public void SetupLTR11(uint slot, uint divider, uint points)
        {
            if (divider < 37)
                _ltr11CustomSetup = false;
            else
            {
                _ltr11Slot = slot;
                _ltr11Points = points;
                _ltr11Divider = divider;
                _ltr11CustomSetup = true;
            }
        }

        //private struct SettedParametersLTR
        //{

        //}

        private bool SetupChannelsLTR11(IList<USBBaseNew> chList)
        {
            if (Globals.Emulation)
                return true;
            if ((_ltr11CustomSetup) && ((uint)chList[0].Slot == _ltr11Slot))
            {
                try
                {
                    LTRManager.LTR11SetupAdc((uint)chList[0].Slot, LTRCards.LTR11.StartADCMode.INT,
                        LTRCards.LTR11.INPMode.INT, (uint)chList.Count, _ltr11Divider);
                }
                catch
                {
                    return false;
                }
            }
            else
                LTRManager.LTR11SetupAdc((uint)chList[0].Slot, LTRCards.LTR11.StartADCMode.INT,
                                         LTRCards.LTR11.INPMode.INT, (uint)chList.Count, 300);
            //if (!LTRManager.LTR11SetupAdc((uint)chList[0].Slot, (uint)chList.Count))
            //  return false;

            for (int i = 0; i < chList.Count; i++)
            {
                try
                {
                    LTRManager.LTR11SetupChannelSimple((uint)chList[i].Slot, (uint)i, (uint)(chList[i].Channel - 1),
                        LTRCards.LTR11.ChannelWorkMode.b16Low,
                        GetRangeLTR11((uint)chList[i].Slot, chList[i].Channel));
                    _logger.Trace("Переключение на диапазон " + GetRange((uint)chList[i].Slot, chList[i].Channel) + " на слоте: " + chList[i].Slot + " на канале: " +chList[i].Channel);
                    //(LTRCards.LTR11.ChannelRange) chList[i].ReadyRange);
                }
                catch
                {
                    return false;
                }
            }

            try
            {
                LTRManager.LTR11SetAdc((uint)chList[0].Slot);
            }
            catch
            {
                return false;
            }

            return true;
        }

        private static bool SetupChannelsLTR22(IList<USBBaseNew> chList)
        {
            if (Globals.Emulation)
                return true;
            uint slot = (uint)(chList[0].Slot);
            for (int i = 0; i < 4; i++) // выключили все каналы 
                if (!LTRManager.LTR22SetOnOffChannel(slot, (uint)i, false))
                    return false;

            for (int i = 0; i < chList.Count; i++)
            {
                // включаем каналы кот.нам нужны
                if (!LTRManager.LTR22SetOnOffChannel(slot, (uint)(chList[i].Channel - 1), true))
                    return false;
                if (
                    !LTRManager.LTR22SetADCRange(slot, (uint) (chList[i].Channel - 1),
                        GetRangeLTR22(slot, chList[i].Channel)))
                {
                    Logger _loggerTrace = LogManager.GetLogger("KPK2Trace");
                    _loggerTrace.Trace("Переключение на диапазон " + GetRange(slot, chList[i].Channel) + " на слоте: " + slot + " на канале: " + chList[i].Channel);
                    return false;
                }
                    //(LTRCards.LTR22.ChannelRange)chList[i].ReadyRange))
                    
            }
            return true;
        }

        private static bool SetupChannelsLTR114(IList<USBBaseNew> chList)
        {
            if (Globals.Emulation)
                return true;
            if (!LTRManager.LTR114SetActiveChannels((uint)chList[0].Slot, (uint)chList.Count))
                return false;
            for (int i = 0; i < chList.Count; i++)
                if (
                    !LTRManager.LTR114SetChannelParams((uint) chList[i].Slot, (uint) i, (uint) (chList[i].Channel - 1),
                        LTRCards.LTR114.MeasMode.U,
                        GetRangeLTR114((uint) chList[i].Slot, chList[i].Channel)))
                {
                    Logger _loggerTrace = LogManager.GetLogger("KPK2Trace");
                    _loggerTrace.Trace("Переключение на диапазон " + GetRange((uint)chList[i].Slot, chList[i].Channel) + " на слоте: " + chList[i].Slot + " на канале: " + chList[i].Channel);
                    return false;
                }
                    //(LTRCards.LTR114.ChannelRange) chList[i].ReadyRange))
                    
            return true;
        }

        /// <summary>
        /// Вложенный словарь для работы с диапазонами на каналах и платах
        /// </summary>
        public static Dictionary<uint, Dictionary<int, CorrectRangeLTRMeasure>> confDictionary = new Dictionary<uint, Dictionary<int, CorrectRangeLTRMeasure>>();

        /// <summary>
        /// Получить диапазон из словоря, если первый раз измеряем - то добовляем значение в словарь
        /// </summary>
        /// <param name="slot">Номер слота</param>
        /// <param name="channel">Номер канала</param>
        /// <returns>Диапазон для канала, в Вольтах</returns>
        public static double GetRange(uint slot, int channel)
        {

            if (!confDictionary.ContainsKey(slot))
            {
                confDictionary.Add(slot, new Dictionary<int, CorrectRangeLTRMeasure>());
            }
            var channelDictionary = confDictionary[slot];
            if (!channelDictionary.ContainsKey(channel))
            {
                var plate = USBBaseNew.GetTypeModule(slot);
                double[] ranges = { };
                switch (plate)
                {
                    case USBBaseNew.LTRType.LTR11:
                        ranges = LTRRanges.LTR11Ranges.Values.ToArray();
                        break;
                    case USBBaseNew.LTRType.LTR22:
                        ranges = LTRRanges.LTR22Ranges.Values.ToArray();
                        break;
                    case USBBaseNew.LTRType.LTR114:
                        ranges = LTRRanges.LTR114Ranges.Values.ToArray();
                        break;

                }
                channelDictionary.Add(channel, new CorrectRangeLTRMeasure(ranges, ranges.Max()));
            }
            Logger _loggerTrace = LogManager.GetLogger("KPK2Trace");
            _loggerTrace.Trace("Диапазон на слоте : " + slot + " - " + confDictionary[slot][channel].Range + " Канал - " + channel);
            confDictionary[slot][channel].ConfigureNeed = false;
            return confDictionary[slot][channel].Range;
        }

        /// <summary>
        /// Получить диапазон на LTR11 для канала
        /// </summary>
        /// <param name="slot">Номер слота</param>
        /// <param name="channel">Номер канала</param>
        /// <returns>Диапазон, в Вольтах</returns>
        public static LTRCards.LTR11.ChannelRange GetRangeLTR11(uint slot, int channel)
        {
            var range = GetRange(slot, channel); 
            var key = LTRRanges.LTR11Ranges.First(x => Math.Abs(x.Value - range) < double.Epsilon).Key;
            return key;
        }

        /// <summary>
        /// Получить диапазон на LTR22 для канала
        /// </summary>
        /// <param name="slot">Номер слота</param>
        /// <param name="channel">Номер канала</param>
        /// <returns>Диапазон, в Вольтах</returns>
        public static LTRCards.LTR22.ChannelRange GetRangeLTR22(uint slot, int channel)
        {
            var range = GetRange(slot, channel);
            var key = LTRRanges.LTR22Ranges.First(x => Math.Abs(x.Value - range) < double.Epsilon).Key;
            return key;
        }

        /// <summary>
        /// Получить диапазон на LTR114 для канала
        /// </summary>
        /// <param name="slot">Номер слота</param>
        /// <param name="channel">Номер канала</param>
        /// <returns>Диапазон, в Вольтах</returns>
        public static LTRCards.LTR114.ChannelRange GetRangeLTR114(uint slot, int channel)
        {
            var range = GetRange(slot, channel);
            var key = LTRRanges.LTR114Ranges.First(x => Math.Abs(x.Value - range) < double.Epsilon).Key;
            return key;
        }


        /// <summary>
        /// Обновить диапазон
        /// </summary>
        /// <param name="plate">Плата</param>
        /// <param name="channel">Канал</param>
        /// <param name="signal">Сигнал</param>
        private void RenewRange(uint plate, int channel, double[] signal)
        {
            confDictionary[plate][channel].CheckCorrectRange(signal);
        }



        private static List<double[]> EmulationGetData(int channelsCount, int pointsCount)
        {
            Random rnd = new Random(Environment.TickCount);
            List<double[]> result = new List<double[]>(channelsCount);
            for (int channelNum = 0; channelNum < result.Count; channelNum++)
            {
                double[] points = new double[pointsCount];
                for (int pointNum = 0; pointNum < pointsCount; pointNum++)
                    points[pointNum] = rnd.Next();
                result[channelNum] = points;
            }
            return result;
        }

        #endregion

        public void ClearFreqCalculated()
        {
            freqCalculated = null;
        }
    }
}
