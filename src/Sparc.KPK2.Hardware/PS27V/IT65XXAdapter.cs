using Sparc.KPK2.Imitator.Hardware;
using Sparc.KPK2.NFunctions;
using System;
using System.Threading;
using Sparc.Hardware.IT65XX;
using Sparc.Hardware.IT65XX.Enum;
using Sparc.Hardware.IT65XX.Transport;

namespace Sparc.KPK2.Hardware.PS27V
{
    class IT65XXAdapter : IPower27V
    {
        private object syncObject = new object();
        private IT65XX _it65;
        private bool _isInit;

        public bool Init()
        {
            if (_isInit)
                return true;
            try
            {
                lock (syncObject)
                {
                    _it65 = new IT65XX(new USBTransport(IT65XX.DefaultVendorId, IT65XX.DefaultModelId));
                    _it65.Connect();
                    _isInit = _it65.IsConnected;
                    _it65.SetVoltageLowLimit(0);
                    _it65.SetVoltageUpperLimit(33);
                    _it65.SetVoltageProtection(33);
                    _it65.SetVoltageProtectionDelay(0.02);
                    _it65.SetVoltageRiseTime(100); //С заводским значением 0, выдавал всплески при установке напряжения
                    _it65.SetVoltageProtectionState(true);
                    _it65.SetCurrent(1);
                    _it65.SetCurrentProtectionLevel(1);
                    _it65.SetCurrentProtectionState(true); //Выключение выхода при превышении
                }
            }
            catch
            {
                return false;
            }

            return _isInit;
        }

        public bool? GetOutputState()
        {
            if (Globals.Emulation)
                return HImitator.PowerSupply40.isOn();
            if (!Init())
                return null;
            return _it65.GetOutputState();
        }

        public bool OnOffState(bool state)
        {
            try
            {
                if (Globals.Emulation)
                    return HImitator.PowerSupply40.SetOnOffState(state);
                if (Init())
                {
                    if (state)
                        ClearProtection();


                    //var protLevel = _it65.GetCurrentProtectionLevel();
                    //var iLimit = _it65.GetCurrent();

                    //_it65.SetCurrentProtectionLevel(iLimit + iLimit/2);//+50% для исключения ложного срабатывания защиты

                    bool curProtState = _it65.GetCurrentProtectionState();
                    if (curProtState)
                        _it65.SetCurrentProtectionState(false);

                    _it65.SetOutputState(state);

                    //Thread.Sleep(250);
                    //_it65.SetCurrentProtectionLevel(protLevel);

                    if (curProtState)
                    {
                        Thread.Sleep(150);
                        _it65.SetCurrentProtectionState(true);
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

        /// <summary>
        /// Снятие защиты, если сработала и мягкое включение в случае большого напряжения
        /// </summary>
        /// <returns></returns>
        private void ClearProtection()
        {
            if (_it65.GetVoltageProtectionState() || _it65.GetCurrentProtectionState())
            {
                _it65.ClearProtectionState();
                Thread.Sleep(300);
            }
        }

        public double GetVoltage()
        {
            if (Globals.Emulation)
                return HImitator.PowerSupply40.GetVoltage();
            if (!Init())
                return double.NaN;
            return _it65.MeasureVoltage();
        }

        public double GetSettedVoltage()
        {
            if (Globals.Emulation)
                return HImitator.PowerSupply40.GetSettedVoltage();
            if (!Init())
                return double.NaN;
            return _it65.GetVoltage();
        }

        public double GetCurrent()
        {
            if (Globals.Emulation)
                return HImitator.PowerSupply40.GetCurrent();
            if (!Init())
                return double.NaN;
            return _it65.MeasureCurrent();
        }

        public double GetSettedCurrent()
        {
            if (Globals.Emulation)
                return HImitator.PowerSupply40.GetSettedCurrent();
            if (!Init())
                return double.NaN;
            return _it65.GetCurrentProtectionLevel();
        }

        public void SetVoltage(double voltage)
        {
            try
            {
                if (Globals.Emulation)
                    HImitator.PowerSupply40.SetVoltage(voltage);
                if (Init())
                    _it65.SetVoltage(voltage);
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
                if (Init())
                {
                    _it65.SetCurrentProtectionLevel(current-current/100d); //Ток - 1% запаса для выключения выхода, а не стабилизации
                    _it65.SetCurrent(current); 
                }
            }
            catch
            {
                throw new Exception("Set current failed!");
            }
        }

        public bool IsProtectionActive()
        {
            var qRegState = _it65.GetQueryRegister();
            if (qRegState.HasFlag(QueryStatusRegisterState.OverCurrent))
                return true;
            if (qRegState.HasFlag(QueryStatusRegisterState.OverVoltage))
                return true;
            if (qRegState.HasFlag(QueryStatusRegisterState.OverPower))
                return true;
            if (qRegState.HasFlag(QueryStatusRegisterState.OverTemperature))
                return true;
            if (qRegState.HasFlag(QueryStatusRegisterState.LoadOverCurrent))
                return true;
            if (qRegState.HasFlag(QueryStatusRegisterState.LoadOverPower))
                return true;

            var operReg = _it65.GetOperationRegister();
            if (!operReg.HasFlag(OperationStatusRegisterState.ConstantVoltage))
                return false;

            return false;
        }

        public bool IsOverCurrent()
        {
            var qRegState = _it65.GetQueryRegister();
            return qRegState.HasFlag(QueryStatusRegisterState.OverCurrent);
        }

        public bool IsOverVoltage()
        {
            var qRegState = _it65.GetQueryRegister();
            return qRegState.HasFlag(QueryStatusRegisterState.OverVoltage);
        }

        public string GetIdentString()
        {
            return _it65.GetIdentString();
        }
    }
}
