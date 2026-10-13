using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Sparc.KPK2.NFunctions.Enums;
using Res = Sparc.KPK2.Resources.Properties.Resources;

namespace Sparc.KPK2.NFunctions
{
  /// <summary>
  /// Класс общих переменных и методов проекта.
  /// </summary>
  public static class Globals
  {
    #region DevicesID

    public const string TypeDiss32Param = "TypeDiss32";
    public static readonly Color SuccessColor = Color.FromArgb(153, 249, 164);
    public static readonly Color SuccessColorText = Color.LimeGreen;
    public static readonly Color FailColor = Color.Red;

    public static List<Modification> GetAvailableModifications(DevicesID devId)
    {
        switch (devId)
        {
            case DevicesID.None:
            case DevicesID.Diss13:
            case DevicesID.Diss3228:
            case DevicesID.Diss32:
            case DevicesID.Diss32_WorkTest:
	            return new List<Modification>{Modification.None};
            case DevicesID.Diss15:
                return new List<Modification> {Modification.None, Modification.Diss15G};
            case DevicesID.Diss32_90:
	            return new List<Modification> { Modification.None, Modification.Diss32_90A };
            case DevicesID.Diss32_90_WorkTest:
	            return new List<Modification> { Modification.Diss32_90A };
	        case DevicesID.Diss3228_Monoblock:
                return new List<Modification> {Modification.None};
        }
        throw new Exception("Unknown modification!");
    }

    #endregion

    public enum ValueType
    {
      SpeedForward,
      SpeedBackward,
      SpeedDownUncontrolled,
      SpeedUpUncontrolled,
      SpeedDown,
      SpeedLeft,
      SpeedRight,
      FastSpeed1,
      FastSpeed2,
      NegativeAngle,
      ZeroAngle,
      PositiveAngle,
      FastSpeed,
      SpeedUp
    }

    public static void GetMinAndMaxByValue(ValueType valueType, out int min, out int max)
    {
      min = 0;
      max = 0;
      switch (valueType)
      {
        case ValueType.SpeedForward:
          min = -25;
          max = 50;
          break;
        case ValueType.SpeedBackward:
          min = -50;
          max = 25;
        break;
        case ValueType.SpeedLeft:
        case ValueType.SpeedRight:
          min = -25;
          max = 25;
          break;
        case ValueType.NegativeAngle:
        case ValueType.PositiveAngle:
        case ValueType.ZeroAngle:
          min = -46;
          max = 46;
          break;
        case ValueType.FastSpeed1:
        case ValueType.FastSpeed2:
          min = 50;
          max = 400;
          break;
      }
    }

    #region Error On Devices
    
    public static bool Emulation;


    public enum Device
    {
      Power115V,
      Power27V,
      DiscreteOutputs,
      ControlTask,
      GenSinCos,
      DiscreteInputs,
      Advantech176X
    }

    public enum Operation
    {
      GetValue,
      SetValue,
      SetVoltage,
      TurnOn
    }
        
    public static Color PressedButton = Color.FromArgb(153, 249, 164);

    public static string GetDeviceErrorString(Device dev, Operation op)
    {
      return string.Format(Res.ErrorOnDevice, GetDeviceString(dev),
                           GetOperationString(op));
    }

    private static string GetOperationString(Operation op)
    {
      switch (op)
      {
        case Operation.GetValue:
          return Res.GetValue;
        case Operation.SetValue:
          return Res.SetValue;
        case Operation.SetVoltage:
          return Res.SetVoltage;
        case Operation.TurnOn:
          return Res.TurnOn;
        default:
          return "";
      }
    }

    private static string GetDeviceString(Device dev)
    {
      switch (dev)
      {
        case Device.Power115V:
          return string.Format(Res.PowerSupplyN, 115);
        case Device.Power27V:
          return string.Format(Res.PowerSupplyN, 27);
        case Device.DiscreteOutputs:
          return Res.DiscreteOutputsBoard;
        case Device.ControlTask:
          return Res.ControlTaskBoard;
        case Device.GenSinCos:
          return Res.GenSinBoard;
        case Device.DiscreteInputs:
          return Res.DiscreteInputsBoard;
        default:
          return "";
      }
    }

    #endregion

    #region Диалоги

    /// <summary>
    /// Выводит сообщение об ошибке.
    /// </summary>
    /// <param name="mes">Сообщение.</param>
    public static void ShowErrorMessageBox(string mes)
    {
      MessageBox.Show(mes, Res.ApplicationName, MessageBoxButtons.OK, MessageBoxIcon.Error);
    }

    /// <summary>
    /// Выдает запрос о подтверждении действия.
    /// </summary>
    /// <param name="mes">Сообщение.</param>
    public static DialogResult ShowQuestionDialog(string mes)
    {
      return MessageBox.Show(mes, Res.ApplicationName, MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
    }

    /// <summary>
    /// Ответ на вопрос.
    /// </summary>
    /// <param name="mes">Сообщение.</param>
    public static DialogResult ShowYesNoDialog(string mes)
    {
      return MessageBox.Show(mes, Res.ApplicationName, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
    }

    /// <summary>
    /// Выдает предупреждение.
    /// </summary>
    /// <param name="mes">Сообщение.</param>
    public static DialogResult ShowWarningMessageBox(string mes)
    {
      return MessageBox.Show(mes, Res.ApplicationName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
    }

    /// <summary>
    /// Выдает информационное сообщение.
    /// </summary>
    /// <param name="mes">Сообщение.</param>
    public static DialogResult ShowInformationMessageBox(string mes)
    {
      return MessageBox.Show(mes, Res.ApplicationName, MessageBoxButtons.OK, MessageBoxIcon.Information);
    }

    /// <summary>
    /// Выводит диалог.
    /// </summary>
    /// <param name="mes">Сообщение.</param>
    public static DialogResult ShowOkCancelDialog(string mes)
    {
      return MessageBox.Show(mes, Res.ApplicationName, MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
    }

    /// <summary>
    /// Выдает запрос о подтверждении/отказе/отмене действия.
    /// </summary>
    /// <param name="mes">Сообщение.</param>
    public static DialogResult ShowYesNoCancelDialog(string mes)
    {
      return MessageBox.Show(mes, Res.ApplicationName, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
    }

    #endregion

    #region Формы

    public static void CantOpenForm(Form frm)
    {
      MessageBox.Show(Res.OpenFormError, Res.ErrorCaption, MessageBoxButtons.OK, MessageBoxIcon.Error);
      frm.Close();
    }

    #endregion

    public enum NumberSlotUsb
    {
      LTR114 = 11,
      LTR11_1 = 12,
      LTR11_2 = 13,
      LTR11_3 = 14,
      LTR22_1 = 15,
      LTR22_2 = 16
    }

    public enum IndicatorNumber
    {
      Power115_U = 100,
      Power115_I = 101,
      Speed1 = 102,
      Speed2 = 103,
      Speed3 = 104,
      Zero1,
      Zero2,

      Zero3,
      Zero4,
      ZoptPVK,
      ZoptPNP,
      Speed1_2,
      Speed2_2,
      MapCorner,
      Aist4W,
      Aist4A,
      Aist5A,
      Aist6A,
      Aist6W,
      SAU6A,
      SAU1Wxr,
      SAU1Wyr,
      SUWxr,
      SUWzr,
      SUWyr,
      SU_KZ4_Wyr,
      LandyshKZ2Wxr,
      LandyshKZ2Wzr,
      LandyshKZ4Wxr,
      LandyshKZ4Wzr,
      LandyshKZ6Wzr,
      LandyshKZ6Wxr,
      LandyshKZ5Wzr,
      LandyshStator,
      SAU2Wzr,
      SAU3Wxr,
      SAU3Wyr,
      SAU3Wzr,
      LandyshTime1,
      LandyshTime2,
      ChannelMeas1,
      ChannelMeas2,
      ChannelMeasCurrent1,
      ChannelMeasCurrent2,
      SU_KZ5_Wxr,
      SU_KZ5_Wzr,
      Zero1PKV,
      Zero2PKV,
      Zero3PKV,
      Zero1Tester,
      Zero2Tester,
      Zero3Tester,
      Zero5,
      Zero6,
      AistZeroA,
      SU_KZ1_Wyr,
      SU_KZ1_Wyr2
    }

    public static Form MainForm
    {
      get { return Application.OpenForms["frmMain"]; }
    }

    //public static double MoveToDouble(string value)
    //{
    //  try
    //  {
    //    return Convert.ToDouble(value.Replace(".", ","));
    //  }
    //  catch
    //  {
    //    try
    //    {
    //      return Convert.ToDouble(value.Replace(",", "."));
    //    }
    //    catch
    //    {
    //      return double.NaN;
    //    }
    //  }
    //}

    public static int MoveToInt32(string value)
    {
      return (int) Functions.MoveToDouble(value);
    }

    public static float MoveToSingle(string value)
    {
      try
      {
        return Convert.ToSingle(value.Replace(".", ","));
      }
      catch
      {
        return Convert.ToSingle(value.Replace(",", "."));
      }
    }

    private static readonly string[] strProcessed = new string[] { "Alfa_0_15", "Task3_PC2_32", "Task4_PC2_32" };
    //Проверяем нужна ли дополнительная обработка сигналу
    public static bool NeedProcessing(string paramName)
    {
      for (int i = 0; i < strProcessed.Length; i++)
        if (paramName == strProcessed[i])
        {
          return true;
        }
      return false;
    }

    //Обрабатываем результаты для сигналов, которые этого требуют
    public static double Processing(string paramName, double result)
    {
      switch (paramName)
      {
        //case "Alfa_0_15":
        //  result = (int) result + (result - (int) result)*0.6; // в град.и мин.
        //  break; никогда ранее не применялся
        case "Task3_PC2_32":
        case "Task4_PC2_32":
          result /= -1.494; // Временно для подгона //TODO: исправить после команды электронщиков
          break;
      }
      return result;
    }

    /// <summary>
    /// Ошибка ввода в TextBox - ввели не число(double) 
    /// </summary>
    /// <param name="txtBox">TextBox для ввода</param>
    public static void ErrorInputTextBox(TextBox txtBox)
    {
      MessageBox.Show(string.Format(Res.ErrorInputNValue, txtBox.Text), Res.ApplicationName,
                      MessageBoxButtons.OK, MessageBoxIcon.Error);
      txtBox.Focus();
      txtBox.SelectAll();
    }

    /// <summary>
    /// Ошибка ввода в TextBox - ввели число, которое выходит
    /// за допустимые границы
    /// </summary>
    /// <param name="txtBox">TextBox для ввода</param>
    /// <param name="dMin">минимальное вводимое значение</param>
    /// <param name="dMax">максимальное вводимое значение</param>
    /// <param name="str">название параметра</param>
    public static void ErrorCorridorTextBox(TextBox txtBox, double dMin, double dMax, string str)
    {
      MessageBox.Show(string.Format(Res.ErrorDiapazonValue, str, dMin, dMax), Res.ApplicationName,
                      MessageBoxButtons.OK, MessageBoxIcon.Error);
      txtBox.Focus();
      txtBox.SelectAll();
    }

    /// <summary>
    /// Возвращает время работы системы в секундах
    /// </summary>
    /// <returns></returns>
    public static double Uptime()
    {
      int ticks = Environment.TickCount;
      if (ticks < 0)
        return int.MaxValue - ticks;
      return ticks/1000d;
    }

    /// <summary>
    /// Возвращаем не рано ли стартует приложение после запуска ОС (до 3 минут считаем что рано, иначе может не успеть запуститься SQL Server)
    /// </summary>
    /// <returns>True - рано, False - не рано</returns>
    public static bool EarlyUptime()
    {
      const int earlySec = 180;
      return Uptime() <= earlySec;
    }

    public static void SetLabelValue(Label lLineResistance, char separator, string value)
    {
      int index = lLineResistance.Text.IndexOf(separator);
      if (index == -1)
        return;
      lLineResistance.Text = lLineResistance.Text.Substring(0, index + 1) + " " + value;
    }

    public static string GetDissCaption(DevicesID _dissType, Modification mod)
    {
      if (_dissType == DevicesID.Diss3228)
        return Res.Diss3228;
      if (_dissType == DevicesID.Diss3228_Monoblock)
          return Res.Diss3228_Monoblock;
      if (_dissType == DevicesID.Diss13)
        return Res.Diss13;
      if (_dissType == DevicesID.Diss15)
        if (mod == Modification.None)
          return Res.Diss15;
        else
        //if (mod == Modification.Diss15D)
        //    return Res.Diss15D;
        //else
            return Res.Diss15G;
      if (_dissType == DevicesID.Diss32)
        return Res.Diss32;
      if (_dissType == DevicesID.Diss32_90)
        if (mod == Modification.Diss32_90A)
          return Res.Diss32_90A;
        else
          return Res.Diss32_90;
      if (_dissType == DevicesID.Diss32_90_WorkTest)
          if (mod == Modification.Diss32_90A)
		      return Res.Diss32_90A_CheckWork;
          else
	          return Res.Diss32_90_CheckWork;
      if (_dissType == DevicesID.Diss32_WorkTest)
	      return Res.Diss32_CheckWork;
      throw new Exception("Unknown modification!");
    }

    public static Modification GetModificationByStr(string mod)
    {
      if (mod == Modification.Diss15G.ToString())
        return Modification.Diss15G;
      //if(mod == Modification.Diss15D.ToString())
      //    return Modification.Diss15D;
      if (mod == Modification.Diss32_90A.ToString())
        return Modification.Diss32_90A;
      return Modification.None;
    }

    /// <summary>
    /// Разница между двумя углами по минимальной дуге
    /// </summary>
    /// <param name="angle1">Угол в градусах от -360 до 360</param>
    /// <param name="angle2">Угол в градусах от -360 до 360</param>
    /// <returns>Результат в градусах от 0 до 180</returns>
    public static double DiffAngleMin(double angle1, double angle2)
    {
      double diff = angle2 > angle1 ? 360 - angle2 + angle1 : 360 - angle1 + angle2;
      return Math.Min(Math.Abs(angle1 - angle2), Math.Abs(diff));
    }

    public static double NormalizeAngle(double result)
    {
      while (result > 360)
        result -= 360;
      while (result < 0)
        result += 360;
      return result;
  }

    /// <summary>
    /// Это было сделано чтобы когда напряжение выключено не отображалось напряжение вида 0,1В, 0,2В
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public static double Process115Voltage(double value)
    {
        double result = value;
        return result < 0.6 ? 0 : result;
    }

    /// <summary>
    /// Это было сделано чтобы когда напряжение выключено не отображалось напряжение вида 0,1В, 0,2В
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public static double Process27Voltage(double value)
    {
      double result = value;
      return result < 0.5 ? 0 : result;
    }
  }
}