using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO.Ports;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using AktorPS115v;
using KPK2.Properties;
using Sparc.KPK2.NFunctions;

namespace KPK2.GeneralForms.ServiceForm
{
    public partial class frmCheckComDevices : Form
    {
        private List<KeyValuePair<string, string>> _modules;

        private List<KeyValuePair<string, object[]>> checkSequence;

        private const string MainCaption = "Com-порты:";

        private const string PortsFilter = "COM";

        private const string AktorName = "Aktor115V";

        private const string PortNotAvailable = "Порт недоступен";

        public frmCheckComDevices()
        {
            InitializeComponent();
        }
        
        private void bStart_Click(object sender, EventArgs e)
        {
            EnableButtons(false);
            CaptionInit(true);

            DataGridCheck();            

            CaptionInit(false);
            EnableButtons(true);
        }

        private void SetLastRowStatus(string result)
        {
            if (InvokeRequired)
            {
                BeginInvoke(new Action<string>(SetLastRowStatus), result);
                return;
            }
            dgvCheckDevices.Rows[dgvCheckDevices.Rows.Count - 1].Cells[1].Value = String.IsNullOrEmpty(result)
                                                                                      ? Resources.DevicesNotFound
                                                                                      : result;
            RepaintGrid(dgvCheckDevices.Rows.Count - 1);
            Application.DoEvents();
        }

        private class PortComparer : Comparer<string>
        {
            public override int Compare(string x, string y)
            {
                try
                {
                    int num1 = GetLastNumber(x);
                    int num2 = GetLastNumber(y);
                    if (num1 == num2)
                        return 0;
                    if (num1 > num2)
                        return 1;
                    return -1;
                }
                catch
                {
                    return 0;
                }
            }

            private int GetLastNumber(string checkStr)
            {
                int decCount = 0;
                for (int charNum = 0;charNum< checkStr.Length;charNum++)
                {
                    if (!Char.IsDigit(checkStr[checkStr.Length - charNum-1]))
                        break;
                    decCount++;
                }
                return Convert.ToInt32(checkStr.Substring(checkStr.Length - decCount, decCount));
            }
        }

        private void DataGridCheck()
        {            
            string[] ports = SerialPort.GetPortNames();
            List<string> portsList = new List<string>(ports);
            portsList.Sort(new PortComparer());
            for (int portNum = 0; portNum < ports.Length; portNum++)
            {
                if (!portsList[portNum].Contains(PortsFilter))
                    continue;
                AddCheckRow(portsList[portNum]);
                SetLastRowStatus(CheckPort(portsList[portNum]));
            }
        }

        private string CheckPort(string port)
        {
            const int WaitBeforeRead = 100;
            StringBuilder devices = new StringBuilder();            
            try
            {
                SerialPort sp = new SerialPort();
                for (int moduleNum =0; moduleNum < _modules.Count;moduleNum++)
                {
                    sp.PortName = _modules[moduleNum].Value;
                    DataManager.Instance.SetPortParameters(ref sp);
                    sp.PortName = port;
                    //тест порта на доступность
                    try
                    {
                        sp.Open();                        
                    }
                    catch
                    {
                        return PortNotAvailable;
                    }
                    sp.Close();
                    if (_modules[moduleNum].Key == AktorName)
                    {
                        AktorPS115 testAktor = new AktorPS115(sp);
                        if (testAktor.GetStatus())
                            AddDeviceToList(devices, _modules[moduleNum].Key);                            
                        sp.Close();
                    }
                    else
                    {
                        
                        object[] parameters = GetSequenceParams(_modules[moduleNum].Key);
                        if (parameters == null)
                            throw new Exception("Параметры для данного устройства отсутствуют!");
                        sp.Open();
                        if (parameters[0] is string)
                        {
                            sp.Write((string)parameters[0]);
                            Thread.Sleep(WaitBeforeRead);
                            string answer = sp.ReadExisting();
                            if (answer == (string)parameters[1])
                            {
                                AddDeviceToList(devices, _modules[moduleNum].Key);
                                continue;
                            }
                        }
                        else
                        {
                            byte[] toSend = (byte[]) parameters[0];
                            sp.Write(toSend, 0, toSend.Length);
                            Thread.Sleep(WaitBeforeRead);
                            int readCount = sp.BytesToRead;
                            if (sp.BytesToRead != 0)
                            {
                                byte[] toReceive = (byte[]) parameters[1];
                                byte[] answer = new byte[readCount];
                                sp.Read(answer, 0, readCount);
                                bool result = true;
                                for (int byteNum = 0; byteNum < Math.Min(answer.Length, toReceive.Length); byteNum++)
                                    result &= toReceive[byteNum] == answer[byteNum];
                                if (result)
                                {
                                    AddDeviceToList(devices, _modules[moduleNum].Key);
                                    continue;
                                }
                            }
                        }
                        sp.Close();
                    }
                }
            }
            catch
            {
                return null;
            }
            return devices.ToString();
        }
        
        private static void AddDeviceToList (StringBuilder list, string moduleName)
        {
            if (list.Length != 0)
                list.Append(' ');
            list.Append(moduleName);
        }



        /// <summary>
        /// Ищет нужное устройство в списке тестовых последовательностей и выдаёт последовательности на выход
        /// </summary>
        /// <param name="moduleName"></param>
        /// <returns></returns>
        private object[] GetSequenceParams(string moduleName)
        {
            for (int seqNum = 0;seqNum < checkSequence.Count; seqNum++)
            {
                if (checkSequence[seqNum].Key == moduleName)
                    return checkSequence[seqNum].Value;
            }
            return null;
        }

        private void AddCheckRow(string checkRow)
        {
            if (InvokeRequired)
            {
                BeginInvoke(new Action<string>(AddCheckRow), checkRow);
                return;
            }
            dgvCheckDevices.Rows.Add(checkRow, SelfTest.CheckStatus.InProgress);            
            RepaintGrid(dgvCheckDevices.Rows.Count - 1);
            Application.DoEvents();
        }

        private void RepaintGrid(int row)
        {
            if (dgvCheckDevices.Rows[row].Cells[1].Value == null)
                return;
            DataGridViewCellStyle successStyle = new DataGridViewCellStyle();
            successStyle.BackColor = Color.LightGreen;
            DataGridViewCellStyle failStyle = new DataGridViewCellStyle();
            failStyle.BackColor = Color.Red;
            if (dgvCheckDevices.Rows[row].Cells[1].Value.GetHashCode() == SelfTest.CheckStatus.Success.GetHashCode())
                dgvCheckDevices.Rows[row].DefaultCellStyle = successStyle;
            if (dgvCheckDevices.Rows[row].Cells[1].Value.GetHashCode() == SelfTest.CheckStatus.Fail.GetHashCode())
                dgvCheckDevices.Rows[row].DefaultCellStyle = failStyle;
        }

        private delegate void CI(bool isStart);
        /// <summary>
        /// Начальные приготовления к проверке
        /// </summary>
        /// <param name="isStart"></param>
        private void CaptionInit(bool isStart)
        {
            if (InvokeRequired)
            {
                Invoke(new CI(CaptionInit), isStart);
                return;
            }
            if (isStart)
            {
                lCaption.Text = Resources.WaitWhileDeviceTesting;                
                dgvCheckDevices.Font = new Font("Microsoft Sans Serif", 11);
                dgvCheckDevices.Rows.Clear();
                dgvCheckDevices.Rows.Add(MainCaption);
            }
            else
            {
                lCaption.Text = Resources.TestFinished;                
            }
        }

        /// <summary>
        /// Блокируем или разблокируем кнопки управления
        /// </summary>
        /// <param name="state"></param>
        private void EnableButtons(bool state)
        {
            bClose.Enabled = bStart.Enabled = state;
        }

        private void bClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void frmCheckComDevices_Load(object sender, EventArgs e)
        {
            _modules = DataManager.Instance.GetModulesWithPorts();
            if (_modules.Count == 0)
            {
                MessageBox.Show(Resources.ErrorLoadFromDb);
                Close();
            }
            FillSequences();            
        }

        private void FillSequences()
        {
            checkSequence = new List<KeyValuePair<string, object[]>>();
            checkSequence.Add(new KeyValuePair<string, object[]>("Aktor115V", new object[] {}));
            checkSequence.Add(new KeyValuePair<string, object[]>("Lambda40V", new object[] {"ADR 6\r", "OK\r"}));
            checkSequence.Add(new KeyValuePair<string, object[]>("KZ", new object[] {new byte[] {0x0A, 0xFD}, new byte[] {0x0A, 0xFE}}));
            checkSequence.Add(new KeyValuePair<string, object[]>("SKT", new object[] { new byte[] { 0x0A, 0xFD }, new byte[] { 0x0A, 0xFB } }));
            checkSequence.Add(new KeyValuePair<string, object[]>("DiscretInputs", new object[] { new byte[] { 0x0A, 0xFD }, new byte[] { 0x0A, 0x05 } }));
            checkSequence.Add(new KeyValuePair<string, object[]>("DiscretOutputs", new object[] { new byte[] { 0x0A, 0xFD }, new byte[] { 0x0A, 0xFC } }));
        }

        private void frmCheckComDevices_Shown(object sender, EventArgs e)
        {
            bStart.PerformClick();
        }
    }
}