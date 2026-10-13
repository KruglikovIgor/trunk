using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using KPK2.Properties;
using Sparc.KPK2.NFunctions;

namespace KPK2.GeneralForms.ServiceForm
{
    public partial class FrmDocumentLink : Form
    {
        public FrmDocumentLink()
        {
            InitializeComponent();
        }

        private void FrmDocumentLink_Load(object sender, EventArgs e)
        {
            InitGrid();
            LoadData();
        }

        private const int numColId = 0;
        private const int numColDevice = 1;
        private const int numColTestReference = 2;
        private const int numColText = 3;
        private const int numColNote = 4;

        private void LoadData()
        {
            dgvLinks.ReadOnly = true;
            DataTable dtLinks = DataManager.Instance.GetDocumentLinks();
            foreach (DataRow dr in dtLinks.Rows)
                dgvLinks.Rows.Add(dr[numColId], dr[numColDevice].ToString(), dr[numColTestReference].ToString(), dr[numColText]);
            dgvLinks.ReadOnly = false;
        }

        private const string cbDeviceIdColumn = "Id";
        private const string cbDeviceNameColumn = "Device";

        private void InitGrid()
        {
            FillComboBoxColumn(DataManager.Instance.GetDevices(), (DataGridViewComboBoxColumn)dgvLinks.Columns[numColDevice]);
            //FillComboBoxColumn(DataManager.Instance.GetTestReferences(7), (DataGridViewComboBoxColumn)dgvLinks.Columns[numColTestReference]);
        }

        private static void FillComboBoxColumn(Dictionary<int,string> items, DataGridViewComboBoxColumn cbColumn)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add(cbDeviceIdColumn);
            dt.Columns.Add(cbDeviceNameColumn);
            foreach (KeyValuePair<int, string> kv in items)
                dt.Rows.Add(kv.Key, kv.Value);
            if (cbColumn != null)
            {
                cbColumn.DataSource = dt;
                cbColumn.DisplayMember = cbDeviceNameColumn;
                cbColumn.ValueMember = cbDeviceIdColumn;
            }
        }

        private const string CommonReferenceString = "Общая";

        private static void FillComboBoxCell(Dictionary<int, string> items, DataGridViewComboBoxCell cbCell)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add(cbDeviceIdColumn);
            dt.Columns.Add(cbDeviceNameColumn);
            dt.Rows.Add(0, CommonReferenceString);
            foreach (KeyValuePair<int, string> kv in items)
                dt.Rows.Add(kv.Key, kv.Value);
            if (cbCell != null)
            {
                cbCell.DataSource = dt;
                cbCell.DisplayMember = cbDeviceNameColumn;
                cbCell.ValueMember = cbDeviceIdColumn;
            }
        }

        private void dgvLinks_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {            
            if (e.RowIndex == -1)
                return;
            ((DataGridView)sender).Rows[e.RowIndex].Cells[e.ColumnIndex].Style.BackColor = ChangedColor;
            bSave.Enabled = true;
            if (e.ColumnIndex == numColDevice)
            {
                ReFillTestReferenceColumn(e.RowIndex);
            }
        }

        private void ReFillTestReferenceColumn(int rowIndex)
        {
            FillComboBoxCell(DataManager.Instance.GetTestReferences(Convert.ToInt32(dgvLinks.Rows[rowIndex].Cells[numColDevice].Value)), (DataGridViewComboBoxCell)dgvLinks.Rows[rowIndex].Cells[numColTestReference]);
        }

        private void dgvLinks_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            ReFillTestReferenceColumn(e.RowIndex);
            if (((DataGridView)sender).ReadOnly)
                return;
            foreach (DataGridViewCell cell in ((DataGridView)sender).Rows[e.RowIndex - 1].Cells)
                cell.Style.BackColor = ChangedColor;
        }

        private void dgvLinks_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
           // MessageBox.Show(string.Format("Ошибка в строке: {0} в столбце: {1}", e.RowIndex, e.ColumnIndex));
        }

        private void bSave_Click(object sender, EventArgs e)
        {
            if (!Save())
                MessageBox.Show(Resources.ErrorCaption);
            else
                bSave.Enabled = false;
        }

        public static readonly Color ChangedColor = Color.Red;
        public static readonly Color UnchangedColor = Color.White;

        private bool Save()
        {
            bool saved = true;
            foreach (DataGridViewRow row in dgvLinks.Rows)
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
                else
                    if (isChangedRow)
                        saved &= SaveChangedRow(row.Index);
            }
            return saved;
        }        

        private bool SaveNewRow(int rowIndex)
        {
            DataGridViewCellCollection coll = dgvLinks.Rows[rowIndex].Cells;
            foreach (DataGridViewCell cell in coll)
                if (cell.Value == null)
                    cell.Value = "";
            coll[numColId].Value = DataManager.Instance.InsertNewLink(Convert.ToInt32(coll[numColDevice].Value),
                                                                         Convert.ToInt32(coll[numColTestReference].Value),
                                                                         coll[numColText].Value.ToString(), "");
            if (Convert.ToInt32(coll[numColId].Value) != 0)
            {
                RowSaved(rowIndex);
                return true;
            }
            return false;
        }

        private void RowSaved(int rowIndex)
        {
            foreach (DataGridViewCell cell in dgvLinks.Rows[rowIndex].Cells)
                cell.Style.BackColor = UnchangedColor;
        }

        private bool SaveChangedRow(int rowIndex)
        {
            DataGridViewCellCollection coll = dgvLinks.Rows[rowIndex].Cells;
            foreach (DataGridViewCell cell in coll)
                if (cell.Value == null)
                    cell.Value = "";
            try
            {
                if (DataManager.Instance.UpdateLink(Convert.ToInt32(coll[numColId].Value),
                                                      Convert.ToInt32(coll[numColDevice].Value),
                                                      Convert.ToInt32(coll[numColTestReference].Value),
                                                      coll[numColText].Value.ToString()))
                {
                    RowSaved(rowIndex);
                    return true;
                }
            }
            catch { }
            return false;
        }
    }
}