using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KPK2.Common.MeasureVoltage
{
  /// <summary>
  /// источник параметров измерения напряжения
  /// </summary>
  public interface ISource
  {
    /// <summary>
    /// добавление параметров измерения в список
    /// </summary>
    /// <param name="voltages"></param>
    void Add(VoltageValues voltages);
  }
}
