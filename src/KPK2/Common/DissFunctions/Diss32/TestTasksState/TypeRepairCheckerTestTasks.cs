using KPK2.Common.DissFunctions.Diss32.MeasurementPerfomanceElement;
using Sparc.KPK2.NFunctions.Enums;
using KPK2.Properties;

namespace KPK2.Common.DissFunctions.Diss32.TestTasksState
{
	public class TypeRepairCheckerTestTasks : ITypeTestTasks
	{
		public DevicesID DeviceID { get; private set; }
		public Modification Modification { get; private set; }

		public IndicatorViewerCreator Task1 { get; private set; }
		public IndicatorViewerCreator Task2 { get; private set; }
		public IndicatorViewerCreator Task3 { get; private set; }
		public IndicatorViewerCreator Task4 { get; private set; }
		public IndicatorViewerCreator BPK { get; private set; }
		public IndicatorViewerCreator ZTask { get; private set; }
		public IndicatorViewerCreator ZPNPTask { get; private set; }
		public IndicatorViewerCreator ZZeroTask { get; private set; }
		public IndicatorViewerCreator Zero { get; private set; }
		public IndicatorViewerCreator BortSystem { get; private set; }
		public IndicatorViewerCreator ModeMC { get; private set; }
		public IndicatorViewerCreator ModeMB { get; private set; }

		public TypeRepairCheckerTestTasks(int checkerId, bool isOneChannel, DevicesID deviceID, Modification modification)
		{
			DeviceID = deviceID;
			Modification = modification;
			bool isDiss32_90 = DeviceID == DevicesID.Diss32_90 && modification == Modification.None;

			string velocityFirstName = "Wxr, км/ч";
			string velocityLeftName = "Wzr, км/ч";
            string velocityNameDown = "Wyr, м/с";
			string firstName = "Вперед";
			string leftName = "Влево";
			string downName = "Вниз";

			#region Task1
			string paramNameTask1X = "Task1_X_32";
			string paramNameTask1Z = "Task1_Z_32";
			string paramNameTask1Y = "Task1_Y_32";

			Task1 = new IndicatorViewerCreator();
			Task1.CreateIndicator(checkerId, paramNameTask1X, (int)Globals.IndicatorNumber.Speed1,
				velocityFirstName, firstName, isOneChannel);
			Task1.CreateIndicator(checkerId, paramNameTask1Z, (int)Globals.IndicatorNumber.Speed2,
				velocityLeftName, leftName, isOneChannel);
			Task1.CreateIndicator(checkerId, paramNameTask1Y, (int)Globals.IndicatorNumber.Speed3,
				velocityNameDown, downName, isOneChannel);
			#endregion

			#region Task2
			firstName = "Назад";
			leftName = "Вправо";
			downName = "Вверх";
			string paramNameTask2X = "Task2_X_32";
			string paramNameTask2Z = "Task2_Z_32";
			string paramNameTask2Y = "Task2_Y_32";
			Task2 = new IndicatorViewerCreator();
			Task2.CreateIndicator(checkerId, paramNameTask2X, (int)Globals.IndicatorNumber.Speed1,
				velocityFirstName, firstName, isOneChannel);
			Task2.CreateIndicator(checkerId, paramNameTask2Z, (int)Globals.IndicatorNumber.Speed2,
				velocityLeftName, leftName, isOneChannel);
			Task2.CreateIndicator(checkerId, paramNameTask2Y, (int)Globals.IndicatorNumber.Speed3,
				velocityNameDown, downName, isOneChannel);
			#endregion

			#region Task3


			string Task3_PC_32_Const = "Task3_PC_32_RepairTest";
			string Task3_Y0_32_Const = "Task3_Y0_32_RepairTest";
			string Task3_PC_32 = Task3_PC_32_Const;
			string Task3_Y0_32 = Task3_Y0_32_Const;
			string Task3_PC2_32 = "Task_PC2_32";
			string Task3_Y20_32 = "Task_Y20_32";

			Task3 = new IndicatorViewerCreator();
			Task3.CreateIndicator(checkerId, Task3_PC_32, (int)Globals.IndicatorNumber.Speed1,
				string.Format(Resources.WN, Resources.KmH), string.Format(Resources.PS, 1));
			Task3.CreateIndicator(checkerId, Task3_Y0_32, (int)Globals.IndicatorNumber.Speed2,
				Resources.AlfaGrad, string.Format(Resources.US, 0));
			if (isDiss32_90)
			{
				Task3.CreateIndicator(checkerId, Task3_PC2_32, (int)Globals.IndicatorNumber.Speed1_2,
					string.Format(Resources.WN, Resources.KmH), string.Format(Resources.PS, 1));
				Task3.CreateIndicator(checkerId, Task3_Y20_32, (int)Globals.IndicatorNumber.Speed2_2, Resources.AlfaGrad,
					string.Format(Resources.US, 0));
			}
			#endregion

			#region Task4

			string velosicyPS2 = "ПС 2";
			string US = "УС 9,5 Влево";

			string Task4_PC_32_Const = "Task4_PC_32_RepairTest";
			string Task4_Y9_32_Const = "Task4_Y9_32_RepairTest";
			string Task4_PC_32 = Task4_PC_32_Const;
			string Task4_Y0_32 = Task4_Y9_32_Const;
			string Task4_PC2_32 = "Task4_PC2_32";
			string Task4_Y20_32 = "Task4_Y29_32";

			Task4 = new IndicatorViewerCreator();
			Task4.CreateIndicator(checkerId, Task4_PC_32, (int)Globals.IndicatorNumber.Speed1,
				string.Format(Resources.WN, Resources.KmH), velosicyPS2);
			Task4.CreateIndicator(checkerId, Task4_Y0_32, (int)Globals.IndicatorNumber.Speed2,
				Resources.AlfaGrad, US);
			if (isDiss32_90)
			{
				Task4.CreateIndicator(checkerId, Task4_PC2_32, (int)Globals.IndicatorNumber.Speed1_2,
					string.Format(Resources.WN, Resources.KmH), string.Format(Resources.PS, 2));
				Task4.CreateIndicator(checkerId, Task4_Y20_32, (int)Globals.IndicatorNumber.Speed2_2, Resources.AlfaGrad,
					string.Format(Resources.US, "9.5") + " Влево");
			}
			#endregion

			#region Z ?? это связано с BPK//todo дать другое название

			string ZTaskStr = "Z_Task_32";
			string ZPNPStr = "Z_PNP_32";
			string Z_Zero_32 = "Z_Zero_32";

			ZTask = new IndicatorViewerCreator();
			ZPNPTask = new IndicatorViewerCreator();
			ZZeroTask = new IndicatorViewerCreator();

			ZTask.CreateIndicator(checkerId, ZTaskStr, (int)Globals.IndicatorNumber.ZoptPVK, "", "");
			ZPNPTask.CreateIndicator(checkerId, ZPNPStr, (int)Globals.IndicatorNumber.ZoptPNP, "", "");
			ZZeroTask.CreateIndicator(checkerId, Z_Zero_32, (int)Globals.IndicatorNumber.ZoptPVK, "", "");

			#endregion

			#region zero task

			string Wx = "NKV_X_Zero32_Diss32";
			string Wz = "NKV_Z_Zero32_Diss32";
			string Wy = "NKV_Y_Zero_Diss32";

			string WxPKV = "PKV_X_Zero32_Diss32";
			string WzPKV = "PKV_Z_Zero32_Diss32";
			string WyPKV = "PKV_Y_Zero32_Diss32";

			if (DeviceID == DevicesID.Diss32)
			{
				const string addStr = "_Diss32";
				Wx += addStr;
				Wy += addStr;
				Wz += addStr;
			}

			Zero = new IndicatorViewerCreator();
			Zero.CreateIndicator(checkerId, Wx, (int)Globals.IndicatorNumber.Zero1, string.Format(Resources.UV, Resources.miliVolt), "");
			Zero.CreateIndicator(checkerId, Wz, (int)Globals.IndicatorNumber.Zero2, string.Format(Resources.UV, Resources.miliVolt), "");
			Zero.CreateIndicator(checkerId, Wy, (int)Globals.IndicatorNumber.Zero3, string.Format(Resources.UV, Resources.miliVolt), "");
			Zero.CreateIndicator(checkerId, "NKV_W_Zero", (int)Globals.IndicatorNumber.Zero4, string.Format(Resources.UV, Resources.miliVolt), "");

            Zero.CreateIndicator(checkerId, WxPKV, (int)Globals.IndicatorNumber.Zero1PKV, string.Format(Resources.UV, Resources.miliVolt), "");
			Zero.CreateIndicator(checkerId, WzPKV, (int)Globals.IndicatorNumber.Zero2PKV, string.Format(Resources.UV, Resources.miliVolt), "");
			Zero.CreateIndicator(checkerId, WyPKV, (int)Globals.IndicatorNumber.Zero3PKV, string.Format(Resources.UV, Resources.miliVolt), "");

			#endregion

			#region Mode M-C

			string modeMC = "Mode_M_C_WorkTest";

			ModeMC = new IndicatorViewerCreator();
			ModeMC.CreateIndicator(checkerId, modeMC, (int)Globals.IndicatorNumber.Speed1, "Wмс", null);

            #endregion

            #region Mode M-B

            string modeMB = "Mode_M_B_WorkTest";

            ModeMB = new IndicatorViewerCreator();
            ModeMB.CreateIndicator(checkerId, modeMB, (int)Globals.IndicatorNumber.Speed1, "Wмб", null);

            #endregion
        }

        public void InitializeBortSystem(int taskNum, int checkerId)
		{
			string Wx = "Wx_{0}_Task{1}_SystemDiss32";
			string Wz = "Wz_{0}_Task{1}_SystemDiss32";
			string Wy = "Wy_{0}_Task{1}_SystemDiss32";
			string NKV = "NKV";
			string PKV = "PKV";
			string SPSV = "SPSV";
			string Tester = "Tester";

			BortSystem = new IndicatorViewerCreator();
			BortSystem.CreateIndicator(checkerId, string.Format(Wx, NKV, taskNum), (int)Globals.IndicatorNumber.Zero1, string.Format(Resources.UV, Resources.miliVolt), "");
			BortSystem.CreateIndicator(checkerId, string.Format(Wz, NKV, taskNum), (int)Globals.IndicatorNumber.Zero2, string.Format(Resources.UV, Resources.miliVolt), "");
			BortSystem.CreateIndicator(checkerId, string.Format(Wy, NKV, taskNum), (int)Globals.IndicatorNumber.Zero3, string.Format(Resources.UV, Resources.miliVolt), "");

			BortSystem.CreateIndicator(checkerId, string.Format(Wx, PKV, taskNum), (int)Globals.IndicatorNumber.Zero1PKV, string.Format(Resources.UV, Resources.miliVolt), "");
			BortSystem.CreateIndicator(checkerId, string.Format(Wz, PKV, taskNum), (int)Globals.IndicatorNumber.Zero2PKV, string.Format(Resources.UV, Resources.miliVolt), "");
			BortSystem.CreateIndicator(checkerId, string.Format(Wy, PKV, taskNum), (int)Globals.IndicatorNumber.Zero3PKV, string.Format(Resources.UV, Resources.miliVolt), "");

			BortSystem.CreateIndicator(checkerId, string.Format(Wx, SPSV, taskNum), (int)Globals.IndicatorNumber.Zero4, string.Format(Resources.UV, Resources.miliVolt), Resources.WxgSPSV);
			BortSystem.CreateIndicator(checkerId, string.Format(Wz, SPSV, taskNum), (int)Globals.IndicatorNumber.Zero5, string.Format(Resources.UV, Resources.miliVolt), "");
			BortSystem.CreateIndicator(checkerId, string.Format(Wy, SPSV, taskNum), (int)Globals.IndicatorNumber.Zero6, string.Format(Resources.UV, Resources.miliVolt), "");

			BortSystem.CreateIndicator(checkerId, string.Format(Wx, Tester, taskNum), (int)Globals.IndicatorNumber.Zero1Tester, string.Format(Resources.UV, Resources.miliVolt), "");
			BortSystem.CreateIndicator(checkerId, string.Format(Wz, Tester, taskNum), (int)Globals.IndicatorNumber.Zero2Tester, string.Format(Resources.UV, Resources.miliVolt), "");
			BortSystem.CreateIndicator(checkerId, string.Format(Wy, Tester, taskNum), (int)Globals.IndicatorNumber.Zero3Tester, string.Format(Resources.UV, Resources.miliVolt), "");
		}
	}
}
