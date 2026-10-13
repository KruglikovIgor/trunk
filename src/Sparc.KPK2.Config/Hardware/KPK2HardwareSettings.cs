using System;
using NLog;
using System.IO;
using System.IO.Ports;
using System.Xml.Serialization;
using Sparc.KPK2.Config.Settings;

namespace Sparc.KPK2.Config.Hardware
{
	[XmlRoot("KPK2HardwareSettings")]
	public class KPK2HardwareSettings
	{
		#region Поля

		private const string name = "KPK2Hardware.config";
		public const string pathSharedFolder = "Sparc\\kpk2";
        public static readonly object path;
        private static KPK2HardwareSettings _settings;
		#endregion

		#region Свойства
		[XmlElement("DCPS")] public RsDeviceElement DCPowerSupply { get; set; }

		[XmlElement("ACPS")] public RsDeviceElement ACPowerSupply { get; set; }

        [XmlElement("LanACPS")] public RsDeviceElement ACPowerSupplyLan { get; set; }

        [XmlElement("KZ")] public RsDeviceElement KZ { get; set; }

		[XmlElement("SKT")] public RsDeviceElement SKT { get; set; }

		[XmlElement("DInputs")] public RsDeviceElement DiscretInputs { get; set; }

		[XmlElement("DOutputs")] public RsDeviceElement DiscretOutputs { get; set; }

		[XmlArray("LTRUnits")]
		[XmlArrayItem("LTRUnit")]
		public ModbusElement[] LTRUnits { get; set; }
		#endregion

		#region Конструктор
		private KPK2HardwareSettings() {}
        #endregion

        #region Методы

        private static bool _settingsModified;

        public static KPK2HardwareSettings GetSettings()
        {
            if (_settings == null)
            {
                string[] file = new string[]
                    { Environment.GetFolderPath(Environment.SpecialFolder.CommonDocuments), pathSharedFolder, name };
                string path = Path.Combine(file);

                    if (File.Exists(path))
                    {
                        Deserialize(path);
                        return _settings;
                    }
                    else if (File.Exists(name))
                    {
                        File.Copy(name, path);
                        Deserialize(path);
                        return _settings;
                    }
                    else
                    {
                        _settings = Initialize();
                        Serialize(path);
                        return _settings;
                    }
            }
            return _settings;
        }


        public static bool AvailabilityDataKPK2HardwareSettings()
        {
			bool isAvailabilityData=true;

            if (_settings == null) isAvailabilityData=false;

            if (_settings.DCPowerSupply == null || _settings.DCPowerSupply.Port == null)
            {
                LogManager.GetCurrentClassLogger().Error(Resources.Properties.Resources.ErrorDataHardwareSettings + " ИП постоянного тока (секция <DCPS>).");
                isAvailabilityData = false;
            }
            if (_settings.ACPowerSupply == null|| _settings.ACPowerSupply.Port == null)
            {
                LogManager.GetCurrentClassLogger().Error(Resources.Properties.Resources.ErrorDataHardwareSettings + " ИП переменного тока (секция <ACPS>).");
                isAvailabilityData = false;
            }
            if (_settings.ACPowerSupplyLan == null || _settings.ACPowerSupplyLan.LanPort == null)
            {
                LogManager.GetCurrentClassLogger().Error(Resources.Properties.Resources.ErrorDataHardwareSettings + " ИП переменного тока (секция <LanACPS>).");
                isAvailabilityData = false;
            }
            if (_settings.KZ == null || _settings.KZ.Port == null)
            {
                LogManager.GetCurrentClassLogger().Error(Resources.Properties.Resources.ErrorDataHardwareSettings + " модуля контрольных задач (секция <KZ>).");
                isAvailabilityData = false;
            }
            if (_settings.SKT == null || _settings.SKT.Port == null)
            {
                LogManager.GetCurrentClassLogger().Error(Resources.Properties.Resources.ErrorDataHardwareSettings + " модуля генератора (секция <SKT>).");
                isAvailabilityData = false;
            }

            if (_settings.DiscretInputs == null || _settings.DiscretInputs.Port == null)
            {
                LogManager.GetCurrentClassLogger().Error(Resources.Properties.Resources.ErrorDataHardwareSettings + " модуля дискретных входов (секция <DInputs>).");
                isAvailabilityData = false;
            }

			if (_settings.DiscretOutputs == null || _settings.DiscretOutputs.Port == null)
            {
                LogManager.GetCurrentClassLogger().Error(Resources.Properties.Resources.ErrorDataHardwareSettings + " модуля дискретных выходов (секция <DOutputs>).");
                isAvailabilityData = false;
            }

            if (_settings.LTRUnits == null)
            {
                LogManager.GetCurrentClassLogger().Error(Resources.Properties.Resources.ErrorDataHardwareSettings + " LTR модулей (секция <LTRUnits>).");
                isAvailabilityData = false;
            }

            return isAvailabilityData;
        }

        private static void Deserialize(string path)
		{

			XmlSerializer serializer = new XmlSerializer(typeof(KPK2HardwareSettings));
			FileStream fs = new FileStream(path, FileMode.Open);
			_settings = (KPK2HardwareSettings)serializer.Deserialize(fs);
			fs.Close();
		}

		private static void Serialize(string path)
		{

			XmlSerializer serializer = new XmlSerializer(typeof(KPK2HardwareSettings));
			TextWriter writer = new StreamWriter(path);
			serializer.Serialize(writer, _settings);
			writer.Close();
		}

		private static KPK2HardwareSettings Initialize()
		{
			var settings = new KPK2HardwareSettings();

			settings.DCPowerSupply = new RsDeviceElement()
			{
				Port = new SerialPortElement()
				{
					PortName = "COM2",
					BaudRate = 9600,
					Parity = Parity.None,
					StopBits = StopBits.One,
					DataBits = 8,
					ReadTimeout = 300,
					WriteTimeout = 100
				}
            };

			settings.ACPowerSupply = new RsDeviceElement()
			{
				Port = new SerialPortElement()
				{
					PortName = "COM1",
					BaudRate = 9600,
					Parity = Parity.None,
					StopBits = StopBits.One,
					DataBits = 8,
					ReadTimeout = 300,
					WriteTimeout = 100
				}
			};

            settings.ACPowerSupplyLan = new RsDeviceElement()
            {
                LanPort = new LanPortElement()
                {
                IPAddress = "172.16.51.10"
                }
            };

            settings.KZ = new RsDeviceElement()
			{
				Port = new SerialPortElement()
				{
					PortName = "COM6",
					BaudRate = 9600,
					Parity = Parity.None,
					StopBits = StopBits.One,
					DataBits = 8,
					ReadTimeout = 300,
					WriteTimeout = 100
				}
			};

			settings.SKT = new RsDeviceElement()
			{
				Port = new SerialPortElement()
				{
					PortName = "COM3",
					BaudRate = 9600,
					Parity = Parity.None,
					StopBits = StopBits.One,
					DataBits = 8,
					ReadTimeout = 300,
					WriteTimeout = 100
				}
			};

			settings.DiscretInputs = new RsDeviceElement()
			{
				Port = new SerialPortElement()
				{
					PortName = "COM5",
					BaudRate = 9600,
					Parity = Parity.None,
					StopBits = StopBits.One,
					DataBits = 8,
					ReadTimeout = 300,
					WriteTimeout = 100
				}
			};

			settings.DiscretOutputs = new RsDeviceElement()
			{
				Port = new SerialPortElement()
				{
					PortName = "COM4",
					BaudRate = 9600,
					Parity = Parity.None,
					StopBits = StopBits.One,
					DataBits = 8,
					ReadTimeout = 300,
					WriteTimeout = 100
				}
			};

			settings.LTRUnits = new ModbusElement[]
			{
				new ModbusElement() { Name = "LTR114", Address = 11 },
				new ModbusElement() { Name = "LTR11_1", Address = 12 },
				new ModbusElement() { Name = "LTR11_2", Address = 13 },
				new ModbusElement() { Name = "LTR11_3", Address = 14 },
				new ModbusElement() { Name = "LTR22_1", Address = 15 },
				new ModbusElement() { Name = "LTR22_2", Address = 16 }
			};

			return settings;
		}
		#endregion
	}
}
