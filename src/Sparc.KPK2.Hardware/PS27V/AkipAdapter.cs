using System;
using System.Threading;
using Sparc.KPK2.Imitator.Hardware;
using Sparc.KPK2.NFunctions;

namespace Sparc.KPK2.Hardware.PS27V
{
    public class AkipAdapter : IPower27V
    {
        private Sparc.Hardware.Akip_1133_40_19.Akip_1133_40_19 _akip = Sparc.Hardware.Akip_1133_40_19.Akip_1133_40_19.CreateLAN("192.168.1.2");

        public bool Init()
        {
            if (_akip.IsConnected)
                return true;
            try
            {
                _akip.Connect("192.168.1.2");
                _akip.SetVoltageLimit(0);
                _akip.SetVoltageProtection(33);
                _akip.SetRampUpTime(0.1);
                _akip.OutputProtectionClear();
                _akip.SetCurrent(1);
                _akip.SetCurrentProtection(true);
            }
            catch
            {
                return false;
            }
            if (!_akip.IsConnected)
                return false;
            return true;
        }

        public bool? GetOutputState()
        {
            if (Globals.Emulation)
                return HImitator.PowerSupply40.isOn();
            if (!Init())
                return null;
            return _akip.GetOutputState();
        }

        public bool OnOffState(bool state)
        {
            try
            {
                if (Globals.Emulation)
                    return HImitator.PowerSupply40.SetOnOffState(state);
                if (Init())
                {
                    bool curProtState = _akip.GetCurrentProtection();
                    if (curProtState)
                        _akip.SetCurrentProtection(false);

                    _akip.SetOutput(state);

                    if (curProtState)
                    {
                        Thread.Sleep(150);
                        _akip.SetCurrentProtection(true);
                    }
                    Thread.Sleep(150);
                    return true;
                }
                return false;
            }
            catch
            {
                throw new Exception("OnOffState failed!");
            }
            
        }

        public double GetVoltage()
        {
            if (Globals.Emulation)
                return HImitator.PowerSupply40.GetVoltage();
            if (!Init())
                return double.NaN;
            return _akip.MeasureVoltage();
        }

        public double GetSettedVoltage()
        {
            if (Globals.Emulation)
                return HImitator.PowerSupply40.GetSettedVoltage();
            if (!Init())
                return double.NaN;
            return _akip.GetSettedVoltage();
        }

        public double GetCurrent()
        {
            if (Globals.Emulation)
                return HImitator.PowerSupply40.GetCurrent();
            if (!Init())
                return double.NaN;
            return _akip.MeasureCurrent();
        }

        public double GetSettedCurrent()
        {
            if (Globals.Emulation)
                return HImitator.PowerSupply40.GetSettedCurrent();
            if (!Init())
                return double.NaN;
            return _akip.GetSettedCurrent();
        }

        public void SetVoltage(double voltage)
        {
            try
            {
                if (Globals.Emulation)
                    HImitator.PowerSupply40.SetVoltage(voltage);
                if(Init())
                    _akip.SetVoltage(voltage);
            }
            catch
            {
                throw new Exception("Set voltage failed!");
            }
        }

        public void SetCurrent(double current)
        {
            try
            {
                if (Globals.Emulation)
                    HImitator.PowerSupply40.SetCurrentLimit(current);
                if(Init())
                    _akip.SetCurrent(current);
            }
            catch
            {
                throw new Exception("Set current failed!");
            }
        }

        public bool IsProtectionActive()
        {
            if (_akip.GetCurrentProtectionTrip())
                return true;
            if (_akip.GetVoltageProtectionTrip())
                return true;
            return false;
        }

        public bool IsOverCurrent()
        {
            return _akip.GetCurrentProtectionTrip();
        }

        public bool IsOverVoltage()
        {
            return _akip.GetVoltageProtectionTrip();
        }

        public string GetIdentString()
        {
            return _akip.GetIdentStr();
        }
    }
}
