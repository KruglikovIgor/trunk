using System;
using KPK2.Common.DissFunctions.Diss32;
using KPK2.Common.DissFunctions.Diss32.MeasurementPerfomanceState;
using KPK2.Common.TokKFunction.OneChannel.Events;
using KPK2.Properties;

namespace KPK2.Common.TokKFunction.OneChannel
{
	public class CurTok0 : ICurTok
	{
		public event EventHandler<EventImageTok> EventExecuting;

		public bool IsHigh { get; } = false;
		public ICurTok Execude(MeasurementPerformance s, ITypeChecker type)
		{
			EventExecuting?.Invoke(this, new EventImageTok()
			{
				Name = Resources.VoltagesKCurrentsHighOff,
				CheckType = CheckType.Tok_K_Off
			});
			s.ViewIndicators(type.OffTokKIndicatorViewer, true);
			s.SetChannelIndicatorsVisible(s.ChannelState - 1, true);
			s.StartWorker();
			return new CurTok1();
		}
	}
}
