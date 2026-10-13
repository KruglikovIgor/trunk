using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KPK2.Common.MeasureVoltage;
using NUnit.Framework;

namespace Sparc.KPK2.Tests
{
  class ValidVoltageIntervalTest
  {
    private Voltages _voltages = new Voltages();
    private double _voltage = Double.NaN;
    /// <summary>
    /// Проверяет ошибки
    /// </summary>
    /// <param name="step">номер шага</param>
    /// <param name="error">значение параметра отклонения</param>
    /// <returns></returns>
    private bool CheckADCDirectError(int step, double error)
    {
      //DataGridViewRow row = dgvValues.Rows[step];
      //double voltage = Convert.ToDouble(row.Cells[(int)ADCDirectColumn.idealV].Value);
      double allowError = SearchAllowError(_voltages.ErrorsRanges, _voltage);
      if (double.IsNaN(allowError))
      {
        return false;
      }

      //row.Cells[(int)ADCDirectColumn.errorV].ToolTipText = "±" + allowError;
      return Math.Abs(error) <= allowError;
    }

    /// <summary>
    /// поиск допустимых ошибок
    /// </summary>
    /// <param name="errorInterval">интервал напряжений и допустимая погрешность</param>
    /// <param name="voltage"></param>
    /// <returns></returns>
    private double SearchAllowError(double[,] errorInterval, double voltage)
    {
      if (errorInterval == null || errorInterval.GetLength(0) < 1)
      {
        return double.NaN;
      }

      for (int i = 0; i < errorInterval.GetLength(0); i++)
      {
        if (voltage >= errorInterval[i, 0] && voltage <= errorInterval[i, 1])
        {
          return errorInterval[i, 2];
        }
      }

      return double.NaN;
    }

  }
}
