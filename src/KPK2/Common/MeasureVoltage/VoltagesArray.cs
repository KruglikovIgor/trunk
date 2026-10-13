using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KPK2.Common.MeasureVoltage
{
    /// <summary>
    /// Инициализирует и возвращает массив устанавливаемых напряжений тестирования
    /// для определенных измерительных каналов
    /// </summary>
    public static class VoltagesArray
    {
        private static Dictionary<string, IEnumerable<double>> _controlPoints;
        private static Dictionary<string, Voltages> _voltages;

        /// <summary>
        /// +6,3 В, +5 В, СД 1-1, СД 1-2, СД 1-3, ТКМ1, МОД1, ГЛПД 1, ТК 1-1(ДИСС-15), ТК 1-2	(ДИСС-15),
        /// ТК 1-3(ДИСС-15), ТК 1-1(ДИСС-32),	ТК 1-2(ДИСС-32),	ТК 1-3(ДИСС-32), Ток ГЛПД, Zорт (ПНП)
        /// -6,3 В, Р СВЧ, СД 2-1, СД 2-2, СД 2-3, ТКМ2, МОД2, ГЛПД2, ТК 2-1(ДИСС-15), ТК 2-2	(ДИСС-15),
        /// ТК 2-3	(ДИСС-15), ТК 2-1	(ДИСС-32), ТК 2-2	(ДИСС-32), ТК 2-3	(ДИСС-32), Ток М, Zорт (ПКВ)
        /// </summary>
        private static readonly double[] _arrayTable7 = new[] { 0.1, 1.0, 2.0, 5.0, 9.9, -9.9, -5.0, -2.0, -1.0, -0.1 };
        private static readonly double[,] _voltageTable7 = { { -11, -1.000001, 0.010 }, { -1.0, 1.0, 0.002 }, { 1.000001, 11, 0.010 } };
        private static readonly Voltages _v_voltageTable7 = new Voltages(ADCDirect.VoltagesTable7, _arrayTable7, _voltageTable7);

        /// <summary>
        /// Wx ПКВ, Wz ПКВ, Wy НКВ, Wx СПСВ, Wz СПСВ, Wy Тестер, W,
        /// Wy ПКВ, Wx НКВ, Wz НКВ, Wy СПСВ, Wx Тестер, Wz Тестер, L
        /// </summary>
        private static readonly double[] _arrayTable8 = new[] { 0.1, 1.0, 2.0, 5.0, 9.9, -9.9, -5.0, -2.0, -1.0, -0.1 };
        private static readonly double[,] _voltageTable8 = { { -11, -1.000001, 0.002 }, { -1.0, 1.0, 0.001 }, { 1.000001, 11, 0.002 } };
        private static readonly Voltages _v_voltageTable8 = new Voltages(ADCDirect.VoltagesTable8, _arrayTable8, _voltageTable8);


        /// <summary>
        /// +27 В, -27 В, +27 В сл., -27 В оп.
        /// </summary>
        private static readonly double[] _array27 = new[] { 15.0, 35.0, 49.0, -49.0, -30.0, -10.0 };
        private static readonly double[,] _voltage27 = { { -50.0, 50.0, 0.05 } };
        private static readonly Voltages _v_voltage27 = new Voltages(ADCDirect.Voltages27, _array27, _voltage27);

        /// <summary>
        /// +15 В, -15 В, -20 В
        /// </summary>
        private static readonly double[] _array15_20 = new[] { 5.0, 15.0, 29.0, -29.0, -20.0, -10.0 };
        private static readonly double[,] _voltage15_20 = { { -30.0, 30.0, 0.025 } };
        private static readonly Voltages _v_voltage15_20 = new Voltages(ADCDirect.Voltages15_20, _array15_20, _voltage15_20);

        /// <summary>
        /// +12,6 В, -12,6 В, +10,6 В
        /// </summary>
        private static readonly double[] _array12_10 = new[] { 6.0, 12.0, 19.0, -19.0, -10.0, -5.0 };
        private static readonly double[,] _voltage12_10 = { { -20.0, 20.0, 0.025 } };
        private static readonly Voltages _v_voltage12_10 = new Voltages(ADCDirect.Voltages12_10, _array12_10, _voltage12_10);

        /// <summary>
        /// ВН (-48В)
        /// </summary>
        private static readonly double[] _array48 = new[] { 5.0, 10.0, 25.0, 40.0, 60.0, 89.0, -5.0, -10.0, -25.0, -40.0, -60.0, -89.0 };
        private static readonly double[,] _voltage48 = { { -90.0, -40.000001, 0.5 }, { -40.0, -10.000001, 0.2 }, { -10.0, 10.0, 0.1 }, { 10.000001, 40.0, 0.2 }, { 40.000001, 90.0, 0.5 } };
        private static readonly Voltages _v_voltage48 = new Voltages(ADCDirect.Voltage48, _array48, _voltage48);

        /// <summary>
        /// Δ27 В
        /// </summary>
        private static readonly double[] _arrayDelta27 = new[] { 0.05, 0.1, 0.5, 1.0, 10.0, 25.0, 49.0, -49.0, -25.0, -10.0, -1.0, -0.5, -0.1, -0.05 };
        private static readonly double[,] _voltageDelta27 = { { -50.0, -1.000001, 0.025 }, { -1.0, -0.1000001, 0.015 }, { -0.1, 0.1, 0.010 }, { -0.1000001, 1.0, 0.015 }, { 1.000001, 50.0, 0.025 } };
        private static readonly Voltages _v_voltageDelta27 = new Voltages(ADCDirect.VoltageDelta27, _arrayDelta27, _voltageDelta27);

        /// <summary>
        /// Δ15 В
        /// </summary>
        private static readonly double[] _arrayDelta15 = new[] { 0.05, 0.1, 0.5, 1.0, 10.0, 20.0, 29.0, -29.0, -20.0, -10.0, -1.0, -0.5, -0.1, -0.05 };
        private static readonly double[,] _voltageDelta15 = { { -30.0, -0.1000001, 0.010 }, { -0.1, 0.1, 0.005 }, { 0.1000001, 30.0, 0.010 } };
        private static readonly Voltages _v_voltageDelta15 = new Voltages(ADCDirect.VoltageDelta15, _arrayDelta15, _voltageDelta15);

        /// <summary>
        /// Сигналы УНЧ 1-1
        /// </summary>
        private static readonly int[] _arrayFreq = new[] { 20, 100, 1000, 6000 };
        private static readonly double[] _arrayUNCH = new[] { 0.05, 0.1, 0.5, 1, 3, 7 };
        private static readonly double[,] _voltageUNCH = { { 5.0, -5.0, 0.05 } };
        private static readonly Voltages _v_voltageUNCH_11 = new Voltages(ADCDirect.VoltageUNCH_11, _arrayUNCH, _voltageUNCH,_arrayFreq);
        /// <summary>
        /// Сигналы УНЧ 1-2
        /// </summary>
        private static readonly Voltages _v_voltageUNCH_12 = new Voltages(ADCDirect.VoltageUNCH_12, _arrayUNCH, _voltageUNCH, _arrayFreq);
        /// <summary>
        /// Сигналы УНЧ 1-3
        /// </summary>
        private static readonly Voltages _v_voltageUNCH_13 = new Voltages(ADCDirect.VoltageUNCH_13, _arrayUNCH, _voltageUNCH, _arrayFreq);
        /// <summary>
        /// Сигналы УНЧ 2-1
        /// </summary>
        private static readonly Voltages _v_voltageUNCH_21 = new Voltages(ADCDirect.VoltageUNCH_21, _arrayUNCH, _voltageUNCH, _arrayFreq);
        /// <summary>
        /// Сигналы УНЧ 2-2
        /// </summary>
        private static readonly Voltages _v_voltageUNCH_22 = new Voltages(ADCDirect.VoltageUNCH_22, _arrayUNCH, _voltageUNCH, _arrayFreq);
        /// <summary>
        /// Сигналы УНЧ 2-3
        /// </summary>
        private static readonly Voltages _v_voltageUNCH_23 = new Voltages(ADCDirect.VoltageUNCH_23, _arrayUNCH, _voltageUNCH, _arrayFreq);

        private static void FillDictVoltages()
        {
            if (_voltages != null)
                return;

            _voltages = new Dictionary<string, Voltages>();

            // страница Напряжение1
            _voltages.Add("+6.3В", _v_voltageTable7);
            _voltages.Add("-6.3В", _v_voltageTable7);
            _voltages.Add("+5В", _v_voltageTable7);
            _voltages.Add("Р СВЧ", _v_voltageTable7);
            _voltages.Add("СД 1-1", _v_voltageTable7);
            _voltages.Add("СД 1-2", _v_voltageTable7);
            _voltages.Add("СД 1-3", _v_voltageTable7);
            _voltages.Add("СД 2-1", _v_voltageTable7);
            _voltages.Add("СД 2-2", _v_voltageTable7);
            _voltages.Add("СД 2-3", _v_voltageTable7);
            _voltages.Add("TK 1-1(ДИСС-15)", _v_voltageTable7);
            _voltages.Add("TK 1-2(ДИСС-15)", _v_voltageTable7);
            _voltages.Add("TK 1-3(ДИСС-15)", _v_voltageTable7);
            _voltages.Add("TK 2-1(ДИСС-15)", _v_voltageTable7);
            _voltages.Add("TK 2-2(ДИСС-15)", _v_voltageTable7);
            _voltages.Add("TK 2-3(ДИСС-15)", _v_voltageTable7);
            _voltages.Add("TK 1-1(ДИСС-32)", _v_voltageTable7);
            _voltages.Add("TK 1-2(ДИСС-32)", _v_voltageTable7);
            _voltages.Add("TK 1-3(ДИСС-32)", _v_voltageTable7);
            _voltages.Add("TK 2-1(ДИСС-32)", _v_voltageTable7);
            _voltages.Add("TK 2-2(ДИСС-32)", _v_voltageTable7);
            _voltages.Add("TK 2-3(ДИСС-32)", _v_voltageTable7);
            _voltages.Add("ТКМ 1", _v_voltageTable7);
            _voltages.Add("ТКМ 2", _v_voltageTable7);
            _voltages.Add("МОД 1", _v_voltageTable7);
            _voltages.Add("МОД 2", _v_voltageTable7);
            _voltages.Add("ГЛПД1", _v_voltageTable7);
            _voltages.Add("ГЛПД2", _v_voltageTable7);
            _voltages.Add("Ток ГЛПД", _v_voltageTable7);
            _voltages.Add("Ток М", _v_voltageTable7);
            _voltages.Add("Zорт (ПНП)", _v_voltageTable7);
            _voltages.Add("Zорт (ПКВ)", _v_voltageTable7);

            // Страница Напряжение 2
            _voltages.Add("Wx ПКВ", _v_voltageTable8);
            _voltages.Add("Wy ПКВ", _v_voltageTable8);
            _voltages.Add("Wz ПКВ", _v_voltageTable8);
            _voltages.Add("Wx НКВ", _v_voltageTable8);
            _voltages.Add("Wy НКВ", _v_voltageTable8);
            _voltages.Add("Wz НКВ", _v_voltageTable8);
            _voltages.Add("Wx СПСВ", _v_voltageTable8);
            _voltages.Add("Wy СПСВ", _v_voltageTable8);
            _voltages.Add("Wz СПСВ", _v_voltageTable8);
            _voltages.Add("Wx Тестер", _v_voltageTable8);
            _voltages.Add("Wy Тестер", _v_voltageTable8);
            _voltages.Add("Wz Тестер", _v_voltageTable8);
            _voltages.Add("W", _v_voltageTable8);
            _voltages.Add("L", _v_voltageTable8);
            _voltages.Add("+27В", _v_voltage27);
            _voltages.Add("-27В", _v_voltage27);
            _voltages.Add("+27В сл.", _v_voltage27);
            _voltages.Add("-27В оп.", _v_voltage27);
            _voltages.Add("+15В", _v_voltage15_20);
            _voltages.Add("-15В", _v_voltage15_20);
            _voltages.Add("-20В", _v_voltage15_20);
            _voltages.Add("+10В ВCC", _v_voltage12_10);
            _voltages.Add("+12,6В", _v_voltage12_10);
            _voltages.Add("-12,6В", _v_voltage12_10);
            _voltages.Add("ВН(-48В)", _v_voltage48);
            _voltages.Add(Delta15VName, _v_voltageDelta15);
            _voltages.Add(Delta27VName, _v_voltageDelta27);

            _voltages.Add("Сигнал УНЧ 1-1", _v_voltageUNCH_11);
            _voltages.Add("Сигнал УНЧ 1-2", _v_voltageUNCH_12);
            _voltages.Add("Сигнал УНЧ 1-3", _v_voltageUNCH_13);
            _voltages.Add("Сигнал УНЧ 2-1", _v_voltageUNCH_21);
            _voltages.Add("Сигнал УНЧ 2-2", _v_voltageUNCH_22);
            _voltages.Add("Сигнал УНЧ 2-3", _v_voltageUNCH_23);

        }

        public const string Delta15VName = "Δ 15В";
        public const string Delta27VName = "Δ 27В";

        private static void FillDictCtrlPoints()
        {
            if (_controlPoints != null)
                return;

            _controlPoints = new Dictionary<string, IEnumerable<double>>();

            // страница Напряжение1
            _controlPoints.Add("+6.3В", _arrayTable7);
            _controlPoints.Add("-6.3В", _arrayTable7);
            _controlPoints.Add("+5В", _arrayTable7);
            _controlPoints.Add("Р СВЧ", _arrayTable7);
            _controlPoints.Add("СД 1-1", _arrayTable7);
            _controlPoints.Add("СД 1-2", _arrayTable7);
            _controlPoints.Add("СД 1-3", _arrayTable7);
            _controlPoints.Add("СД 2-1", _arrayTable7);
            _controlPoints.Add("СД 2-2", _arrayTable7);
            _controlPoints.Add("СД 2-3", _arrayTable7);
            _controlPoints.Add("TK 1-1(ДИСС-15)", _arrayTable7);
            _controlPoints.Add("TK 1-2(ДИСС-15)", _arrayTable7);
            _controlPoints.Add("TK 1-3(ДИСС-15)", _arrayTable7);
            _controlPoints.Add("TK 2-1(ДИСС-15)", _arrayTable7);
            _controlPoints.Add("TK 2-2(ДИСС-15)", _arrayTable7);
            _controlPoints.Add("TK 2-3(ДИСС-15)", _arrayTable7);
            _controlPoints.Add("TK 1-1(ДИСС-32)", _arrayTable7);
            _controlPoints.Add("TK 1-2(ДИСС-32)", _arrayTable7);
            _controlPoints.Add("TK 1-3(ДИСС-32)", _arrayTable7);
            _controlPoints.Add("TK 2-1(ДИСС-32)", _arrayTable7);
            _controlPoints.Add("TK 2-2(ДИСС-32)", _arrayTable7);
            _controlPoints.Add("TK 2-3(ДИСС-32)", _arrayTable7);
            _controlPoints.Add("ТКМ 1", _arrayTable7);
            _controlPoints.Add("ТКМ 2", _arrayTable7);
            _controlPoints.Add("МОД 1", _arrayTable7);
            _controlPoints.Add("МОД 2", _arrayTable7);
            _controlPoints.Add("ГЛПД1", _arrayTable7);
            _controlPoints.Add("ГЛПД2", _arrayTable7);
            _controlPoints.Add("Ток ГЛПД", _arrayTable7);
            _controlPoints.Add("Ток М", _arrayTable7);
            _controlPoints.Add("Zорт (ПНП)", _arrayTable7);
            _controlPoints.Add("Zорт (ПКВ)", _arrayTable7);

            // Страница Напряжение 2
            _controlPoints.Add("Wx ПКВ", _arrayTable8);
            _controlPoints.Add("Wy ПКВ", _arrayTable8);
            _controlPoints.Add("Wz ПКВ", _arrayTable8);
            _controlPoints.Add("Wx НКВ", _arrayTable8);
            _controlPoints.Add("Wy НКВ", _arrayTable8);
            _controlPoints.Add("Wz НКВ", _arrayTable8);
            _controlPoints.Add("Wx СПСВ", _arrayTable8);
            _controlPoints.Add("Wy СПСВ", _arrayTable8);
            _controlPoints.Add("Wz СПСВ", _arrayTable8);
            _controlPoints.Add("Wx Тестер", _arrayTable8);
            _controlPoints.Add("Wy Тестер", _arrayTable8);
            _controlPoints.Add("Wz Тестер", _arrayTable8);
            _controlPoints.Add("W", _arrayTable8);
            _controlPoints.Add("L", _arrayTable8);
            _controlPoints.Add("+27В", _array27);
            _controlPoints.Add("-27В", _array27);
            _controlPoints.Add("+27В сл.", _array27);
            _controlPoints.Add("-27В оп.", _array27);
            _controlPoints.Add("+15В", _array15_20);
            _controlPoints.Add("-15В", _array15_20);
            _controlPoints.Add("-20В", _array15_20);
            _controlPoints.Add("+10В ВCC", _array12_10);
            _controlPoints.Add("+12,6В", _array12_10);
            _controlPoints.Add("-12,6В", _array12_10);
            _controlPoints.Add("ВН(-48В)", _array48);
            _controlPoints.Add(Delta15VName, _arrayDelta15);
            _controlPoints.Add(Delta27VName, _arrayDelta27);

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
        public static IEnumerable<double> GetVoltagesArray(string ikName)
        {
            FillDictCtrlPoints();
            if (_controlPoints.ContainsKey(ikName))
            {
                IEnumerable<double> voltagesArray;
                _controlPoints.TryGetValue(ikName, out voltagesArray);
                return voltagesArray;
            }
            throw new Exception("Выбранный канал не проверяется автоматической поверкой");
        }

        /// <summary>
        /// возвращает параметры тестирования: перечень контрольных точек и диапазоны
        /// </summary>
        /// <param name="ikName">измерительный канал</param>
        /// <returns></returns>
        public static Voltages GetVoltages(string ikName)
        {
            FillDictVoltages();
            if (_voltages.ContainsKey(ikName))
            {
                Voltages voltages;
                _voltages.TryGetValue(ikName, out voltages);
                return voltages;
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
    public class Voltages
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
        public int[] FreqPoints{ get; set; }

        /// <summary>
        /// Диапазоны погрешностей
        /// </summary>
        public double[,] ErrorsRanges { get; set; }

        public Voltages()
        {

        }

        public Voltages(ADCDirect rangeNames, IEnumerable<double> controlPoints, double[,] errorsRanges, int[] freqPoint = null)
        {
            RangeNames = rangeNames;
            ControlPoints = controlPoints.ToArray();
            ErrorsRanges = errorsRanges;
            FreqPoints = freqPoint;
        }
    }

    /// <summary>
    /// диапазоны проверяемых значений напряжений на допустимую погрешность
    /// согласно протокола поверки № 569
    /// </summary>
    public enum ADCDirect
    {
        /// <summary>
        /// 7 таблица протокола
        /// </summary>
        VoltagesTable7,

        /// <summary>
        /// 8 таблица протокола
        /// </summary>
        VoltagesTable8,
        Voltages27,
        Voltages15_20,
        Voltages12_10,
        Voltage48,
        VoltageDelta27,
        VoltageDelta15,

        VoltageUNCH_11,
        VoltageUNCH_12,
        VoltageUNCH_13,
        VoltageUNCH_21,
        VoltageUNCH_22,
        VoltageUNCH_23
    }
}
