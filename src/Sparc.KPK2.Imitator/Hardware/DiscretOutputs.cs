using System;
using System.Collections;
using System.Collections.Generic;

namespace Sparc.KPK2.Imitator.Hardware
{
    public partial class HImitator
    {
        public static class DiscretOutputs
        {
            const int NumReles = 24;
            const int GroupCount = 4;
            private const int ItemsInGroup = 6;
            private static BitArray _states = new BitArray(NumReles,false);
            private static int[] _immunable = {22, 23};
            
            public static event Action<int> ReleStateChanged;

          private static void ReleStateChangedFunc(int rele)
          {
            try
            {
              if (ReleStateChanged != null)
                ReleStateChanged(rele);
            }
            catch {}
          }

            public static bool SetOutputs(BitArray states)
            {
                if ((states.Length > NumReles) || (states.Length == 0))
                    return false;
                for (int i = 0; i < states.Length; i++)
                    _states[i] = states[i];
                foreach (int n in _immunable)
                    _states[n] = false;
              ReleStateChangedFunc(0);
                return true;
            }

            public static bool GetOutputs(ref BitArray states)
            {
                states = (BitArray)_states.Clone();
                return true;
            }

            public static bool GetOutput(int channel)
            {
              if (_states.Length > channel)
                return _states[channel];
              return false;
            }

            public static bool SetOutput(int number, bool state)
            {
                if ((number<0) || (number > NumReles - 1 ))
                    return false;
                _states[number] = state;
              ReleStateChangedFunc(number);
                return true;
            }

            public static bool SetGroupOutputs(int group, BitArray states)
            {
                if ((group > GroupCount - 1) || (states.Length == 0) || (states.Length > ItemsInGroup))
                    return false;
                
                for (int i = 0; i < ItemsInGroup; i++)
                    _states[group*ItemsInGroup + i] = states[i];
                ReleStateChangedFunc(0);
                return true;
            }

            public static bool SetChannelsOff()
            {
                for (int i=0;i<NumReles;i++)
                    _states[i] = false;
                ReleStateChangedFunc(0);
                return true;
            }

          public static bool GetVersion(ref bool isDO, ref DateTime date)
          {
            isDO = true;
            date = DateTime.Now;
            return true;
          }

          public static bool SetOutputs(int group, BitArray states)
          {
            return true;
          }

          public static bool SetOutputs(Dictionary<int, bool> table)
          {
            foreach (KeyValuePair<int, bool> pair in table)
              _states[pair.Key] = pair.Value;
            ReleStateChangedFunc(0);
            return true;
          }
        }
    }
}
