using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using KPK2.Properties;
using Sparc.KPK2.Hardware;
using Sparc.KPK2.NFunctions;
//using USBVoltmeter;

namespace KPK2.GeneralForms.ServiceForm
{
    public partial class frmGenSinCosCalibrate : Form
    {

        public static readonly Color ChangedColor = Color.Red;
        public static readonly Color UnchangedColor = Color.White;
        private const int BaseFrequency = 20;
        private const int BaseAngle = 0;
        private bool nudLimitIncrement;
        private bool nudLimitClick;

        public frmGenSinCosCalibrate()
        {
            InitializeComponent();
        }

        private void cbChannel_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadGridValues();
        }

        private void LoadGridValues()
        {
            DataTable dtResult = DataManager.Instance.GetCalibration(DataManager.GenSinCosModuleId,
                                                                       Convert.ToInt32(cbChannel.SelectedIndex));
            dgvValues.ReadOnly = true;
            dgvValues.Rows.Clear();
            for (int i = 0; i < dtResult.Rows.Count; i++)
                dgvValues.Rows.Add(dtResult.Rows[i][0], dtResult.Rows[i][2], (int)Convert.ToDouble(dtResult.Rows[i][3]), dtResult.Rows[i][4]);
            dgvValues.ReadOnly = false;
            dgvValues.Columns[1].ReadOnly = true;
        }

        private void dgvValues_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (((DataGridView)sender).ReadOnly)
                return;
            if (e.RowIndex >= 0)
            {
                ((DataGridView)sender).Rows[e.RowIndex].Cells[e.ColumnIndex].Style.BackColor = ChangedColor;
                bSave.Enabled = true;
            }
            if (e.ColumnIndex == numColValue)
                SetCurrentSignal(e.RowIndex);
        }

        private void dgvValues_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            if (((DataGridView)sender).ReadOnly)
                return;
            foreach (DataGridViewCell cell in ((DataGridView)sender).Rows[e.RowIndex - 1].Cells)
                cell.Style.BackColor = ChangedColor;
            dgvValues.Rows[e.RowIndex-1].Cells[numColChannel].Value = cbChannel.SelectedIndex;
        }

        private const int numColValue = 3;
        //private const int numColMeasure = 4;
        //private const int numColIdealValue = 2;
        private const int numColChannel = 1;
        private const int numColId = 0;

        private void dgvValues_CellClick(object sender, DataGridViewCellEventArgs e)
        {
          if (e.ColumnIndex == numColValue)
            SetCurrentSignal(e.RowIndex);
          //else
          //  if (e.ColumnIndex == numColMeasure)
          //    if (V7_VoltmeterManagement.VoltmetersCount > 0)
          //      dgvValues.Rows[e.RowIndex].Cells[numColIdealValue].Value = (int)Math.Round(V7_VoltmeterManagement.Measure(0)*1000,0);
        }

        private void SetCurrentSignal(int rowIndex)
        {
            try
            {
                GenSinCos.Instance().SetSignal(Convert.ToInt32(cbChannel.SelectedIndex),
                                               Convert.ToInt32(dgvValues.Rows[rowIndex].Cells[numColValue].Value),
                                               Convert.ToInt32(cbFrequency.SelectedItem), BaseAngle);
            }
            catch
            {
            }
        }

        private void bSave_Click(object sender, EventArgs e)
        {
          if (!GenSinCos.Instance().GeneratorOff())
            MessageBox.Show(string.Format(Resources.ErrorOnDeviceN, Resources.GenSinBoard));
            if (!Save())
                MessageBox.Show(Resources.SaveNotCompleted);
            else
                bSave.Enabled = false;
        }

        private bool Save()
        {
            bool saved = true;
          if (nudLimit.Enabled)
          {
            DataManager.Instance.UpdateParamCommand(GenSinLimit, nudLimit.Value.ToString());
            nudLimit.ReadOnly = false;
            nudLimit.Increment = 0;
          }
          foreach (DataGridViewRow row in dgvValues.Rows)
          {
            bool isNewRow = true;
            bool isChangedRow = false;
            foreach (DataGridViewCell cell in row.Cells)
              if (cell.Style.BackColor == ChangedColor)
                isChangedRow = true;
              else
                isNewRow = false;
            if (isNewRow)
              saved &= SaveNewRow(row.Index);
            else if (isChangedRow)
              saved &= SaveChangedRow(row.Index);
          }
          return saved;
        }

        private bool SaveChangedRow(int rowIndex)
        {
            DataGridViewCellCollection coll = dgvValues.Rows[rowIndex].Cells;
            foreach (DataGridViewCell cell in coll)
                if (cell.Value == null)
                    cell.Value = "";
            coll[numColId].Value = DataManager.Instance.UpdateCalibration(Convert.ToInt32(coll[0].Value),
                                                   Convert.ToInt32(coll[2].Value), Convert.ToInt32(coll[3].Value));
            if (Convert.ToInt32(coll[numColId].Value) != 0)
            {
                RowSaved(rowIndex);
                return true;
            }
            return false;
        }

        private void RowSaved(int rowIndex)
        {
            foreach (DataGridViewCell cell in dgvValues.Rows[rowIndex].Cells)
                cell.Style.BackColor = UnchangedColor;
        }

        private bool SaveNewRow(int rowIndex)
        {
            DataGridViewCellCollection coll = dgvValues.Rows[rowIndex].Cells;
            foreach (DataGridViewCell cell in coll)
                if (cell.Value == null)
                    cell.Value = "";
            coll[numColId].Value = DataManager.Instance.SaveCalibration(DataManager.GenSinCosModuleId, Convert.ToInt32(coll[1].Value),
                                                   Convert.ToInt32(coll[2].Value), Convert.ToInt32(coll[3].Value));
            if (Convert.ToInt32(coll[numColId].Value) != 0)
            {
                RowSaved(rowIndex);
                return true;
            }
            return false;
        }

        private void dgvValues_UserDeletingRow(object sender, DataGridViewRowCancelEventArgs e)
        {
          DataManager.Instance.RemoveCalibration(Convert.ToInt32(e.Row.Cells[0].Value));
        }

        private string GenSinLimit = "Gen_Sin_Limit";

        private void frmGenSinCosCalibrate_Load(object sender, EventArgs e)
        {
          cbFrequency.SelectedIndex = 1;
          //V7_VoltmeterManagement.SearchVoltmetersOnce();
          //if (V7_VoltmeterManagement.VoltmetersCount > 0)
          //  V7_VoltmeterManagement.GetVoltmeter(0).SetMeasureVoltageAC();
          nudLimit.Value = Convert.ToInt32(DataManager.Instance.GetParamCommand(GenSinLimit));
        }

        private void nudLimit_DoubleClick(object sender, EventArgs e)
        {
          if (nudLimitIncrement)
            return;
          nudLimit.ReadOnly = !nudLimit.ReadOnly;
          nudLimit.Increment = nudLimit.ReadOnly ? 0 : 1;
        }


      private void nudLimit_ValueChanged(object sender, EventArgs e)
      {
        if (nudLimitClick)
          nudLimitIncrement = true;
        bSave.Enabled = true;
      }
      
      private void nudLimit_Click(object sender, EventArgs e)
      {
        nudLimitIncrement = false;
        nudLimitClick = true;
      }

      private void bShiftCalibrate_Click(object sender, EventArgs e)
      {
        frmGenSinCosAngleCalibrate frm = new frmGenSinCosAngleCalibrate();
        frm.ShowDialog();
      }

      private void frmGenSinCosCalibrate_FormClosing(object sender, FormClosingEventArgs e)
      {
        // Снять напряжение со всех каналов генератора
        GenSinCos.Instance().SetRealSignalEx(0, 0, BaseFrequency, 0);
        GenSinCos.Instance().SetRealSignalEx(1, 0, BaseFrequency, 0);
        GenSinCos.Instance().SetRealSignalEx(2, 0, BaseFrequency, 0);

        GenSinCos.Instance().GeneratorOff();
      }
    }
}