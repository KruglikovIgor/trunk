using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Timer = System.Windows.Forms.Timer;

namespace KPK2.GeneralForms.AddForms
{
    public partial class fProgressMessage : Form
    {
        private TimeSpan _expectedTime;
        private Timer _tmr = new Timer();
        private Action _act;
        private Task _tsk;

        public fProgressMessage(string message, TimeSpan expectedTime, Action act)
        {
            InitializeComponent();
            lMessage.Text = message;
            _expectedTime = expectedTime;
            _tmr.Interval = 1000;
            _tmr.Tick += TmrOnTick;
            _act = act;
        }

        private void TmrOnTick(object sender, EventArgs e)
        {
            if (pbProgress.Value == pbProgress.Maximum)
            {
                return;
            }
            pbProgress.PerformStep();
        }

        private void fProgressMessage_Shown(object sender, EventArgs e)
        {
            pbProgress.Maximum = (int)_expectedTime.TotalSeconds;
            _tsk = new Task(_act);
            _tsk.ContinueWith(t => EndOfCalibration());
            _tmr.Start();
            _tsk.Start();
        }

        private void EndOfCalibration()
        {
            if (InvokeRequired)
            {
                Invoke(new Action(EndOfCalibration));
                return;
            }
            _tmr.Stop();
            Close();
        }
    }
}
