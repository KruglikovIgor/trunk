using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;

namespace Sparc.KPK2.Tests
{
  [TestFixture]
  public class SearchAllowErrorTest
  {

    [Test]
    public void Test1()
    {
      // arrange
      double[,] errorInterval = { { -9.9, -1.0001 , 0.01 }, { -1.0, 1.0 , 2.0 }, { 1.0001, 9.9, 10.0 }};
    }

    /// <summary>
    /// поиск допустимых ошибок
    /// </summary>
    /// <param name="errorInterval"></param>
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
