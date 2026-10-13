using System;
using System.Drawing;
using System.Windows.Forms;

namespace UserControlsKPK2
{
    public partial class SIWithLabel : UserControl
    {
        public double Value
        {
            get { return siIndicator.Value; }
            set { siIndicator.Value = value; }
        }

        public Color InvalidColor
        {
            get { return siIndicator.InvalidColor; }
            set { siIndicator.InvalidColor = value; }
        }

        public Color LightColor
        {
            get { return siIndicator.LightColor; }
            set { siIndicator.LightColor = value; }
        }

        public string ValueString
        {
            get { return siIndicator.ValueString; }
            set { siIndicator.ValueString = value; }
        }

        public double MaxValue
        {
            get { return siIndicator.MaxValue; }
            set { siIndicator.MaxValue = value; }
        }

        public double MinValue
        {
            get { return siIndicator.MinValue; }
            set { siIndicator.MinValue = value; }
        }

        public string NoteText
        {
            get { return UpperNote.Text; }
            set { UpperNote.Text = value; }
        }

        public Font IndicatorFont
        {
            get { return siIndicator.Font; }
            set { siIndicator.Font = value; }
        }

        public string Unit
        {
            get { return siIndicator.Unit; }
            set { siIndicator.Unit = value; }
        }

        public SIWithLabel()
        {
            FontChanged += SIWithLabel_FontChanged;
            InitializeComponent();
        }

        private void SIWithLabel_FontChanged(object sender, EventArgs e)
        {            
            UpperNote.Font = Font;
            Invalidate();
        }
    }
}
