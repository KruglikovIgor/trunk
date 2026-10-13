using System;
using KPK2.Common.DissFunctions.Diss32;
using KPK2.Common.DissFunctions.Diss32.MeasurementPerfomanceState;
using KPK2.Common.TokKFunction.OneChannel.Events;

namespace KPK2.Common.TokKFunction.OneChannel
{
	public class CurTok3 : ICurTok
	{
		public event EventHandler<EventImageTok> EventExecuting;
		public bool IsHigh { get; }
		public ICurTok Execude(MeasurementPerformance measurementPerformance, ITypeChecker type)
		{
			measurementPerformance.StopWorker(false);
			measurementPerformance.ImageStop();
			EventExecuting?.Invoke(this, new EventImageTok()
			{
				Name = "",
				CheckType = CheckType.Stop
			});
			return new CurTok0();
		}
	}
}
