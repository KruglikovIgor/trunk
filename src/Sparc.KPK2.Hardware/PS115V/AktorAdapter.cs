using System;
using AktorPS115v;
using Sparc.KPK2.Config.Hardware;
using Sparc.KPK2.Imitator.Hardware;
using Sparc.KPK2.NFunctions;

namespace Sparc.KPK2.Hardware.PS115V
{
    internal class AktorAdapter : IPower115V
    {
        private AktorPS115 _power115;
        private string _power115Phase;
        private bool _init;

        public bool Connect()
        {
            try
            {
                //var port = SpManager.Open(DataManager.Instance.GetModulePort(DataManager.Aktor115V));
                var port = SpManager.Open(KPK2HardwareSettings.GetSettings().ACPowerSupply.Port);
                var savedBaudRate = port.BaudRate;
                try
                {
                    _power115 = new AktorPS115(port);
                    _power115Phase = DataManager.Instance.GetParamCommand("115V_Read_U");
                    if (!_power115.GetStatus())
                        throw new Exception();
                    _init = true;
                }
                finally
                {
                    if (!_init)
                    {
                        SpManager.Close(port.PortName);
                        port.BaudRate = savedBaudRate;
                    }
                }
            }
            catch
            {
                _init = false;
            }

            return _init;
        }

        public bool SetVoltageSimple(double voltage)
        {
            if (!_init)
                return false;

            try
            {
                bool result = true;
                result &= _power115.SetVoltages(Convert.ToSingle(voltage), Convert.ToSingle(voltage),
                    Convert.ToSingle(voltage));
                return result;
            }
            catch
            {
                return false;
            }
        }

        public bool SetVoltageInner(double voltage)
        {
            if (Globals.Emulation)
            {
                return HImitator.PowerSupply115.SetVoltage(voltage);
            }
            if (!_init)
                return false;

            try
            {
                return SetVoltageSimple(voltage);
            }
            catch
            {
                return false;
            }
        }

        public bool SetPhase(double phase)
        {
            return true;
        }

        public bool GetOutputState()
        {
            return false;
        }

        public double GetVoltage(bool correct)
        {
            if (!_init)
                return double.NaN;
            try
            {

                switch (_power115Phase)
                {
                    case "A":
                        return _power115.GetAVoltage();
                    case "B":
                        return _power115.GetBVoltage();
                    default:
                        return _power115.GetCVoltage();
                }
            }
            catch
            {
                return double.NaN;
            }
        }

        public double GetClearVoltage()
        {
            return GetVoltage(false);
        }

        public double GetVoltage()
        {
            return GetVoltage(true);
        }

        public double GetSettedVoltage()
        {
            if (!_init)
                return double.NaN;

            try
            {
                switch (_power115Phase)
                {
                    case "A":
                        return _power115.GetAVoltage();
                    case "B":
                        return _power115.GetBVoltage();
                    default:
                        return _power115.GetCVoltage();
                }
            }
            catch
            {
                return double.NaN;
            }
        }

        public bool GetStatus()
        {
            if (Globals.Emulation)
                return true;
            if (!_init)
                return false;

            try
            {
                return _power115.GetStatus();
            }
            catch
            {
                return false;
            }
        }

        public double GetCurrent()
        {
            if (!_init)
                return 0;

            try
            {
                switch (_power115Phase)
                {
                    case "A":
                        return _power115.GetACurrent();
                    case "B":
                        return _power115.GetBCurrent();
                    default:
                        return _power115.GetCCurrent();
                }
            }
            catch
            {
                return double.NaN;
            }
        }

        public bool SetOnOff(bool state)
        {
            if (Globals.Emulation)
            {
                if (state)
                    HImitator.PowerSupply115.SetOn();
                else
                    HImitator.PowerSupply115.SetOff();
            }

            if (!_init)
                return false;

            try
            {
                return _power115.SetOnOff(state);
            }
            catch
            {
                return false;
            }
        }

        public bool SetPermitOnOff(bool state)
        {
            if (!_init)
                return false;

            try
            {
                return _power115.SetPermitOnOff(state);
            }
            catch
            {
                return false;
            }
        }

        public bool SetCurrentLimit(double current)
        {
            if (!_init)
                return false;

            return true; //Эта функция в Актор не реализована
        }

        public bool SetMaxVoltageRange(bool state)
        {
            return true;
        }

        public bool SetFrequency(double frequency)
        {
            return true;
        }

        public double GetFrequency()
        {
            return 400;
        }

        public bool IsConnected()
        {
            return _init;
        }

        public double GetCurrentLimit()
        {
            return 10;
        }

        public double LineResistance { get; set; }
        public void SetVoltageWOCorrection(double voltage)
        {
            SetVoltageSimple(voltage);
        }

        public void Disconnect()
        {
            //Не требуется
        }

        /// <summary>
        /// На данный момент не смог найти протокол этого ИП, чтобы понять может он выдавать информацию о себе или нет
        /// </summary>
        /// <returns></returns>
        public string GetIdentString()
        {
            return "Актор, не реализовано";
        }
    }
}
