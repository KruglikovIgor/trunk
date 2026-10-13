using Sparc.KPK2.Hardware.MyClasseUSB;
using System.Collections.Generic;

namespace KPK2.Common.DissFunctions.Diss32.MeasurementPerfomanceElement
{
	public class IndicatorViewerCreator
	{
		public List<IndicatorViewer> Viewers { get; }
		public bool IsChannelOne { get; private set; }
		public int CheckId { get; set; }

		public IndicatorViewerCreator()
		{
			Viewers = new List<IndicatorViewer>();
		}

		public void CreateIndicator(int checkId, string paramName, int indicatorNumber, string upString,
			string downString, bool isChannelOne = false)
		{
			CheckId = checkId;
			IsChannelOne = isChannelOne;
			Viewers.Add(new IndicatorViewer()
			{
				CheckId = checkId,
				ParameterName = paramName,
				IndicatorNumber = indicatorNumber,
				NameLabelUp = upString,
				NameLabelDown = downString,
				UsbBaseNew = new USBBaseNew(checkId, paramName, indicatorNumber)
			});
		}

		public bool Remove(IndicatorViewer indicatorViewer)
		{
			return Viewers.Remove(indicatorViewer);
		}

		public void Clear()
		{
			Viewers.Clear();
		}
	}
}
