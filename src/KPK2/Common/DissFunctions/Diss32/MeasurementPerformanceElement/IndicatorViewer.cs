using Sparc.KPK2.Hardware.MyClasseUSB;

namespace KPK2.Common.DissFunctions.Diss32.MeasurementPerfomanceElement
{
	public class IndicatorViewer
	{
		public IndicatorViewer()
		{

		}

		public int CheckId { get; set; }
		public string ParameterName { get; set; }
		public int IndicatorNumber { get; set; }
		public string NameLabelUp { get; set; }
		public string NameLabelDown { get; set; }
		public USBBaseNew UsbBaseNew { get; set; }
	}
}
