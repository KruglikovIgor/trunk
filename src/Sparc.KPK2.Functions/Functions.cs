using System;
using System.Globalization;

namespace Sparc.KPK2.NFunctions
{
  public static class Functions
  {
    public enum CalcFunction
    {
      Min,
      Deviation,
      Tmin,
      Tmax,
      Amplitude,
      RMS,
      Mean,
      PairSubtraction
    }

    public static double MoveToDouble(string value)
    {
      value = CheckDecimalSeparatorAndReplace(value, ".");
      value = CheckDecimalSeparatorAndReplace(value, ",");
      double result;
      if (Double.TryParse(value, out result))
        return result;
      return double.NaN;
      //if (Double.TryParse(value.Replace(",", "."), out result))
      //  return result;
      //return Double.TryParse(value.Replace(".", ","), out result) ? result : double.NaN;
    }

    private static string CheckDecimalSeparatorAndReplace(string value, string p)
    {
      if (value.Contains(p) && String.Compare(CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator, p) != 0)
        return value.Replace(p, CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator);
      return value;
    }

    private static CultureInfo ciWithDot;

    public static double MoveToDoubleWithDot(string value)
    {
      CreateCultureInfoWithDot();
      try
      {
        return Convert.ToDouble(value, ciWithDot);
      }
      catch
      {
        return Double.NaN;
      }
    }

    private static void CreateCultureInfoWithDot()
    {
      if (ciWithDot == null)
        ciWithDot = CultureInfo.InvariantCulture;
    }

    //Подсчёт RMS
    public static double CalcRMS(double[] arr)
    {
      double result = 0;
      //double shift = 0;
      //result = FastFourierTransform.Instance.RMS(arr, ref shift);

        double sum1 = 0;
        double sum2 = 0;

        double directVolt = CalcMean(arr);

        for (int i = 0; i < arr.Length; i++)
        {
            arr[i] -= directVolt;
            double wind = CalcBlackManWindowValue(arr.Length, i);
            sum1 += Math.Pow(arr[i] * wind, 2);
            sum2 += Math.Pow(wind, 2);
        }

        result = Math.Sqrt(sum1 / sum2);

      return result;
    }

      public static double CalcBlackManNattalWindowValue(int count, int index)
      {
          double a0 = 0.3635819;
          double a1 = 0.4891775;
          double a2 = 0.1365995;
          double a3 = 0.0106411;

          double preVal = Math.PI * ((double)index / count);
          double result = a0 - a1 * Math.Cos(2 * preVal)  + a2 * Math.Cos(4 * preVal) - a3 * Math.Cos(6 * preVal);
          return result;
      }

      public static double CalcBlackManHarrisWindowValue(int count, int index)
      {
          double a0 = 0.35875;
          double a1 = 0.48829;
          double a2 = 0.14128;
          double a3 = 0.01168;

          double preVal = Math.PI * ((double)index / count);
          double result = a0 - a1 * Math.Cos(2 * preVal) + a2 * Math.Cos(4 * preVal) - a3 * Math.Cos(6 * preVal);
          return result;
      }

      public static double CalcBlackManWindowValue(int count, int index)
      {
          double a0 = 0.42;
          double a1 = 0.5;
          double a2 = 0.08;

          double preVal = Math.PI * ((double)index / (count - 1));
          double result = a0 - a1 * Math.Cos(2 * preVal) + a2 * Math.Cos(4 * preVal);
          return result;
      }

    //Подсчёт среднего значения
    public static double CalcMean(double[] arr)
    {
      double result = 0;
      for (int i = 0; i < arr.Length; i++)
        result += arr[i];
      result = result/arr.Length;
      return result;
    }

    //Подсчёт минимального значения
    public static double CalcMin(double[] arr)
    {
      double min = arr[0];
      for (int i = 1; i < arr.Length; i++)
        if (arr[i] < min)
          min = arr[i];
      return min;
    }

    //Подсчёт максимального
    public static double CalcMax(double[] arr)
    {
      double max = arr[0];
      for (int i = 1; i < arr.Length; i++)
        if (arr[i] > max)
          max = arr[i];
      return max;
    }

    //Подсчёт Tmin для ТК9 для ДИСС13 (в работе)
    public static double CalcTmin(double[] arr)
    {
      return CalcMin(arr);
    }

    //Подсчёт Tmax для ТК9 для ДИСС13 (в работе)
    public static double CalcTmax(double[] arr)
    {
      return CalcMean(arr);
    }

    public static double CalcFunc(CalcFunction function, double[] arr)
    {
      switch (function)
      {
        case CalcFunction.Min:
          return CalcMin(arr);
        case CalcFunction.Tmin:
          return CalcTmin(arr);
        case CalcFunction.Tmax:
          return CalcTmax(arr);
        case CalcFunction.Deviation:
          return CalcDeviation(arr);
        case CalcFunction.Amplitude:
          return CalcAmplitude(arr);
        case CalcFunction.RMS:
          return CalcRMS(arr);
        case CalcFunction.Mean:
          return CalcMean(arr);
      }
      return double.NaN;
    }

    public static double CalcFunc(CalcFunction function, double[] arr, double[] arr1)
    {
      switch (function)
      {
        case CalcFunction.PairSubtraction:
          return CalcPairSubtraction(arr, arr1);
      }
      return double.NaN;
    }

    private static double CalcPairSubtraction(double[] arr, double[] arr1)
    {
      double result = 0;
      int count = 0;
      double mean = CalcMean(arr);
      for (int i = 0; i < arr.Length; i++)
        if (arr[i] > mean)
        {
          result += arr[i] - arr1[i];
          count++;
        }
      result /= count;
      return Math.Abs(result);
    }

    public static double CalcDeviation(double[] arr)
    {
      double max = CalcMax(arr);
      double min = CalcMin(arr);
      return (max - min)/(max + min)*100;
    }

    public static double CalcAmplitude(double[] arr)
    {
      double max = CalcMax(arr);
      double min = CalcMin(arr);
      return Math.Abs(max - min);
    }

    public static double CalcRMSDirect(double[] arr)
    {
        double sum = 0;
        for (int i = 0; i < arr.Length; i++)
            sum += arr[i]*arr[i];
        sum /= arr.Length;
        return Math.Sqrt(sum);
    }

    public static double CalcFrequency(double[] samples, int freqSamples)
    {
      double min = samples[0];
      double max = samples[0];
      foreach (double item in samples)
      {
        if (item < min)
          min = item;
        if (item > max)
          max = item;
      }
      double half = min + (max - min) / 2; //половина размаха сигнала
      bool flag = samples[0] > half; //Индикатор перехода
      int sSwitch = 0; //количество переходов через половину размаха
      foreach (double item in samples)
      {
        bool tempFlag = item > half;
        if (tempFlag != flag)
          sSwitch++;
        flag = tempFlag;
      }
      double freq = freqSamples / (samples.Length / (double) sSwitch)/2;
      return freq;
    }

    public static bool CheckFrequencyDiss13(double freq, double temperature)
    {
      double minValue, maxValue;
      GetFrequencyLimitDiss13(temperature, out minValue, out maxValue);
      bool result = (freq >= minValue) && (freq <= maxValue);
      return result;
    }

    public static void GetFrequencyLimitDiss13(double temperature, out double minFreq, out double maxFreq)
    {
      minFreq = -0.2 * temperature + 8790;
      maxFreq = -0.2 * temperature + 8810;
    }
  }
}
