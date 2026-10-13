using System;
using System.Collections.Generic;
using System.Linq;

namespace KPK2.Common.MeasureVoltage
{
    public class UNCHArray
    {
        private static Dictionary<string, IEnumerable<double>> _controlPoints;
        private static Dictionary<string, UNCH> _UNCH;

        /// <summary>
        /// Сигналы УНЧ 1-1
        /// </summary>
        private static readonly int[] _arrayFreq = new[] { 20, 100, 1000, 5000 };
        private static readonly double[] _arrayUNCH = new[] { 0.05, 0.1, 0.15, 0.2, 0.5, 1, 2, 3};
        private static readonly double[,] _voltageUNCH = { { 0, 8.0, 0.05 } };
        private static readonly UNCH _v_voltageUNCH_11 = new UNCH(ADCDirect.VoltageUNCH_11, _arrayUNCH, _voltageUNCH, _arrayFreq);
        /// <summary>
        /// Сигналы УНЧ 1-2
        /// </summary>
        private static readonly UNCH _v_voltageUNCH_12 = new UNCH(ADCDirect.VoltageUNCH_12, _arrayUNCH, _voltageUNCH, _arrayFreq);
        /// <summary>
        /// Сигналы УНЧ 1-3
        /// </summary>
        private static readonly UNCH _v_voltageUNCH_13 = new UNCH(ADCDirect.VoltageUNCH_13, _arrayUNCH, _voltageUNCH, _arrayFreq);
        /// <summary>
        /// Сигналы УНЧ 2-1
        /// </summary>
        private static readonly UNCH _v_voltageUNCH_21 = new UNCH(ADCDirect.VoltageUNCH_21, _arrayUNCH, _voltageUNCH, _arrayFreq);
        /// <summary>
        /// Сигналы УНЧ 2-2
        /// </summary>
        private static readonly UNCH _v_voltageUNCH_22 = new UNCH(ADCDirect.VoltageUNCH_22, _arrayUNCH, _voltageUNCH, _arrayFreq);
        /// <summary>
        /// Сигналы УНЧ 2-3
        /// </summary>
        private static readonly UNCH _v_voltageUNCH_23 = new UNCH(ADCDirect.VoltageUNCH_23, _arrayUNCH, _voltageUNCH, _arrayFreq);

        private static void FillDictUNCH()
        {
            if (_UNCH != null)
                return;

            _UNCH = new Dictionary<string, UNCH>();

           _UNCH.Add("Сигнал УНЧ 1-1", _v_voltageUNCH_11);
            _UNCH.Add("Сигнал УНЧ 1-2", _v_voltageUNCH_12);
            _UNCH.Add("Сигнал УНЧ 1-3", _v_voltageUNCH_13);
            _UNCH.Add("Сигнал УНЧ 2-1", _v_voltageUNCH_21);
            _UNCH.Add("Сигнал УНЧ 2-2", _v_voltageUNCH_22);
            _UNCH.Add("Сигнал УНЧ 2-3", _v_voltageUNCH_23);

        }

        private static void FillDictCtrlPoints()
        {
            if (_controlPoints != null)
                return;

            _controlPoints = new Dictionary<string, IEnumerable<double>>();

            _controlPoints.Add("Сигнал УНЧ 1-1", _arrayUNCH);
            _controlPoints.Add("Сигнал УНЧ 1-2", _arrayUNCH);
            _controlPoints.Add("Сигнал УНЧ 1-3", _arrayUNCH);
            _controlPoints.Add("Сигнал УНЧ 2-1", _arrayUNCH);
            _controlPoints.Add("Сигнал УНЧ 2-2", _arrayUNCH);
            _controlPoints.Add("Сигнал УНЧ 2-3", _arrayUNCH);
        }

        /// <summary>
        /// возвращает массив напряжений для тестируемого измерительного канала
        /// </summary>
        /// <param name="ikName">измерительный канал</param>
        /// <returns>массив устанавливаемых напряжений</returns>
        public static IEnumerable<double> GetUNCHArray(string ikName)
        {
            FillDictCtrlPoints();
            if (_controlPoints.ContainsKey(ikName))
            {
                IEnumerable<double> UNCHArray;
                _controlPoints.TryGetValue(ikName, out UNCHArray);
                return UNCHArray;
            }
            throw new Exception("Выбранный канал не проверяется автоматической поверкой");
        }

        /// <summary>
        /// возвращает параметры тестирования: перечень контрольных точек и диапазоны
        /// </summary>
        /// <param name="ikName">измерительный канал</param>
        /// <returns></returns>
        public static UNCH GetUNCH(string ikName)
        {
            FillDictUNCH();
            if (_UNCH.ContainsKey(ikName))
            {
                UNCH UNCH;
                _UNCH.TryGetValue(ikName, out UNCH);
                return UNCH;
            }
            throw new Exception("Выбранный канал не проверяется автоматической поверкой");
        }

        /// <summary>
        /// Проверка на наличие канала
        /// </summary>
        /// <param name="nameChannal">имя канала</param>
        /// <returns></returns>
        public static bool IfContains(string nameChannal)
        {
            FillDictCtrlPoints();
            if (_controlPoints.ContainsKey(nameChannal))
                return true;
            return false;
        }
    }

    /// <summary>
    /// Класс, содержащий сведения о диапазоне проверяемых напряжений,
    /// их контрольных точках, и диапазонах погрешностей
    /// </summary>
    public class UNCH
    {
        /// <summary>
        /// Диапазоны проверяемых значений
        /// </summary>
        public ADCDirect RangeNames { get; set; }

        /// <summary>
        /// Контрольные точки
        /// </summary>
        public double[] ControlPoints { get; set; }

        /// <summary>
        /// Контрольные точки
        /// </summary>
        public int[] FreqPoints { get; set; }

        /// <summary>
        /// Диапазоны погрешностей
        /// </summary>
        public double[,] ErrorsRanges { get; set; }

        public UNCH()
        {

        }

        public UNCH(ADCDirect rangeNames, IEnumerable<double> controlPoints, double[,] errorsRanges, int[] freqPoint)
        {
            RangeNames = rangeNames;
            ControlPoints = controlPoints.ToArray();
            ErrorsRanges = errorsRanges;
            FreqPoints = freqPoint;
        }
    }

    /// <summary>
    /// диапазоны проверяемых значений напряжений на допустимую погрешность
    /// </summary>
    public enum ACDirect
    {
        VoltageUNCH_11,
        VoltageUNCH_12,
        VoltageUNCH_13,
        VoltageUNCH_21,
        VoltageUNCH_22,
        VoltageUNCH_23
    }
}

