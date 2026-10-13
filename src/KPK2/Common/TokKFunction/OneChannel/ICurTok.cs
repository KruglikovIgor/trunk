using System;
using KPK2.Common.DissFunctions.Diss32;
using KPK2.Common.DissFunctions.Diss32.MeasurementPerfomanceState;
using KPK2.Common.TokKFunction.OneChannel.Events;

namespace KPK2.Common.TokKFunction.OneChannel
{
	public interface ICurTok
	{
		event EventHandler<EventImageTok> EventExecuting;

		bool IsHigh { get; }

		ICurTok Execude(MeasurementPerformance measurementPerformance, ITypeChecker type);
	}
}
