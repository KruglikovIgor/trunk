using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;
using KPK2.Properties;
using Sparc.KPK2.NFunctions;

namespace KPK2.GeneralForms.ServiceForm
{
    public partial class frmReportEditor : Form
    {
        private DataTable dtDev = new DataTable();

        public frmReportEditor()
        {
            InitializeComponent();
        }

        private void dgvReport_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (((DataGridView)sender).ReadOnly)
                return;
            if (e.RowIndex >= 0)
            {
                ((DataGridView) sender).Rows[e.RowIndex].Cells[e.ColumnIndex].Style.BackColor = frmCalibrate.ChangedColor;
                bSave.Enabled = true;
            }
        }

        private void bSave_Click(object sender, EventArgs e)
        {
            if (!Save())
                MessageBox.Show(Resources.SaveNotCompleted);
            else
                bSave.Enabled = false;
        }

        private bool Save()
        {
            bool saved = true;
            foreach (DataGridViewRow row in dgvReport.Rows)
            {
                bool isNewRow = true;
                bool isChangedRow = false;
                foreach (DataGridViewCell cell in row.Cells)
                    if (cell.Style.BackColor == frmCalibrate.ChangedColor)
                        isChangedRow = true;
                    else
                        isNewRow = false;
                if (isNewRow)
                    saved &= SaveNewRow(row.Index);
                else
                    if (isChangedRow)
                        saved &= SaveChangedRow(row.Index);
            }
            return saved;
        }

        private const int numColId = 0;
        private const int numColItemNumber = 1;
        private const int numColSubItemNumber = 2;
        private const int numColOrderNumber = 3;
        private const int numColName = 4;
        private const int numColUnits = 5;
        private const int numColRequirement = 6;
        private const int numColParamName = 7;
        private const int numColNameLang = 8;
        private const int numColRequirementLang = 9;
        

        private bool SaveChangedRow(int rowIndex)
        {
            try
            {
              if (cbLanguage.SelectedIndex != -1)
                return UpdateParamLang(rowIndex);
              return UpdateParam(rowIndex);

            }
            catch { }
            return false;
        }

        private bool UpdateParamLang(int rowIndex)
        {
          DataGridViewCellCollection coll = dgvReport.Rows[rowIndex].Cells;
          foreach (DataGridViewCell cell in coll)
            if (cell.Value == null)
              cell.Value = "";

          string colNameLang = String.Empty;
          if (coll[numColNameLang].Style.BackColor == frmCalibrate.ChangedColor)
            colNameLang = coll[numColNameLang].Value.ToString();

          string colRequirementLang = String.Empty;
          if (coll[numColRequirementLang].Style.BackColor == frmCalibrate.ChangedColor)
            colRequirementLang = coll[numColRequirementLang].Value.ToString();

          if (DataManager.Instance.UpdateReportParamLang(Convert.ToInt32(coll[numColId].Value),
            Convert.ToInt32(coll[numColItemNumber].Value),
            Convert.ToInt32(coll[numColSubItemNumber].Value),
            coll[numColOrderNumber].Value.ToString(),
            coll[numColName].Value.ToString(),
            coll[numColUnits].Value.ToString(),
            coll[numColRequirement].Value.ToString(),
            coll[numColParamName].Value.ToString(),
            colNameLang,
            colRequirementLang,
            DataManager.EnglishLang))
          {
            RowSaved(rowIndex);
            return true;
          }

          return false;
        }

        private bool UpdateParam(int rowIndex)
        {
          DataGridViewCellCollection coll = dgvReport.Rows[rowIndex].Cells;
          foreach (DataGridViewCell cell in coll)
            if (cell.Value == null)
              cell.Value = "";

          if (DataManager.Instance.UpdateReportParam(Convert.ToInt32(coll[numColId].Value),
            Convert.ToInt32(coll[numColItemNumber].Value),
            Convert.ToInt32(coll[numColSubItemNumber].Value),
            coll[numColOrderNumber].Value.ToString(),
            coll[numColName].Value.ToString(),
            coll[numColUnits].Value.ToString(),
            coll[numColRequirement].Value.ToString(),
            coll[numColParamName].Value.ToString()))
          {
            RowSaved(rowIndex);
            return true;
          }

          return false;
        }

        private bool SaveNewRow(int rowIndex)
        {
            DataGridViewCellCollection coll = dgvReport.Rows[rowIndex].Cells;
            foreach (DataGridViewCell cell in coll)
                if (cell.Value == null)
                    cell.Value = "";
            coll[numColId].Value =
                DataManager.Instance.InsertReportParam(Convert.ToInt32(cbDevices.SelectedValue),
                                                         Convert.ToInt32(coll[numColItemNumber].Value),
                                                         Convert.ToInt32(coll[numColSubItemNumber].Value),
                                                         coll[numColOrderNumber].Value.ToString(),
                                                         coll[numColName].Value.ToString(),
                                                         coll[numColUnits].Value.ToString(),
                                                         coll[numColRequirement].Value.ToString(),
                                                         coll[numColParamName].Value.ToString());
            if (Convert.ToInt32(coll[numColId].Value) != 0)
            {
                RowSaved(rowIndex);
                return true;
            }
            return false;
        }

        private void RowSaved(int rowIndex)
        {
            foreach (DataGridViewCell cell in dgvReport.Rows[rowIndex].Cells)
                cell.Style.BackColor = frmCalibrate.UnchangedColor;
        }

        private void FillMainTable()
        {
            dgvReport.ReadOnly = true;
            dgvReport.Rows.Clear();
          if (cbLanguage.SelectedIndex != -1)
          {
            DataTable parameters = DataManager.Instance.GetAllReportParamsLang(Convert.ToInt32(cbDevices.SelectedValue), DataManager.EnglishLang);
            if (parameters != null)
              foreach (DataRow dtr in parameters.Rows)
                dgvReport.Rows.Add(dtr[0], dtr[1], dtr[2], dtr[3], dtr[4], dtr[5], dtr[6], dtr[7], dtr[8], dtr[9]);
          }
          else
          {
            DataTable parameters = DataManager.Instance.GetAllReportParams(Convert.ToInt32(cbDevices.SelectedValue));
            if (parameters != null)
              foreach (DataRow dtr in parameters.Rows)
                dgvReport.Rows.Add(dtr[0], dtr[1], dtr[2], dtr[3], dtr[4], dtr[5], dtr[6], dtr[7]);
          }

          dgvReport.ReadOnly = false;
        }

        private void frmReportEditor_Load(object sender, EventArgs e)
        {
            FillComboBox();
        }

        private void FillComboBox()
        {
            Dictionary <int,string> devices = DataManager.Instance.GetDevices();
            dtDev.Columns.Add("Id");
            dtDev.Columns.Add("Name");            
            if (devices != null)
                foreach (KeyValuePair<int, string> dev in devices)
                    dtDev.Rows.Add(dev.Key, dev.Value);
            cbDevices.DisplayMember = "Name";
            cbDevices.ValueMember = "Id";
            cbDevices.DataSource = dtDev;
            
        }

        private void cbDevices_SelectedIndexChanged(object sender, EventArgs e)
        {            
             FillMainTable();
        }

        private void dgvReport_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == numColParamName)
            {
                frmParameters frm = new frmParameters();
                if (dgvReport[e.ColumnIndex, e.RowIndex].Value != null)
                    frm.CurParam = dgvReport[e.ColumnIndex, e.RowIndex].Value.ToString();
                frm.ShowDialog();
                if ((dgvReport[e.ColumnIndex, e.RowIndex].Value == null) || (dgvReport[e.ColumnIndex, e.RowIndex].Value.ToString() != frm.CurParam))
                    dgvReport[e.ColumnIndex, e.RowIndex].Value = frm.CurParam;
            }
        }

        private void dgvReport_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            if (((DataGridView)sender).ReadOnly)
                return;
            foreach (DataGridViewCell cell in ((DataGridView)sender).Rows[e.RowIndex - 1].Cells)
                cell.Style.BackColor = frmCalibrate.ChangedColor;
        }

      private bool _columnsAdded = false;

        private void cbLanguage_SelectedIndexChanged(object sender, EventArgs e)
        {
          if (!_columnsAdded)
          {
            int col = dgvReport.Columns.Add("cCheckName", "CheckName (English)");
            dgvReport.Columns[col].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            col = dgvReport.Columns.Add("cRequirement", "Requirement (English)");
            dgvReport.Columns[col].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            _columnsAdded = true;
          }
          FillMainTable();
        }
    }
}