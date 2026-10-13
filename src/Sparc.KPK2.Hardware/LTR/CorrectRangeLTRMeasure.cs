using System;
using System.Linq;
using NLog;

namespace Sparc.KPK2.Hardware.LTR
{
    public class CorrectRangeLTRMeasure
    {
        /// <summary>
        /// Текущий массив диапазонов для платы
        /// </summary>
        private readonly double[] _currentRanges;

        private static Logger _logger = LogManager.GetLogger("KPK2Trace");

        /// <summary>
        /// Текущий диапазон
        /// </summary>
        public double Range { get; set; }
        public bool ConfigureNeed { get; set; }

        /// <summary>
        /// Части диапазонов нужные для оценки возможности переключения диапазонов
        /// </summary>
        public double KoefRange => Convert.ToDouble(2d/3d);
        public double StayKoefRange => Convert.ToDouble(5d / 6d);

        /// <summary>
        /// Конуструктор
        /// </summary>
        /// <param name="ranges">Массив диапазонов измерений</param>
        /// <param name="currentRange">Текущий диапазон измерений</param>
        public CorrectRangeLTRMeasure(double[] ranges, double currentRange)
        {
            _currentRanges = ranges;
            Range = currentRange;
        }

        /// <summary>
        /// Найти максимальное значение по модулю из точек
        /// </summary>
        /// <param name="points">Полученные точки</param>
        /// <returns>Макс. значение точки по модулю, в Вольтах</returns>
        private double GetAbsMaxFromPoints(double[] points)
        {
            return  points.Select(x => Math.Abs(x)).Max(); 
        }

        /// <summary>
        /// Найти рекомендуемый диапазон измерений
        /// </summary>
        /// <param name="result">Полученный сигнал</param>
        /// <returns>Диапазон измерений, в Вольтах</returns>
        private double FindCorrectRange(double[] result)
        {
            double findedRange = _currentRanges.Max();
            var maxPoint = GetAbsMaxFromPoints(result);

            foreach (var range in _currentRanges) 
            {
                var partRange = range * KoefRange;
                if (maxPoint <= partRange)
                {
                    findedRange = range < findedRange ? range : findedRange;
                }
            }
                return findedRange;
        }

        /// <summary>
        /// Проверка актуальности диапазона
        /// </summary>
        /// <param name="result">Полученный сигнал</param>
        /// <returns></returns>
        public bool CheckCorrectRange(double[] result)
        {
            var correctRange = FindCorrectRange(result);

            if (Math.Abs(correctRange - Range) > double.Epsilon)
            {
                if (correctRange > Range)
                {
                    var partRange = Range * StayKoefRange;
                    if (IsStayCurrentRange(result, partRange))
                        return true;
                }
                _logger.Trace("Текущий диапазон: " + Range + ", РЕКОМЕНДУЕМЫЙ ДИАПАЗОН: " + correctRange);
                Range = correctRange;
                ConfigureNeed = true;
                return false;
            }
                
            return true;
        }

        /// <summary>
        /// Проверка на то, можно ли остаться в текущем диапазоне (нужна для того, чтобы лишний раз не переключать диапазоны)
        /// </summary>
        /// <param name="signal">Сигнал</param>
        /// <param name="partRange">Часть диапазона для оценки</param>
        /// <returns></returns>
        private bool IsStayCurrentRange(double[] signal, double partRange)
        {
            var result = GetAbsMaxFromPoints(signal);
            var range = partRange * StayKoefRange;
            if (result <= range)
                return true;
            return false;
        }
    }
}
