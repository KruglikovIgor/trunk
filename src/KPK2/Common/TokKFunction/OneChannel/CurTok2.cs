using System;
using System.Threading;
using KPK2.Common.DissFunctions.Diss32;
using KPK2.Common.DissFunctions.Diss32.MeasurementPerfomanceState;
using KPK2.Common.TokKFunction.OneChannel.Events;
using KPK2.GeneralForms.DissForms.Setting;
using KPK2.Properties;
using Sparc.KPK2.NFunctions;

namespace KPK2.Common.TokKFunction.OneChannel
{
	public class CurTok2 : ICurTok
	{
		public event EventHandler<EventImageTok> EventExecuting;
		public bool IsHigh { get; }
		public ICurTok Execude(MeasurementPerformance measurementPerformance, ITypeChecker type)
		{
			string parameterDelta = Mode.GetMode().ModeApplication == ModeApplication.RepairCheck
				? "I_K_{0}_{1}_Delta_WorkTest"
				: "I_K_{0}_{1}_Delta";
			int group1Count = measurementPerformance.ChannelState;
			const int group2Count = 2;
			double result;
			int idInd = 0;
			Thread.Sleep(50);
			type.InitializeDeltaAbsTok();
			measurementPerformance.CalcDelta(SettingsObj.GetSettings().MeasList);
			measurementPerformance.StopWorker(false);
			measurementPerformance.SaveToKByChannelStateAndNumParam(1);
			measurementPerformance.SaveToKByChannelStateAndNumParam(2);
			EventExecuting?.Invoke(this, new EventImageTok()
			{
				Name = Resources.MeasVoltageAfterSVChOn,
				CheckType = CheckType.Tok_K
			});
			measurementPerformance.ViewIndicators(type.DeltaAbsTokIndicatorViewer, true);
			for (int group1 = group1Count; group1 <= group1Count; group1++)
			for (int group2 = 1; group2 <= group2Count; group2++)
			{
				idInd = group2 == 1 ? (int)Globals.IndicatorNumber.ChannelMeas1 : (int)Globals.IndicatorNumber.ChannelMeas2;
				result = measurementPerformance.DeltaResults[group2 - 1];
				measurementPerformance.SetIndicatorValue(idInd, result);
				DataManager.Instance.SaveParameter(type.TestId, string.Format(parameterDelta, group1, group2), result);
			}

			for (int i = 0; i < 1; i++) // определяем Abs 
			{
				idInd = (measurementPerformance.ChannelState - 1) * 3 + 9 - 1;
				result = Math.Abs(measurementPerformance.DeltaResults[i * 2] - measurementPerformance.DeltaResults[i * 2 + 1]);
				measurementPerformance.SetIndicatorValue(idInd, result);
				DataManager.Instance.SaveParameter(type.TestId, string.Format("I_K_{0}_Abs", measurementPerformance.ChannelState), result);
			}

			return new CurTok3();
		}
	}
}
