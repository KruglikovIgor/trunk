using System;
using System.Collections;
using System.IO.Ports;
using System.Threading;
using NLog;
using Sparc.KPK2.Config.Hardware;
using Sparc.KPK2.Imitator.Hardware;
using Sparc.KPK2.NFunctions;

namespace Sparc.KPK2.Hardware
{
    /// <summary> Обмен с модулем дискретных входов (МДВ) в БИ ШС КПК-2 
    /// </summary>
    /// <remarks> Адаптер к DLL DiscreteInputsBoard 
    /// для иниц-ии DLL и SerialPort,
    /// работы в режиме эмуляции с HImitator.DiscretInputs
    /// и для инверсии состояния некот. каналов (см. invList).
    /// Назначение каналов - см. БД КПК-2.
    /// </remarks>
    public class DiscreteInputs
    {
        private DiscreteInputsBoard.DiscreteInputs _bDInputs; // dll
        private static DiscreteInputs _instance;

        /// <summary> Список каналов/входов МДВ, состояния кот. подлежат инвертированию
        /// (костыль, чтобы не переделывать схему).
        /// В наст. вр. инвертируется только канал 22 "Refuse_BVK/Отказ БВК".
        /// </summary>
        private static int[] invList;

        private static Logger _logger = LogManager.GetLogger("KPK2Info");

        private bool _init;

        public static int[] InversionList
        {
            set { invList = value; }
        }

        public DiscreteInputs()
        {
            //SerialPort sp = SpManager.Open(DataManager.Instance.GetModulePort(DataManager.DiscretInputs));
            SerialPort sp = SpManager.Open(KPK2HardwareSettings.GetSettings().DiscretInputs.Port);
            if (Globals.Emulation)
            {
                _init = true;
                return;
            }

            try
            {
                _bDInputs = new DiscreteInputsBoard.DiscreteInputs(sp);
                _bDInputs.GetInputsStates();
                _init = true;
                bool isDiscreteInputs = false;
                DateTime diPO_date = new DateTime();
                _bDInputs.GetVersion(ref isDiscreteInputs, ref diPO_date);
                _init &= isDiscreteInputs;
                //var logger = LogManager.GetCurrentClassLogger();
                _logger.Info(string.Format("Плата КЗ подключена: {0} {1}", isDiscreteInputs ? "Да" : "Нет", diPO_date.ToString()));
            }
            catch
            {
                _init = false;
            }
        }

        public static DiscreteInputs Instance()
        {
            return _instance ?? (_instance = new DiscreteInputs());
        }

        /// <summary> Не применяется! </summary>
        public bool GetVersion(ref bool isDiscreteInputs, ref DateTime date)
        {
            if (Globals.Emulation)
                return HImitator.DiscretInputs.GetVersion(ref isDiscreteInputs, ref date);
            return _bDInputs.GetVersion(ref isDiscreteInputs, ref date);
        }

        /// <summary> Не применяется! </summary>
        public bool GetVersion()
        {
            if (Globals.Emulation)
                return HImitator.DiscretInputs.GetVersion();
            bool isDiscreteInputs = false;
            DateTime date = new DateTime();
            return _bDInputs.GetVersion(ref isDiscreteInputs, ref date);
        }

        /// <summary> Возвращает состояния 24 каналов МДВ.
        /// </summary>
        /// <returns></returns>
        public BitArray GetInputsStates()
        {
            if (Globals.Emulation)
                return HImitator.DiscretInputs.GetInputsStates();
            if (!_init)
                return null;

            _bDInputs.StartMeasuring(null);
            Thread.Sleep(500);
            _bDInputs.StopMeasuring();
            BitArray ba = _bDInputs.GetInputsStates();
            if (ba == null)
                return null;
            Invert(ba);
            return ba;
        }

        /// <summary> Инвертирование состояния некот. каналов
        /// (костыль, чтобы не переделывать схему).
        /// </summary>
        private static void Invert(BitArray ba)
        {
            if (invList == null)
                return;
            foreach (int n in invList)
                ba[n] = !ba[n];
        }

        /// <summary> Запуск непрерывного считывания состояния 24 каналов МДВ.
        /// Периодически принимаемая от МДВ инфо будет передается в callback()
        /// </summary>
        /// <param name="callback"></param>
        /// <returns> Успешность запуска. </returns>
        /// <remarks> Останов вып-ся вызовом StopMeasuring(). </remarks>
        public bool StartMeasuring(Action<BitArray> callback)
        {
            if (Globals.Emulation)
            {
                return HImitator.DiscretInputs.StartMeasuring(ba =>
                {
                    Invert(ba);
                    callback(ba);
                });
            }
            return _bDInputs.StartMeasuring(ba =>
              {
                  Invert(ba);
                  callback(ba);
              });
        }

        /// <summary> Останов непрерывного считывания состояния входов МДВ.
        /// </summary>
        /// <returns> Успешность останова. </returns>
        public bool StopMeasuring()
        {
            if (Globals.Emulation)
                return HImitator.DiscretInputs.StopMeasuring();
            return _bDInputs.StopMeasuring();
        }
    }
}
