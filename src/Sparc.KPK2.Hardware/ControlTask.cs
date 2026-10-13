using System;
using System.Collections.Generic;
using System.IO.Ports;
using NLog;
using Sparc.KPK2.Config.Hardware;
using Sparc.KPK2.Imitator.Hardware;
using Sparc.KPK2.NFunctions;


namespace Sparc.KPK2.Hardware
{
  public class ControlTask //: ComDevice
  {
    private static Logger _logger = LogManager.GetLogger("KPK2Info");

    private readonly ControlTaskBoard.ControlTask _bControlTask;
    private static ControlTask _instance;

    public const int BaseAmplitudeAttestation = 10;
    public const int BaseShiftAttestation = 88;

    public bool _init = false;

    private ControlTask()
    {
            //SerialPort sp = SpManager.Open(DataManager.Instance.GetModulePort(DataManager.KZ));
      SerialPort sp = SpManager.Open(KPK2HardwareSettings.GetSettings().KZ.Port);
      if (Globals.Emulation)
      {
          _init = true;
          return;
      }

      try
      {
          _bControlTask = new ControlTaskBoard.ControlTask(sp);
          bool isControlTask = false;
          DateTime date = DateTime.Now;
          _init = _bControlTask.GetVersion(ref isControlTask, ref date);
          _init &= isControlTask;
          //var logger = LogManager.GetCurrentClassLogger();
          _logger.Info(string.Format("Плата КЗ подключена: {0} {1}", isControlTask? "Да" : "Нет", date.ToString()));
      }
      catch
      {
          _init = false;
      }
    }

    public static ControlTask Instance()
    {
      return _instance ?? (_instance = new ControlTask());
    }

    public bool GetVersion(ref bool isControlTask, ref DateTime date)
    {
      if (Globals.Emulation)
        return HImitator.ControlTask.GetVersion(ref isControlTask, ref date);
      if (!_init)
          return false;
      return _bControlTask.GetVersion(ref isControlTask, ref date);
    }

    public bool GetVersion()
    {
        if (Globals.Emulation)
            return HImitator.ControlTask.GetVersion();
        if (!_init)
            return false;
        bool isControlTask = false;
        DateTime dt = DateTime.Now;
        return _bControlTask.GetVersion(ref isControlTask, ref dt);
    }

    public bool SetControlTask(int number, int amplitude, int shift)
    {
      if (Globals.Emulation)
        return HImitator.ControlTask.SetControlTask(number, amplitude, shift);
      if (!_init)
          return false;
      if ((number < 0) || (number > 9))
        return false;

      _logger.Trace( string.Format(
        "МКЗ: Вкл КЗ-{0}, амлитуда= {1}, смещение= {2}",
        number, amplitude, shift ));
      return _bControlTask.SetControlTask( number, amplitude, shift);
    }

    public bool SetControlTaskWithCorrection(int number, int amplitude, int shift)
    {
      bool result = true;
      result &= SetControlTask(number, amplitude, shift);
      for (int i = 1; i < 6; i++)
      {
        int curShift = DataManager.Instance.GetKZChannelShift(i);
        if (curShift != -1)
          result &= TunningAmplitude(i, curShift);
      }
      return result;
    }

    /// <summary>
    /// Включает заданную контрольную задачу
    /// </summary>
    /// <param name="name">Наименование задачи</param>
    /// <returns></returns>
    public bool SetControlTask(string name)
    {
      int number = 0, amplitude = 0, shift = 0;
      if (DataManager.Instance.GetControlTaskParamsByName(name, ref number, ref amplitude, ref shift))
      {
        if (Globals.Emulation)
          return HImitator.ControlTask.SetControlTask(number, amplitude, shift);
        return SetControlTask(number, amplitude, shift);
      }
      return false;
    }

    /// <summary>
    /// Включает заданную контрольную задачу
    /// </summary>
    /// <param name="name">Наименование задачи</param>
    /// <returns></returns>
    public void SetControlTaskEx(string name)
    {
      if (!SetControlTask(name))
        throw new Exceptions.ControlTaskException();
    }

    public bool GetControlTask(ref int number, ref int amplitude, ref int shift)
    {
      if (Globals.Emulation)
        return HImitator.ControlTask.GetControlTask(ref number, ref amplitude, ref shift);
      return _bControlTask.GetControlTask(ref number, ref amplitude, ref shift);
    }

    public bool TunningAmplitude(int channel, int amplitude)
    {
      if (Globals.Emulation)
      {
        HImitator.ControlTask.TunningAmplitude(channel, amplitude);
        return true;
      }
      return _bControlTask.TunningAmplitude(channel, amplitude);
    }

    public bool GetFrequency(ref List<double> data)
    {
      if (Globals.Emulation)
        return HImitator.ControlTask.GetFrequency(out data);
      return _bControlTask.GetFrequencies(ref data);
    }

    public bool GetDuration(ref List<double> data)
    {
      if (Globals.Emulation)
        return HImitator.ControlTask.GetDuration(out data);
      return _bControlTask.GetDurations(ref data);
    }

    public bool SetLimitVoltages(bool Minus2_5V)
    {
      byte setByte = (byte) (Minus2_5V ? 0x3F : 0);
      if (Globals.Emulation)
        return HImitator.ControlTask.SetLimitVoltages(setByte);
      return _bControlTask.SetLimitVoltages(setByte);
    }

    /// <summary>
    /// Отключение КЗ
    /// </summary>
    /// <returns></returns>
    public bool ControlTaskOff()
    {
      if (Globals.Emulation)
        return HImitator.ControlTask.SetOff();

      _logger.Info( "МКЗ: Выключение КЗ" );

      return _bControlTask.ControlTaskOff();
    }

    /// <summary>
    /// Отключение КЗ
    /// </summary>
    /// <returns></returns>
    public void ControlTaskOffEx()
    {
      if (!ControlTaskOff())
        throw new Exceptions.ControlTaskException();
    }

    public List<int> GetArrays()
    {
      if (Globals.Emulation)
        return HImitator.ControlTask.GetArrays();
      List<int> result;
      if (!_bControlTask.GetARINCCodes(out result))
        return null;
      return result;
    }
  }
}
