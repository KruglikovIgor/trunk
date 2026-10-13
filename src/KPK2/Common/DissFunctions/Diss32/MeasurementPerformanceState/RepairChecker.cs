using Sparc.KPK2.NFunctions.Enums;
using Sparc.KPK2.NFunctions;
using System;
using KPK2.Common.DissFunctions.Diss32.MeasurementPerfomanceElement;
using KPK2.Properties;

namespace KPK2.Common.DissFunctions.Diss32.MeasurementPerfomanceState
{
	public class RepairChecker : ITypeChecker
	{
		public IndicatorViewerCreator FirstPitenIndicatorViewer { get; private set; }
		public IndicatorViewerCreator SecPitanIndicatorViewer { get; private set; }
		public IndicatorViewerCreator GLPDIndicatorViewer { get; private set; }
		public IndicatorViewerCreator OffTokKIndicatorViewer { get; private set; }
		public IndicatorViewerCreator OnTokKIndicatorViewer { get; private set; }
		public IndicatorViewerCreator DeltaAbsTokIndicatorViewer { get; private set; }
		public IndicatorViewerCreator CdIndicatorViewer { get; private set; }
		public IndicatorViewerCreator NoiseIndicatorViewer { get; private set; }
		public IndicatorViewerCreator LoadModificationDiss3290 { get; private set; }
		public IndicatorViewerCreator LoadPower115VActor { get; private set; }
		public IndicatorViewerCreator ElecInsulationResistance { get; private set; }


		public int ChannelState { get; set; }
		public bool IsOneChannel { get; set; }
		public int TestId { get; private set; }

		public RepairChecker()
		{
		}

		public void Initialize(int checkerId, Modification modification, DevicesID devicesId, bool isOneChannel)
		{
			TestId = checkerId;
			#region Инициализация проверки источника питания

			string PS115V_Read_U = "115V_Read_U";
			string PS115V_Read_I = modification == Modification.Diss32_90A
				? "Plus_115V_32(90A)_I"
				: "Plus_115V_32_I";
			string PS27V_Read_U = "Plus_27V_32";
			string PS27V_Read_I = modification == Modification.Diss32_90A
				? "Plus_27V_32(90A)_I"
				: "Plus_27V_32_I";

			FirstPitenIndicatorViewer = new IndicatorViewerCreator();
			FirstPitenIndicatorViewer.CreateIndicator(checkerId, PS115V_Read_U, 4,
				string.Format(Resources.UV, Resources.Volt),
				string.Format(Resources.Positive115V, Resources.Volt));
			FirstPitenIndicatorViewer.CreateIndicator(checkerId,
				PS115V_Read_I,
				12,
				string.Format(Resources.IA, Resources.Amper),
				string.Format(Resources.Positive115V, Resources.Volt));
			FirstPitenIndicatorViewer.CreateIndicator(checkerId, PS27V_Read_U, 0,
				string.Format(Resources.UV, Resources.Volt),
				string.Format(Resources.Positive27V, Resources.Volt));
			FirstPitenIndicatorViewer.CreateIndicator(checkerId, PS27V_Read_I, 8,
				string.Format(Resources.IA, Resources.Amper),
				string.Format(Resources.Positive27V, Resources.Volt));

			#endregion

			#region GLPD mode

			string glpd1 = "GLPD_1";
			string glpd2 = "GLPD_2";
			string igd = "I_GD";
			string mod1Glpd = "MOD_1";
			string mod2Glpd = "MOD_2";

			if (devicesId == DevicesID.Diss32)
			{
				const string addStr = "_Diss32";
				mod1Glpd += addStr;
				mod2Glpd += addStr;
			}

			GLPDIndicatorViewer = new IndicatorViewerCreator();
			GLPDIndicatorViewer.CreateIndicator(checkerId, glpd1, 0, string.Format(Resources.UV, Resources.Volt),
				string.Format(Resources.GLPD, 1));
			GLPDIndicatorViewer.CreateIndicator(checkerId, glpd2, 1, string.Format(Resources.UV, Resources.Volt),
				string.Format(Resources.GLPD, 2));
			GLPDIndicatorViewer.CreateIndicator(checkerId, igd, 2, string.Format(Resources.UV, Resources.Volt),
				Resources.GLPDCurrent);
			GLPDIndicatorViewer.CreateIndicator(checkerId, mod1Glpd, 4, string.Format(Resources.UV, Resources.Volt),
				string.Format(Resources.MOD, 1));
			GLPDIndicatorViewer.CreateIndicator(checkerId, mod2Glpd, 5, string.Format(Resources.UV, Resources.Volt),
				string.Format(Resources.MOD, 2));

			#endregion

			#region Tok k
			InitializeOffTokK(checkerId);
			#region Off

			#endregion

			#region on

			InitializeOnTokK(checkerId);
			#endregion
			#endregion ТОК К

			#region СД
			InitializeSD(TestId);
			#endregion

			#region Шум УНЧ

			InitializeNoise32(TestId);

			#endregion

			#region Load модификации 3290

			LoadModificationDiss3290 = new IndicatorViewerCreator();
			LoadModificationDiss3290.CreateIndicator(checkerId, "MapCorner", (int)Globals.IndicatorNumber.MapCorner, "", "");
			#endregion

			#region Загрузка ИП 115В

			LoadPower115VActor = new IndicatorViewerCreator();
			LoadPower115VActor.CreateIndicator(checkerId, "115V_Read_U_LCard", (int)Globals.IndicatorNumber.Power115_U, "", "");
			LoadPower115VActor.CreateIndicator(checkerId, "115V_Read_I_LCard", (int)Globals.IndicatorNumber.Power115_I, "", "");

            #endregion

            #region ElecInsulationResistance
            ElecInsulationResistance = new IndicatorViewerCreator();
			ElecInsulationResistance.CreateIndicator(TestId, "ElecResist_VCH_H34_38", (int)Globals.IndicatorNumber.VCH3438, "ВЧ.Ш34:38", "МОм");
            ElecInsulationResistance.CreateIndicator(TestId, "ElecResist_VCH_H34_39", (int)Globals.IndicatorNumber.VCH3439, "ВЧ.Ш34:39", "МОм");
            ElecInsulationResistance.CreateIndicator(TestId, "ElecResist_VCH_H35_1", (int)Globals.IndicatorNumber.VCH351, "ВЧ.Ш35:1", "МОм");
            ElecInsulationResistance.CreateIndicator(TestId, "ElecResist_VCH_H35_4", (int)Globals.IndicatorNumber.VCH354, "ВЧ.Ш35:4", "МОм");
            ElecInsulationResistance.CreateIndicator(TestId, "ElecResist_VSS_H4_38", (int)Globals.IndicatorNumber.VSSH438, "ВСС.Ш4:38", "МОм");
            ElecInsulationResistance.CreateIndicator(TestId, "ElecResist_VSS_H4_39", (int)Globals.IndicatorNumber.VSSH439, "ВСС.Ш4:39", "МОм");
			
			if (modification == Modification.Diss32_90A)
			{
				ElecInsulationResistance.CreateIndicator(TestId, "ElecResist_BVK_H2_41", (int)Globals.IndicatorNumber.BVKH241, "БВК.Ш2:41", "МОм");
				ElecInsulationResistance.CreateIndicator(TestId, "ElecResist_BVK_H2_40", (int)Globals.IndicatorNumber.BVKH240, "БВК.Ш2:40", "МОм");
				ElecInsulationResistance.CreateIndicator(TestId, "ElecResist_BL_H2_49", (int)Globals.IndicatorNumber.BL8H249, "Бл8.Ш2:49", "МОм");
				ElecInsulationResistance.CreateIndicator(TestId, "ElecResist_BL_H2_50", (int)Globals.IndicatorNumber.BL8H250, "Бл8.Ш2:50", "МОм");
			}

            #endregion

        }

        public void InitializeSecPitanIndicatorViewerDiss32(int testId)
		{
			#region Вторичные источники питания

			string plus12v32 = "Plus_12V_WorkTest";
			string minus12V32 = "Minus_12V_WorkTest";
			string plus6V = "Plus_6V";
			string minus6V32 = "Minus_6V_32";
			string minus20V = "Minus_20V";
			string plus15V = "Plus_15V";
			string minus15V = "Minus_15V";
			string delta15V = "Delta_15V";
			string plus5V = "Plus_5V";
			string minus27V = "Minus_27V";
			string VSS10VWotkTest = "VSS_10V_WorkTest";

			SecPitanIndicatorViewer = new IndicatorViewerCreator();
			SecPitanIndicatorViewer.CreateIndicator(testId, plus12v32, 0,
				string.Format(Resources.UV, Resources.Volt),
				"+12,6" + Resources.Volt);
			SecPitanIndicatorViewer.CreateIndicator(testId, minus12V32, 1,
				string.Format(Resources.UV, Resources.Volt),
				"-12,6" + Resources.Volt);
			SecPitanIndicatorViewer.CreateIndicator(testId, plus6V, 3, string.Format(Resources.UV, Resources.Volt),
				"+6,3" + Resources.Volt);
			SecPitanIndicatorViewer.CreateIndicator(testId, minus6V32, 4,
				string.Format(Resources.UV, Resources.Volt),
				"-6,3" + Resources.Volt);
			SecPitanIndicatorViewer.CreateIndicator(testId, minus20V, 6,
				string.Format(Resources.UV, Resources.Volt),
				"-20" + Resources.Volt);
			SecPitanIndicatorViewer.CreateIndicator(testId, plus15V, 8,
				string.Format(Resources.UV, Resources.Volt),
				"+15" + Resources.Volt);
			SecPitanIndicatorViewer.CreateIndicator(testId, minus15V, 9,
				string.Format(Resources.UV, Resources.Volt),
				"-15" + Resources.Volt);
			SecPitanIndicatorViewer.CreateIndicator(testId, delta15V, 10,
				string.Format(Resources.UV, Resources.miliVolt),
				((Char)0x0394) + "15" + Resources.Volt);
			SecPitanIndicatorViewer.CreateIndicator(testId, plus5V, 12,
				string.Format(Resources.UV, Resources.Volt),
				"+5" + Resources.Volt);
			SecPitanIndicatorViewer.CreateIndicator(testId, minus27V, 14,
				string.Format(Resources.UV, Resources.Volt),
				"-27" + Resources.Volt);
			SecPitanIndicatorViewer.CreateIndicator(testId, VSS10VWotkTest, 15,
				string.Format(Resources.UV, Resources.Volt),
				"+10 ОПОР " + Resources.Volt);
			//	SecPitanIndicatorViewer.CreateIndicator(testId, VSS10VWotkTest, 11,
			//	string.Format(Resources.UV, Resources.Volt),
			//	"Udc");
			#endregion
		}

		public void InitializeSecPitanIndicatorViewerDiss3290A(int testId)
		{
			#region  загрузка модификации дисс32 90А

			SecPitanIndicatorViewer = new IndicatorViewerCreator();
			SecPitanIndicatorViewer.CreateIndicator(testId, "MapCorner", (int)Globals.IndicatorNumber.MapCorner, "", "");

			#endregion
		}

		public void InitializeDeltaAbsTok()
		{
			#region дельта АБС

			DeltaAbsTokIndicatorViewer = new IndicatorViewerCreator();
			if (IsOneChannel)
				CalcDeltaAbsOneChannel(TestId, ChannelState, DeltaAbsTokIndicatorViewer);
			else
				CalcDeltaAbs(TestId, DeltaAbsTokIndicatorViewer);
			#endregion
		}

		public void InitializeOnTokK(int testId)
		{
			string nameParameter = "I_K_{0}_{1}_32";
			OnTokKIndicatorViewer = new IndicatorViewerCreator();
			if (IsOneChannel)
			{
				InitializeOneChannel(testId, ChannelState, 8, nameParameter, Resources.CurrentK, OnTokKIndicatorViewer);
			}
			else
			{
				InitializeNotOneChannel(testId, 8, nameParameter, Resources.CurrentK, OnTokKIndicatorViewer);
			}
		}

		public void InitializeOffTokK(int testId)
		{
			string nameParamTok = "I_K_{0}_{1}_32";
			OffTokKIndicatorViewer = new IndicatorViewerCreator();
			if (IsOneChannel)
				InitializeOneChannel(testId, ChannelState, 0, nameParamTok, Resources.CurrentK, OffTokKIndicatorViewer);
			else
				InitializeNotOneChannel(testId, 0, nameParamTok, Resources.CurrentK, OffTokKIndicatorViewer);
		}

		public void InitializeSD(int testId)
		{
			string sdParamName = "SD_{0}_{1}";
			CdIndicatorViewer = new IndicatorViewerCreator();
			if (IsOneChannel)
				InitializeOneChannel(testId, ChannelState, 0, sdParamName, Resources.SD, CdIndicatorViewer);
			else
				InitializeNotOneChannel(testId, 0, sdParamName, Resources.SD, CdIndicatorViewer);
		}

		public void InitializeNoise32(int testId)
		{
			string noiseParamName = "UNCh_{0}_{1}_Noise32";
			NoiseIndicatorViewer = new IndicatorViewerCreator();
			if (IsOneChannel)
				InitializeOneChannel(testId, ChannelState, 0, noiseParamName, Resources.NoiseNN, NoiseIndicatorViewer);
			else
				InitializeNotOneChannel(testId, 0, noiseParamName, Resources.NoiseNN, NoiseIndicatorViewer);


		}

		private void InitializeOneChannel(int checkerId, int channelState, int shiftIndication, string paramName, string lableName,
		IndicatorViewerCreator tokKViewerCreator)
		{
			int numGroup1 = channelState;
			for (int group1 = numGroup1; group1 <= numGroup1; group1++)
			{
				for (int group2 = 1; group2 <= 2; group2++)
				{
					int numIndicator = group2 == 1
							? (int)Globals.IndicatorNumber.ChannelMeas1
							: (int)Globals.IndicatorNumber.ChannelMeas2;
					if (shiftIndication == 8)
						numIndicator = group2 == 1
							? (int)Globals.IndicatorNumber.ChannelMeasCurrent1
							: (int)Globals.IndicatorNumber.ChannelMeasCurrent2;
					tokKViewerCreator.CreateIndicator(checkerId, string.Format(paramName, group1, group2), numIndicator,
						string.Format(Resources.UV, Resources.miliVolt),
						string.Format(lableName, group1, group2),
						true);
				}
			}
		}

		private void InitializeNotOneChannel(int checkerId, int shiftIndication, string paramName, string lableName,
			IndicatorViewerCreator tokKViewerCreator)
		{
			int numGroup1 = 3;
			for (int group1 = 1; group1 <= numGroup1; group1++)
			{
				for (int group2 = 1; group2 <= 2; group2++)
				{
					int numIndicator = (group1 - 1) * numGroup1 + group2 + shiftIndication - 1;
					tokKViewerCreator.CreateIndicator(checkerId, string.Format(paramName, group1, group2),
						numIndicator,
						string.Format(Resources.UV, Resources.miliVolt),
						string.Format(lableName, group1, group2));
				}
			}
		}

		private void CalcDeltaAbs(int checkerId, IndicatorViewerCreator deltaTok)
		{
			string paramName = Mode.GetMode().ModeApplication == ModeApplication.RepairCheck
				? "I_K_{0}_{1}_Delta_WorkTest"
				: "I_K_Delta";
			int group1Count = 3;
			int group2Count = 2;
			int idInd = 0;
			for (int group1 = 1; group1 <= group1Count; group1++)
				for (int group2 = 1; group2 <= group2Count; group2++)
				{
					idInd = (group1 - 1) * group1Count + group2 - 1;
					deltaTok.CreateIndicator(checkerId, string.Format(paramName, group1, group2), idInd, string.Format(Resources.UV, Resources.miliVolt),
						string.Format(((Char)0x0394) + Resources.CurrentK, group1, group2));
				}

			for (int i = 0; i < 3; i++) // определяем Abs 
			{
				idInd = i * 3 + 9 - 1;
				deltaTok.CreateIndicator(checkerId, "I_K_Abs", idInd, string.Format(Resources.UV, Resources.miliVolt),
					string.Format(((Char)0x0394) + Resources.Uabs, i + 1));
			}
		}

		public void CalcDeltaAbsOneChannel(int checkerId, int channelState, IndicatorViewerCreator deltaTok)
		{
			string paramName = Mode.GetMode().ModeApplication == ModeApplication.RepairCheck
				? "I_K_{0}_{1}_Delta_WorkTest"
				: "I_K_Delta";
			int group1Count = channelState;
			int group2Count = 2;
			int idInd = 0;
			for (int group1 = group1Count; group1 <= group1Count; group1++)
				for (int group2 = 1; group2 <= group2Count; group2++)
				{
					idInd = group2 == 1 ? (int)Globals.IndicatorNumber.ChannelMeas1 : (int)Globals.IndicatorNumber.ChannelMeas2;
					deltaTok.CreateIndicator(checkerId, string.Format(paramName, group1, group2), idInd, string.Format(Resources.UV, Resources.miliVolt),
						string.Format(((Char)0x0394) + "Ток К {0}-{1}", group1, group2));

				}

			for (int i = 0; i < 3; i++) // определяем Abs 
			{
				idInd = i * 3 + 9 - 1;
				deltaTok.CreateIndicator(checkerId, "I_K_Abs", idInd, string.Format(Resources.UV, Resources.miliVolt),
					string.Format(((Char)0x0394) + Resources.Uabs, i + 1));
			}
		}
	}

}
