using System;
using System.Collections;
using System.Collections.Generic;
using System.IO.Ports;
using NLog;
using ReleBoard;
using Sparc.KPK2.Config.Hardware;
using Sparc.KPK2.Imitator.Hardware;
using Sparc.KPK2.NFunctions;

namespace Sparc.KPK2.Hardware
{
    /// <summary> Управление модулем дискр. команд (МДК) в БИ ШС КПК-2.
    /// </summary>
    /// <remarks> МДК имеет каналы (реле) [0, 21]. </remarks>
    public class DiscreteOutputs
    {
        private Rele _bRele;

        private static DiscreteOutputs _instance;
        private bool _init = false;

        private static Logger _logger = LogManager.GetLogger("KPK2Info");

        private DiscreteOutputs()
        {
            if (Globals.Emulation)
            {
                _init = true;
                return;
            }
            //SerialPort sp = SpManager.Open(DataManager.Instance.GetModulePort(DataManager.DiscretOutputs));
            SerialPort sp = SpManager.Open(KPK2HardwareSettings.GetSettings().DiscretOutputs.Port);
            try
            {
                _bRele = new Rele(sp);
                bool isDiscreteOutpus = false;
                DateTime dt = new DateTime();
                _init = GetVersion(ref isDiscreteOutpus, ref dt);
                _init &= isDiscreteOutpus;
                //var logger = LogManager.GetCurrentClassLogger();
                _logger.Info(string.Format("Плата КЗ подключена: {0} {1}", isDiscreteOutpus ? "Да" : "Нет", dt.ToString()));
            }
            catch
            {
                _init = false;
            }
        }

        public static DiscreteOutputs Instance()
        {
            return _instance ?? (_instance = new DiscreteOutputs());
        }

        public bool GetVersion()
        {
            bool isDiscreteOutputs = false;
            DateTime date = new DateTime();
            if (Globals.Emulation)
                return HImitator.DiscretOutputs.GetVersion(ref isDiscreteOutputs, ref date);
            return _bRele.GetVersion(ref isDiscreteOutputs, ref date);
        }

        public bool GetVersion(ref bool isDiscreteOutputs, ref DateTime date)
        {
            if (Globals.Emulation)
                return HImitator.DiscretOutputs.GetVersion(ref isDiscreteOutputs, ref date);
            return _bRele.GetVersion(ref isDiscreteOutputs, ref date);
        }

       

        public bool SetOutputs(int group, BitArray states)
        {
            if (Globals.Emulation)
                return HImitator.DiscretOutputs.SetOutputs(group, states);
            return _bRele.SetReleStates(group, states);
        }

        public bool SetOutputs(Dictionary<int, bool> table)
        {
            if (Globals.Emulation)
                return HImitator.DiscretOutputs.SetOutputs(table);
            if (!_init)
                return false;
            return _bRele.SetReleStates(table);
        }

        public bool SetOutputs(Hashtable ht)
        {
            Dictionary<int, bool> dict = new Dictionary<int, bool>();
            foreach (DictionaryEntry de in ht)
                dict.Add(DataManager.Instance.GetChannelByName(DataManager.DiscretOutputs, de.Key.ToString()), (bool)de.Value);
            if (Globals.Emulation)
                return HImitator.DiscretOutputs.SetOutputs(dict);
            if (!_init)
                return false;
            return _bRele.SetReleStates(dict);
        }

        public bool SetOutputs(BitArray states)
        {
            if (Globals.Emulation)
                return HImitator.DiscretOutputs.SetOutputs(states);
            if (!_init)
                return false;
            if (states.Length == 0)
                return false;
            if (states.Length >= 24)
                states[23] = false; // 23 и 24 выхода не существует
            if (states.Length >= 23)
                states[22] = false;
            return _bRele.SetReleStates(states);
        }

        public bool GetOutputs(ref BitArray states)
        {
            if (Globals.Emulation)
                return HImitator.DiscretOutputs.GetOutputs(ref states);
            return _bRele.GetReleStates(ref states);
        }

        /// <summary>
        /// Устанавливает реле в нужное состояние
        /// </summary>
        /// <param name="number">Номер реле</param>
        /// <param name="state">True - замкнуто</param>
        /// <returns></returns>
        public bool SetOutput(int number, bool state)
        {
            if (number >= 22)
                return false;
            if (Globals.Emulation)
                return HImitator.DiscretOutputs.SetOutput(number, state);
            return _bRele.SetReleState(number, state);
        }

        /// <summary>
        /// Устанавливает реле в нужное состояние
        /// </summary>
        /// <param name="number">Номер реле</param>
        /// <param name="state">True - замкнуто</param>
        /// <returns></returns>
        public void SetOutputEx(int number, bool state)
        {
            if (number >= 22)
                throw new Exceptions.ReleException();
            if (Globals.Emulation)
                if (!HImitator.DiscretOutputs.SetOutput(number, state))
                    throw new Exceptions.ReleException();
            if (!_bRele.SetReleState(number, state))
                throw new Exceptions.ReleException();
        }

        /// <summary>
        /// Устанавливает реле в нужное состояние
        /// </summary>
        /// <param name="channelName"></param>
        /// <param name="state">Наименование канала</param>
        /// <returns>True - замкнуто</returns>
        public bool SetOutput(string channelName, bool state)
        {
            int channel = DataManager.Instance.GetChannelByName(DataManager.DiscretOutputs, channelName);
            if (channel == -1) return false;
            return SetOutput(channel, state);
        }

        /// <summary>
        /// Устанавливает реле в нужное состояние, в случае неудачи генерирует Exception
        /// </summary>
        /// <param name="channelName"></param>
        /// <param name="state">Наименование канала</param>
        /// <returns>True - замкнуто</returns>
        public void SetOutputEx(string channelName, bool state)
        {
            int channel = DataManager.Instance.GetChannelByName(DataManager.DiscretOutputs, channelName);
            if (channel == -1) throw new Exceptions.ReleException();
            if (!SetOutput(channel, state))
                throw new Exceptions.ReleException();
        }

        public bool SetChannelsOff()
        {
            if (Globals.Emulation)
                return HImitator.DiscretOutputs.SetChannelsOff();
            if (!_init)
                return false;
            return _bRele.SetAllChannelsOff();
        }
    }
}
