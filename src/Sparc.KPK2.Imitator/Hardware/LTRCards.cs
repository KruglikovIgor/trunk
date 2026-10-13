using System.Collections.Generic;

namespace Sparc.KPK2.Imitator.Hardware
{
    public partial class HImitator
    {
      public static class LTRCards
      {
        private static Dictionary<int, Dictionary<int, double>> slotList = new Dictionary<int, Dictionary<int, double>>();

        public static void SetVoltage(int slot, int channel, double voltage)
        {
          if (!slotList.ContainsKey(slot))
            slotList.Add(slot, new Dictionary<int, double>());
          if (!slotList[slot].ContainsKey(channel))
            slotList[slot].Add(channel, voltage);
          else
            slotList[slot][channel] = voltage;
        }

        public static double GetVoltage(int slot, int channel)
        {
          if (slotList.ContainsKey(slot))
            if (slotList[slot].ContainsKey(channel))
              return slotList[slot][channel];
          return double.NaN;
        }
      }
    }
}
