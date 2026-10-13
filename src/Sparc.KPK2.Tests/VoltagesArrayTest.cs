using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KPK2.Common.MeasureVoltage;
using NUnit.Framework;

namespace Sparc.KPK2.Tests
{
  [TestFixture]
  public class VoltagesArrayTest
  {
    [Test]
    public void IsAllChannelTest() // выясняем все ли измерительные каналы учтены
    {
      string voltages1 = "+6.3В;-6.3В;+5В;Р СВЧ;СД 1-1;СД 1-2;СД 1-3;СД 2-1;СД 2-2;СД 2-3;TK 1-1(ДИСС-15);TK 1-2(ДИСС-15);TK 1-3(ДИСС-15);TK 2-1(ДИСС-15);TK 2-2(ДИСС-15);TK 2-3(ДИСС-15);TK 1-1(ДИСС-32);TK 1-2(ДИСС-32);TK 1-3(ДИСС-32);TK 2-1(ДИСС-32);TK 2-2(ДИСС-32);TK 2-3(ДИСС-32);ТКМ 1;ТКМ 2;МОД 1;МОД 2;ГЛПД1;ГЛПД2;Ток ГЛПД;Ток М;Zорт (ПНП);Zорт (ПКВ);";
      string voltages2 = "Wx ПКВ;Wy ПКВ;Wz ПКВ;Wx НКВ;Wy НКВ;Wz НКВ;Wx СПСВ;Wy СПСВ;Wz СПСВ;Wx Тестер;Wy Тестер;Wz Тестер;W;L;" +
                         "+27В;-27В;+27В сл.;-27В оп.;" +
                         "+15В;-15В;-20В;" +
                         "+10В ВCC;+12,6В;-12,6В;ВН(-48В);Δ 15В;Δ 27В;";
      voltages1 += voltages2;
      string[] strings = voltages1.Split(new[] {';'}, StringSplitOptions.RemoveEmptyEntries);

      foreach (var s in strings)
      {
        Assert.DoesNotThrow(() => VoltagesArray.GetVoltagesArray(s));
      }
    }

    [Test]
    public void CountArrayTest()
    {
      string[] names = new[] {"+6.3В", "СД 2-2", "Zорт (ПКВ)", "Wx ПКВ", "L"};
      int expected = 10;
      NewFunction2(names, expected);

      expected = 6;
      names = new[] {"+27В", "-27В оп.", "+15В", "-20В", "+10В ВCC", "-12,6В"};
      NewFunction2(names, expected);

      expected = 14;
      names = new[] {"Δ 15В", "Δ 27В"};
      NewFunction2(names, expected);

      expected = 12;
      names = new[] {"ВН(-48В)"};
      NewFunction2(names, expected);
    }

    void NewFunction2(string[] names, double expected) 
    {
        foreach (string name in names)
        {
            var actual = VoltagesArray.GetVoltagesArray(name).ToArray();
            Assert.AreEqual(expected, actual);
        }
    }

    void NewFunction(string[] names, double[] expected)
    {
        foreach (string name in names)
        {
            var actual = VoltagesArray.GetVoltagesArray(name).ToArray();
            Assert.AreEqual(expected, actual);
        }
    }

    [Test]
    public void ConstraintTest()
    {
      string[] names =  {"+6.3В", "СД 2-2", "Zорт (ПКВ)", "Wx ПКВ", "L"};
      double[] expected =  {0.1, 1.0, 2.0, 5.0, 9.9, -9.9, -5.0, -2.0, -1.0, -0.1};
      NewFunction(names, expected);

      names = new string[] {"+27В", "-27В оп."};
      expected = new[] {15.0, 35.0, 49.0, -49.0, -30.0, -10.0};
      NewFunction(names, expected);

      names = new[] {"+15В", "-20В"};
      expected = new[] {5.0, 15.0, 29.0, -29.0, -20.0, -10.0};
      NewFunction(names, expected);

      names = new[] {"+10В ВCC", "-12,6В"};
      expected = new[] {6.0, 12.0, 19.0, -19.0, -10.0, -5.0};
      NewFunction(names, expected);

      string name1 = "ВН(-48В)";
      expected = new[] {5.0, 10.0, 25.0, 40.0, 60.0, 89.0, -5.0, -10.0, -25.0, -40.0, -60.0, -89.0};
      var actual1 = VoltagesArray.GetVoltagesArray(name1).ToArray();
      Assert.AreEqual(expected, actual1);

      name1 = "Δ 15В";
      expected = new[] {0.05, 0.1, 0.5, 1.0, 10.0, 20.0, 29.0, -29.0, -20.0, -10.0, -1.0, -0.5, -0.1, -0.05};
      actual1 = VoltagesArray.GetVoltagesArray(name1).ToArray();
      Assert.AreEqual(expected, actual1);

      name1 = "Δ 27В";
      expected = new[] {0.05, 0.1, 0.5, 1.0, 10.0, 25.0, 49.0, -49.0, -25.0, -10.0, -1.0, -0.5, -0.1, -0.05};
      actual1 = VoltagesArray.GetVoltagesArray(name1).ToArray();
      Assert.AreEqual(expected, actual1);

    }
  }
}
