using System.Collections;
using NLog;
using Sparc.Hardware.Advantech.PCI176X;
using Sparc.KPK2.Imitator.Hardware;
using Sparc.KPK2.NFunctions;

namespace Sparc.KPK2.Hardware
{
    public class DO176X
    {
        private static DO176X _instance;
        public static DO176X Instance
        {
            get { return _instance = _instance ?? new DO176X(); }
        }
        private static Logger _logger = LogManager.GetLogger("KPK2Info");

        //private Logger _logger = LogManager.GetCurrentClassLogger();

        public DO176X()
        {
            if (Globals.Emulation)
                return;
            Board176X.Instance.OpenDevice();
            Board176X.Instance.ReadDO();
            Board176X.Instance.ReadDI(true);
            LogVersion();
        }

        public void LogVersion()
        {
            _logger.Info(string.Format("Плата Advantech подключена: {0} {1} ", Board176X.Instance.CurrentDevice.DeviceNum, Board176X.Instance.CurrentDevice.DeviceName));
        }

        public int ReleCount()
        {
            return Board176X.Instance.PortCount;
        }

        public BitArray GetRelaysState()
        {
            if (Globals.Emulation)
                return HImitator.DiscretOutpus1760X.GetRelaysState();
            Board176X.Instance.ReadDO();
            return Board176X.Instance.GetRelaysState();
        }
        
        public bool SetOutput(int numRele, bool state)
        {
            if (Globals.Emulation)
                return HImitator.DiscretOutpus1760X.SetOutput(numRele, state);
            return Board176X.Instance.ChangeStateRelay(numRele, state);
        }

        public bool SetOutputs(Hashtable ht)
        {
          if (Globals.Emulation)
            return HImitator.DiscretOutpus1760X.SetOutputs(ht);
          return Board176X.Instance.ChangeStateRelays(ht);
        }

        public bool SetChannelsOff()
        {
            if (Globals.Emulation)
                return HImitator.DiscretOutpus1760X.SetChannelsOff();
            return Board176X.Instance.OpenAllRelays();
        }

        public bool ChangeStateAllRelays(BitArray ba)
        {
            if (Globals.Emulation)
                return HImitator.DiscretOutpus1760X.ChangeStatesAllRelays(ba);
            Board176X.Instance.ReadDO();
            return Board176X.Instance.ChangeStateAllRelays(ba);
        }

      #region Diss32 //Обнаружилось что у ДИСС-32 есть смещение на дискретных входах в зависимости от типа платы 1761 или 1762

      public BitArray ReadDI32(bool flag)
      {
        if (Globals.Emulation)
          return HImitator.DiscretOutpus1760X.GetRelaysState();
        if (Board176X.Instance.TypeOfBoard == Board176X.BoardType._1762)
        {
          BitArray ba = Board176X.Instance.ReadDI(flag);
          BitArray result = new BitArray(ba.Length, false);

          for (int i = 0; i < ba.Length - 1; i++)
            result[i + 1] = ba[i];
          
          return result;
        }
        BitArray ba1;
        if (Globals.Emulation)
          ba1 = new BitArray(7, true);
        else
          ba1 = Board176X.Instance.ReadDI(flag);
        return ba1;
      }

      public bool SetBVKRefuse(bool state)
      {
        if (Board176X.Instance.TypeOfBoard == Board176X.BoardType._1762)
          state = !state;
        _logger.Trace("Board: {0} Closing: {1}", Board176X.Instance.TypeOfBoard == Board176X.BoardType._1762, state);
        if (state)
          return Board176X.Instance.CloseRelay(DataManager.Instance.GetChannelByName(DataManager.Advantech1760, "BVK_Refuse"));
        return Board176X.Instance.OpenRelay(DataManager.Instance.GetChannelByName(DataManager.Advantech1760, "BVK_Refuse"));
      }

      public bool SetRelay(int channel)
      {
          return Board176X.Instance.CloseRelay(channel);
      }

      public bool ClearRelay(int channel)
      {
          return Board176X.Instance.OpenRelay(channel);
      }

        #endregion Diss32

    }
}
