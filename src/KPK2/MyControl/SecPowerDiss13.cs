using System;
using System.Drawing;
using System.Windows.Forms;

namespace KPK2.MyControl
{
    public partial class SecPowerDiss13 : UserControl
    {
        public SecPowerDiss13()
        {
            InitializeComponent();
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void SecPowerDiss13_Load(object sender, EventArgs e)
        {
            si60V.LightColor = siMinus12.LightColor = siPlus12.LightColor = siMinus27.LightColor = Color.Lime;
        }
    }
}
