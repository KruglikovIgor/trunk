using System;
using System.Collections.Generic;

namespace Sparc.KPK2.Imitator.Hardware
{
  public partial class HImitator
  {
    public static class GenSinCos
    {
      private static ushort _degree;
      private static bool _isOn;
      private static Dictionary<int, double[]> channelSignals = new Dictionary<int, double[]>();
      public static event Action<ushort> AngleChanged;

      private static void AngleChangedFunc(ushort angle)
      {
        try
        {
          if (AngleChanged != null)
            AngleChanged(angle);
        }
        catch
        {
        }
      }

      public static bool GeneratorOn(ushort degree)
      {
        if (degree > 360)
          return false;
        _degree = degree;
        AngleChangedFunc(degree);
        return true;
      }

      public static bool GeneratorOnOff(bool state)
      {
        _isOn = state;
        return true;
      }

      public static bool GetVersion(ref bool isGenerator, ref DateTime date)
      {
        isGenerator = true;
        date = DateTime.Now;
        return true;
      }

      public static bool SetSignal(int channel, int amplitude, double frequency, double angle)
      {
        double[] parameters = new double[] {amplitude, frequency, angle};
        if (channelSignals.ContainsKey(channel))
          channelSignals[channel] = parameters;
        else
          channelSignals.Add(channel, parameters);
        AngleChangedFunc((ushort)angle);
        return true;
      }
    }
  }
}
