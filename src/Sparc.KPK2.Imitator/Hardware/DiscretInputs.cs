using System;
using System.Collections;
using System.Timers;

namespace Sparc.KPK2.Imitator.Hardware
{
    public partial class HImitator
    {
        public static class DiscretInputs
        {
            const int NumReles = 24;
            const int GroupCount = 3;
            private const int ItemsInGroup = 8;
            private static BitArray _states = new BitArray(NumReles,false);            
            private static Timer tMeasure = new Timer() { Interval =  100};
            public delegate void DataReceivedDelegate(BitArray ba);
            public static DataReceivedDelegate _callback;
                                   
            //public static event Action<int> InputStateChanged;

            public static BitArray GetInputsStates()
            {
                return (BitArray) _states.Clone();
            }

            public static bool SetInputState(int num, bool value)
            {
                if ((num<0) || (num >= NumReles))
                    return false;
                _states[num] = value;
                return true;
            }

            public static bool StartMeasuring(DataReceivedDelegate callback)
            {
                _callback = callback;
                tMeasure = new Timer() { Interval =  100};
                tMeasure.Elapsed += Measure_Elapsed;
                tMeasure.Start();
                return true;
            }

            public static bool StopMeasuring()
            {
                tMeasure.Stop();
                return true;
            }


            private static void Measure_Elapsed(object sender, ElapsedEventArgs e)
            {

                if (_callback != null)
                    _callback(GetInputsStates());
            }

            public static bool GetVersion(ref bool isDiscreteInputs, ref DateTime date)
            {
              isDiscreteInputs = true;
              date = DateTime.Now;
              return true;
            }

            public static bool GetVersion()
            {
                return true;
            }
        }
    }
}
