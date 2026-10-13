using KPK2.GeneralForms.DissForms.Setting;
using System;
using KPK2.Common.DissFunctions.Diss32;
using KPK2.Common.DissFunctions.Diss32.MeasurementPerfomanceState;
using KPK2.Common.TokKFunction.OneChannel.Events;
using KPK2.Properties;

namespace KPK2.Common.TokKFunction.OneChannel
{
	public class CurTok1 : ICurTok
	{
		public event EventHandler<EventImageTok> EventExecuting;

		public bool IsHigh { get; } = false;
		public ICurTok Execude(MeasurementPerformance measurementPerformance, ITypeChecker type)
		{
			measurementPerformance.SaveAll(SettingsObj.GetSettings().MeasList);
			measurementPerformance.StopWorker(false);
			measurementPerformance.SaveToKByChannelStateAndNumParam(1);
			measurementPerformance.SaveToKByChannelStateAndNumParam(2);
			EventExecuting?.Invoke(this, new EventImageTok()
			{
				Name = Resources.VoltagesKCurrentHighOn,
				CheckType = CheckType.Tok_K_On
			});
			type.InitializeOnTokK(type.TestId);
			measurementPerformance.ImageStop();
			measurementPerformance.ViewIndicators(type.OnTokKIndicatorViewer, true);
			//measurementPerformance.SetChannelIndicatorsVisible(measurementPerformance.CurTok, true);
			measurementPerformance.StartWorker();
			return new CurTok2();
		}
	}
}
