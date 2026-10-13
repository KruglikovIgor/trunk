using System;
using System.Windows.Forms;

namespace KPK2.GeneralForms
{
    public partial class frmProgress : Form
    {
        public bool Result;
        public delegate bool ProgressTh(DoStep callback);
        public ProgressTh Process;
        public delegate void DoStep(int step, string operation);

        public frmProgress()
        {
            InitializeComponent();
        }
        
        public void StepIt(int step, string operation)
        {            
            if (InvokeRequired)
            {
                DoStep d = StepIt;
                Invoke(d, step, operation);
                return;
            }
            progressBar1.Value += step;
            if (!String.IsNullOrEmpty(operation))
                label1.Text = operation;
            Application.DoEvents();
        }

        public void SetMaximum(int max)
        {
            progressBar1.Maximum = max;
        }        
        
        private void LetsClose(bool result)
        {
            if (InvokeRequired)
            {                
                Invoke(new Action<bool>(LetsClose), result);
                return;
            }
            Result = result;
            Close();
        }

        private void EndProgress (IAsyncResult result)
        {
            LetsClose(Process.EndInvoke(result));
        }

        private void Progress_Shown(object sender, EventArgs e)
        {
            if (Process != null)
                Process.BeginInvoke(StepIt,EndProgress,null);
        }
    }
}