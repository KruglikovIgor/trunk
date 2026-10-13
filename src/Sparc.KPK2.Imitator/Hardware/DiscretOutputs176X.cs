using System;
using System.Collections;

namespace Sparc.KPK2.Imitator.Hardware
{
    public partial class HImitator
    {
        public static class DiscretOutpus1760X
        {
            const int NumReles = 16;                        
            private static BitArray _states = new BitArray(NumReles,false);                                    
                                   
            public static event Action<int> OutputStateChanged;

            public static BitArray GetRelaysState()
            {
                return (BitArray) _states.Clone();
            }

            public static bool GetRelayState(int numRele)
            {
              if (numRele >= _states.Length)
                return false;
              return _states[numRele];
            }

            public static bool SetOutput(int numRele, bool state)
            {
                if ((numRele < 0) || (numRele >= NumReles))
                    return false;
                _states[numRele] = state;
                OutputStateChangedFunc(numRele);
                return true;
            }

            public static bool SetOutputs(Hashtable ht)
            {
              bool result = true;
              foreach (DictionaryEntry de in ht)
                result &= SetOutput((int) de.Key, (bool) de.Value);
              if (result)
                OutputStateChangedFunc(0);
              return result;
            }

            public static bool SetChannelsOff()
            {
                for (int i=0;i<NumReles;i++)
                    _states[i] = false;
                return true;
            }

            public static bool ChangeStatesAllRelays()
            {
                throw new System.Exception("The method or operation is not implemented.");
            }

            public static bool ChangeStatesAllRelays(BitArray ba)
            {
                if ((ba.Length == 0) || (ba.Length >= NumReles))
                    return false;
                for (int i = 0; i < ba.Length; i++)
                    _states[i] = ba[i];
              OutputStateChangedFunc(0);
                return true;
            }

            private static void OutputStateChangedFunc(int numRele)
            {
              try
              {
                if (OutputStateChanged != null)
                  OutputStateChanged(numRele);
              }
              catch
              {}

            }
        }
    }
}
