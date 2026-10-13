using System;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;
using KPK2.Common;
using KPK2.GeneralForms.AddForms;
using KPK2.MyControl.DISS.FrmDiss32;
using KPK2.Properties;
using Sparc.KPK2.Hardware;
using Sparc.KPK2.NFunctions;
using KPK2.Interfaces;

namespace KPK2.MyControl
{
    public partial class SVChFreqAndPowerDiss32 : UserControl
    {
        private const string ManualFreqParam = "Mnl_F_32_WorkTest";
        private const string ManualPowerParam = "Power_Mnl_F_32";

        private double MaxFreq = 13332.5;
        private double MinFreq = 13325;
        private const double NominalTemperature = 50;

        private double MinPower = 0.1;
        private double MaxPower = 10;

        private IUICheckingControl _frm;
        private double freq;
        private double power;
        private double _calcPower;
        private int _testId;

        private PowerMeter _pMeter;

        private Thread tSearchPowerMeter;
        private ManualResetEvent mreStopSearchThread = new ManualResetEvent(false);

        public SVChFreqAndPowerDiss32(int testId, IUICheckingControl frm)
        {
            InitializeComponent();
            _frm = frm;
            _testId = testId;
            cbSavedKoef.Checked = true;
            DataManager.Instance.GetParamLimitAndId(ManualFreqParam, ref MinFreq, ref MaxFreq);
            DataManager.Instance.GetParamLimitAndId(ManualPowerParam, ref MinPower, ref MaxPower);
        }

        public void Close()
        {
            if(_pMeter.IsConnected)
                _pMeter.Disconnect();
        }

        private void TryConnectToPowerMeter()
        {
            _pMeter = new PowerMeter();
            if (_pMeter.IsConnected)
                PowerMeterConnected();
            else
                StartPowerMeterMonitor();
        }

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
                    PowerMeterConnected();
                    return;
                }
                Thread.Sleep(150);
            }
        }

        private void PowerMeterConnected()
        {
            if (InvokeRequired)
            {
                Invoke(new Action(PowerMeterConnected));
                return;
            }

            bMeasurePower.Enabled = true;
            CommonSettings.Default.CabelDiss15DbmKoef = 0;
            CommonSettings.Default.Save();
            CalibratePowerMeter();
        }

        private void txtBox_TextChanged(object sender, EventArgs e)
        {
            ((TextBox)sender).Text = ((TextBox)sender).Text.Trim();
        }

        private void CalibratePowerMeter()
        {
            var pMessage = new fProgressMessage(Resources.PowerMeterCalibration, TimeSpan.FromSeconds(20), () =>
            {
                Thread.Sleep(4000);
                _pMeter.CalibrateZero();
                });
            pMessage.ShowDialog();
            //MessageBox.Show(Resources.PowerMeterCalibrationFinished);
        }

        private void CalcFreq()
        {
            freq = 0;
            bool bl = false;
            if (txtBox_Freq.Text == "") // нет частоты - делать нечего 
                return;
            try
            {
                freq = Math.Round(Functions.MoveToDouble(txtBox_Freq.Text), 2);
            }
            catch
            {
                bl = true;
                txtBox_Freq.Text = "";
            }
            if (bl) 
                return;
            txtBox_Freq.ForeColor = MinFreq <= freq && freq <= MaxFreq ? Color.Green : Color.Red;
            return;
        }

        private void CalcPower()
        {
            power = 0;
            bool bl = false;
            // не всё ввели - ушли 
            if (txtBox_Diss.Text == "" || tbVP.Text == "" || tbKVP.Text == "")
                return;

            double[] arKoefPower = new double[] { 0, 0, 0};
            try
            {
                power = _calcPower;
                arKoefPower[0] = Functions.MoveToDouble(txtBox_Diss.Text);
                arKoefPower[1] = Functions.MoveToDouble(tbVP.Text);
                arKoefPower[2] = Functions.MoveToDouble(tbKVP.Text);
                for (int i = 0; i < 3; i++)
                    arKoefPower[i] = Math.Abs(arKoefPower[i]);
            }
            catch // ввели ерунду, не числа 
            {
                tbPower.Text = txtBox_Diss.Text = tbVP.Text = tbKVP.Text = "";
                bl = true;
            }
            if (bl)
                return;
            double fallDbm = (arKoefPower[0] + arKoefPower[1] + arKoefPower[2]);
            power = power * Math.Pow(10.0, fallDbm / 10.0) / 1000.0;
            power = Math.Round(power * 1000, 2);
            tbPower.Text = power.ToString();
            tbPower.ForeColor = MinPower <= power && power <= MaxPower ? Color.Green : Color.Red;
        }

        private void SVChFreqAndPower_Load(object sender, EventArgs e)
        {
            TryConnectToPowerMeter();
            LoadKoef();
            tbPower.Text = txtBox_Freq.Text = "";
            txtBox_Freq.Focus();
            txtBox_Diss.Text = "19,7";

            var savedFreq = DataManager.Instance.GetParameterValue(_testId, ManualFreqParam);
            if (!double.IsNaN(savedFreq))
                txtBox_Freq.Text = savedFreq.ToString();
        }

        /// <summary>
        /// Загрузка коэф. для расчёта из Settings
        /// </summary>
        private void LoadKoef()
        {
            tbVP.Text = CommonSettings.Default.VPDbmKoef.ToString();
            tbKVP.Text = CommonSettings.Default.KVPDiss32DbmKoef.ToString();
        }

        public void Save()
        {

            if (txtBox_Freq.Text != "" && double.TryParse(txtBox_Freq.Text, out freq)) // расчитана частота магнетрона 
                _frm.ManualRecordDouble(ManualFreqParam, freq);
            if (tbPower.Text != "" && double.TryParse(tbPower.Text, out power)) // расчитана мощность магнетрона 
                _frm.ManualRecordDouble(ManualPowerParam, power);
        }


        private void grpBox_Freq_EnabledChanged(object sender, EventArgs e)
        {
            if (!Enabled)
                Save();
        }

        private void txtBox_Leave(object sender, EventArgs e)
        {
            TextBox current = (TextBox)sender;
            if (String.IsNullOrEmpty(current.Text.Trim()))
                return;
            double result = Functions.MoveToDouble(current.Text);
            bool error = double.IsNaN(result);
            if (result < 0)
                error = true;
            if (error)
            {
                MessageBox.Show(Resources.ErrorNumber + " " + Sparc.KPK2.Resources.Properties.Resources.NumberCantBeNegative);
                current.Focus();
                return;
            }
            CalcFreq();
        }

        private void cbSavedKoef_CheckedChanged(object sender, EventArgs e)
        {

            tbKVP.ReadOnly = cbSavedKoef.Checked;
            tbVP.ReadOnly = cbSavedKoef.Checked;
            if (cbSavedKoef.Checked)
            {
                tbKVP.Text = CommonSettings.Default.KVPDiss32DbmKoef.ToString();
                tbVP.Text = CommonSettings.Default.VPDbmKoef.ToString();
            }
        }

        private void bMeasurePower_Click(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(txtBox_Freq.Text.Trim()))
                _pMeter.SetFrequency((uint) Functions.MoveToDouble(txtBox_Freq.Text));
            RenewPower();
            CalcFreq();
            CalcPower();
        }

        private void RenewPower()
        {
            Thread.Sleep(1000);
            _calcPower = Math.Round(_pMeter.MeasurePowerW() * 1000, 4);
        }

        public void Stop()
        {
            mreStopSearchThread.Set();
        }

        private void tbPower_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = true;
        }
    }
}
