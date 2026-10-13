using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;
using KPK2.Properties;
using Sparc.KPK2.NFunctions;

namespace KPK2.GeneralForms.ServiceForm
{
  public partial class frmBlockService : Form
  {
    public frmBlockService()
    {
      InitializeComponent();
    }

    private void frmBlockService_Load(object sender, System.EventArgs e)
    {
      Dictionary<int, string> devices = DataManager.Instance.GetDevices();
      Dictionary<int, DataTable> tests = new Dictionary<int, DataTable>();
      foreach (KeyValuePair<int, string> kp in devices)
      {
        DataTable dt = new DataTable();
        for (int i = 0; i <= 10; i++)
          dt.Columns.Add();
          tests.Add(kp.Key, dt);
        DataManager.Instance.GetTestings(kp.Key, ref dt);
      }
      foreach (KeyValuePair<int, DataTable> kp in tests)
      {
        string deviceStr = "";
        foreach (KeyValuePair<int, string> kpIn in devices)
        {
          if (kpIn.Key == kp.Key)
            deviceStr = kpIn.Value;
        }
        
        foreach(DataRow dr in kp.Value.Rows)  
          dgvTests.Rows.Add(dr[9], kp.Key, deviceStr, dr[0], dr[3], dr[4]);
        
      }
    }
    
    private void dgvTests_CellClick(object sender, DataGridViewCellEventArgs e)
    {
      if (e.RowIndex == -1)
        return;
      DataTable dt = new DataTable();
      for (int i = 0; i <= 4; i++)
        dt.Columns.Add();
      DataManager.Instance.FillCompositionWithIdBlocks(Convert.ToInt32(dgvTests.Rows[e.RowIndex].Cells[0].Value), dt);
      DataTable dtRef = new DataTable();
      DataManager.Instance.FillCompositionReference(GetDeviceId(), dtRef);

      dgvBlocks.Rows.Clear();
      foreach (DataRow dr in dt.Rows)
      {
        object IdBlock = 0;
        foreach (DataRow drIn in dtRef.Rows)
          if (drIn[1].ToString() == dr[0].ToString())
            IdBlock = drIn[0];
        dgvBlocks.Rows.Add(IdBlock, dr[0], dr[1], dr[2], dr[3], "X");
      }

      //dgvBlocks.DataSource = dt;
    }

    private int GetDeviceId()
    {
      return Convert.ToInt32(dgvTests.Rows[dgvTests.SelectedCells[0].RowIndex].Cells[1].Value);
    }

    private void bSave_Click(object sender, EventArgs e)
    {
      if (dgvTests.SelectedCells.Count == 0)
        return;
      foreach (DataGridViewRow dr in dgvBlocks.Rows)
        if (
          !DataManager.Instance.UpdateComposition(GetTestId(),
                                                    Convert.ToInt32(dr.Cells[0].Value),
                                                    dr.Cells[2].Value.ToString(),
                                                    string.IsNullOrEmpty(dr.Cells[3].Value.ToString())
                                                      ? null
                                                      : (int?)Convert.ToInt32(dr.Cells[3].Value),
                                                    String.IsNullOrEmpty(dr.Cells[4].Value.ToString())
                                                      ? null
                                                      : (int?)Convert.ToInt32(dr.Cells[4].Value)))
          throw new Exception("Can't insert composition in database");
    }

    private void dgvBlocks_Click(object sender, EventArgs e)
    {

    }


    private const int colDelete = 5;
    private void dgvBlocks_CellClick(object sender, DataGridViewCellEventArgs e)
    {
      if (e.RowIndex == -1)
        return;
      if (dgvTests.SelectedCells.Count == 0)
        return;
      int testId = GetTestId();

      if (e.ColumnIndex == colDelete)
        if (MessageBox.Show(Resources.DeleteBlockRow, Resources.Attention, MessageBoxButtons.YesNo) ==
            DialogResult.Yes)
        {
          if (testId != 0)
            if (
              !DataManager.Instance
                          .RemoveComposition(testId,
                                             Convert.ToInt32(dgvBlocks.Rows[e.RowIndex].Cells[0].Value)))
            {
              MessageBox.Show(Resources.ErrorDeleteBlockFromDb);
              return;
            }
          dgvBlocks.Rows.RemoveAt(e.RowIndex);
        }
    }

    private int GetTestId()
    {
      return Convert.ToInt32(dgvTests.Rows[dgvTests.SelectedCells[0].RowIndex].Cells[0].Value);
    }
  }
}
