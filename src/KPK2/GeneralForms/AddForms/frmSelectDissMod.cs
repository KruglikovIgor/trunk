using System.Collections.Generic;
using System.Windows.Forms;
using Sparc.KPK2.NFunctions.Enums;

namespace KPK2.GeneralForms.AddForms
{
    public partial class frmSelectDissMod : Form
    {
        public Modification SelectedModification;
        public bool Canceled = false;
        private List<string> LIncoming = new List<string>();

        public frmSelectDissMod()
        {
            InitializeComponent();
        }

        public frmSelectDissMod(string[] mods)
        {
            InitializeComponent();
            LIncoming.AddRange(mods);
        }

        private void frmSelectDissMod_Load(object sender, System.EventArgs e)
        {
            btnOk.DialogResult = DialogResult.OK;
            btnCancel.DialogResult = DialogResult.Cancel;
            lbDissMods.Items.AddRange(LIncoming.ToArray());
            lbDissMods.SelectedIndex = 0;
        }

        private void btnOk_Click(object sender, System.EventArgs e)
        {
            if (lbDissMods.SelectedIndex != -1)
            {
                var selectedMod = lbDissMods.SelectedItem.ToString();
                switch (selectedMod)
                {
                    case "ДИСС-15Г":
                        SelectedModification = Modification.Diss15G;
                        break;
                    //case "ДИСС-15Д":
                    //    SelectedModification = Modification.Diss15D;
                    //    break;
                }

                Canceled = false;
            }
            Close();
        }

        private void btnCancel_Click(object sender, System.EventArgs e)
        {
            Canceled = true;
            Close();
        }
    }
}
