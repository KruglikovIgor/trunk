using System;
using APS1102;
using APS1102.Enums;
using APS1102.Transport;
using Sparc.KPK2.Imitator.Hardware;
using Sparc.KPK2.NFunctions;

namespace Sparc.KPK2.Hardware.PS115V
{
    internal class APS7102Adapter : IPower115V
    {
        private int _currentFrequency = 400;
        private bool _init;
        private const int DelayToMeasVoltage = 500;
        private APS1102_ASR7200 _power115V;

        public double LineResistance { get; set; }

        public void SetVoltageWOCorrection(double voltage)
        {
            if (!_init)
                return;
            if (!SetVoltageInner(voltage))
                throw new Exception();
        }

        //Не потребовалось
        //private void CorrectVoltageRange(double voltage)
        //{
        //    var vRange = VoltRange.R200V;
        //    if (voltage < 100)
        //        vRange = VoltRange.R100V;
        //    if (_curVoltRange != vRange)
        //        SetMaxVoltageRange(vRange == VoltRange.R200V);
        //}

        public void Disconnect()
        {
            //Не требуется
        }

        public string GetIdentString()
        {
            return _power115V.GetIdentStr();
        }

        public bool Connect()
        {
            try
            {
                _power115V = new APS1102_ASR7200(new PSVisaUSBTransport());
                _power115V.ConnectToAPS1102();
                Connected();
                _init = true;
                return true;
            }
            catch
            {
                return false;
            }
        }

        private void Connected()
        {
            if (_power115V.GetMode() != WorkMode.AC_INT)
                SetModeEx(WorkMode.AC_INT);
            SetMaxVoltageRange(true);
            SetFrequencyEx(_currentFrequency);
        }

        private void SetModeEx(WorkMode wm)
        {
            _power115V.SetMode(wm);
        }

        private void SetFrequencyEx(int currentFreq)
        {
            _power115V.SetFrequency(currentFreq);
        }

        public bool SetVoltageSimple(double voltage)
        {
            if (!_init)
                return false;

            return SetVoltageInner(voltage);
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
                _power115V.SetVoltage(voltage);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return false;
            }
        }

        public bool SetPhase(double phase)
        {
            if (!_init)
                return false;
            try
            {
                _power115V.SetPhase(phase);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool GetOutputState()
        {
            if (!_init)
                return false;
            return _power115V.GetOutput();
        }

        public double GetVoltage(bool correct)
        {
            if (!_init)
                return double.NaN;
            try
            {

                double voltage = _power115V.MeasureRMSVoltage();
                if (correct)
                    CorrectVoltageByCurrent(ref voltage);
                return voltage;
            }
            catch
            {
                return double.NaN;
            }
        }

        private void CorrectVoltageByCurrent(ref double voltage)
        {
            double current = _power115V.MeasureRMSCurrent();
            double downVoltage = current * LineResistance;
            voltage -= downVoltage;
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
                return _power115V.GetVoltage();
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
                return true;
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
                return _power115V.MeasureRMSCurrent();
            }
            catch
            {
                return double.NaN;
            }
        }

        public bool SetOnOff(bool state)
        {
            if (!_init)
                return false;

            try
            {
                _power115V.SetOutput(state);
                return true;
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
                return true;
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

            try
            {
                _power115V.SetCurrentLimit(current);
                return true;
            }
            catch
            {
                return false;
            }
        }

        private VoltRange _curVoltRange = VoltRange.UNDEFINED;

        public bool SetMaxVoltageRange(bool state)
        {
            if (Globals.Emulation)
                return true;
            try
            {
                var vRange = state ? VoltRange.R200V : VoltRange.R100V;
                _power115V.SetVoltageRange(vRange);
                _curVoltRange = vRange;
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool SetFrequency(double frequency)
        {
            try
            {
                _power115V.SetFrequency(frequency);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public double GetFrequency()
        {
            return _power115V.GetFrequency();
        }

        public bool IsConnected()
        {
            return _power115V.IsConnected;
        }

        public double GetCurrentLimit()
        {
            return _power115V.GetCurrentLimit();
        }
    }
}
