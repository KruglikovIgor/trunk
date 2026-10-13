using System;
using System.Globalization;
using System.Windows.Forms;
using KPK2.Properties;
using Res = Sparc.KPK2.Resources.Properties.Resources;

namespace KPK2.GeneralForms.ServiceForm
{
    public partial class FrmGenSinSettings : Form
    {
        public FrmGenSinSettings()
        {
            InitializeComponent();
        }

        private void FrmGenSinSettings_Load(object sender, EventArgs e)
        {
            //tbLastByte.Text = Settings.Default.GenSinCosLastByte.ToString();
        }

        private void bSave_Click(object sender, EventArgs e)
        {
            byte lastByte;
            if (Byte.TryParse(tbLastByte.Text, NumberStyles.Any, null, out lastByte) || Byte.TryParse(tbLastByte.Text, NumberStyles.AllowHexSpecifier, null, out lastByte))
            {
                //Settings.Default.GenSinCosLastByte = lastByte;
                Settings.Default.Save();
                Close();
            }
            else
            {
                MessageBox.Show(string.Format(Res.NeedEnterIntervalValue,0,255));
                tbLastByte.Focus();
            }
        }

        private void bCancel_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}