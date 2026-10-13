using System;
using System.Windows.Forms;

namespace KPK2.MyControl
{
    public partial class ZeroVoltage : UserControl
    {
        public ZeroVoltage()
        {
            InitializeComponent();
        }

        private void ZeroVoltage_EnabledChanged(object sender, EventArgs e)
        {
          if (!Enabled)
            siZero1.Inactive = siZero2.Inactive = siZero3.Inactive = true;
        }
    }
}
