using System;
using System.Data;
using NLog;
using Sparc.KPK2.Imitator.Hardware;
using Sparc.KPK2.NFunctions;

namespace Sparc.KPK2.Hardware.PS115V
{
    /// <summary>
    /// Класс-адаптер для работы ИП 115В в КПК-2
    /// Потокобезопасный
    /// </summary>
    public class Power115V
    {
        private IPower115V _power115V;
        private static Power115V _instance;
        private double _lineResistance = double.NaN;
        private object _lockObj = new object();

        public const double StandardFrequency = 400;

        public static Func<string> GetDefaultPort;
        public static Func<string> GetAlternatePort;
        public static Func<int> GetAlternateBaudRate;
        public static Func<string> GetIPAddress;
        public static Action<string> SetDefaultPort;

        private static Logger _logger = LogManager.GetLogger("KPK2Info");

        public double LineResistance
        {
            private get
            {
                if (double.IsNaN(_lineResistance))
                {
                    DataTable dt = DataManager.Instance.GetCalibration(DataManager.PS115VModuleId, 0);
                    if (dt.Rows.Count > 0)
                        _lineResistance = Convert.ToDouble(dt.Rows[0][3]);
                    else
                        _lineResistance = 0.15;
                }
                return _lineResistance;
            }
            set
            {
                _lineResistance = double.NaN;
            }
        }

        public static Power115V Instance
        {
            get { return _instance ?? (_instance = new Power115V()); }
        }

        public bool IsAktor
        {
            get { return _power115V is AktorAdapter; }
        }

        public static void RenewInstance()
        {
            if (_instance != null)
                _instance.Close();
            _instance = new Power115V();
        }

        private void Close()
        {
            lock (_lockObj)
                _power115V.Disconnect();
        }

        private Power115V()
        {
            if (Globals.Emulation)
                return;

            lock (_lockObj)
            {
                PS115DeviceSelector dSelector = new PS115DeviceSelector();
                dSelector.Add(new ASR72100Adapter(GetIPAddress.ToString()));
                dSelector.Add(new APS7102Adapter());
                dSelector.Add(new ASR72100Adapter(GetDefaultPort.ToString(), GetAlternatePort.ToString()));
                dSelector.Add(new AktorAdapter());


                _power115V = dSelector.GetLastOrConnected();
                _power115V.LineResistance = LineResistance;
                if (_power115V.IsConnected())
                {
                    _power115V.SetFrequency(StandardFrequency);
                    //var logger = LogManager.GetCurrentClassLogger();
                    _logger.Info(string.Format("ИП 115В подключён: {0}", _power115V.GetIdentString()));
                }
            }
        }

        public bool SetVoltageSimple(double voltage)
        {
            if (Globals.Emulation)
            {
                HImitator.PowerSupply115.SetOn();
                return HImitator.PowerSupply115.SetVoltage(voltage);
            }

            lock(_lockObj)
                return _power115V.SetVoltageSimple(voltage);
        }

        public bool SetVoltage(double voltage)
        {
            if (Globals.Emulation)
            {
                HImitator.PowerSupply115.SetOn();
                return HImitator.PowerSupply115.SetVoltage(voltage);
            }

            lock (_lockObj)
                return _power115V.SetVoltage(voltage);
        }

        public bool CorrectVoltageIfBigError(double voltage, double curVoltage)
        {
            lock (_lockObj)
                return _power115V.CorrectVoltageIfBigError(voltage, curVoltage);
        }

        public bool SetPhase(double phase)
        {
            if (Globals.Emulation)
                return true;
            lock (_lockObj)
                return _power115V.SetPhase(phase);
        }

        public void SetPhaseEx(double phase)
        {
            if (!SetPhase(phase))
                throw new Exception("Set phase failed!");
        }

        public bool GetOutputState()
        {
            if (Globals.Emulation)
                return HImitator.PowerSupply115.OutputState();

            lock (_lockObj)
                return _power115V.GetOutputState();
        }

        public bool SetVoltageGradually(double voltage)
        {
            if (Globals.Emulation)
            {
                HImitator.PowerSupply115.SetOn();
                return HImitator.PowerSupply115.SetVoltage(voltage);
            }
            lock (_lockObj)
                return _power115V.SetVoltageGradually(voltage);
        }

        public double GetVoltage(bool correct)
        {
            if (Globals.Emulation)
                return HImitator.PowerSupply115.GetVoltage();

            lock (_lockObj)
                return _power115V.GetVoltage(correct);
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
            if (Globals.Emulation)
                return HImitator.PowerSupply115.GetVoltage();

            lock (_lockObj)
                return _power115V.GetSettedVoltage();
        }

        public bool GetStatus()
        {
            if (Globals.Emulation)
                return true;

            lock (_lockObj)
                return _power115V.GetStatus();
        }

        public double GetCurrent()
        {
            if (Globals.Emulation)
                return HImitator.PowerSupply115.GetCurrent();

            lock (_lockObj)
                return _power115V.GetCurrent();
        }

        public bool SetOnOff(bool state)
        {
            if (Globals.Emulation)
                return HImitator.PowerSupply115.OnOffState(state);

            lock (_lockObj)
                return _power115V.SetOnOff(state);
        }

        public bool SetPermitOnOff(bool state)
        {
            if (Globals.Emulation)
                return true;

            lock (_lockObj)
                return _power115V.SetPermitOnOff(state);
        }

        public void SetPermitOnOffEx(bool state)
        {
            if (!SetPermitOnOff(state))
                throw new Exception("Set permit failed");
        }

        //Устанавливает ток ограничения в амперах (только не для Актора)
        public bool SetMaxCurrent(double current)
        {
            if (Globals.Emulation)
                return HImitator.PowerSupply115.SetCurrentLimit(current);

            lock (_lockObj)
                return _power115V.SetCurrentLimit(current);
        }

        public void SetMaxCurrentEx(double current)
        {
            if (!SetMaxCurrent(current))
                throw new Exception("Set max current failed!");
        }

        public bool SetMaxVoltageRange(bool state)
        {
            if (Globals.Emulation)
                return true;
            lock (_lockObj)
                return _power115V.SetMaxVoltageRange(state);
        }

        public bool SetFrequency(double frequency)
        {
            if (Globals.Emulation)
                return true;

            lock (_lockObj)
                return _power115V.SetFrequency(frequency);
        }

        public double GetFrequency()
        {
            if ((Globals.Emulation))
                return 400;

            lock (_lockObj)
                return _power115V.GetFrequency();
        }

        public bool IsConnected()
        {
            if ((Globals.Emulation))
                return true;

            lock (_lockObj)
                return _power115V.IsConnected();
        }

        public double GetCurrentLimit()
        {
            if ((Globals.Emulation))
                return 10;

            lock (_lockObj)
                return _power115V.GetCurrentLimit();
        }
    }
}
