using System.Threading;
using NLog;
using Sparc.Hardware.PowerMeterU8481A;
using Sparc.KPK2.NFunctions;

namespace Sparc.KPK2.Hardware
{
    public class PowerMeter
    {
        private static Timer tmrDoActive;
        private static bool _isConnectedStatic = false;
        private PowerMeterU8481A _pMeter;
        private bool _isConnected = false;

        private static Logger _logger = LogManager.GetLogger("KPK2Info");

        public PowerMeter()
        {
            if (Globals.Emulation)
            {
                if (!_isConnected)
                    StartTimerDoActive();
                return;
                //_isConnected = true;
                //return;
            }

            try
            {
                _pMeter = new PowerMeterU8481A(PowerMeterU8481A.DefaultAgilentVendorId);
                _isConnected = true;
                LogVersion();
            }
            catch
            {
                _isConnected = false;
            }
        }

        private void LogVersion()
        {
            //var logger = LogManager.GetCurrentClassLogger();
            _logger.Info(string.Format("Измеритель мощности подключён: {0} Int:{1}°C Ext:{2}°C", _pMeter.GetInfo(), _pMeter.GetTemperature(true), _pMeter.GetTemperature(false)));
        }

        private void StartTimerDoActive()
        {
            if (_isConnectedStatic)
            {
                _isConnected = true;
                return;
            }
            if (tmrDoActive == null)
                tmrDoActive = new Timer(MakeConnected, null, 10000, 10000);
        }

        private void MakeConnected(object state)
        {
            _isConnectedStatic = true;
            _isConnected = true;
            tmrDoActive.Dispose();
        }

        /// <summary>
        /// Частота в кГц
        /// </summary>
        /// <param name="freq"></param>
        /// <returns></returns>
        public bool SetFrequency(uint freq)
        {
            if (Globals.Emulation)
                return true;
            if (!_isConnected)
                return false;
            _pMeter.SetFrequency(freq, FreqUnits.kHz);
            return true;
        }

        public double MeasurePowerDbm()
        {
            if (Globals.Emulation)
                return -3.096;
            if (!_isConnected)
                return double.NaN;
            _pMeter.SetPowerUnits(PowerUnits.Dbm);
            return _pMeter.MeasurePower(20, 1);
        }

        public double MeasurePowerW()
        {
            if (Globals.Emulation)
                return 0.0012;
            if (!_isConnected)
                return double.NaN;
            _pMeter.SetPowerUnits(PowerUnits.W);
            return _pMeter.MeasurePower(20, 1);
        }

        public bool IsConnected
        {
            get { return _isConnected; }
        }

        public bool Calibrate()
        {
            if (Globals.Emulation)
            {
                Thread.Sleep(10000);
                return true;
            }
            if (!_isConnected)
                return false;
            
            _pMeter.Calibrate();
            return true;
        }

        public bool CalibrateZero()
        {
            if (Globals.Emulation)
            {
                Thread.Sleep(10000);
                return true;
            }
            if (!_isConnected)
                return false;

            _pMeter.CalibrateZero();
            return true;
        }

        public void Disconnect()
        {
            try
            {
                if (_pMeter != null)
                {
                    _pMeter.Dispose();
                    _pMeter = null;
                    _isConnected = false;
                    _isConnectedStatic = false;
                }
            }
            catch
            {}
        }
    }
}
