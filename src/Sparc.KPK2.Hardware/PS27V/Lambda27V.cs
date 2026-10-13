using System;
using Sparc.Hardware.LambdaGEN;
using Sparc.Hardware.LambdaGEN.Transport;
using Sparc.KPK2.Config.Hardware;
using Sparc.KPK2.Imitator.Hardware;
using Sparc.KPK2.NFunctions;

namespace Sparc.KPK2.Hardware.PS27V
{
    public class Lambda27V : IPower27V //стал не статическим
    {
        private static bool _isInit;
        /// <summary>
        /// Старый порт для подключения ИП27В, для обеспечения совместимости со старыми стендами
        /// </summary>
        private const string OldPort = "COM8";
        private static LambdaGEN40_19 _lambda;

        public bool? GetOutputState()
        {
            if (Globals.Emulation)
                return HImitator.PowerSupply40.isOn();
            if (!Init())
                return null;
            return _lambda.GetOutputState();
        }

        public bool OnOffState(bool state)
        {
            try
            {
                if (Globals.Emulation)
                    return HImitator.PowerSupply40.SetOnOffState(state);
                if (!Init())
                    return false;
                return _lambda.SetOutput(state);
            }
            catch
            {
                throw new Exception("Change output state failed");
            }
        }

        public double GetVoltage()
        {
            if (Globals.Emulation)
                return HImitator.PowerSupply40.GetVoltage();
            if (!Init())
                return double.NaN;
            return _lambda.MeasureVoltage();
        }

        public double GetSettedVoltage()
        {
            if (Globals.Emulation)
                return HImitator.PowerSupply40.GetSettedVoltage();
            if (!Init())
                return double.NaN;
            return _lambda.GetSettedVoltage();
        }

        public double GetCurrent()
        {
            if (Globals.Emulation)
                return HImitator.PowerSupply40.GetCurrent();
            if (!Init())
                return double.NaN;
            return _lambda.MeasureCurrent();
        }

        public double GetSettedCurrent()
        {
            if (Globals.Emulation)
                return HImitator.PowerSupply40.GetSettedCurrent();
            if (!Init())
                return double.NaN;
            return _lambda.GetSettedCurrent();
        }

        public void SetVoltage(double voltage)
        {

            if (Globals.Emulation)
            {
                HImitator.PowerSupply40.SetVoltage(voltage);
                return;
            }

            if (Init())
                _lambda.SetVoltage(voltage);
        }

        public void SetCurrent(double current)
        {

            if (Globals.Emulation)
            {
                HImitator.PowerSupply40.SetCurrentLimit(current);
                return;
            }

            if (Init())
                _lambda.SetCurrent(current);
        }

        public bool IsProtectionActive()
        {
            if (_lambda.GetFoldbackState() != false)
                return true;
            return false;
        }

        public bool IsOverCurrent()
        {
            return _lambda.GetFoldbackState() != false;
        }

        public bool IsOverVoltage()
        {
            return false;
        }

        public string GetIdentString()
        {
            return _lambda.GetIdentStr();
        }

        public bool Init()
        {
            if (Globals.Emulation)
                return true;
            if (_isInit)
                return true;
            //SerialPort sp = SpManager.Open(DataManager.Instance.GetModulePort(DataManager.Lambda40V));
            var sp = SpManager.Open(KPK2HardwareSettings.GetSettings().DCPowerSupply.Port);
            if (sp == null)
                return false;
            DataManager.Instance.SetPortParameters(ref sp);
            _lambda = new LambdaGEN40_19(new LambdaComTransport(sp), new LambdaLogger());
            bool connected = true;
            try
            {
                _lambda.GetOutputState();
            }
            catch
            {
                connected = false;
            }
            finally
            {
                SpManager.Close(sp.PortName);
            }
            if (!connected)
            {
                sp = SpManager.Get(OldPort);
                if (sp == null)
                    return false;

                if (sp.IsOpen)
                    return false;
                try
                {
                    sp.Open();
                    DataManager.Instance.SetPortParameters(ref sp);

                    _lambda = new LambdaGEN40_19(new LambdaComTransport(sp.PortName, sp.BaudRate, sp.Parity), new LambdaLogger());
                    try
                    {
                        _lambda.GetOutputState();
                        connected = true;
                    }
                    catch
                    {
                        connected = false;
                    }
                    if (!connected)
                        return false;
                }
                catch
                {
                    return false;
                }
                finally
                {
                    SpManager.Close(sp.PortName);
                }
            }
            _isInit = true;
            return true;
        }
    }
}
