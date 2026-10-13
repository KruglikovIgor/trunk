using KPK2.Common.DissFunctions.Diss32.MeasurementPerfomanceElement;
using Sparc.KPK2.NFunctions.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KPK2.Common.DissFunctions.Diss32.TestTasksState
{
	public interface ITypeTestTasks
	{
		DevicesID DeviceID { get; }
		Modification Modification { get; }
		IndicatorViewerCreator Task1 { get; }
		IndicatorViewerCreator Task2 { get; }
		IndicatorViewerCreator Task3 { get; }
		IndicatorViewerCreator Task4 { get; }
		IndicatorViewerCreator BPK { get; }
		IndicatorViewerCreator ZTask { get; }
		IndicatorViewerCreator ZPNPTask { get; }
		IndicatorViewerCreator ZZeroTask { get; }

		IndicatorViewerCreator Zero { get; }
		IndicatorViewerCreator BortSystem { get; }
		IndicatorViewerCreator ModeMC { get; }
		IndicatorViewerCreator ModeMB { get; }

		void InitializeBortSystem(int taskNum, int checkerId);
	}
}
