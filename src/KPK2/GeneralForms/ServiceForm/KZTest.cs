using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using Sparc.KPK2.Hardware;
using USBVoltmeter;
using Res = Sparc.KPK2.Resources.Properties.Resources;

namespace KPK2.GeneralForms.ServiceForm
{
    public partial class KZTest : Form
    {

        string logname = "log.txt";
        public KZTest()
        {
            InitializeComponent();
            swLog = new StreamWriter(logname);
            swLog.WriteLine(DateTime.Now.ToString());
        }

        private void button1_Click(object sender, EventArgs e)
        {
            button1.Enabled = false;
            int taskNumber = GetTaskNumber();
            byte[] arr = GetArray(GetVoltage());
            if (!ControlTask.Instance().SetControlTask(taskNumber,(int)(128 - GetVoltage()*128/5000d), 0))
                MessageBox.Show(Res.Error);
            button1.Enabled = true;
        }

        private int GetVoltage()
        {
            int result;
            try
            {
                if (int.TryParse(cbAmplitude.Text, out result))
                    return result;
            }
            catch { }
            return 1000;
        }

        private int GetTaskNumber()
        {
            int result;
            try
            {
                if (int.TryParse(cbTask.Text.ToString(), out result))
                    return result;
            }
            catch { }
            return 0;
        }

        int timeout = 500;
        bool started;
        Thread tMeasure;
        StreamWriter swLog;

        private readonly byte[] _ar1000 = new byte[] { 0x92, 0x85 },
                                _ar1750 = new byte[] { 0xA1, 0x8A },
                                _ar3000 = new byte[] { 0xB9, 0x92 },
                                _ar4500 = new byte[] { 0xD6, 0x9C };
        /// <summary>
        /// Заменяем 2 последних байта в arWrite[5] 
        /// </summary>
        private byte[] GetArray(int voltMeandr)
        {
            if (voltMeandr == 1000)
            {
                return _ar1000;
            }
            if (voltMeandr == 1750)
            {
                return _ar1750;
            }
            if (voltMeandr == 3000)
            {
                return _ar3000;
            }
            return _ar4500;
        }


        private void button2_Click(object sender, EventArgs e)
        {
            if (started)
            {
                StopRequests();
                started = false;
                button2.FlatStyle = FlatStyle.Standard;
                swLog.WriteLine(DateTime.Now.ToString());
                swLog.Flush();
                
            }
            else
            {
                GetTimeout();
                StartRequests();
                started = true;
                button2.FlatStyle = FlatStyle.Flat;
            }

        }

        private void GetTimeout()
        {
            double result;
            if (!double.TryParse(textBox1.Text, out result))
                return;
            timeout = (int)result; 
        }

        private void StopRequests()
        {
            if (tMeasure != null)
                if (tMeasure.IsAlive)
                    tMeasure.Abort();
        }

        private void StartRequests()
        {
            if (tMeasure != null)
                if (tMeasure.IsAlive)
                    return;
            tMeasure = new Thread(Measuring);
            tMeasure.Start();
        }

        private void Measuring()
        {
            while (true)
            {
                List<double> data = new List<double>();
                string result = "";
                ControlTask.Instance().GetFrequency(ref data);
                foreach (int i in data)
                    result += Alignment(i, 6) + " ";
                ControlTask.Instance().GetDuration(ref data);
                foreach (int i in data)
                    result += Alignment(i, 6) + " ";
                AddTextToTextBox(result);
                WriteToFile(result);
                Thread.Sleep(timeout);
            }
        }

        private string Alignment(int i, int signs)
        {
            StringBuilder sb = new StringBuilder(i.ToString());            
            int count = sb.Length;
            while (count < signs)
            {
                sb.Insert(0, " ");                
                count++;
            }
            return sb.ToString();
        }

        private void WriteToFile(string result)
        {
            try
            {
                swLog.WriteLine(result);
            }
            catch { }
        }

        const int textLimit = 1000;

        private void AddTextToTextBox(string result)
        {
            if (InvokeRequired)
            {
                BeginInvoke(new Action<string>(AddTextToTextBox), result);
                return;
            }
            try
            {
                result += Environment.NewLine;
                richTextBox1.Text = richTextBox1.Text.Insert(0, result);
                if (richTextBox1.Text.Length > textLimit)
                    richTextBox1.Text = richTextBox1.Text.Remove(richTextBox1.Text.Length - textLimit / 10, textLimit / 10);
            }
            catch { }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Process p = new Process();
            p.StartInfo.FileName = logname;
            p.Start();
        }

        private void KZTest_FormClosing(object sender, FormClosingEventArgs e)
        {
            swLog.Close();
            StopRequests();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            V7_VoltmeterManagement.SearchVoltmetersOnce();
            label3.Text = V7_VoltmeterManagement.VoltmetersCount.ToString();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            label3.Text = V7_VoltmeterManagement.Measure().ToString();
        }
    }
}