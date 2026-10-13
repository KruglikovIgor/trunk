using System;
using System.Windows.Forms;
using Sparc.KPK2.Algorithms.Diss3228M;
using Sparc.KPK2.NFunctions;

namespace KPK2.MyControl
{
    public partial class MeasFreqDiss3228M : UserControl
    {
        private readonly AlgorithmDiss3228M _algorithm;
        private readonly TimeSpan _tsWaitBeforeMeas = Globals.Emulation ? TimeSpan.FromSeconds(3) : TimeSpan.FromMinutes(3);
        private bool _measFreq1_3;
        private DateTime _startTime;
        private DateTime _stopTime;

        public bool Freq1_3_Measured = false;
        public bool Freq2_Measured = false;

        public double Freq1_3
        {
            get { return Functions.MoveToDouble(tbFreq1_3.Text); }
        }

        public double Freq2
        {
            get { return Functions.MoveToDouble(tbFreq2.Text); }
        }

        public MeasFreqDiss3228M(AlgorithmDiss3228M algorithm)
        {
            InitializeComponent();

            _algorithm = algorithm;
        }

        private void bMeasFreq1_3_Click(object sender, EventArgs e)
        {
            ResetAllStates();
            _measFreq1_3 = true;
            _algorithm.SetMeasChannel(0); // Каналы 1-3
            StartTimer(_tsWaitBeforeMeas);
        }

        private void bMeasFreq2_Click(object sender, EventArgs e)
        {
            ResetAllStates();
            _measFreq1_3 = false;
            _algorithm.SetMeasChannel(1); // Канал 2
            StartTimer(_tsWaitBeforeMeas);
        }

        private void ResetAllStates()
        {
            tbFreq1_3.ReadOnly = tbFreq2.ReadOnly = true;
            bMeasFreq1_3.Enabled = bMeasFreq2.Enabled = false;
        }

        private Timer _tTimer;
        private object _lockObject = new object();

        public void StartTimer(TimeSpan time)
        {
            lock (_lockObject)
            {
                _tTimer = new Timer();
                _tTimer.Interval = 1000;
                _tTimer.Tick += TTimerOnTick;
                _startTime = DateTime.Now;
                _stopTime = _startTime + time;
                siTimer.ValueString = string.Format("{0:00}:{1:00}", time.Minutes, time.Seconds);
                siTimer.Visible = lTimerText.Visible = true;
                _tTimer.Enabled = true;
            }
        }

        private void TTimerOnTick(object sender, EventArgs e)
        {
            TimeSpan timeLeft = _stopTime - DateTime.Now;
            if (_stopTime <= DateTime.Now)
            {
                _tTimer.Enabled = false;
                StopTimer();
            }

            siTimer.ValueString = string.Format("{0:00}:{1:00}", timeLeft.Minutes, timeLeft.Seconds);
        }

        private void StopTimer()
        {
            tbFreq1_3.ReadOnly = !_measFreq1_3;
            tbFreq2.ReadOnly = _measFreq1_3;
            if (_measFreq1_3)
                Freq1_3_Measured = true;
            else
                Freq2_Measured = true;

            bMeasFreq1_3.Enabled = !_measFreq1_3;
            bMeasFreq2.Enabled = _measFreq1_3;
        }
    }
}
