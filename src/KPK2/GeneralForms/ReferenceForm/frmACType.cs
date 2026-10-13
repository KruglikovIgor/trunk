using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;
using Sparc.KPK2.NFunctions;
using Sparc.KPK2.Resources.Properties;

namespace KPK2.GeneralForms.ReferenceForm
{
  public partial class frmACType : Form
  {
    /// <summary>
    ///   Таймер для закрытия формы в случае отсутствия соединения с БД.
    /// </summary>
    private Timer _closeTimer;

    public const int MinModTypeId = 1000;

    public frmACType()
    {
      InitializeComponent();
      MinimumSize = new Size(Size.Width, Size.Height);
    }

    private void frmHelicopter_Load(object sender, EventArgs e)
    {
      try
      {
        FillTable();
        if (dgvACType.RowCount == 0)
        {
          bEditMod.Enabled = bDeleteMod.Enabled = false;
        }
      }
      catch (SqlException)
      {
        MessageBox.Show(Resources.CantReadDb);
        _closeTimer = new Timer();
        _closeTimer.Interval = 10;
        _closeTimer.Tick += closeTimer_Tick;
        _closeTimer.Start();
      }
    }

    private void closeTimer_Tick(object sender, EventArgs e)
    {
      if (_closeTimer != null)
      {
        Close();
        _closeTimer.Stop();
        _closeTimer.Dispose();
      }
    }

    private void btnAdd_Click(object sender, EventArgs e)
    { 
        frmACTypeEdit editForm = new frmACTypeEdit(CurrentACStr);
        editForm.CurrentTypeId = CurrentTypeId;
        editForm.Text = Resources.NewModAC;
        var editDialogResult = editForm.ShowDialog();
        if (editDialogResult == DialogResult.OK)
            AddHelicopterOrCorrect(editForm.CurrentOperation, editForm.CurrentACStr, editForm.CurrentModStr);
    }

    private void AddHelicopterOrCorrect(frmACTypeEdit.Operation curOperation, string acStr, string modStr)
    {
        string combined = acStr + modStr;

        if (curOperation == frmACTypeEdit.Operation.Add)
        {
            if (!DataManager.Instance.AddACType(combined))
            {
                MessageBox.Show(Resources.ErrorSavingDataOnServer);
                return;
            }
        }
        else
        {
            if (!DataManager.Instance.CorrectACType(CurrentTypeId, combined))
            {
                MessageBox.Show(Resources.ErrorSavingDataOnServer);
                return;
            }
        }

        FillTable(curOperation == frmACTypeEdit.Operation.Add);
    }

    private void FillTable(bool moveToLast = false)
    {
        int cRow = 0;
        if (dgvACType.CurrentRow != null)
            cRow = dgvACType.CurrentRow.Index;
        DataTable dt = DataManager.Instance.GetACTypes();
        dgvACType.DataSource = dt;
        if (moveToLast)
            dgvACType.Rows[dgvACType.Rows.Count-1].Cells[1].Selected = true;
        else
            if (dgvACType.Rows.Count > cRow)
                dgvACType.Rows[cRow].Cells[1].Selected = true;
            //helicopterTypeBindingSource.Position = cRow;
        //if (dgvACType.Rows.Count > cRow)
        //{
        //    dgvACType.ClearSelection();
        //    dgvACType.Rows[cRow].Selected = true;
        //    dgvACType.CurrentCell = dgvACType.Rows[cRow].Cells[0];
        //}
    }

    private void btnEdit_Click(object sender, EventArgs e)
    {
      if (dgvACType.RowCount != 0)
      {
        if (dgvACType.CurrentRow != null)
        {
            string acType = GetMainType(CurrentACStr);
            string modType = CurrentACStr.Remove(0, acType.Length);
            var editForm = new frmACTypeEdit(acType, modType);
            var result = editForm.ShowDialog();
            if (result == DialogResult.OK)
                AddHelicopterOrCorrect(editForm.CurrentOperation, editForm.CurrentACStr, editForm.CurrentModStr);
        }
      }
    }

    private string GetMainType(string fullAcStr)
    {
        foreach (DataGridViewRow item in dgvACType.Rows)
        {
            if (Convert.ToInt32(item.Cells[0].Value) >= MinModTypeId)
                continue;
            string acType = item.Cells[1].Value.ToString();
            if (fullAcStr.StartsWith(acType))
                return acType;
        }
        throw new NotImplementedException();
    }

    private void btnCancel_Click(object sender, EventArgs e)
    {
      Close();
    }

    public int CurrentTypeId
    {
      get
      {
        if (dgvACType.CurrentRow == null)
          return -1;
        int id = Convert.ToInt32(dgvACType.CurrentRow.Cells[0].Value);
        return id;
      }
    }

    public string CurrentACStr
    {
        get
        {
            if (dgvACType.CurrentRow == null)
                return "";
            return dgvACType.CurrentRow.Cells[1].Value.ToString();
        }
    }

    private void btnDelete_Click(object sender, EventArgs e)
    {
      if (dgvACType.CurrentRow == null)
        return;
      DialogResult res = Common.Globals.ShowQuestion(string.Format(Resources.QuestionAboutDeleting, dgvACType.CurrentRow.Cells[1].Value));
      if (res != DialogResult.OK)
        return;
      RemoveCurrentModification();
      FillTable();
    }

    private void RemoveCurrentModification()
    {
        if (CurrentTypeId < MinModTypeId)
            MessageBox.Show(Resources.CantDeleteMainACType);
        else
            if (!DataManager.Instance.RemoveAcType(CurrentTypeId))
                MessageBox.Show(Resources.ErrorSavingDataOnServer);
    }

    private void helicopterDataGridView_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
    {
    }

    private void helicopterDataGridView_RowsRemoved(object sender, DataGridViewRowsRemovedEventArgs e)
    {
    }

    protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
    {
      const int WM_KEYDOWN = 0x100;
      const int WM_SYSKEYDOWN = 0x104;

      if ((msg.Msg == WM_KEYDOWN) || (msg.Msg == WM_SYSKEYDOWN))
      {
        switch (keyData)
        {
          case Keys.Escape:
            Close();
            break;

          case Keys.A | Keys.Control:
            bAddMod.Focus();
            bAddMod.PerformClick();
            return true;
        }
      }
      return base.ProcessCmdKey(ref msg, keyData);
    }

    private void jobsReferenceDataGridView_KeyUp(object sender, KeyEventArgs e)
    {
      if (dgvACType.RowCount != 0 && e.KeyCode == Keys.Space)
        btnEdit_Click(null, EventArgs.Empty);
    }

    private void helicopterDataGridView_RowEnter(object sender, DataGridViewCellEventArgs e)
    {
        
    }

    private void dgvACType_CurrentCellChanged(object sender, EventArgs e)
    {
        bAddMod.Enabled = CurrentTypeId < MinModTypeId;
        bEditMod.Enabled = bDeleteMod.Enabled = CurrentTypeId >= MinModTypeId;
    }
  }
}