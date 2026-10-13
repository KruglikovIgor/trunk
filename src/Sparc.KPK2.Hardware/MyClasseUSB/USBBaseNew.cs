using LCards;
using NLog;
using Sparc.KPK2.NFunctions;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace Sparc.KPK2.Hardware.MyClasseUSB
{
    public class USBBaseNew
    {
        private const double _task4_258 = 258;
        private const double _task4_95 = 9.5;
        private const double _task3_127 = 127;


        private static Logger _logger = LogManager.GetLogger("KPK2Trace");
        public enum LTRType
        {
            LTR11,
            LTR114,
            LTR22
        }

        private const int DefaultIntegratorDepth = 3;

        private int _blockID;
        private string _paramName;
        private readonly int _indicatorNumber;
        //private int _paramID;
        private readonly double _min;
        private readonly double _max;
        private readonly int _slot;
        private readonly int _channel;
        private double _range;
        private int _readyRange;
        private readonly bool _alone;
        /// <summary>
        /// DC- True, AC - False
        /// </summary>
        private readonly bool _ACDC;
        private readonly LTRType _typeLTR;
        private readonly Integrator chIntegrator = new Integrator();
        private double _result = double.NaN;
        private readonly double _koef = 1;
        private readonly double _addLinear = 0;
        private bool _needProcessing = false;
        private double _maxResult = 0;

        private readonly int _precision;
        private string _additional;
        private List<Functions.CalcFunction> _orderedFunctions;
        private Dictionary<int, double> _functionResults;

        private string[] nameParamConstValue17 = new string[]
        {
            "Mnl_Forward_32_RepairTest",
            "Mnl_Left_32_RepairTest",
            "Mnl_Back_32_RepairTest",
            "Mnl_Right_32_RepairTest",
            "BPK_Forward_32_RepairTest",
            "BPK_Left_32_RepairTest",
            "BPK_Back_32_RepairTest",
            "BPK_Right_32_RepairTest"
        };

        private string[] nameParamConstValue3 = new string[]
        {
            "Mnl_Down_32_RepairTest",
            "Mnl_Up_32_RepairTest",
            "BPK_Down_32_RepairTest",
            "BPK_Up_32_RepairTest"
        };


        private string[] nameParamConstValue0 = new string[]
        {
            "Task3_Y0_32_RepairTest",
            "Mnl_Alfa_0_32_RepairTest",
            "BPK_YC0_32_RepairTest"
        };

        string[] nameParamConstValue258 = new string[]
        {
                "Task4_PC_32_RepairTest",
                "Mnl_Spd_Task4_32_RepairTest",
                "BPK_PC2_32_RepairTest",
                "BPK_PC2_32_FlightVSS_RepairTest"
        };

        string[] nameParamConstValue127 = new string[]
        {
                "Task3_PC_32_RepairTest",
                "Mnl_Spd_Task3_32_RepairTest",
                "BPK_PC1_32_RepairTest",
                "BPK_PC1_32_FlightVSS_RepairTest"
        };

        string[] nameParamConstValue95 = new string[]
        {
                "Task4_Y9_32_RepairTest",
                "Mnl_Alfa_9_32_RepairTest",
                "BPK_YC9_32_RepairTest"
        };

        public double Koef
        {
            get { return _koef; }
        }

        public bool IsAC
        {
            get { return !_ACDC; }
        }

        public List<Functions.CalcFunction> OrderedFunctions
        {
            get { return _orderedFunctions; }
        }

        public string ParamName
        {
            get { return _paramName; }
        }

        public int IndicatorNumber
        {
            get { return _indicatorNumber; }
        }

        public int Slot
        {
            get { return _slot; }
        }

        public bool IsAlone
        {
            get { return _alone; }
        }

        public int Channel
        {
            get { return _channel; }
        }

        public int ReadyRange
        {
            get { return _readyRange; }
        }

        public double Min
        {
            get { return _min; }
        }

        public double Max
        {
            get { return _max; }
        }

        public int Precision
        {
            get { return _precision; }
        }

        public double MaxResult
        {
            get { return _maxResult; }
            set
            {
                if (value > _maxResult)
                    _maxResult = value;
            }
        }

        public double Result
        {
            get
            {
                double result = _result * _koef + _addLinear;

                if (_needProcessing)
                    result = Globals.Processing(_paramName, result);
                if (Globals.Emulation)
                    return result;
                return result;
            }
            set
            {
                chIntegrator.AddValue(0, value);
                _result = chIntegrator.GetValue(0);
                MaxResult = Result;
            }
        }

        public double GetFunctionResult(Functions.CalcFunction function)
        {
            if (_functionResults == null)
                return double.NaN;
            int position = (int)function;
            if (_functionResults.ContainsKey(position))
                return _functionResults[position];
            return double.NaN;
        }

        public void AddFunctionResult(Functions.CalcFunction function, double value)
        {
            value = value * _koef;
            int position = (int)function;
            if (_functionResults == null)
                _functionResults = new Dictionary<int, double>();
            if (_functionResults.ContainsKey(position))
                _functionResults[position] = value;
            else
                _functionResults.Add(position, value);
        }

        public USBBaseNew(string paramName, int indicatorNumber) : this(-1, paramName, indicatorNumber)
        {
        }


        public USBBaseNew(int blockID, string paramName, int indicatorNumber)
        {

            _blockID = blockID;
            _paramName = paramName;
            _indicatorNumber = indicatorNumber;
            if (
              !DataManager.Instance
                          .GetParamLimitKoefAndPrecision(paramName, ref _min, ref _max, ref _koef, ref _precision,
                                                         ref _additional))
                throw new ApplicationException(Resources.Properties.Resources.CantReadDb + $@" данные из-за которых ошибка {_blockID}, {_paramName}, {_indicatorNumber}");



            double min = 0, max = 0;
            double koef = 0;
            int precision = 0;
            string additional = string.Empty;
            string inParamName = string.Empty;

            if (DataManager.Instance
                           .GetParamLimitKoefAndPrecisionInnerParam(paramName, ref min, ref max, ref koef, ref precision,
                                                                    ref additional, ref inParamName))
                _koef *= koef;
            else
                inParamName = paramName;
            if (Globals.Emulation && (paramName == "Plus_27V_32"))
                _koef = 1;
            Double.TryParse(_additional, NumberStyles.Any, CultureInfo.InvariantCulture.NumberFormat, out _addLinear);
            _needProcessing = Globals.NeedProcessing(paramName);
            if (DataManager.Instance.GetParamsLTR(inParamName, ref _slot, ref _channel, ref _range, ref _alone, ref _ACDC))
            {
                _slot = (int)LTRSupport.GetLTRSlotFromConfigByNum(_slot);
                _typeLTR = GetTypeModule((uint)_slot);
                SetReadyRange();
            }
            _logger.Trace("ParamName:{0}, Slot:{1}", paramName, _slot);
            chIntegrator.DefaultDepth = DefaultIntegratorDepth;


            if (nameParamConstValue258.Contains(paramName))
            {
                _max = _max + 258;
                _min += 258;
            }
            else if (nameParamConstValue127.Contains(paramName))
            {
                _max += 127;
                _min += 127;
            }
            else if (nameParamConstValue95.Contains(paramName))
            {
                _max += 9.5;
                _min += 9.5;
            }
        }

        public USBBaseNew(int blockID, string paramName, int indicatorNumber,
                          params Functions.CalcFunction[] orderedFunctions)
        {       
            _blockID = blockID;
            _paramName = paramName;
            _indicatorNumber = indicatorNumber;
            if (
              !DataManager.Instance
                          .GetParamLimitKoefAndPrecision(paramName, ref _min, ref _max, ref _koef, ref _precision,
                                                         ref _additional))
                throw new ApplicationException(Resources.Properties.Resources.CantReadDb);
            Double.TryParse(_additional, out _addLinear);
            _needProcessing = Globals.NeedProcessing(paramName);


            if (DataManager.Instance.GetParamsLTR(paramName, ref _slot, ref _channel, ref _range, ref _alone, ref _ACDC))
            {
                _slot = (int)LTRSupport.GetLTRSlotFromConfigByNum(_slot);
                _typeLTR = GetTypeModule((uint)_slot);
                SetReadyRange();
            }

            _logger.Trace("ParamName:{0}, Slot:{1}", paramName, _slot);
            chIntegrator.DefaultDepth = DefaultIntegratorDepth;

            _orderedFunctions = new List<Functions.CalcFunction>();
            foreach (Functions.CalcFunction cf in orderedFunctions)
                _orderedFunctions.Add(cf);
        }

        public static LTRType GetTypeModule(uint slot)
        {
            if (Globals.Emulation)
                return LTRType.LTR11;
            string moduleStr = LTRManager.GetModuleBySlot(slot);
            if (moduleStr == "LTR11")
                return LTRType.LTR11;
            if (moduleStr == "LTR114")
                return LTRType.LTR114;
            if (moduleStr == "LTR22")
                return LTRType.LTR22;
            _logger.Trace("Slot:{0} BoardType:{1}", slot, moduleStr);
            throw new ApplicationException(string.Format(Resources.Properties.Resources.ErrorTypeModuleNBySlotN, slot, moduleStr));
        }

        public void SetRange(double valueRange)
        {
            _range = valueRange;
            SetReadyRange();
        }

        private void SetReadyRange()
        {
            if (_typeLTR == LTRType.LTR11)
                _readyRange = _range == 10 ? 0 : _range == 2.5 ? 1 : _range == 0.6 ? 2 : 3; //_range == 0.15 -> 3
            if (_typeLTR == LTRType.LTR22)
                _readyRange = _range == 10
                                ? 4
                                : _range == 3 ? 5 : _range == 1 ? 0 : _range == 0.3 ? 1 : _range == 0.1 ? 2 : 3;
            // _range == 0.03 -> 3                
            if (_typeLTR == LTRType.LTR114)
                _readyRange = _range == 10 ? 0 : _range == 2 ? 1 : 2; // _range == 0.4 -> 2                 
        }

        internal bool Save()
        {
            if (_paramName == "SVCh_P")
                return DataManager.Instance.SaveParameter(_blockID, _paramName, MaxResult);
            //if (_paramName == "Z_Task_32" || _paramName == "Z_PNP_32")
            //return DataManager.Instance.SaveParameter(_blockID, _paramName, Result*-1);
            if (_paramName == "Delta_27V_15")
                return true;

            if (nameParamConstValue258.Contains(_paramName))
            {

                return DataManager.Instance.SaveParameter(_blockID, _paramName, Result - 258);
            }
            else if (nameParamConstValue127.Contains(_paramName))
            {

                return DataManager.Instance.SaveParameter(_blockID, _paramName, Result - 127);
            }
            else if (nameParamConstValue95.Contains(_paramName))
            {
                return DataManager.Instance.SaveParameter(_blockID, _paramName, Result - 9.5);
            }
            return DataManager.Instance.SaveParameter(_blockID, _paramName, Result);
        }

        private void SetValueMaxAndMin(string paramName)
        {
        }
    }
}
