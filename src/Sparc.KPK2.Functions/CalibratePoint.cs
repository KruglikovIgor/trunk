using System;
using System.Collections.Generic;
using System.Text;

namespace Sparc.KPK2.NFunctions
{
  /// <summary>
  /// Класс для хранения пар точек
  /// </summary>
  public class CalibratePoint
  {
    /// <summary>
    ///замеренное значение с прибора
    /// </summary>
    public double Ideal = double.NaN;
    /// <summary>
    /// Заданное значение (на плате, приборе)
    /// </summary>
    public double Real = double.NaN;

    public CalibratePoint(double inIdeal, double inReal)
    {
      Ideal = inIdeal;
      Real = inReal;
    }

    /// <summary>
    /// Инициализирует значения как NaN
    /// </summary>
    public CalibratePoint()
    { }
  }
}
