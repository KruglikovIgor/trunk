using Sparc.KPK2.NFunctions.Enums;
using KPK2.Common.DissFunctions.Diss32.MeasurementPerfomanceElement;

namespace KPK2.Common.DissFunctions.Diss32.MeasurementPerfomanceState
{
	public interface ITypeChecker
	{
		IndicatorViewerCreator FirstPitenIndicatorViewer { get; }
		IndicatorViewerCreator SecPitanIndicatorViewer { get; }
		IndicatorViewerCreator GLPDIndicatorViewer { get; }
		IndicatorViewerCreator OffTokKIndicatorViewer { get; }
		IndicatorViewerCreator OnTokKIndicatorViewer { get; }
		IndicatorViewerCreator DeltaAbsTokIndicatorViewer { get; }
		IndicatorViewerCreator CdIndicatorViewer { get; }
		IndicatorViewerCreator NoiseIndicatorViewer { get; }
		IndicatorViewerCreator LoadModificationDiss3290 { get; }
		IndicatorViewerCreator LoadPower115VActor { get; }
		IndicatorViewerCreator ElecInsulationResistance { get; }

        int ChannelState { get; set; }
		bool IsOneChannel { get; set; }
		int TestId { get; }
		void Initialize(int checkerId, Modification modification, DevicesID devicesId, bool isOneCheck);
		void InitializeDeltaAbsTok();
		void InitializeSecPitanIndicatorViewerDiss32(int testId);
		void InitializeOnTokK(int testId);
		void InitializeSD(int testId);
		void InitializeNoise32(int testId);
		void InitializeOffTokK(int testId);
		void InitializeSecPitanIndicatorViewerDiss3290A(int testId);
	}

}
