using Sparc.KPK2.NFunctions;
using Sparc.KPK2.NFunctions.Enums;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace KPK2.Common
{
    [Serializable]
    public class CommonSettings
    {
        #region Внутреннее устройство
        private static string path = Application.CommonAppDataPath + @"\";

        [NonSerialized]
        public static Exception LoadException;

        private static CommonSettings _instance;

        public static CommonSettings Default
        {
            get
            {
                if (_instance == null)
                {
                    try
                    {
	                    string nameFileOfMode = 
		                    Mode.GetMode().ModeApplication == ModeApplication.RepairCheck ? 
			                    "settingsWorkCheck.all" : 
			                    "settings.all";
	                    path += nameFileOfMode;
                        _instance = SettingsSaver.Load<CommonSettings>(path);
                        _instance.SetEnableChecksByOtherSettings();
                        _instance.EnsureUniqueCaptions();
                    }
                    catch (Exception ex)
                    {
                        if (!(ex is FileNotFoundException))
                            LoadException = ex;
                        _instance = new CommonSettings();
                        _instance.FillEnableChecks();
                        _instance.EnsureUniqueCaptions();
                    }
                }
                return _instance;
            }
        }

        private void FillEnableChecks()
        {
            var devices = Enum.GetValues(typeof(DevicesID));
            foreach (var item in devices)
            {
                if (((DevicesID)item) == DevicesID.None)
                    continue;
                var mods = Sparc.KPK2.NFunctions.Globals.GetAvailableModifications((DevicesID)item);
                foreach (var itemMod in mods)
                    EnableChecks.Add(new DeviceAndMod { DeviceId = (DevicesID)item, Modification = itemMod, CheckState = false });
            }

            SetEnableChecksByOtherSettings();
        }

        public void Save()
        {
            SettingsSaver.Save(this, path);
        }

        private void SetEnableChecksByOtherSettings()
        {
            if (SettingsVersion > 0)
                return;

            if (Mode.GetMode().ModeApplication == ModeApplication.RepairCheck)
            {
                SetCheckEnable(new DeviceAndMod { DeviceId = DevicesID.Diss32_90_WorkTest, Modification = Modification.Diss32_90A }, true);
                SetCheckEnable(new DeviceAndMod { DeviceId = DevicesID.Diss32_WorkTest, Modification = Modification.None }, true);
                SetCheckEnable(new DeviceAndMod { DeviceId = DevicesID.Diss15, Modification = Modification.Diss15G }, true);
            }
            else
            {
	            SetCheckEnable(
		            new DeviceAndMod { DeviceId = DevicesID.Diss32_90, Modification = Modification.Diss32_90A }, true);
	            SetCheckEnable(new DeviceAndMod { DeviceId = DevicesID.Diss15, Modification = Modification.None },
		            true);

	            if (IsSilkway)
		            SetCheckEnable(new DeviceAndMod { DeviceId = DevicesID.Diss13, Modification = Modification.None },
			            true);
	            if (ShowDiss32)
		            SetCheckEnable(new DeviceAndMod { DeviceId = DevicesID.Diss32, Modification = Modification.None },
			            true);
	            if (IsNSK)
	            {
		            SetCheckEnable(
			            new DeviceAndMod { DeviceId = DevicesID.Diss32_90, Modification = Modification.None }, true);
		            SetCheckEnable(new DeviceAndMod { DeviceId = DevicesID.Diss3228, Modification = Modification.None },
			            true);
		            SetCheckEnable(
			            new DeviceAndMod { DeviceId = DevicesID.Diss3228_Monoblock, Modification = Modification.None },
			            true);
		            SetCheckEnable(
			            new DeviceAndMod { DeviceId = DevicesID.Diss15, Modification = Modification.Diss15G }, true);
	            }
            }

            SettingsVersion = 1;
        }

        public void SetCheckEnable(DeviceAndMod deviceAndMod, bool state)
        {
            foreach (var item in EnableChecks)
            {
                if (item.DeviceId == deviceAndMod.DeviceId)
                    if (item.Modification == deviceAndMod.Modification)
                    {
                        item.CheckState = state;
                        return;
                    }
            }
            throw new NotImplementedException(); //Нет такой проверки
        }


        #endregion Внутреннее устройство

        #region Поля настроек

        public string PathToBackup = @"D:\BackupDiss\KPK2.bak";
        public string ReportHeader = string.Empty;
        public string RightReportHeader = string.Empty;
        public bool ShowReportLogo = false;
        public string LogoFile = "logo.bmp";
        public bool PoverkaEnabled = true;
        public bool IsSilkway = true;
        public bool IsNSK = true;
        public bool ShowDiss32 = true;
        public string CurrentLanguage = DataManager.RussianLang;
        public bool SpectrumAnalyzerActive = true;

        /// <summary>
        /// Аттенюаторы в сумме для ДИСС-15
        /// </summary>
        public double AttenDbmKoef = 30;
        public double CabelDiss15DbmKoef = 1.5;
        /// <summary>
        /// Волноводный переход для ДИСС-15
        /// </summary>
        public double VPDbmKoef = 1;


        public double KVPDiss13DbmKoef = 0.4;
        public double KVPDiss15DbmKoef = 0.1;
        public double KVPDiss32DbmKoef = 0.1;
        /// <summary>
        /// Сборка кабельная для ДИСС-013
        /// </summary>
        public double CabelDbmKoef = 1.5;
        /// <summary>
        /// Аттенюатр ДИСС-013
        /// </summary>
        public double AdapterDbmKoef = 3;

        public double PlugDiss13DbmKoef = 0.1;

        /// <summary>
        /// Дополнительный разворот угла для полёта для ДИСС-32
        /// </summary>
        public double AddFlyDirectionDiss32 = 0;

        /// <summary>
        /// Дополнительный разворот угла для наклонов в задаче 1 Дисс-32
        /// Неизвестно точно почему именно такое значение нужно, но это было в скрытом виде в версии 2.0.16, а теперь в явном виде появляется здесь
        /// </summary>
        public double AddAngleKZ1Diss32 = 180;
        /// <summary>
        /// Версия настроке, 0 - первая версия, 1 - с произвольным включением/выключением проверок
        /// </summary>
        public int SettingsVersion = 0;

        public List<DeviceAndMod> EnableChecks = new List<DeviceAndMod>();

        #endregion Поля настроек

        public object this[string pairValue]
        {
            get
            {
                switch (pairValue)
                {
                    case "IsSilkWay":
                        return IsSilkway;
                    case "PoverkaEnabled":
                        return PoverkaEnabled;
                    case "IsNSK":
                        return IsNSK;
                    case "ShowDiss32":
                        return ShowDiss32;
                    case "SpectrumAnalyzerActive":
                        return SpectrumAnalyzerActive;
                }
                throw new Exception("Parameter not exist!");
            }

            set
            {
                bool bValue = (bool)value;
                switch (pairValue)
                {
                    case "IsSilkWay":
                        IsSilkway = bValue;
                        return;
                    case "PoverkaEnabled":
                        PoverkaEnabled = bValue;
                        return;
                    case "IsNSK":
                        IsNSK = bValue;
                        return;
                    case "ShowDiss32":
                        ShowDiss32 = bValue;
                        return;
                    case "SpectrumAnalyzerActive":
                        SpectrumAnalyzerActive = bValue;
                        return;
                }
                throw new Exception("Parameter not exist!");
            }
        }

        internal bool GetCheckEnable(DeviceAndMod deviceAndMod)
        {
            var inDeviceAndMod = deviceAndMod;

            if (inDeviceAndMod.DeviceId == DevicesID.Diss15 && inDeviceAndMod.Modification == Modification.Diss15G)
                inDeviceAndMod.Modification = Modification.Diss15G;

            foreach (var item in EnableChecks)
            {
                if (item.DeviceId == deviceAndMod.DeviceId && item.Modification == deviceAndMod.Modification)
                    return item.CheckState;
            }
            throw new NotImplementedException();
        }
        /// Метод удаляет дублирующиеся записи из списка EnableChecks, оставляя только первые вхождения
        /// для каждого уникального названия, возвращаемого GetDissCaption.
        public void EnsureUniqueCaptions()
        {
            var unique = new Dictionary<string, DeviceAndMod>();
            foreach (var item in EnableChecks)
            {
                var caption = Sparc.KPK2.NFunctions.Globals.GetDissCaption(item.DeviceId, item.Modification);
                if (!unique.ContainsKey(caption))
                {
                    unique.Add(caption, item);
                }
                // если ключ уже есть — пропускаем (дубликат удаляется)
            }
            // Перезаписываем список только уникальными элементами
            EnableChecks = new List<DeviceAndMod>(unique.Values);
        }
    }
}
