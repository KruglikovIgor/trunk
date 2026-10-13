using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using KPK2.Properties;
using Sparc.KPK2.NFunctions;

namespace KPK2.GeneralForms
{
    public partial class frmPassportData : Form
    {
        private int _testId = 0;
        private string sParamId = "ParamId";
        private string sName = "Name";
        private string sValue = "Value";
        private int ColParamId = 0;
        //private int ColName = 1;
        private int ColValue = 2;
        public static readonly Color ChangedColor = Color.Yellow;
        public static readonly Color UnchangedColor = Color.White;

        public frmPassportData(int testId)
        {
            InitializeComponent();
            _testId = testId;
            FillGrid();
        }

        private void FillGrid()
        {
            DataTable result = DataManager.Instance.GetPassportValues(_testId);
            foreach (DataRow dr in result.Rows)
                dgvValues.Rows.Add(dr[sParamId], dr[sName], dr[sValue] == DBNull.Value? dr[sValue] : Math.Round(Convert.ToDouble(dr[sValue]),1));

        }

        private void bClose_Click(object sender, EventArgs e)
        {
            Close();
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
            foreach (DataGridViewRow row in dgvValues.Rows)
            {
                //bool isNewRow = true;
                bool isChangedRow = false;
                foreach (DataGridViewCell cell in row.Cells)
                    if (cell.Style.BackColor == ChangedColor)
                        isChangedRow = true;
                    
                    if (isChangedRow)
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
            try
            {
                if (DataManager.Instance.SetPassportValue(_testId, Convert.ToInt32(dgvValues.Rows[rowIndex].Cells[ColParamId].Value), Convert.ToDouble(dgvValues.Rows[rowIndex].Cells[ColValue].Value)))
                {
                    RowSaved(rowIndex);
                    return true;
                }
            }
            catch
            {}
            return false;
        }

        private void RowSaved(int rowIndex)
        {
            foreach (DataGridViewCell cell in dgvValues.Rows[rowIndex].Cells)
                cell.Style.BackColor = UnchangedColor;
        }
        
    }
}