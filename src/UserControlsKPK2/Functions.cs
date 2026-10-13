using System;
using System.Globalization;

namespace UserControlsKPK2
{
  /// <summary>
  /// Класс общих переменных и методов проекта.
  /// </summary>
  public static class Functions
  {
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
  }
}