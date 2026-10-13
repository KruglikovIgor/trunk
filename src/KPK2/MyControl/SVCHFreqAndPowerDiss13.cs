using System;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using KPK2.Common;
using KPK2.GeneralForms.AddForms;
using KPK2.Properties;
using Sparc.KPK2.Hardware;
using Sparc.KPK2.NFunctions;
using Res = Sparc.KPK2.Resources.Properties.Resources;

namespace KPK2.MyControl
{
    public partial class SVCHFreqAndPowerDiss13 : UserControl
    {
        private readonly Color _errorColor = Color.Red;
        private double _power = -1;
        private double _frequency = -1;
        private double _temperature = double.NaN;
        public event Action<bool> SetModChanged;

        private PowerMeter _pMeter;

        private void TryConnectToPowerMeter()
        {
            _pMeter = new PowerMeter();
            if (_pMeter.IsConnected)
                PowerMeterConnected();
            else
                StartPowerMeterMonitor();
        }

        Thread tSearchPowerMeter;
        ManualResetEvent mreStopSearchThread = new ManualResetEvent(false);

        private void StartPowerMeterMonitor()
        {
            mreStopSearchThread.Reset();
            tSearchPowerMeter = new Thread(SearchPowerMeterInThread);
            tSearchPowerMeter.Start();
        }

        private void SearchPowerMeterInThread()
        {
            while (!mreStopSearchThread.WaitOne(1))
            {
                _pMeter = new PowerMeter();
                if (_pMeter.IsConnected)
                {
                    mreStopSearchThread.Set();
                    PowerMeterConnected();
                    return;
                }
                Thread.Sleep(500);
            }
        }

        private void PowerMeterConnected()
        {
            if (InvokeRequired)
            {
                Invoke(new Action(PowerMeterConnected));
                return;
            }

            rbMvt.Checked = true;
            rbMvt.Visible = rbDBM.Visible = false;
            bMeasurePower.Visible = true;
            tbPower.Enabled = false;
            rbDBM.Enabled = false;
            rbMvt.Enabled = false;
            label1.Text = Res.Plug;
            tbAdapter.Text = CommonSettings.Default.PlugDiss13DbmKoef.ToString();
            Thread.Sleep(1500);
            CalibratePowerMeter();
        }

        private void CalibratePowerMeter()
        {
            MessageBox.Show(Resources.PrepareToCalibratePowerMeter);
            var pMessage = new fProgressMessage(Resources.PowerMeterCalibration, TimeSpan.FromSeconds(20), CalibrateOrWait);
            pMessage.ShowDialog();
            if (calibrationPMResult)
                MessageBox.Show(Resources.PowerMeterCalibrationFinished);
        }

        private bool calibrationPMResult = false;

        private void CalibrateOrWait() //Иногда бывают глюки, якобы быстрая калибровка, но по факту он долго калибруется
        {
            TimeSpan tsControl = TimeSpan.FromSeconds(15);
            Stopwatch sw = new Stopwatch();
            sw.Start();
            calibrationPMResult = false;
            int tryCount = 5;
            for (int i = 0; i < tryCount; i++)
            {
                try
                {
                    calibrationPMResult = _pMeter.CalibrateZero();
                    if (!calibrationPMResult)
                        Thread.Sleep(500);
                }
                catch (COMException)
                {
                    ReconnectPowerMeter();
                    return;
                }
                catch
                {
                    //MessageBox.Show(Resources.Error, "Попытка калибровки провалилась!");
                    calibrationPMResult = false;
                }

                if (calibrationPMResult)
                    break;
            }

            if (!calibrationPMResult)
                MessageBox.Show(string.Format(Resources.CalibrationNotCompleteAfterN, tryCount),Resources.Error);
            sw.Stop();
            TimeSpan ts = TimeSpan.FromMilliseconds(sw.ElapsedMilliseconds);
            if (ts < tsControl)
                Thread.Sleep(tsControl - ts);
        }

        private void ReconnectPowerMeter()
        {
            try
            {
                if (_pMeter != null)
                    _pMeter.Disconnect();
            }
            catch
            {

            }
            TryConnectToPowerMeter();
        }

        private void SetModChangedFunc(bool state)
        {
            if (SetModChanged != null)
                SetModChanged(state);
        }

        public double Power
        {
            get { return _power; }
        }

        public double Frequency
        {
            get { return _frequency; }
        }

        public double Temperature
        {
            get { return _temperature; }
        }

        public SVCHFreqAndPowerDiss13()
        {
            InitializeComponent();
            cbSavedKoef.Checked = true;
        }

        private void bCalculate_Click(object sender, EventArgs e)
        {
            //CalcFreq();
            CalcPower();
        }

        private static double GetTextBoxValue(TextBox tb)
        {
            if (String.IsNullOrEmpty(tb.Text))
                return 0;
            return Functions.MoveToDouble(tb.Text);
        }

        private void CalcPower()
        {
            if (!CheckTextBoxValue(tbDiss) || !CheckTextBoxValue(tbKVP) || !CheckTextBoxValue(tbWire) || !CheckTextBoxValue(tbPower) || !CheckTextBoxValue(tbAdapter))
            {
                SetPowerValue(double.NaN);
                return;
            }
            double koef = Math.Abs(GetTextBoxValue(tbDiss)) + Math.Abs(GetTextBoxValue(tbKVP)) + Math.Abs(GetTextBoxValue(tbWire)) + Math.Abs(GetTextBoxValue(tbAdapter)); //Независимо от знака это всё равно затухание!
            if (String.IsNullOrEmpty(tbPower.Text))
                return;
            double power = GetTextBoxValue(tbPower);
            if (rbMvt.Checked && power < 0)
            {
                MessageBox.Show(
                    Resources.ErrorNumber + " " + Res.NumberCantBeNegative);
                return;
            }
            if (rbDBM.Checked)
                power = Math.Pow(10, power / 10);
            power = power * Math.Pow(10, koef / 10) / 1000;
            power = Math.Round(power, 2);
            SetPowerValue(power);
        }

        private void SetPowerValue(double value)
        {
            if (double.IsNaN(value))
                lPowerResult.Text = Res.NotDefined;
            else
            {
                _power = value;
                lPowerResult.Text = value.ToString();
            }
        }

        private void CalcFreq()
        {
            if (!CheckTextBoxValue(tbFreq) || !CheckTextBoxValue(tbTemperature))
            {
                SetFreqValue(double.NaN);
                return;
            }
            if (String.IsNullOrEmpty(tbFreq.Text) || String.IsNullOrEmpty(tbTemperature.Text))
                return;
            _temperature = Functions.MoveToDouble(tbTemperature.Text);
            double freq = Math.Round(Functions.MoveToDouble(tbFreq.Text), 1);
            SetFreqValue(freq);
        }

        private static bool CheckTextBoxValue(TextBox tb)
        {
            double result;
            if (!String.IsNullOrEmpty(tb.Text))
            {
                result = Functions.MoveToDouble(tb.Text);
                return !double.IsNaN(result);
            }
            return true;
        }

        private static bool CheckTextBoxForValue(TextBox tb)
        {
            if (String.IsNullOrEmpty(tb.Text.Trim()))
                return false;
            return !double.IsNaN(Functions.MoveToDouble(tb.Text));
        }

        private void SetFreqValue(double freq)
        {
            if (double.IsNaN(freq))
                lFreqResult.Text = Res.NotDefined;
            else
            {
                _frequency = freq;
                lFreqResult.Text = freq.ToString();
                lFreqResult.ForeColor = Functions.CheckFrequencyDiss13(freq, Temperature) ? Color.Black : _errorColor;
            }
        }

        private void tb_TextChanged(object sender, EventArgs e)
        {
            CheckPowerButton();
        }

        private void tb_Leave(object sender, EventArgs e)
        {
            CheckFrequencyButton();
            TextBox tbCurrent = (TextBox)sender;
            tbCurrent.Text = tbCurrent.Text.Trim();
            if (String.IsNullOrEmpty(tbCurrent.Text))
                return;
            if (!CheckTextBoxValue(tbCurrent))
            {
                MessageBox.Show(string.Format(Resources.ErrorInputNValue, tbCurrent.Text));
                tbCurrent.Focus();
            }
        }

        private void CheckPowerButton()
        {
            bCalculatePower.Enabled = CheckTextBoxForValue(tbPower) && CheckTextBoxForValue(tbDiss) && CheckTextBoxForValue(tbKVP) && CheckTextBoxForValue(tbWire) && CheckTextBoxForValue(tbAdapter);
        }

        private void CheckFrequencyButton()
        {
            bCalculateFrequency.Enabled = CheckTextBoxForValue(tbFreq) && CheckTextBoxValue(tbTemperature);
        }

        private void rbFrequency_CheckedChanged(object sender, EventArgs e)
        {
            gbFrequency.Enabled = rbFrequency.Checked;
            gbPower.Enabled = rbPower.Checked;
            SetModChangedFunc(!rbFrequency.Checked);
        }

        private void bCalculateFrequency_Click(object sender, EventArgs e)
        {
            CalcFreq();

        }

        private void rbPower_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void cbSavedKoef_CheckedChanged(object sender, EventArgs e)
        {
            tbKVP.ReadOnly = cbSavedKoef.Checked;
            tbWire.ReadOnly = cbSavedKoef.Checked;
            tbAdapter.ReadOnly = cbSavedKoef.Checked;
            if (cbSavedKoef.Checked)
            {
                tbKVP.Text = CommonSettings.Default.KVPDiss13DbmKoef.ToString();
                tbWire.Text = CommonSettings.Default.CabelDbmKoef.ToString();
                tbAdapter.Text = CommonSettings.Default.AdapterDbmKoef.ToString();
            }
            CheckPowerButton();
        }

        private void SVCHFreqAndPowerDiss13_Load(object sender, EventArgs e)
        {
            TryConnectToPowerMeter();
        }

        private void bMeasurePower_Click(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(tbFreq.Text.Trim()))
                _pMeter.SetFrequency((uint)(Functions.MoveToDouble(tbFreq.Text) * 1000));
            RenewPower();
            bCalculatePower.PerformClick();
        }

        private void RenewPower()
        {
            tbPower.Text = (Math.Round(Math.Abs(_pMeter.MeasurePowerW()) * 1000, 4)).ToString();
        }

        private void rbMvt_CheckedChanged(object sender, EventArgs e)
        {
            lPower.Text = "P, " + Res.miliWatt;
        }

        private void rbDBM_CheckedChanged(object sender, EventArgs e)
        {
            lPower.Text = "P, dBm";
        }

        public void StopSearchPMonitor()
        {
            mreStopSearchThread.Set();
            if (_pMeter != null)
            {
                _pMeter.Disconnect();
                _pMeter = null;
            }
        }
    }
}
