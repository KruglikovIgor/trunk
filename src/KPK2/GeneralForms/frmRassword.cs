using System;
using System.Windows.Forms;

namespace KPK2.GeneralForms 
{
    /// <summary>
    /// Ввод пароля для доступа к "Аттестации" 
    /// </summary>
    public partial class frmRassword : Form 
    {
        public frmRassword() 
        {
            InitializeComponent();
        }

        private void btn_Ok_Click(object sender, EventArgs e) 
        {
            // строка пароля
            string sss = KPK2.Properties.Settings.Default.Password_Attestation;
            frmMain.isOpenAttestion = sss.Equals(txtBox.Text) ? true : false;
            Close();
        }

        private void btn_Cancel_Click(object sender, EventArgs e) 
        {
            frmMain.isOpenAttestion = false;
            Close();
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData) 
        {
            if (keyData == Keys.Escape)
            {
                frmMain.isOpenAttestion = false;
                this.Close();
            }
            if (keyData == Keys.Enter)
                btn_Ok.PerformClick();

            return base.ProcessCmdKey(ref msg, keyData);
        }
    }
}