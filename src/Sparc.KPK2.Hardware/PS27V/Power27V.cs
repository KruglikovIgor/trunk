using System;
using NLog;
using Sparc.KPK2.Imitator.Hardware;
using Sparc.KPK2.NFunctions;

namespace Sparc.KPK2.Hardware.PS27V
{
    /// <summary>
    /// Класс адаптер для работы источника 27В в КПК-2
    /// Потокобезопасный
    /// </summary>
    public class Power27V
    {
        private IPower27V _power27V;
        private static Power27V _instance;
        private object _lockObj = new object();

        private static Logger _logger = LogManager.GetLogger("KPK2Info");
        public static Power27V Instance
        {
            get { return _instance ?? (_instance = new Power27V()); }
        }
        

        private Power27V()
        {
            if (Globals.Emulation)
                return;

            PS27DeviceSelector dSelector = new PS27DeviceSelector();
            dSelector.Add(new Lambda27V());
            dSelector.Add(new AkipAdapter());
            dSelector.Add(new IT65XXAdapter());

            //_power27V.Init();
            _power27V = dSelector.GetLastOrConnected();
            //var logger = LogManager.GetCurrentClassLogger();
            _logger.Info(string.Format("ИП 27В подключён: {0}", _power27V.GetIdentString()));
        }

        public bool? GetOutputState()
        {
            if (Globals.Emulation)
                return HImitator.PowerSupply40.isOn();

            lock(_lockObj)
                return _power27V.GetOutputState();
        }

        public double GetVoltage()
        {
            if (Globals.Emulation)
                return HImitator.PowerSupply40.GetVoltage();

            lock (_lockObj)
                return _power27V.GetVoltage();
        }

        public double GetCurrent()
        {
            if (Globals.Emulation)
                return HImitator.PowerSupply40.GetCurrent();

            lock (_lockObj)
                return _power27V.GetCurrent();
        }

        public double GetSettedVoltage()
        {
            if (Globals.Emulation)
                return HImitator.PowerSupply40.GetSettedVoltage();

            lock (_lockObj)
                return _power27V.GetSettedVoltage();
        }

        public double GetSettedCurrent()
        {
            if (Globals.Emulation)
                return HImitator.PowerSupply40.GetSettedCurrent();

            lock (_lockObj)
                return _power27V.GetSettedCurrent();
        }

        public bool SetOnOff(bool state)
        {
            if (Globals.Emulation)
                return HImitator.PowerSupply40.SetOnOffState(state);

            lock (_lockObj)
                return _power27V.OnOffState(state);
        }

        public void SetVoltage(double voltage)
        {
            try
            {
                if (Globals.Emulation)
                {
                    HImitator.PowerSupply40.SetVoltage(voltage);
                    return;
                }

                lock (_lockObj)
                    _power27V.SetVoltage(voltage);
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
                {
                    HImitator.PowerSupply40.SetCurrentLimit(current);
                    return;
                }

                lock (_lockObj)
                    _power27V.SetCurrent(current);
            }
            catch
            {
                throw new Exception("Set current failed!");
            }
        }

        public bool IsProtectionActive()
        {
            if (Globals.Emulation)
                return false;
            lock (_lockObj)
                return _power27V.IsProtectionActive();
        }

        public bool IsOverCurrent()
        {
            if (Globals.Emulation)
                return false;
            lock (_lockObj)
                return _power27V.IsOverCurrent();
        }

        public bool IsOverVoltage()
        {
            if (Globals.Emulation)
                return false;
            lock (_lockObj)
                return _power27V.IsOverVoltage();
        }
    }
}
