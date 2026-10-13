using System;
using System.Collections.Generic;
using System.Data;
using System.IO.Ports;
using NLog;
using Sparc.KPK2.Config.Hardware;
using Sparc.KPK2.Imitator.Hardware;
using Sparc.KPK2.NFunctions;

namespace Sparc.KPK2.Hardware
{
  public class GenSinCos
  {
    private static Logger _logger = LogManager.GetLogger("KPK2Info");
    private GenSinCosBoard.GenSinCos _bSinCos;
    private static GenSinCos _instance;
    public const double AngleSaveKoef = 10;

    private GenSinCos()
    {
            //SerialPort sp = SpManager.Open(DataManager.Instance.GetModulePort(DataManager.SKT));
      SerialPort sp = SpManager.Open(KPK2HardwareSettings.GetSettings().SKT.Port);
      if (sp == null)
        return;
      if (!Globals.Emulation)
        _bSinCos = new GenSinCosBoard.GenSinCos(sp);
    }

    public static GenSinCos Instance()
    {
      return _instance ?? (_instance = new GenSinCos());
    }

    public bool GetVersion(ref bool isGenerator, ref DateTime date)
    {
      if (Globals.Emulation)
        return HImitator.GenSinCos.GetVersion(ref isGenerator, ref date);
      if (_bSinCos == null)
        return false;
      return _bSinCos.GetVersion(ref isGenerator, ref date);
    }

    /// <summary>
    /// Убирает напряжение с выхода генератора
    /// </summary>
    /// <returns></returns>
    public bool GeneratorOff()
    {
      if (Globals.Emulation)
        return HImitator.GenSinCos.GeneratorOnOff(false);
      if (_bSinCos == null)
        return false;

      _logger.Info( "СКТ: Выключение");

      return _bSinCos.GeneratorOff();
    }

    private string GenSinLimit = "Gen_Sin_Limit";

    /// <summary>
    /// Убирает напряжение с выхода генератора
    /// </summary>
    public void GeneratorOffEx()
    {
      if (!GeneratorOff())
        throw new Exceptions.GeneratorOffException();
    }

    private const int SelsinRefVoltageLow = 7000;
    public const int SelsinRefVoltageNormal = 36000;
    public static int SelsinRefVoltage = SelsinRefVoltageNormal;
    private const int SelsinPhaseChannelsCount = 3;
    public const int SelsinFreq = 400; //частота сигнала
    private ushort _curAngle;

    public int _voltage1;
    public int _voltage2;
    public int _shift1;
    public int _shift2;

    public bool GeneratorOnLastAngle()
    {
        return GeneratorOn(_curAngle);
    }

    /// <summary>
    /// Включает генератор сразу на нужный угол
    /// </summary>
    /// <param name="degree">Угол поворота</param>
    /// <returns></returns>
    public bool GeneratorOn(ushort degree)
    {
      if (degree > 360)
        return false;
      
      // Гарантированный интервал после предыдущей команды (м.б. выключения)
      // на переходный процесс в ПлСКТ.
      System.Threading.Thread.Sleep( 1000);
      _curAngle = degree;

      int shift1, shift2, voltage1, voltage2;
      GetSKTPhaseVoltageAndShift(1, degree, out voltage1, out shift1);
      GetSKTPhaseVoltageAndShift(2, degree, out voltage2, out shift2);
      double koef = CalculateKoef(voltage1, voltage2); //Расчёт коэффициента для повышения напряжения до номинального
      _voltage1 = (int)(voltage1*koef);
      _voltage2 = (int)(voltage2*koef);
      _shift1 = shift1;
      _shift2 = shift2;
      if (!Globals.Emulation)
      {
          if (_bSinCos == null)
              return false;
          if (_bSinCos.IsOldVersion)
              return _bSinCos.GeneratorOn(degree, Convert.ToByte(DataManager.Instance.GetParamCommand(GenSinLimit)));
      }
      else
          return HImitator.GenSinCos.GeneratorOn(degree);

      bool result = true;
      result &= SetRealSignal(0, SelsinRefVoltage, SelsinFreq, 0);

      for (int i = 1; i < SelsinPhaseChannelsCount; i++)
      {
        int voltage;
        int shift;
        GetSKTPhaseVoltageAndShift(i, degree, out voltage, out shift);

        _logger.Info( string.Format( 
          "GenSinCos: Вкл канал={0}], угол={1}, напряжение={2}, смещение={3}",
          i, degree, voltage, shift ) );

        result &= SetRealSignal(i, (int)(voltage*koef), SelsinFreq, shift);
      }
      return result;
    }

    private double CalculateKoef(int voltage1, int voltage2)
    {
        if (voltage1 > voltage2)
            return SelsinRefVoltage/(double)voltage1;
        return SelsinRefVoltage/(double) voltage2;
    }

    public static void GetSKTPhaseVoltageAndShift(int phase, int angle, out int voltage, out int shift)
    {
      voltage = GetSKTPhaseVoltage(phase, angle);
      shift = voltage < 0 ? 180 : 0;
      voltage = Math.Abs(voltage);
    }

    /// <summary>
    ///   Вычисляет напряжение для угла СКТ по фазе и углу
    /// </summary>
    /// <param name="phase"></param>
    /// <param name="angle"></param>
    /// <returns></returns>
    public static int GetSKTPhaseVoltage(int phase, int angle)
    {
      if (phase > 2)
        return 0;
      double radAngle = angle * Math.PI / 180;
      return (int)(SelsinRefVoltage * (phase == 1 ? Math.Sin(radAngle) : Math.Cos(radAngle)));
    }

    //private void GetSelsinPhaseVoltageAndShift(int phase, ushort angle, out int voltage, out int shift)
    //{
    //  voltage = GetSelsinPhaseVoltage(phase, angle);
    //  shift = voltage < 0 ? 180 : 0;
    //  voltage = Math.Abs(voltage);
    //}

    //private int GetSelsinPhaseVoltage(int phase, ushort angle)
    //{
    //  if (phase > 2)
    //    return 0;
    //  return (int)((SelsinRefVoltage * Math.Sin((angle - 120 * phase) * Math.PI / 180)) * Math.Sqrt(2));
    //}

    /// <summary>
    /// Включает генератор сразу на нужный угол
    /// </summary>
    /// <param name="degree">Угол поворота</param>
    /// <returns></returns>
    public void GeneratorOnEx(ushort degree)
    {
      if (!GeneratorOn(degree))
          throw new Exceptions.GeneratorOnException();
    }

    /// <summary>
    /// Устанавливает сигнал заданной формы на выходе генератора синуса-косинуса
    /// </summary>
    /// <param name="channel"></param>
    /// <param name="amplitude"></param>
    /// <param name="frequency"></param>
    /// <param name="angle"></param>
    /// <returns></returns>
    public bool SetSignal(int channel, int amplitude, double frequency, double angle)
    {
      if (Globals.Emulation)
        return HImitator.GenSinCos.SetSignal(channel, amplitude, frequency, angle);
      if (_bSinCos == null)
        return false;

      _logger.Trace(
          string.Format( "СКТ: Вкл( канал={0}], амплитуда={1}, частота={2}, угол={3}",
          channel, amplitude, frequency, angle ) );

      return _bSinCos.SetSignal(channel, amplitude, frequency, angle);
    }

    public void SetRealSignalEx(int channel, int amplitude, double frequency, double angle)
    {
      if (!SetRealSignal(channel,amplitude,frequency,angle))
        throw new Exception();
    }

    public bool SetRealSignal(int channel, int amplitude, double frequency, double angle)
    {
      amplitude = CorrectAmplitude(channel, amplitude);
      double origAngle = angle;
      angle = CorrectPhase(channel, frequency, angle);
      _logger.Trace("Оригинальный угол: {0} Скорректированный угол:{1}", origAngle, angle);
      return SetSignal(channel, amplitude, frequency, angle);
    }

    public bool SetRealAmplitudeSignal(int channel, int amplitude, double frequency, double angle)
    {
      amplitude = CorrectAmplitude(channel, amplitude);
      return SetSignal(channel, amplitude, frequency, angle);
    }

    private double CorrectPhase(int channel, double freq, double phase)
    {
      if (channel == 0)
        return phase; //0 это опорный канал, для него фаза не корректируется

      DataTable dtPhaseCalibration = GetChannelCalibration(channel);

      List<CalibratePoint> calPoints = new List<CalibratePoint>();
      for (int i = 0; i < dtPhaseCalibration.Rows.Count; i++)
        calPoints.Add(new CalibratePoint(Convert.ToDouble(dtPhaseCalibration.Rows[i][numColIdealValue]) / AngleSaveKoef, Convert.ToDouble(dtPhaseCalibration.Rows[i][numColValue]) / AngleSaveKoef));

        if (channel == 1)
            phase += 180;
        while (phase >= 360)
            phase -= 360;

        phase = CalibrateFunctions.CorrectPhase(calPoints, freq, phase, CorrectPhaseByFreq);
        return phase;    
    }

    private DataTable GetChannelCalibration(int channel)
    {
      return DataManager.Instance.GetCalibration(DataManager.GenSinCosModuleId, channel + 2);
    }

    /// <summary>
    /// Коррекитровка фазы в зависимости от частоты
    /// </summary>
    /// <param name="freq">частота сигнала</param>
    /// <param name="phase">фаза</param>
    /// <param name="corrPhase">скорректированная фаза (по таблице калибровки)</param>
    /// <returns></returns>
    private double CorrectPhaseByFreq(double freq, double phase, double corrPhase)
    {
      const int idealFreq = 3000;
      corrPhase = Globals.NormalizeAngle(corrPhase);
      if (Math.Abs(corrPhase - phase) > 180)
      {
        if (corrPhase > phase)
          corrPhase -= 360;
        else
          phase -= 360;
      }
      double result = (corrPhase - phase) / idealFreq * freq + phase;
      result = Globals.NormalizeAngle(result);
      return result;
    }

    private const int numColIdealValue = 3;
    private const int numColValue = 4;

    private int CorrectAmplitude(int channel, int amplitude)
    {
      DataTable dtResult = DataManager.Instance.GetCalibration(DataManager.GenSinCosModuleId, channel);

      double maxIdealValue = double.NaN, minIdealValue = double.NaN;
      int maxRealValue = 0, minRealValue = 0;

      if (dtResult.Rows.Count == 0)
        return amplitude;

      if (dtResult.Rows.Count == 1)
        return
          (int)
          (Convert.ToDouble(dtResult.Rows[0][numColValue])/Convert.ToDouble(dtResult.Rows[0][numColIdealValue])*
           amplitude);
      if (dtResult.Rows.Count == 2)
      {
        int first = 0, second = 1;
        double ideal1 = Convert.ToDouble(dtResult.Rows[first][numColIdealValue]);
        double ideal2 = Convert.ToDouble(dtResult.Rows[second][numColIdealValue]);
        if (ideal1 > ideal2)
        {
          first = 1;
          second = 0;
        }
        ideal1 = Convert.ToDouble(dtResult.Rows[first][numColIdealValue]);
        ideal2 = Convert.ToDouble(dtResult.Rows[second][numColIdealValue]);
        double value1 = Convert.ToDouble(dtResult.Rows[first][numColValue]);
        double value2 = Convert.ToDouble(dtResult.Rows[second][numColValue]);

        double k = (value2 - value1)/(ideal2 - ideal1);
        double b = value1 - k*ideal1;
        return (int) (k*amplitude + b);
      }

      GetMinAndMaxPointByAmplitude(dtResult, amplitude, ref maxIdealValue, ref maxRealValue, ref minIdealValue, ref minRealValue);

      if (double.IsNaN(maxIdealValue))
      {
        //double minIdealValuePrev = double.NaN;
        //double minRealValuePrev = double.NaN;
        //GetMaxPointLessThan(dtResult, minIdealValue, ref minIdealValuePrev, ref minRealValuePrev);
        return (int) (minRealValue/minIdealValue*amplitude);
      }

      if (double.IsNaN(minIdealValue))
      {
       // double maxIdealValuePast = double.NaN;
        return (int) (maxRealValue/maxIdealValue*amplitude);
      }

      int diffRealValue = maxRealValue - minRealValue;
      double diffIdealvalue = maxIdealValue - minIdealValue;

      return (int) (minRealValue + (amplitude - minIdealValue)*(diffRealValue/diffIdealvalue));
    }

    private void GetMinAndMaxPointByAmplitude(DataTable dtResult, double amplitude, ref double maxIdealValue, ref int maxRealValue, ref double minIdealValue, ref int minRealValue)
    {
      for (int i = 0; i < dtResult.Rows.Count; i++)
      {
        double curIdealValue = Convert.ToDouble(dtResult.Rows[i][numColIdealValue]);
        if (curIdealValue >= amplitude)
          if (double.IsNaN(maxIdealValue) || (curIdealValue < maxIdealValue))
          {
            maxIdealValue = curIdealValue;
            maxRealValue = Convert.ToInt32(dtResult.Rows[i][numColValue]);
          }

        if (curIdealValue < amplitude)
          if (double.IsNaN(minIdealValue) || (curIdealValue > minIdealValue))
          {
            minIdealValue = Convert.ToDouble(dtResult.Rows[i][numColIdealValue]);
            minRealValue = Convert.ToInt32(dtResult.Rows[i][numColValue]);
          }
      }
    }

    public enum SelsinRefVoltageType
    {
      Normal,
      Low
    }

    public static void SetSelsinRefVoltage(SelsinRefVoltageType type)
    {
      switch (type)
      {
        case SelsinRefVoltageType.Low:
          SelsinRefVoltage = SelsinRefVoltageLow;
          break;
        case SelsinRefVoltageType.Normal:
          SelsinRefVoltage = SelsinRefVoltageNormal;
          break;
      }
    }
  }
}
