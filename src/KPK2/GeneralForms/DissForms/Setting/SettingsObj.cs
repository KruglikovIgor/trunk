using KPK2.DB;
using Sparc.KPK2.Hardware.LTR;
using Sparc.KPK2.Hardware.MyClasseUSB;
using Sparc.KPK2.NFunctions.Enums;
using System;
using System.Collections.Generic;
using KPK2.MyClasse;

namespace KPK2.GeneralForms.DissForms.Setting
{
	public class SettingsObj
	{
		private static Lazy<SettingsObj> _obj;

		public int TestId { get; set; } = -1;

		public bool IsOneChannel { get; set; }

		public Mode Mode { get; set; }

		public MeasuringValueInfo MeasValueInfo { get; set; }

		public Measure Measure { get; set; }

		public DataBaseKPK2 DBKPK2 { get; set; }

		public DevicesID DevicesId { get; set; }

		public Modification Modification { get; set; }

		public List<USBBaseNew> MeasList { get; set; }

		private SettingsObj()
		{

		}

		public static SettingsObj GetSettings()
		{
			if (_obj == null)
			{
				_obj = new Lazy<SettingsObj>(() => new SettingsObj());
			}
			return _obj.Value;
		}
	}
}
