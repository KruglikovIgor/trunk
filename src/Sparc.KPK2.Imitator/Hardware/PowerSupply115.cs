using System.Threading;
using System.Windows.Forms;

namespace Sparc.KPK2.Imitator.Hardware
{
    public partial class HImitator
    {
        public static class PowerSupply115
        {
            private const int UpperVoltageLimit = 210;
            private const int UpperCurrentLimit = 19;
            private static double _currentLimit = UpperCurrentLimit;
            private static double _settedVoltage = 163;
            private static int _frequency = 400;
            private static double _voltage = 163;
            private static double _current = 0;
            private static bool _isOn = false;
            private static double _extResist = 115;

          public static MethodInvoker VoltageChanged;
          public static MethodInvoker OutputChanged;


          private static void VoltageChangedFunc()
          {
            try
            {
              if (VoltageChanged != null)
                VoltageChanged();
            }
            catch{}
          }

          private static void OutputChangedFunc()
          {
            try
            {
              if (OutputChanged != null)
                OutputChanged();
            }
            catch { }
          }

            public static double GetVoltage()
            {
                Thread.Sleep(50);
                if (!_isOn)
                    return 0;
                return _voltage;
            }

            public static double GetCurrent()
            {
                Thread.Sleep(50);
                if (!_isOn)
                    return 0;
                return _current;
            }

            public static bool SetVoltage(double vlt)
            {
                if ((vlt > UpperVoltageLimit) || (vlt < 0))
                    return false;
                _settedVoltage = vlt;
                CalculateCurrent();
              VoltageChangedFunc();
                return true;
            }

            public static void SetFrequency(int freq)
            {
                _frequency = freq;
            }

            public static int GetFrequency()
            {
                return _frequency;
            }

            public static bool SetCurrentLimit(double cur)
            {
                if ((cur > UpperCurrentLimit) || (cur <= 0))
                    return false;
                _currentLimit = cur;
                CalculateCurrent();
                return true;
            }

            private static void CalculateCurrent()
            {
                if (!_isOn)
                    return;
                _voltage = _settedVoltage;
                _current = _voltage / _extResist;
                if (_current > _currentLimit)
                {
                    _voltage = _currentLimit * _extResist;
                    _current = _currentLimit;
                }
            }

            public static bool SetExtResistance(double res)
            {
                if (res <= 0)
                    return false;
                _extResist = res;
                CalculateCurrent();
                return true;
            }

            public static void SetOn()
            {
                _isOn = true;
                CalculateCurrent();
                OutputChangedFunc();
            }

            public static void SetOff()
            {
                _isOn = false;
              CalculateCurrent();
              OutputChangedFunc();
            }

            public static bool OutputState()
            {
                return _isOn;
            }

            public static bool OnOffState(bool state)
            {
                if (state)
                    SetOn();
                else
                    SetOff();
                return true;
            }
        }
    }
}