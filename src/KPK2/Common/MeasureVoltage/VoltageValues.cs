using System;
using System.ComponentModel;

namespace KPK2.Common.MeasureVoltage
{
  public class VoltageValues
  {
    private double _benchmark = 0;
    private double _measured = 0;
    private double _deviation = 0;

    public VoltageValues()
    {
        Installed = 0;
    }

    /// <summary>
    ///   установленное
    /// </summary>
    [DisplayName("Установленное")]
    public double Installed { get; set; }

    /// <summary>
    ///   Эталонное
    /// </summary>
    [DisplayName("Эталонное")]
    public double Benchmark
    {
        get
        {
            return _benchmark;
        }
        set
      {
        if (_benchmark != value)
        {
          _benchmark = value;
          DeltaComput();
        }
      }
    }

    /// <summary>
    ///   Измеренное
    /// </summary>
    [DisplayName("Измеренное")]
    public double Measured
    {
        get { return _measured; }
        set
      {
        if (_measured != value)
        {
          _measured = value;
          DeltaComput();
        }
      }
    }

    /// <summary>
    ///   Отклонение
    /// </summary>
    public double Deviation {
        get { return _deviation; }
    }

    /// <summary>
    /// Вычисление отклонения
    /// </summary>
    [DisplayName("Отклонение")]
    private void DeltaComput()
    {
      _deviation = _measured - _benchmark;
    }
  }
}