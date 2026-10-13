using KPK2.GeneralForms.DissForms.Setting;
using Sparc.KPK2.NFunctions.Enums;
using Sparc.KPK2.NFunctions;
using KPK2.Common.DissFunctions.Diss32.TestTasksState;

namespace KPK2.Common.DissFunctions.Diss32
{
	public class TestTasks
	{
		public ITypeTestTasks TypeTestTasks { get; private set; }

		public TestTasks(int checkerId, bool isOneChannel, DevicesID devicesId, Modification modification)
		{
			var mode = SettingsObj.GetSettings().Mode;
			if (mode.ModeApplication == ModeApplication.DefaultCheck)
				TypeTestTasks = new TypeDefaultCheckerTestTasks(checkerId, isOneChannel, devicesId, modification);
			else
				TypeTestTasks = new TypeRepairCheckerTestTasks(checkerId, isOneChannel, devicesId, modification);
		}

		public void TaskCustomView(bool panelTask, int indNum, bool panelSpeed, bool panelCorner, bool buttonPitch,
			bool memory, bool otherButtons)
		{

		}

		public void ShowTask1()
		{

		}
	}
}
