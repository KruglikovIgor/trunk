using System;
using System.Threading;

namespace Sparc.KPK2.Imitator.Hardware
{
    public partial class HImitator
    {
        public static class PowerSupply40
        {
            private const int UpperVoltageLimit = 42;
            private const int UpperCurrentLimit = 19;
            private static double _currentLimit = UpperCurrentLimit;
            private static double _settedVoltage = 0;            
            private static double _voltage = 0;
            private static double _current = 0;
            private static bool _isOn = false;
            private static double _extResist = 27;
            public static event Action<bool> StateChanged;
            public static event Action<double> VoltageChanged;


            public static bool isOn()
            {
                return _isOn;
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

            public static double GetSettedVoltage()
            {
                return _settedVoltage;
            }

            public static double GetSettedCurrent()
            {
                return _currentLimit;
            }

            public static bool SetVoltage(double vlt)
            {
                if ((vlt > UpperVoltageLimit) || (vlt < 0))
                    return false;
                _settedVoltage = vlt;
                CalculateCurrent();
                return true;
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
                _current = _voltage/_extResist;
                if (_current > _currentLimit)
                {
                    _voltage = _currentLimit * _extResist;
                    _current = _currentLimit;
                }
                if (VoltageChanged != null)
                    VoltageChanged(_voltage);
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
                if (StateChanged != null)
                    StateChanged(_isOn);
            }

            public static void SetOff()
            {
                _isOn = false;
                if (StateChanged != null)
                    StateChanged(_isOn);
            }

            public static bool SetOnOffState(bool state)
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
