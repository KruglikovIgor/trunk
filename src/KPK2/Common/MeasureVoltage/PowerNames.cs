using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace KPK2.Common.MeasureVoltage
{
  /// <summary>
  /// Перечень используемых источников питания
  /// </summary>
  public class PowerNamesList
  {
    public List<PowerNames> List { get; set; }

    public PowerNamesList()
    {
        List = new List<PowerNames>();
        List.Add( new PowerNames(Powers.akip1142_3, "AKИП-1142/3"));
    }

    /// <summary>
    /// Класс наименований источников питания
    /// </summary>
    public class PowerNames
    {
      /// <summary>
      /// сокращенное название
      /// </summary>
      public Powers Power { get; set; }
      
      /// <summary>
      /// отображаемое название на панели
      /// </summary>
      public string DisplayName { get; set; }

      public PowerNames(Powers power, string displayName)
      {
        Power = power;
        DisplayName = displayName;
      }


    }

    /// <summary>
    /// Перечисление используемых источников питания
    /// </summary>
    public enum Powers
    {
      akip1142_3
    }

}

  }
