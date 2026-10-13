using System;
using System.Drawing;
using System.Windows.Forms;
using Sparc.KPK2.NFunctions;

namespace KPK2.MyControl
{
    public partial class IndicationDiss3228M : UserControl
    {
        public IndicationDiss3228M()
        {
            InitializeComponent();
        }

        private bool? _indicationWithCtrlVCh = null;

        public bool? IndicationWithCtrlVCh
        {
            get
            {
                return _indicationWithCtrlVCh;
            }
            set
            {
                _indicationWithCtrlVCh = value;
                if (value == true)
                {
                    cbIndicationWithCtrlVCh.Checked = true;
                    cbIndicationWithCtrlVCh.ForeColor = Globals.SuccessColorText;
                }

                if (value == false)
                {
                    cbIndicationWithCtrlVCh.Checked = false;
                    cbIndicationWithCtrlVCh.ForeColor = Globals.FailColor;
                }

                if (value == null)
                {
                    cbIndicationWithCtrlVCh.Checked = false;
                    cbIndicationWithCtrlVCh.ForeColor = SystemColors.ActiveCaption;
                }
            }
        }

        private bool? _indicationWithoutCtrlVCh = null;

        public bool? IndicationWithoutCtrlVCh
        {
            get
            {
                return _indicationWithoutCtrlVCh;
            }
            set
            {
                _indicationWithoutCtrlVCh = value;
                if (value == true)
                {
                    cbIndicationWithoutCtrlVCh.Checked = true;
                    cbIndicationWithoutCtrlVCh.ForeColor = Globals.SuccessColorText;
                }

                if (value == false)
                {
                    cbIndicationWithoutCtrlVCh.Checked = false;
                    cbIndicationWithoutCtrlVCh.ForeColor = Globals.FailColor;
                }

                if (value == null)
                {
                    cbIndicationWithoutCtrlVCh.Checked = false;
                    cbIndicationWithoutCtrlVCh.ForeColor = SystemColors.ActiveCaption;
                }
            }
        }

        private DateTime _startTime;
        private DateTime _stopTime;
        private TimeSpan timeInterval;
        private Timer _tTimer;
        private object _lockObject = new object();

        public void StartTimer(TimeSpan time)
        {
            lock (_lockObject)
            {
                
                timeInterval = time;
                _tTimer = new Timer();
                _tTimer.Interval = 1000;
                _tTimer.Tick += TTimerOnTick;
                _startTime = DateTime.Now;
                _stopTime = _startTime + timeInterval;
                siTimer.ValueString = string.Format("{0:00}:{1:00}", timeInterval.Minutes, timeInterval.Seconds);
                siTimer.Visible = lTimerText.Visible = true;
                _tTimer.Enabled = true;
            }
        }

        private void TTimerOnTick(object sender, EventArgs e)
        {
            TimeSpan timeLeft = _stopTime - DateTime.Now;
            if (_stopTime <= DateTime.Now)
                _tTimer.Enabled = false;
            siTimer.ValueString = string.Format("{0:00}:{1:00}", timeLeft.Minutes, timeLeft.Seconds);
        }

        public bool IsTimerOver()
        {
            lock (_lockObject)
            {
                return !_tTimer.Enabled;
            }
        }

        public void StopTimer()
        {
            _tTimer.Enabled = false;
            siTimer.Visible = lTimerText.Visible = false;
        }

        private void IndicationDiss3228M_Load(object sender, EventArgs e)
        {

        }
    }
}
