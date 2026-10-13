using System;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;
using KPK2.Common;
using KPK2.GeneralForms.AddForms;
using KPK2.GeneralForms.DissForms;
using KPK2.Properties;
using NLog;
using Sparc.KPK2.Hardware;
using Sparc.KPK2.NFunctions;
using KPK2.Interfaces;

namespace KPK2.MyControl
{
    public partial class SVChFreqAndPower : UserControl
    {
        private Logger _logger = LogManager.GetCurrentClassLogger();

        private double MaxFreq = 13335;
        private double MinFreq = 13327;
        private const double NominalTemperature = 50;

        private IFrmWork15Base _frm;
        private double freq;
        private double power;

        private PowerMeter _pMeter;

        public SVChFreqAndPower(IFrmWork15Base frm)
        {
            InitializeComponent();
            _frm = frm;
            cbSavedKoef.Checked = true;
            DataManager.Instance.GetParamLimitAndId(ManualFreqParam, ref MinFreq, ref MaxFreq);
            DataManager.Instance.GetParamLimitAndId(ManualPowerParam, ref MinPower, ref MaxPower);
            txtBox_Temp.Text = NominalTemperature.ToString();
        }

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

            rbtn_mBt.Checked = true;
            rbtn_dBm.Visible = rbtn_mBt.Visible = false;
            label55.Visible = tbCabel.Visible = false;
            bMeasurePower.Visible = true;
            tbPower.Enabled = false;
            rbtn_dBm.Enabled = false;
            rbtn_mBt.Enabled = false;
            CommonSettings.Default.CabelDiss15DbmKoef = 0;
            CommonSettings.Default.Save();
            tbCabel.Text = CommonSettings.Default.CabelDiss15DbmKoef.ToString();
            CalibratePowerMeter();
        }

        private void txtBox_TextChanged(object sender, EventArgs e)
        {
            ((TextBox)sender).Text = ((TextBox)sender).Text.Trim();
        }

        private void rbtn_mBt_CheckedChanged(object sender, EventArgs e)
        {
            lPower.Text = String.Format(Resources.PowerN, rbtn_mBt.Checked ? Resources.miliWatt : Resources.dBm);
            tbPower.Text = "";
        }

        private void CalibratePowerMeter()
        {
            var pMessage = new fProgressMessage(Resources.PowerMeterCalibration, TimeSpan.FromSeconds(20), () => _pMeter.CalibrateZero());
            pMessage.ShowDialog();
            MessageBox.Show(Resources.PowerMeterCalibrationFinished);
        }

        private void btn_CalcFreq_Click(object sender, EventArgs e)
        {
            CalcFreq();
            CalcPower();
        }

        private void CalcFreq()
        {
            freq = 0;
            double t1 = NominalTemperature; // для температуры 
            bool bl = false;
            if (txtBox_Freq.Text == "") // нет частоты - делать нечего 
            {
                lbl_Freq.Visible = lbl_Result.Visible = false;
                lbl_Result.Text = "";
                return;
            }
            if (txtBox_Temp.Text == "") // нет температуры 
            {
                try
                {
                    freq = Math.Round(Functions.MoveToDouble(txtBox_Freq.Text), 1);
                }
                catch
                {
                    bl = true;
                    lbl_Freq.Visible = lbl_Result.Visible = false;
                    txtBox_Freq.Text = txtBox_Temp.Text = lbl_Result.Text = "";
                }
                if (bl) return;
                lbl_Freq.Visible = lbl_Result.Visible = true;
                lbl_Result.Text = freq.ToString();
                lbl_Result.ForeColor = MinFreq <= freq && freq <= MaxFreq ? Color.Green : Color.Red;
                return;
            }
            try
            {
                t1 = Functions.MoveToDouble(txtBox_Temp.Text);
                freq = Functions.MoveToDouble(txtBox_Freq.Text);
            }
            catch
            {
                bl = true;
                lbl_Freq.Visible = lbl_Result.Visible = false;
                txtBox_Freq.Text = txtBox_Temp.Text = lbl_Result.Text = "";
            }
            if (bl)
                return;
            lbl_Freq.Visible = lbl_Result.Visible = true;
            freq = Math.Round(freq + 0.28 * (t1 - 50));
            lbl_Result.Text = freq.ToString();
            lbl_Result.ForeColor = MinFreq <= freq && freq <= MaxFreq ? Color.Green : Color.Red;
        }

        private void CalcPower()
        {
            power = 0;
            bool bl = false;
            // не всё ввели - ушли 
            if (tbPower.Text == "" || txtBox_Diss.Text == "" || tbVP.Text == "" ||
                tbKVP.Text == "" || tbAttenuator.Text == "" || tbCabel.Text == "")
            {
                lbl_Power.Visible = lbl_Pow_Res.Visible = false;
                lbl_Pow_Res.Text = "";
                return;
            }
            double[] arKoefPower = new double[] { 0, 0, 0, 0, 0 };
            try
            {
                power = Functions.MoveToDouble(tbPower.Text);
                arKoefPower[0] = Functions.MoveToDouble(txtBox_Diss.Text);
                arKoefPower[1] = Functions.MoveToDouble(tbVP.Text);
                arKoefPower[2] = Functions.MoveToDouble(tbKVP.Text);
                arKoefPower[3] = Functions.MoveToDouble(tbAttenuator.Text);
                arKoefPower[4] = Functions.MoveToDouble(tbCabel.Text);
                for (int i = 0; i < 5; i++)
                    arKoefPower[i] = Math.Abs(arKoefPower[i]);
            }
            catch // ввели ерунду, не числа 
            {
                lbl_Power.Visible = lbl_Pow_Res.Visible = false;
                lbl_Pow_Res.Text = tbPower.Text = txtBox_Diss.Text =
                                                  tbVP.Text = tbKVP.Text = tbAttenuator.Text =
                                                                                     tbCabel.Text = "";
                bl = true;
            }
            if (bl)
                return;
            lbl_Power.Visible = lbl_Pow_Res.Visible = true;
            double fallDbm = (arKoefPower[0] + arKoefPower[1] + arKoefPower[2] +
                        arKoefPower[3] + arKoefPower[4]);
            if (rbtn_dBm.Checked) // измеряем в dBm 
                power = Math.Pow(10.0, power / 10);
            power = power * Math.Pow(10.0, fallDbm / 10.0) / 1000.0;
            power = Math.Round(power, 2);
            lbl_Pow_Res.Text = power.ToString();
            lbl_Pow_Res.ForeColor = MinPower <= power && power <= MaxPower ? Color.Green : Color.Red;
        }

        private double MinPower = 2;
        private double MaxPower = 10;

        private void SVChFreqAndPower_Load(object sender, EventArgs e)
        {
            TryConnectToPowerMeter();
            LoadKoef();
            tbPower.Text = txtBox_Freq.Text = txtBox_Temp.Text = "";
            txtBox_Freq.Focus();
            txtBox_Diss.Text = "12";
            lbl_Freq.Visible = lbl_Result.Visible = lbl_Power.Visible = lbl_Pow_Res.Visible = false;
            lbl_Result.Text = lbl_Pow_Res.Text = "";

            var savedFreq = DataManager.Instance.GetParameterValue(_frm.TestId, ManualFreqParam);
            if (!double.IsNaN(savedFreq))
                txtBox_Freq.Text = savedFreq.ToString();
        }

        /// <summary>
        /// Загрузка коэф. для расчёта из Settings
        /// </summary>
        private void LoadKoef()
        {
            tbVP.Text = CommonSettings.Default.VPDbmKoef.ToString();
            tbKVP.Text = CommonSettings.Default.KVPDiss15DbmKoef.ToString();
            tbAttenuator.Text = CommonSettings.Default.AttenDbmKoef.ToString();
            tbCabel.Text = CommonSettings.Default.CabelDiss15DbmKoef.ToString();
        }

        public void Save()
        {
            btn_CalcFreq.PerformClick();

            if (lbl_Result.Visible) // расчитана частота магнетрона 
                _frm.ManualRecordDouble(ManualFreqParam, freq);
            if (lbl_Pow_Res.Visible) // расчитана мощность магнетрона 
                _frm.ManualRecordDouble(ManualPowerParam, power);
        }

        private const string ManualFreqParam = "Manual_TKM_Freq";
        private const string ManualPowerParam = "Manual_TKM_Power";

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
            if (result < 0 && rbtn_mBt.Checked)
                error = true;
            if (error)
            {
                MessageBox.Show(Resources.ErrorNumber + " " + Sparc.KPK2.Resources.Properties.Resources.NumberCantBeNegative);
                current.Focus();
            }
            else
            {
                if (String.IsNullOrEmpty(txtBox_Temp.Text.Trim()))
                    txtBox_Temp.Text = NominalTemperature.ToString();
            }
        }

        private void cbSavedKoef_CheckedChanged(object sender, EventArgs e)
        {

            tbKVP.ReadOnly = cbSavedKoef.Checked;
            tbVP.ReadOnly = cbSavedKoef.Checked;
            tbAttenuator.ReadOnly = cbSavedKoef.Checked;
            tbCabel.ReadOnly = cbSavedKoef.Checked;
            if (cbSavedKoef.Checked)
            {
                tbKVP.Text = CommonSettings.Default.KVPDiss15DbmKoef.ToString();
                tbVP.Text = CommonSettings.Default.CabelDiss15DbmKoef.ToString();
                tbAttenuator.Text = CommonSettings.Default.AttenDbmKoef.ToString();
                tbCabel.Text = CommonSettings.Default.CabelDiss15DbmKoef.ToString();
            }
        }

        private void bMeasurePower_Click(object sender, EventArgs e)
        {
            const int tryMeasureCount = 3;
            //Т.к. часто бывают ошибки на измерителе мощности, видимо связанные с USB, сделал несколько попыток измерения
            for (int i = 0; i < tryMeasureCount; i++)
            {
                try
                {
                    TryMeasurePower();
                    break;
                }
                catch (Exception ex)
                {
                    if (i < tryMeasureCount - 1)
                        continue;
                    _logger.Log(LogLevel.Error, ex);
                    MessageBox.Show(Resources.PowerMeterError);
                }
            }

            btn_CalcFreq.PerformClick();
        }

        private void TryMeasurePower()
        {
            if (!String.IsNullOrEmpty(txtBox_Freq.Text.Trim()))
                _pMeter.SetFrequency((uint)Functions.MoveToDouble(txtBox_Freq.Text));
            RenewPower();
        }

        private void RenewPower()
        {
            tbPower.Text = (Math.Round(_pMeter.MeasurePowerW() * 1000, 4)).ToString();
        }

        public void Stop()
        {
            mreStopSearchThread.Set();
            try
            {
                if (_pMeter != null)
                {
                    if (_pMeter.IsConnected)
                        _pMeter.Disconnect();
                    _pMeter = null;
                }
            }
            catch(Exception ex)
            {
                _logger.Log(LogLevel.Error, ex);
            }
        }
    }
}
