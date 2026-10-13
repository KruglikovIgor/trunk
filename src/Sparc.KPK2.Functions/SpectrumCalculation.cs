using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Windows.Forms;
using NLog;

namespace Sparc.KPK2.NFunctions
{
  public static class SpectrumCalculation
  {
    private static Logger _logger = LogManager.GetLogger("KPK2Info");

    /// <summary> Поиск локал. максимумов всех интервалов, 
    /// в кот. все значения > 40% размаха.
    /// </summary>
    /// <param name="result"> Анализируемый массив </param>
    /// <returns> Перечень индексов и значений найденных максимумов. </returns>
    public static Dictionary<int, double> PeaksSearch(double[] result)
    {
      if (result.Length == 0)
        throw new Exception("Not enough length!");

      // Поиск глобальных максимума и минимума.
      double minValue = result[0];
      double maxValue = result[0];
      foreach (double val in result)
      {
        if (val < minValue)
          minValue = val;
        if (val > maxValue)
          maxValue = val;
      }

      // Перечень локал. максимумов и их индексов.
      Dictionary<int, double> peaks = new Dictionary<int, double>();
            
      // Порог определения интервалов (40% размаха).
      double baseValue = minValue + (maxValue - minValue) / 2.5;

      // Поиск локал. максимумов
      double tMaxValue = minValue;
      int tIndex = -1;
      for (int i=0;i<result.Length;i++)
      {
        double inValue = result[i] < baseValue ? minValue : result[i];
        // внутри интервалов inValue будет > minValue.
        if (inValue > tMaxValue)
        {
          tMaxValue = inValue;
          tIndex = i;
        }
        // tIndex - индекс текущ. максимума внутри интервала, 
        // в кот. все значения > 40% размаха. 

        if (inValue <= minValue && tIndex >= 0)
        {
          // выход из интервала, в кот. все значения > 40% размаха.
          peaks.Add(tIndex, tMaxValue);
          tMaxValue = minValue;
          tIndex = -1;
        }
      }
      return peaks;
    }
    //------------------------------------------------------------------------------------

    public class PeakPair
    {
      public double Freq;
      public double Value;

      public PeakPair(double freq, double value)
      {
        Freq = freq;
        Value = value;
      }
    }

    public class PeakResult
    {
      public PeakPair MainFreq;
      public List<PeakPair> MainFreqMinus = new List<PeakPair>();
      public List<PeakPair> MainFreqPlus = new List<PeakPair>();
    }

    /// <summary> Преобразование спектра АЧХ в перечень пиков 
    /// (амплитуд локал. максимумов и их частот) с определением центр. пика (около несущей).
    /// </summary>
    /// <param name="result"> Спектр АЧХ, дБ. </param>
    /// <param name="freqMeasured"> Измеренная частота несущей Fнес.изм, Гц. </param>
    /// <param name="freqDev"> Девиация (?), Гц
    /// (в вызывающем методе frmWork13.TK7_StepSA2() Девиация задана = 2,5 МГц). 
    /// </param>
    /// <param name="startFreq"> Частота начала спектра, Гц. </param>
    /// <param name="binSize"> Шаг спектра, Гц. </param>
    /// <returns></returns>
    public static PeakResult GetPeaksAroundFreq(double[] result, double freqMeasured, double freqDev, double startFreq, double binSize)
    {
      // Перечень амплитуд локал. максимумов и их индексов
      var peaks = PeaksSearch(result);

      // Перечень пиков - амплитуд локал. максимумов и их частот
      List<PeakPair> peaksFreq = new List<PeakPair>();
      foreach (var item in peaks)
        peaksFreq.Add(new PeakPair(startFreq + item.Key*binSize, item.Value));

      PeakResult res = new PeakResult();

      // Поиск пика, ближайшего к Fнес и отстоящего от нее менее, чем на Девиацию(???).
      // (расстояние между пиками, т.е. Fчм, судя по реальному спектру ~= 1 МГц;
      // Fнес м.б. = 8800 +10,-15 МГц).
      int mainFreqIndex = -1;
      double curFreqDev = freqDev+1;
      for (int i = 0; i < peaksFreq.Count; i++)
      {
        // Отклонение очередного пика от Fнес.изм.
        double inFreqDev = Math.Abs(peaksFreq[i].Freq - freqMeasured);
        
        if (inFreqDev <= freqDev && 
            inFreqDev < curFreqDev)
        {
          res.MainFreq = peaksFreq[i]; // Ближайший пик
          mainFreqIndex = i; // Индекс пика
          curFreqDev = inFreqDev; // Текущее мин. откл-ие от Fнес.изм
        }
      }
      if (mainFreqIndex < 0) // не найден?
      // Отклонение от Fнес.изм ближайшего к ней пика больше Девиации!
        return null;

      _logger.Trace( "Spectrum FM:");
      for ( int i = mainFreqIndex - 3;
                i <= mainFreqIndex + 3; 
                i++)
          if (0 <= i && i < peaksFreq.Count)
             _logger.Trace( "{0}: {1:F2} MHz, {2:F2} dB",
                 i, peaksFreq[i].Freq*1e-6, peaksFreq[i].Value);

      // Перенос в res всех пиков ниже Fнес
      for (int i=mainFreqIndex-1;i>= 0;i--)
        res.MainFreqMinus.Add(peaksFreq[i]);

      // Перенос в res всех пиков выше Fнес
      for (int i = mainFreqIndex + 1; i < peaksFreq.Count; i++)
        res.MainFreqPlus.Add(peaksFreq[i]);

      return res;
    }
    //--------------------------------------------------------------------------

    /// <summary> Возвращает массив с мин. значениями отсчетов 
    /// среди заданного списка массивов. 
    /// </summary>
    /// <param name="minValues"></param>
    /// <returns></returns>
    public static double[] ToMinArray(List<double[]> minValues)
    {
      // Массив для мин. значений отсчетов.
      double[] result = new double[minValues[0].Length];
      for (int i = 0; i < minValues[0].Length; i++)
      {
        // Поиск мин. значения i-го отсчета среди всех массивов.
        double minValue = minValues[0][i];
        for (int arrIndex = 0; arrIndex < minValues.Count; arrIndex++)
          if (minValues[arrIndex][i] < minValue)
            minValue = minValues[arrIndex][i];
        result[i] = minValue;
      }
      return result;
    }
    //----------------------------------------------------------------------

    /// <summary> Возвращает массив с макс. значениями отсчетов 
    /// среди заданного списка массивов. 
    /// </summary>
    /// <param name="maxValues"></param>
    /// <returns></returns>
    public static double[] ToMaxArray(List<double[]> maxValues)
    {
      // Массив для макс. значений отсчетов.
      double[] result = new double[maxValues[0].Length];
      for (int i = 0; i < maxValues[0].Length; i++)
      {
        // Поиск макс. значения i-го отсчета среди всех массивов.
        double maxValue = maxValues[0][i];
        for (int arrIndex = 0; arrIndex < maxValues.Count; arrIndex++)
          if (maxValues[arrIndex][i] > maxValue)
            maxValue = maxValues[arrIndex][i];
        result[i] = maxValue;
      }
      return result;
    }
    //----------------------------------------------------------------------

    /// <summary> 
    /// Возвращает массив отсчетов ср. между значениями заданных 2 массивов. 
    /// </summary>
    /// <param name="minHold"></param>
    /// <param name="maxHold"></param>
    /// <returns></returns>
    public static double[] ToAverageArray(double[] minHold, double[] maxHold)
    {
      if (minHold.Length != maxHold.Length)
        return new double[0];
      double[] result = new double[minHold.Length];

      for (int i = 0; i < minHold.Length; i++)
        result[i] = minHold[i] + (maxHold[i] - minHold[i]) / 2;

      return result;
    }
    //----------------------------------------------------------------------

    static double[,] mA10LowPoints = 
    {
      // {m10, a10}}
      {0.166666667,	0.0000000000},
      {1,	    0.1666666667},
      {1.1,     0.8333333333},
      {1.2,	    1.5000000000},
      {1.3,	    2.1666666667},
      {1.4,	    3.0000000000},
      {1.5,	    3.6666666667},
      {1.6,	    4.5833333333},
      {1.7,	    5.5000000000},
      {1.8,	    6.3333333333},
      {1.9,	    7.0833333333},
      {2,	    8.3333333333},
      {2.05,	9.3333333333},
      {2.1,	    10.5000000000},
      {2.15,	11.7500000000},
      {2.2,	    13.7500000000},
      {2.21,	14.1666666667},
      {2.22,	14.3333333333},
      {2.23,	15.0000000000},
      {2.24,	15.4166666667},
      {2.25,	16.1666666667},
      {2.26,	16.8333333333},
      {2.27,	17.2000000000},
      {2.28,	18.0000000000},
      {2.29,	18.6000000000},
      {2.3,	    19.2000000000},
      {2.31,	20.0000000000},
      {2.32,	20.6000000000},
      {2.33,	21.7000000000},
      {2.34,	22.5000000000},
      {2.35,	24.0000000000},
      {2.36,	25.8000000000},
      {2.37,	27.4000000000},
      {2.38,	30.2000000000},
      {2.39,	36.2000000000},
      {2.4,	    39.4000000000},
      {2.41,	46.0000000000}
    };

    private static double[,] mA10HighPoints =
    {
      // {m10, a10}}
      {2.42, 45.0000000000},
      {2.43, 43.0000000000},
      {2.44, 35.0000000000},
      {2.45, 30.4000000000},
      {2.46, 26.0000000000},
      {2.47, 24.2000000000},
      {2.48, 23.0000000000},
      {2.49, 22.0000000000},
      {2.5,  20.6000000000},
      {2.51, 19.6000000000},
      {2.52, 18.6000000000},
      {2.53, 17.8000000000},
      {2.54, 17.0000000000},
      {2.55, 16.6000000000},
      {2.56, 15.9000000000},
      {2.57, 15.3000000000},
      {2.58, 14.9000000000},
      {2.59, 14.4000000000},
      {2.6,  13.9000000000},
      {2.65, 11.5000000000},
      {2.7,  9.5000000000},
      {2.75, 7.5000000000},
      {2.8,  6.1666666667},
      {2.85, 4.6666666667},
      {2.9,  3.5000000000},
      {2.95, 2.5000000000},
      {3,    1.7500000000},
      {3.05, 0.8333333333},
      {3.1,  0.1666666667},
      {3.116666667, 0.0000000000}
    };

    private static double[,] mA12Points =
    {
      // {m12, a12}}
      {1.333333333,	6.6666666667},
      {1.4,	    6.3333333333},
        { 1.5,	6.0000000000},
        {1.6,	5.5833333333},
        {1.7,	5.3333333333},
        {1.8,	4.8333333333},
        {1.9,	4.5000000000},
        {2,	    4.1666666667},
        {2.05,	3.9166666667},
        {2.1,	3.5833333333},
        {2.15,	3.3333333333},
        {2.2,	3.0000000000},
        {2.21,	2.9000000000},
        {2.22,	2.8500000000},
        {2.23,	2.8000000000},
        {2.24,	2.7500000000},
        {2.25,	2.7000000000},
        {2.26,	2.6500000000},
        {2.27,	2.6000000000},
        {2.28,	2.5500000000},
        {2.29,	2.5000000000},
        {2.3,	2.4500000000},
        {2.31,	2.4000000000},
        {2.32,	2.3000000000},
        {2.33,	2.3000000000},
        {2.34,	2.2000000000},
        {2.35,	2.1000000000},
        {2.36,	2.0500000000},
        {2.37,	2.0000000000},
        {2.38,	1.9500000000},
        {2.39,	1.9000000000},
        {2.4,	1.8000000000},
        {2.41,	1.7500000000},
        {2.42,	1.6500000000},
        {2.43,	1.6000000000},
        {2.44,	1.5500000000},
        {2.45,	1.4500000000},
        {2.46,	1.4000000000},
        {2.47,	1.3500000000},
        {2.48,	1.3000000000},
        {2.49,	1.2000000000},
        {2.5,	1.1000000000},
        {2.51,	1.0000000000},
        {2.52,	0.9500000000},
        {2.53,	0.8500000000},
        {2.54,	0.8000000000},
        {2.55,	0.7000000000},
        {2.56,	0.6000000000},
        {2.57,	0.5500000000},
        {2.58,	0.4500000000},
        {2.59,	0.4000000000},
        {2.6,	0.2500000000},
        {2.616666667,	0.0000000000}
    };

    private static double[,] mA01 =
    {
        // {m, a01}}
        {3.116666667,	0.0000000000},
        {3.15,	0.2500000000},
        {3.2,	1.6666666667},
        {3.25,	2.6666666667},
        {3.3,	4.1666666667},
        {3.35,	5.4166666667},
        {3.4,	6.3333333333},
        {3.45,	7.583333333},
        {3.5,	8.5833333333}
    };

    private static double[,] mA01Points =
    {
      // {m, a01}}
      {3.116666667,	0.0000000000},
      {3.15,	0.2500000000},
      {3.2,	    1.6666666667},
      {3.25,	2.6666666667},
      {3.3,	    4.1666666667},
      {3.35,	5.4166666667},
      {3.4,	    6.3333333333},
      {3.45,	7.583333333},
      {3.5,	    8.5833333333}
    };

    private static double[,] mA21Points =
    {
      // {m21, a21}
      {2.65,    0.166666667},
      {2.7,     0.5},
      {2.75,    1},
      {2.8,     1.333333333},
      {2.85,    1.666666667},
      {2.9,     2.166666667},
      {2.95,    2.666666667},
      {3,       3.083333333},
      {3.05,    3.5},
      {3.1,     4},
      {3.15,    4.583333333},
      {3.2,     5.166666667},
      {3.25,    5.75},
      {3.3,     6.5},
      {3.35,    7.333333333},
      {3.4,     8.333333333},
      {3.45,    9.166666667},
      {3.5,     10.16666667}
    };
    //-------------------------------------------------------------------------

    /// <summary>
    ///  Определение mф12 по графику а12=f(mф) (см. п.7 ТК7 РЭ ДИСС-013)
    /// </summary>
    /// <param name="alfa12"> а12</param>
    /// <param name="low"></param>
    /// <returns></returns>
    public static double ConvertToM12(double alfa12)
    {
      return GetLinearApproxResult(mA12Points, alfa12);
    }
    //-------------------------------------------------------------------------

    public class maPoint
    {
      public double MValue;
      public double AValue;

      public maPoint(double mValue, double aValue)
      {
        MValue = mValue;
        AValue = aValue;
      }
    }
    //-------------------------------------------------------------------------
    
    /// <summary>
    /// Возвращает кусочно-лин. интерполяцию ф-ии для заданного аргумента 
    /// по данным таблично заданной ф-ии y(x).
    /// </summary>
    /// <param name="points"> Табличное задание кусочно-лин. ф-ии [y, x]. </param>
    /// <param name="aValue"> Аргумент для интерполяции. </param>
    /// <returns> Интерполяция. </returns>
    private static double GetLinearApproxResult(double[,] points, double aValue)
    {
      maPoint minValuePoint = null;
      maPoint maxValuePoint = null;

      for (int i = 0; i < points.GetLength(0); i++)
      {
        double aVal = points[i, 1]; // x на левой границе i-го интервала
        double mVal = points[i, 0]; // y(x) на левой границе i-го интервала
        if (aVal < aValue)
        {
          if (minValuePoint == null)
            minValuePoint = new maPoint(mVal, aVal);
          if (aVal > minValuePoint.AValue)
          {
            minValuePoint.AValue = aVal;
            minValuePoint.MValue = mVal;
          }
        }

        if (aVal >= aValue)
        {
          if (maxValuePoint == null)
            maxValuePoint = new maPoint(mVal, aVal);
          if (aVal < maxValuePoint.AValue)
          {
            maxValuePoint.AValue = aVal;
            maxValuePoint.MValue = mVal;
          }
        }
      }

      if (minValuePoint == null)
      {
        if (maxValuePoint == null)
          return double.NaN;

        minValuePoint = maxValuePoint;
        maxValuePoint = null;

        for (int i = 0; i < points.GetLength(0); i++)
        {
          double aVal = points[i, 1];
          double mVal = points[i, 0];
            if (aVal > minValuePoint.AValue)
            {
                if (maxValuePoint == null)
                    maxValuePoint = new maPoint(mVal, aVal);
                if (aVal < maxValuePoint.AValue)
                {
                    maxValuePoint.AValue = aVal;
                    maxValuePoint.MValue = mVal;
                }
            }
        }
      }

      if (maxValuePoint == null)
      {
        maxValuePoint = minValuePoint;
          minValuePoint = null;
        for (int i = 0; i < points.GetLength(0); i++)
        {
          double aVal = points[i, 1];
          double mVal = points[i, 0];
            if (aVal < maxValuePoint.AValue)
            {
                if (minValuePoint == null)
                    minValuePoint = new maPoint(mVal, aVal);
                if (aVal > minValuePoint.AValue)
                {
                    minValuePoint.AValue = aVal;
                    minValuePoint.MValue = mVal;
                }
            }
        }
      }

      if (maxValuePoint == null || minValuePoint == null)
        return double.NaN;

      double Kkoef = (maxValuePoint.MValue - minValuePoint.MValue) / (maxValuePoint.AValue - minValuePoint.AValue);
      double BKoef = maxValuePoint.MValue - Kkoef * maxValuePoint.AValue;

      return aValue * Kkoef + BKoef;
    }
    //-------------------------------------------------------------------------

    /// <summary> Определение индекса модуляции mф10 
    /// по графику а10'=f(mф), если (mф12 меньше 2,4), 
    /// и по графику а10"=f(mф), если (mф12 > 2,4) (см. п.10 ТК7 ДИСС-013)
    /// </summary>
    /// <param name="alfa10"> Аргумент а10. </param>
    /// <param name="low"> Условие выбора графика (mф12 меньше 2,4). </param>
    /// <returns> Индекс модуляции. </returns>
    public static double ConvertToM10(double alfa10, bool low)
    {
      return GetLinearApproxResult(low ? mA10LowPoints : mA10HighPoints, alfa10);
    }
    //-------------------------------------------------------------------------

    public static double FullConvertToM10(double alfa10, double alfa12)
    {
      double m12 = ConvertToM12(alfa12); // п.7 ТК7
      return ConvertToM10(alfa10, m12 < 2.4); // п.10 ТК7
    }
    //-------------------------------------------------------------------------

      public static double CalculatePhaseIndex(PeakResult pResult)
      {
          // Амплитуды спектра, дБ (см. пп.4,5,8 ТК7):
          double Aplus1 = pResult.MainFreqPlus[0].Value; // A+1.
          double Aplus2 = pResult.MainFreqPlus[1].Value; // A+2
          double Aminus1 = pResult.MainFreqMinus[0].Value; // A-1
          double MainFreqValue = pResult.MainFreq.Value; // A0

          double alfa12 = Aplus1 - Aplus2 - 0.5 * Math.Abs(Aplus1 - Aminus1); 
            // a12 = (A+1 - A+2) - (A+1 - A-1)/2; (см. п.6 ТК7)

          double alfa10 = Aplus1 - MainFreqValue - 0.5 * Math.Abs(Aplus1 - Aminus1); 
            // a10 = (A+1 - A0) - (A+1 - A-1)/2; (см. п.9 ТК7)

          double m10 = FullConvertToM10(alfa10, alfa12);
          return m10;
      }
  }
}
