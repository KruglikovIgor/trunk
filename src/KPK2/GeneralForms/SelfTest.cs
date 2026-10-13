using System;
using System.Collections;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;
using AktorPS115v;
using Diss15_32.MyClasse;
using Diss15_32.Properties;
using LCards;

namespace Diss32TO
{
    public partial class SelfTest : Form
    {

        public BitArray ports = new BitArray(3);
        private bool isAllDevicesGood = true;
        public static class CheckStatus
        {
            public static string InProgress = Resources.Checking + "...";
            public static string Success = Resources.Success;
            public static string Fail = Resources.Fail;
            public static string NoCheck = Resources.CantCheck;
        }
        public SelfTest()
        {
            InitializeComponent();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void SelfTest_Load(object sender, EventArgs e)
        {
            
        }

        private delegate void SetRowSt(string checkRow, bool result);
        private delegate void SetLastRowStatusDelegate(bool result);
        private delegate void AddCheckRowDelegate(string checkRow);

        private void SetRowStatus(string checkRow, bool result)
        {
            if (InvokeRequired)
            {
                BeginInvoke(new SetRowSt(SetRowStatus), checkRow, result);
                return;
            }
            dgvSelfTest.Rows.Add(checkRow, result ? CheckStatus.Success : CheckStatus.Fail);
            ssStatus.Text = Resources.Checking +  " " + checkRow;            
            isAllDevicesGood &= result;
            RepaintGrid(dgvSelfTest.Rows.Count-1);
            Application.DoEvents();            
        }

        private void SetLastRowStatus(bool result)
        {
            if (InvokeRequired)
            {
                BeginInvoke(new SetLastRowStatusDelegate(SetLastRowStatus), result);
                return;
            }
            dgvSelfTest.Rows[dgvSelfTest.Rows.Count - 1].Cells[1].Value =  result ? CheckStatus.Success : CheckStatus.Fail;            
            isAllDevicesGood &= result;
            RepaintGrid(dgvSelfTest.Rows.Count - 1);
            Application.DoEvents();
        }

        private void AddCheckRow(string checkRow)
        {
            if (InvokeRequired)
            {
                BeginInvoke(new AddCheckRowDelegate(AddCheckRow), checkRow);
                return;
            }
            dgvSelfTest.Rows.Add(checkRow, CheckStatus.InProgress);
            ssStatus.Text = Resources.Checking + " " + checkRow;            
            RepaintGrid(dgvSelfTest.Rows.Count - 1);
            Application.DoEvents();
        }

        private delegate void AddR(string Caption);
        private void AddRow(string caption)
        {
            if (InvokeRequired)
            {
                BeginInvoke(new AddR(AddRow), caption);
                return;
            }
            dgvSelfTest.Rows.Add(caption);
        }

        private void RepaintGrid(int row)
        {
            if (dgvSelfTest.Rows[row].Cells[1].Value == null)
                return;
            DataGridViewCellStyle successStyle = new DataGridViewCellStyle();
            successStyle.BackColor = Color.LightGreen;                
            DataGridViewCellStyle failStyle = new DataGridViewCellStyle();
            failStyle.BackColor = Color.Red;
            if (dgvSelfTest.Rows[row].Cells[1].Value.GetHashCode() == CheckStatus.Success.GetHashCode())
                dgvSelfTest.Rows[row].DefaultCellStyle = successStyle;
            if (dgvSelfTest.Rows[row].Cells[1].Value.GetHashCode() == CheckStatus.Fail.GetHashCode())
                dgvSelfTest.Rows[row].DefaultCellStyle = failStyle;
        }

        private delegate void CI(bool isStart);
        private void CaptionInit(bool isStart)
        {
            if (InvokeRequired)
            {
                BeginInvoke(new CI(CaptionInit), isStart);
                return;
            }
            if (isStart)
            {
                bClose.Enabled = false;
                bCheck.Enabled = false;
                lCaption.Text = Resources.WaitWhileDeviceTesting;
                lCaption.ForeColor = Color.Black;
                dgvSelfTest.Font = new Font("Microsoft Sans Serif", 11);
                dgvSelfTest.Rows.Add("LTR-платы:");
            }
            else
            {
                lCaption.Text = Resources.TestFinished + " ";
                if (isAllDevicesGood)
                {
                    lCaption.Text += Resources.AllDevicesGood;
                    lCaption.ForeColor = Color.Green;
                }
                else
                {
                    lCaption.Text += Resources.SomeDevicesFailed;
                    lCaption.ForeColor = Color.DarkRed;
                }
                bClose.Enabled = true;
                bCheck.Enabled = true;
            }
        }
        
        private void DataGridCheck()
        {
            CaptionInit(true);
            LTRManager.CloseAll();
            AddCheckRow("LTR114");
            SetLastRowStatus(LTRManager.LTR114SetActiveChannels(10, 1));
            for (int i = 1; i < 4; i++)
            {
                AddCheckRow("LTR11 (" + i + ")");
                SetLastRowStatus(LTRManager.LTR11SetupAdc((uint)i + 10, 1));
            }
            for (int i = 1; i < 3; i++)
            {
                AddCheckRow("LTR22 (" + i + ")");
                SetLastRowStatus(LTRManager.LTR22SetACDCState((uint) i+13, true));                
            }            
            AddRow(Resources.PowerSupplies + ":");
            AddCheckRow("27V");
            SetLastRowStatus(Lambda27V.GetOutState() != null);            
            bool result;
            AddCheckRow("115V");
            try
            {
                AktorPS115 aktor = new AktorPS115(SpManager.Open(DataManager.Instance().GetDevicePort(DataManager.Aktor115V)));
                result = aktor.GetStatus();
                if (result)
                    if (!aktor.GetSwitchState())
                    {
                        MessageBox.Show(Resources.IncorrectSwitchMode, Resources.ErrorCaption);
                        result = false;
                    }
            }
            catch
            {
                result = false;
            }
            SetLastRowStatus(result);            
            AddRow(Resources.Others + ":");
            AddCheckRow(Resources.SKTBoard);
            GenSinCos gsc = new GenSinCos();            
            SetLastRowStatus(gsc.GeneratorOff());
            AddCheckRow(Resources.KZBoard);
            ControlTask ct = new ControlTask();            
            bool gen = false;
            DateTime date = new DateTime();
            SetLastRowStatus(ct.GetVersion(ref gen, ref date));
            AddCheckRow(Resources.DiscretInputsBoard);
            DiscreteInputs di = new DiscreteInputs();            
            SetLastRowStatus(di.GetInputsStates() != null);
            AddCheckRow(Resources.DiscretOutputsBoard);
            DiscreteOutputs _do = new DiscreteOutputs();            
            SetLastRowStatus(_do.SetChannelsOff());
            AddCheckRow(Resources.DiscretInputOutputsBoard);
            try
            {
                WorkWith1760.Instance.ReadDI();
                result = true;
            }
            catch
            {
                result = false;
            }
            SetLastRowStatus(result);            
            CaptionInit(false);
        }

        private void SelfTest_Shown(object sender, EventArgs e)
        {
            DataGridCheckInThread();
        }

        private void dgvSelfTest_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if ((e.ColumnIndex == 1) && (e.RowIndex >= 0))
                RepaintGrid(e.RowIndex);
        }

        private void bClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void bCheck_Click(object sender, EventArgs e)        
        {   
            LTRManager.CloseAll();
            isAllDevicesGood = true;
            dgvSelfTest.Rows.Clear();
            DataGridCheckInThread();
        }

        private void DataGridCheckInThread()
        {
            Thread th = new Thread(DataGridCheck);
            th.Start();
        }
    }
}