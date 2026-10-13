using KPK2.Common.DissFunctions.Diss32.MeasurementPerfomanceState;
using Sparc.KPK2.Hardware.MyClasseUSB;
using Sparc.KPK2.NFunctions.Enums;
using System.Windows.Forms;
using UserControlsKPK2;

namespace KPK2.MyControl
{
	public partial class ElecInsulationResistanceControl : UserControl
	{
		public ElecInsulationResistanceControl(ITypeChecker typeChecker, Modification modification)
		{
			InitializeComponent();
			InitializeElecResistance(VCH3438, typeChecker.ElecInsulationResistance.Viewers[0].UsbBaseNew);
			InitializeElecResistance(VCH3439, typeChecker.ElecInsulationResistance.Viewers[1].UsbBaseNew);
			InitializeElecResistance(VCH351, typeChecker.ElecInsulationResistance.Viewers[2].UsbBaseNew);
			InitializeElecResistance(VCH354, typeChecker.ElecInsulationResistance.Viewers[3].UsbBaseNew);
            InitializeElecResistance(VSSH438, typeChecker.ElecInsulationResistance.Viewers[4].UsbBaseNew);
            InitializeElecResistance(VSSH439, typeChecker.ElecInsulationResistance.Viewers[5].UsbBaseNew);
            if (modification == Modification.Diss32_90A)
            {
                InitializeElecResistance(BVKH241, typeChecker.ElecInsulationResistance.Viewers[6].UsbBaseNew);
                InitializeElecResistance(BVKH240, typeChecker.ElecInsulationResistance.Viewers[7].UsbBaseNew);
                InitializeElecResistance(BL8H249, typeChecker.ElecInsulationResistance.Viewers[8].UsbBaseNew);
                InitializeElecResistance(BL8H250, typeChecker.ElecInsulationResistance.Viewers[9].UsbBaseNew);
            }
			
			if(modification != Modification.Diss32_90A)
			{
				BVKH240.Visible = false;
				BVKH241.Visible = false;
				BL8H249.Visible = false;
				BL8H250.Visible = false;
			}
        }

        private void InitializeElecResistance(IndicatorValue indicatorValue, USBBaseNew uSBBase)
		{
			indicatorValue.MaxGreenValue = uSBBase.Max;
			indicatorValue.MinGreenValue = uSBBase.Min;
		}
	}
}
