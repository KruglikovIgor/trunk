using System;
using System.Collections;
using System.Collections.Generic;
using System.Timers;

namespace Sparc.KPK2.Imitator.Hardware
{
    public partial class HImitator
    {
        public static class ControlTask
        {
            private const int NumChannels = 6;
            private static int[] _frequencies = new int[NumChannels];
            private static int[] _durations = new int[NumChannels];
            private static int[] _arrays = new[] { 2458, 2349, 9283 };
            private static List<int[]> _controlTasks = new List<int[]>();
            private const int controlTaskCount = 10;
            private static int _currentControlTask = 0;
            private const int NumChannel = 0;
            private const int AmplitudeChannel = 1;
            private const int ShiftChannel = 2;


            private static Timer tMeasure = new Timer();

            public delegate void DataReceivedDelegate(BitArray ba);

            public static DataReceivedDelegate _callback;

            public static event Action<int> ControlTaskChanged;

            private static void ControlTaskChangedFunc(int task)
            {
                try
                {
                    if (ControlTaskChanged != null)
                        ControlTaskChanged(task);
                }
                catch
                {
                }
            }

            private static List<int[]> ControlTasks
            {
                get
                {
                    if (_controlTasks.Count == 0)
                        for (int i = 0; i < controlTaskCount; i++)
                            _controlTasks.Add(new int[3]);
                    return _controlTasks;
                }
            }


            public static bool GetDuration(out List<double> data)
            {
                data = new List<double>();
                for (int i = 0; i < _durations.Length; i++)
                    data.Add(_durations[i]);
                return true;
            }

            public static bool GetVersion()
            {
                return true;
            }

            public static bool SetDuration(List<int> data)
            {
                for (int i = 0; i < data.Count; i++)
                    _durations[i] = data[i];
                return true;
            }

            public static bool GetFrequency(out List<double> data)
            {
                data = new List<double>();
                for (int i = 0; i < _frequencies.Length; i++)
                    data.Add(_frequencies[i]);
                return true;
            }

            public static bool SetFrequency(List<int> data)
            {
                for (int i = 0; i < data.Count; i++)
                    _frequencies[i] = data[i];
                return true;
            }

            public static int GetControlTaskNumber()
            {
                return ControlTasks[_currentControlTask][0];
            }

            public static int GetControlTaskAmplitude()
            {
                return ControlTasks[_currentControlTask][1];
            }

            public static bool GetControlTask(ref int number, ref int amplitude, ref int shift)
            {
                number = ControlTasks[_currentControlTask][NumChannel];
                amplitude = ControlTasks[_currentControlTask][AmplitudeChannel];
                shift = ControlTasks[_currentControlTask][ShiftChannel];
                return true;
            }


            public static bool SetControlTask(int number, int amplitude, int shift)
            {
                _currentControlTask = number;
                ControlTasks[_currentControlTask][NumChannel] = number;
                ControlTasks[_currentControlTask][AmplitudeChannel] = amplitude;
                ControlTasks[_currentControlTask][ShiftChannel] = shift;
                ControlTaskChangedFunc(number);
                return true;
            }

            public static void TunningAmplitude(int channel, int amplitude)
            {
                ControlTasks[channel][AmplitudeChannel] = amplitude;
            }

            public static bool GetVersion(ref bool isControlTask, ref DateTime date)
            {
                isControlTask = true;
                date = DateTime.Now;
                return true;
            }

            public static bool SetOff()
            {
                return SetControlTask(0, 0, 0);
            }

            public static int GetCurrentTask()
            {
                return ControlTasks[_currentControlTask][NumChannel];
            }

            public static List<int> GetArrays()
            {
                List<int> result = new List<int>(_arrays);
                return result;
            }

            internal static void SetArrayValue(int index, int value)
            {
                if (_arrays.Length > index)
                    _arrays[index] = value;
            }

            public static bool SetLimitVoltages(byte setByte)
            {
                return true;
            }
        }
    }
}
