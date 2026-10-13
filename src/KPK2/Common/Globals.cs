using System;
using System.Drawing;
using System.Globalization;
using System.Threading;
using System.Windows.Forms;
using KPK2.Common.DissFunctions;
using KPK2.Common.DissFunctions.Diss32;
using KPK2.DB;
using KPK2.GeneralForms;
using KPK2.GeneralForms.DissForms;
using KPK2.GeneralForms.DissForms.Setting;
using KPK2.Properties;
using Sparc.KPK2.Hardware;
using Sparc.KPK2.Hardware.PS115V;
using Sparc.KPK2.Hardware.PS27V;
using Sparc.KPK2.NFunctions;
using Sparc.KPK2.NFunctions.Enums;
using Res = Sparc.KPK2.Resources.Properties.Resources;

namespace KPK2.Common
{
  /// <summary>
  /// Класс общих переменных и методов проекта.
  /// </summary>
  public static class Globals
  {

    #region DevicesID

    public const string TypeDiss32Param = "TypeDiss32";
    public static readonly Color SuccessColor = Color.FromArgb(153, 249, 164);
    public static readonly Color SuccessTbColor = Color.LimeGreen;
    public static readonly Color FailColor = Color.Red;

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
          max = 50;
          break;
        case ValueType.SpeedBackward:
        case ValueType.SpeedLeft:
        case ValueType.SpeedRight:
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
      return string.Format(Resources.ErrorOnDevice, GetDeviceString(dev),
                           GetOperationString(op));
    }

    private static string GetOperationString(Operation op)
    {
      switch (op)
      {
        case Operation.GetValue:
          return Resources.GetValue;
        case Operation.SetValue:
          return Resources.SetValue;
        case Operation.SetVoltage:
          return Resources.SetVoltage;
        case Operation.TurnOn:
          return Resources.TurnOn;
        default:
          return "";
      }
    }

    private static string GetDeviceString(Device dev)
    {
      switch (dev)
      {
        case Device.Power115V:
          return string.Format(Resources.PowerSupplyN, 115);
        case Device.Power27V:
          return string.Format(Resources.PowerSupplyN, 27);
        case Device.DiscreteOutputs:
          return Resources.DiscreteOutputsBoard;
        case Device.ControlTask:
          return Resources.ControlTaskBoard;
        case Device.GenSinCos:
          return Resources.GenSinBoard;
        case Device.DiscreteInputs:
          return Resources.DiscreteInputsBoard;
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
    internal static void ShowErrorMessageBox(string mes)
    {
      MessageBox.Show(mes, Resources.ApplicationName, MessageBoxButtons.OK, MessageBoxIcon.Error);
    }

    /// <summary>
    /// Выдает запрос о подтверждении действия.
    /// </summary>
    /// <param name="mes">Сообщение.</param>
    internal static DialogResult ShowQuestionDialog(string mes)
    {
      return MessageBox.Show(mes, Resources.ApplicationName, MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
    }

    /// <summary>
    /// Ответ на вопрос.
    /// </summary>
    /// <param name="mes">Сообщение.</param>
    internal static DialogResult ShowYesNoDialog(string mes)
    {
      return MessageBox.Show(mes, Resources.ApplicationName, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
    }

    /// <summary>
    /// Выдает предупреждение.
    /// </summary>
    /// <param name="mes">Сообщение.</param>
    internal static DialogResult ShowWarningMessageBox(string mes)
    {
      return MessageBox.Show(mes, Resources.ApplicationName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
    }

    /// <summary>
    /// Выдает информационное сообщение.
    /// </summary>
    /// <param name="mes">Сообщение.</param>
    internal static DialogResult ShowInformationMessageBox(string mes)
    {
      return MessageBox.Show(mes, Resources.ApplicationName, MessageBoxButtons.OK, MessageBoxIcon.Information);
    }

    /// <summary>
    /// Выводит диалог.
    /// </summary>
    /// <param name="mes">Сообщение.</param>
    internal static DialogResult ShowOkCancelDialog(string mes)
    {
      return MessageBox.Show(mes, Resources.ApplicationName, MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
    }

    /// <summary>
    /// Выдает запрос о подтверждении/отказе/отмене действия.
    /// </summary>
    /// <param name="mes">Сообщение.</param>
    internal static DialogResult ShowYesNoCancelDialog(string mes)
    {
      return MessageBox.Show(mes, Resources.ApplicationName, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
    }

    #endregion

    #region Формы

    public static void CantOpenForm(Form frm)
    {
      MessageBox.Show(Resources.OpenFormError, Resources.ErrorCaption, MessageBoxButtons.OK, MessageBoxIcon.Error);
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
      SU_KZ1_Wyr2,
      VCH3438,
      VCH3439,
      VCH351,
      VCH354,
      BVKH241,
      BVKH240,
      VSSH438,
      VSSH439,
      BL8H249,
      BL8H250
    }

    public static Form MainForm
    {
      get { return Application.OpenForms["frmMain"]; }
    }

    public static double MoveToDouble(string value)
    {
      try
      {
        return Convert.ToDouble(value.Replace(".", ","));
      }
      catch
      {
        try
        {
          return Convert.ToDouble(value.Replace(",", "."));
        }
        catch
        {
          return double.NaN;
        }
      }
    }

    public static int MoveToInt32(string value)
    {
      return (int) MoveToDouble(value);
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

    /// <summary>
    ///   Выдает запрос о подтверждении действия.
    /// </summary>
    /// <param name="mes">Сообщение.</param>
    internal static DialogResult ShowQuestion(string mes)
    {
      return MessageBox.Show(mes, Resources.ApplicationName, MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
    }

    /// <summary>
    /// Ошибка ввода в TextBox - ввели не число(double) 
    /// </summary>
    /// <param name="txtBox">TextBox для ввода</param>
    public static void ErrorInputTextBox(TextBox txtBox)
    {
      MessageBox.Show(string.Format(Resources.ErrorInputNValue, txtBox.Text), Resources.ApplicationName,
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
      MessageBox.Show(string.Format(Resources.ErrorDiapazonValue, str, dMin, dMax), Resources.ApplicationName,
                      MessageBoxButtons.OK, MessageBoxIcon.Error);
      txtBox.Focus();
      txtBox.SelectAll();
    }

    internal static bool InitPower27V(double PowerSupply27, double MaxCurrent27)
    {
      try
      {
           Power27V.Instance.SetVoltage(PowerSupply27);
           Power27V.Instance.SetCurrent(MaxCurrent27);
           Power27V.Instance.SetOnOff(false);
      }
      catch
      {
        return false;
      }
      return true;
    }

    internal static bool InitPower115(int PowerSupply115Phase, double MaxCurrent115)
    {
      try
      {
        Power115V.Instance.SetPermitOnOffEx(true);
        Power115V.Instance.SetPhaseEx(PowerSupply115Phase);
        Power115V.Instance.SetMaxCurrentEx(MaxCurrent115);
      }
      catch
      {
        return false;
      }
      return true;
    }

    /// <summary>
    /// Возвращает время работы системы в секундах
    /// </summary>
    /// <returns></returns>
    internal static double Uptime()
    {
      int ticks = Environment.TickCount;
      if (ticks < 0)
        return int.MaxValue - ticks;
      return ticks/1000d;
    }

    private static frmProgress _fProgress;

    public static void ReportProgressStandart(string text)
    {
      int result;
      if (int.TryParse(text, out result))
      {
        _fProgress = new frmProgress();
        _fProgress.SetMaximum(result);
        _fProgress.StartPosition = FormStartPosition.CenterScreen;
        _fProgress.ShowDialog();
      }
      else
      {
        if (_fProgress != null)
          _fProgress.StepIt(1, text);
      }
    }

    public static void CloseProgressForm()
    {
      if (_fProgress != null)
      {
        _fProgress.Close();
        _fProgress = null;
      }
    }

    /// <summary>
    /// Возвращаем не рано ли стартует приложение после запуска ОС (до 3 минут считаем что рано, иначе может не успеть запуститься SQL Server)
    /// </summary>
    /// <returns>True - рано, False - не рано</returns>
    internal static bool EarlyUptime()
    {
      const int earlySec = 180;
      return Uptime() <= earlySec;
    }

    internal static void SetLabelValue(Label lLineResistance, char separator, string value)
    {
      int index = lLineResistance.Text.IndexOf(separator);
      if (index == -1)
        return;
      lLineResistance.Text = lLineResistance.Text.Substring(0, index + 1) + " " + value;
    }

    public static string PathToLogo
    {
      get { return Application.CommonAppDataPath + "\\" + CommonSettings.Default.LogoFile; }
    }

    public static void SetKZBeginParams(bool state)
    {
      bool isKZ = false;
      DateTime kzDate = new DateTime();
      DateTime controlDate = new DateTime(2015, 9, 3);
      if (!ControlTask.Instance().GetVersion(ref isKZ, ref kzDate))
        MessageBox.Show(Resources.DetectKZError);
      if (isKZ && kzDate >= controlDate)
        ControlTask.Instance().SetLimitVoltages(state);
    }

    public static Form GetDeviceForm(DevicesID _dissType, Modification _dissMod, int testID, string deviceNumber, Block5Type b5Type)
    {
      Form frm = null;
      if (_dissType == DevicesID.Diss15) // запускаем проверку ДИСС-15
        frm = new frmWork15(testID, _dissMod, b5Type, deviceNumber);
      if ((_dissType == DevicesID.Diss32_90) || (_dissType == DevicesID.Diss32) || 
          _dissType == DevicesID.Diss32_WorkTest || _dissType == DevicesID.Diss32_90_WorkTest)// запускаем проверку ДИСС-32
      {
	      SettingsObj.GetSettings().TestId = testID;
	      SettingsObj.GetSettings().DBKPK2 = new DataBaseKPK2(testID);
	      frm = new frmWork32(new MeasurementPerformance(_dissType, _dissMod, deviceNumber), Mode.GetMode());
      }
      if (_dissType == DevicesID.Diss13) // запускаем проверку ДИСС-32
        frm = new frmWork13(testID, deviceNumber);
      if (_dissType == DevicesID.Diss3228)
        frm = new frmWork3228(testID, deviceNumber);
      if (_dissType == DevicesID.Diss3228_Monoblock)
          frm = new frmWork3228_Monoblock(testID, deviceNumber);
      return frm;
    }

    internal static void SetThreadCulture(Thread th)
    {
      th.CurrentCulture = Application.CurrentCulture;
      th.CurrentUICulture = new CultureInfo(DataManager.Instance.GetCurrentLanguage());
    }

    public static double ConvertMHzToHz(double MHz)
    {
        return MHz * 1e6;
    }

    public static double ConvertHzToMHz(double Hz)
    {
        return Hz / 1e6;
    }
        /// <summary>
        /// Проверка срабатывания защиты
        /// </summary>
        /// <param name="p27"></param>
        /// <param name="showError"></param>
        /// <returns>True - защита сработала</returns>
    public static bool CheckPS27Protection(Power27V p27, Action<string> showError)
    {
        if (p27.IsProtectionActive())
        {
            p27.SetOnOff(false);
            if (p27.IsOverCurrent())
                showError(string.Format(Resources.PS27VCurrentExceedN, p27.GetSettedCurrent()));
            else
                showError(string.Format(Resources.PS27OverVoltage));
            return true;
        }

        return false;
    }
  }
}