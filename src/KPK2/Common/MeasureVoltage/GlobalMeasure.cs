using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace KPK2.Common.MeasureVoltage
{
  /// <summary>
  /// Класс статистических методов, используемых при автоматической
  /// поверке контрольных точек напряжений на измерительных каналах
  /// </summary>
  public static class GlobalMeasure
  {
    /// <summary>
    /// Установка колонок объекта DataGridView в режим: только для чтения
    /// </summary>
    /// <param name="dgv">объект DataGridView</param>
    /// <param name="fields"></param>
    public static void SetReadOnlyFields(DataGridView dgv, params object[] fields)
    {
      try
      {
        foreach (object field in fields)
          dgv.Columns[(int)field].ReadOnly = true;
      }
      catch (Exception exception)
      {
        throw new Exception(exception.Message);
      }
    }

  }
}
