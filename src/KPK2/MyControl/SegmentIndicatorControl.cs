using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using KPK2.Common.DissFunctions.Diss32.MeasurementPerfomanceElement;
using Sparc.KPK2.Hardware.MyClasseUSB;
using UserControlsKPK2.Enums;

namespace UserControlsKPK2
{
	public partial class SegmentIndicatorControl : UserControl
	{
		public string ParameterName
		{
			get;
			set;
		}

		[Localizable(true)]
		public string NameLabelUp
		{
			get => LableUp.Text;
			set => LableUp.Text = value;
		}

		[Localizable(true)]
		public string NameLabelDown
		{
			get => LableDown.Text;
			set => LableDown.Text = value;
		}

		[Localizable(true)]
		public string NameSegment
		{
			get => SegmentIndication1.Name;
			set => SegmentIndication1.Name = value;
		}

		[Localizable(true)]
		public double Value
		{
			get => SegmentIndication1.Value;
			set => SegmentIndication1.Value = value;
		}

		public string ValueString
		{
			get => SegmentIndication1.ValueString;
			set => SegmentIndication1.ValueString = value;
		}

		public bool Inactive
		{
			get => SegmentIndication1.Inactive;
			set => SegmentIndication1.Inactive = value;
		}

		public double MaxValue
		{
			get => SegmentIndication1.MaxValue;
			set => SegmentIndication1.MaxValue = value;
		}

		public double MinValue
		{
			get => SegmentIndication1.MinValue;
			set => SegmentIndication1.MinValue = value;
		}

		public int WholePart
		{
			get => SegmentIndication1.WholePart;
			set
			{
				if (value > 0)
					SegmentIndication1.WholePart = value;
			}
		}

		public int FractPart
		{
			get => SegmentIndication1.FractPart;
			set
			{
				if (value > 0)
					SegmentIndication1.FractPart = value;
			}
		}

		public CalcMethod CalculationMethod
		{
			get => SegmentIndication1.CalculationMethod;
			set => SegmentIndication1.CalculationMethod = value;
		}

		public SegmentIndication.CheckValueDelegate CheckValueFunction
		{
			get => SegmentIndication1.CheckValueFunction;
			set => SegmentIndication1.CheckValueFunction = value;
		}

		public USBBaseNew UsbBaseNew { get; set; }

		[Localizable(true)]
		public int TestId { get; set; }

		public SegmentIndicatorControl()
		{
			InitializeComponent();
			Default();
		}

		public bool SetValue(IndicatorViewer iv)
		{
			if (iv == null | iv.UsbBaseNew == null)
				return false;
			UsbBaseNew = iv.UsbBaseNew;
			if (iv.NameLabelUp != "")
				LableUp.Text = iv.NameLabelUp;
			if (iv.NameLabelDown != "")
				LableDown.Text = iv.NameLabelDown;
			ValueString = "0.00";
			MaxValue = UsbBaseNew.Max;
			MinValue = UsbBaseNew.Min;
			WholePart = ((int)Math.Abs(UsbBaseNew.Max)).ToString().Length;
			FractPart = UsbBaseNew.Precision;

			return true;
		}

		public void Default()
		{
			SegmentIndication1.Font = new Font("Microsoft Sans Serif", 21.75f);
			SegmentIndication1.DarkColor = Color.Transparent;
			SegmentIndication1.Unit = "";
			SegmentIndication1.Value = 0;
		}
	}
}
