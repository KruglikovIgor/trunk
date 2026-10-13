using System;
using System.Data;
using System.Windows.Forms;
using Sparc.KPK2.NFunctions;

namespace KPK2.GeneralForms.ServiceForm
{
  public partial class frmParameters : Form
  {
    public string CurParam = "";
    public static int _scroll;


    public frmParameters()
    {
      InitializeComponent();
    }

    private void bSelect_Click(object sender, EventArgs e)
    {
      CurParam = dgvParams.SelectedRows[0].Cells[1].Value.ToString();
      Close();
    }

    private void frmParameters_Load(object sender, EventArgs e)
    {
      FillTable();
      if (_scroll != 0)
        if (_scroll <= dgvParams.Rows.Count)
          dgvParams.FirstDisplayedScrollingRowIndex = _scroll;
    }

    private void FillTable()
    {
      dgvParams.ReadOnly = true;
      DataTable data = DataManager.Instance.GetAllParams();
      int selectedRow = 0;
      foreach (DataRow dr in data.Rows)
      {
        dgvParams.Rows.Add(dr["ParamId"], dr["ParamName"], dr["Note"]);
        if (dr["ParamName"].ToString() == CurParam)
          selectedRow = dgvParams.Rows.Count - 1;
      }
      dgvParams.Rows[selectedRow].Selected = true;
      dgvParams.FirstDisplayedScrollingRowIndex = selectedRow;
      dgvParams.ReadOnly = false;
    }

    private void dgvParams_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
    {
      bSelect.PerformClick();
    }

    private void frmParameters_FormClosing(object sender, FormClosingEventArgs e)
    {
      _scroll = dgvParams.FirstDisplayedScrollingRowIndex;
    }
  }
}