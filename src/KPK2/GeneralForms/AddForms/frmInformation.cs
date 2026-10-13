using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace KPK2.GeneralForms.AddForms
{
    public partial class frmInformation : Form
    {
        Timer tmrWait = new Timer();
        private bool _ready = false;

        public frmInformation(string opText, TimeSpan tsWait)
        {
            InitializeComponent();
            lOperationText.Text = opText;
            tmrWait.Interval = (int)tsWait.TotalMilliseconds;
            tmrWait.Tick += TmrWaitOnTick;
        }

        private void TmrWaitOnTick(object sender, EventArgs eventArgs)
        {
            tmrWait.Enabled = false;
            _ready = true;
            Close();
        }

        private void frmInformation_Shown(object sender, EventArgs e)
        {
            tmrWait.Enabled = true;
        }

        private void frmInformation_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!_ready)
                e.Cancel = true;
        }


    }
}
