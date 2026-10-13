using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using KPK2.Properties;
using Sparc.KPK2.NFunctions;
using Globals = KPK2.Common.Globals;

namespace KPK2.GeneralForms.ServiceForm
{

  public partial class frmLTRSignals : Form
  {
    private const int numSlots = 16;
    private const int numChannels = 17;
    private const string colSlot = "cSlot";
    private const string colChannel = "cChannel";
    private const string colRange = "cRange";
    private const string colAlone = "cAlone";
    private const string colACDC = "cACDC";

    private const int numColId = 0;
    private const int numColSlot = 1;
    private const int numColChannel = 2;
    private const int numColRange = 3;
    private const int numColAlone = 4;
    private const int numColACDC = 5;
    private const int numColNote = 6;
    public int CurSignal = 0;


    public frmLTRSignals()
    {
      InitializeComponent();
    }

    private void frmLTRSignals_Load(object sender, EventArgs e)
    {
      FillLists();
      LoadData();
    }

    //private bool isLoadData = true;

    private void LoadData()
    {
      dgvSignals.ReadOnly = true;
      DataTable parameters = DataManager.Instance.GetAllLTRParams();
      int selectedRow = 0;
      int index = 0;
      int scrollIndex = 0;
      foreach (DataRow row in parameters.Rows)
      {
        index = dgvSignals.Rows.Add(row[numColId], Convert.ToInt32(row[numColSlot]), Convert.ToInt32(row[numColChannel]),
                            Convert.ToDouble(row[numColRange]), row[numColAlone], row[numColACDC]);
        if (Convert.ToInt32(row[numColId]) == CurSignal)
        {
          selectedRow = dgvSignals.Rows.Count - 2;
          scrollIndex = index;
        }
      }
      dgvSignals.FirstDisplayedScrollingRowIndex = scrollIndex;
      dgvSignals.Rows[selectedRow].Selected = true;
      dgvSignals.ReadOnly = false;
    }

    private void BindComboBoxColumnToDS(DataGridViewComboBoxColumn column, DataTable source, string displayMember,
                                        string valueMamber)
    {
      column.DataSource = source;
      column.DisplayMember = displayMember;
      column.ValueMember = valueMamber;
    }

    private DataTable PrepareChooseDT(string colName, string colValue, bool isInt)
    {
      DataTable dt = new DataTable();
      dt.Columns.Add(colName);
      if (isInt)
        dt.Columns.Add(colValue, Type.GetType("System.Int32"));
      else
        dt.Columns.Add(colValue);
      return dt;
    }

    private void FillLists()
    {
      const string colName = "Name";
      const string colValue = "Value";
      DataTable chooseDt = PrepareChooseDT(colName, colValue, true);
      DataGridViewColumn curColumn = dgvSignals.Columns[colSlot];
      //Добавляем слоты в список
      if (curColumn != null)
      {
        for (int i = 1; i <= numSlots; i++)
          chooseDt.Rows.Add(i, i);
        BindComboBoxColumnToDS((DataGridViewComboBoxColumn) curColumn, chooseDt, colName, colValue);
        curColumn.CellTemplate.Value = 1;
      }
      //Добавляем каналы в список
      chooseDt = PrepareChooseDT(colName, colValue, true);
      curColumn = dgvSignals.Columns[colChannel];
      if (curColumn != null)
      {
        for (int i = 1; i <= numChannels; i++)
          chooseDt.Rows.Add(i, i);
        BindComboBoxColumnToDS((DataGridViewComboBoxColumn) curColumn, chooseDt, colName, colValue);
        curColumn.CellTemplate.Value = 1;
      }
      chooseDt = new DataTable();
      chooseDt.Columns.Add(colName);
      chooseDt.Columns.Add(colValue, Type.GetType("System.Double"));
      curColumn = dgvSignals.Columns[colRange];
      if (curColumn != null)
      {
        chooseDt.Rows.Add(10.ToString(), 10);
        chooseDt.Rows.Add(2.5.ToString(), 2.5);
        chooseDt.Rows.Add(0.4.ToString(), 0.4);
        chooseDt.Rows.Add(0.6.ToString(), 0.6);
        chooseDt.Rows.Add(0.15.ToString(), 0.15);
        chooseDt.Rows.Add(3.ToString(), 3);
        chooseDt.Rows.Add(1.ToString(), 1);
        chooseDt.Rows.Add(2.ToString(), 2);
        chooseDt.Rows.Add(0.3.ToString(), 0.3);
        chooseDt.Rows.Add(0.1.ToString(), 0.1);

        BindComboBoxColumnToDS((DataGridViewComboBoxColumn) curColumn, chooseDt, colName, colValue);
        curColumn.CellTemplate.Value = 10;
      }
      chooseDt = PrepareChooseDT(colName, colValue, false);
      chooseDt.Rows.Add("Да", "True");
      chooseDt.Rows.Add("Нет", "False");
      curColumn = dgvSignals.Columns[colAlone];
      if (curColumn != null)
      {
        BindComboBoxColumnToDS((DataGridViewComboBoxColumn) curColumn, chooseDt, colName, colValue);
        curColumn.CellTemplate.Value = "Нет";
      }

      chooseDt = PrepareChooseDT(colName, colValue, false);
      chooseDt.Rows.Add("DC", "True");
      chooseDt.Rows.Add("AC", "False");
      curColumn = dgvSignals.Columns[colACDC];
      if (curColumn != null)
      {
        BindComboBoxColumnToDS((DataGridViewComboBoxColumn) curColumn, chooseDt, colName, colValue);
        curColumn.CellTemplate.Value = "AC";
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
      foreach (DataGridViewRow row in dgvSignals.Rows)
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
        else if (isChangedRow)
          saved &= SaveChangedRow(row.Index);
      }
      return saved;
    }

    private bool SaveChangedRow(int rowIndex)
    {
      DataGridViewCellCollection coll = dgvSignals.Rows[rowIndex].Cells;
      foreach (DataGridViewCell cell in coll)
        if (cell.Value == null)
          cell.Value = "";
      return DataManager.Instance.UpdateLTRSignal(Convert.ToInt32(coll[numColId].Value),
                                                    Convert.ToInt32(coll[numColSlot].Value),
                                                    Convert.ToInt32(coll[numColChannel].Value),
                                                    Globals.MoveToDouble(coll[numColRange].Value.ToString()),
                                                    Convert.ToBoolean(coll[numColAlone].Value),
                                                    Convert.ToBoolean(coll[numColACDC].Value), "");
    }

    private bool SaveNewRow(int rowIndex)
    {
      DataGridViewCellCollection coll = dgvSignals.Rows[rowIndex].Cells;
      foreach (DataGridViewCell cell in coll)
        if (cell.Value == null)
          cell.Value = "";
      coll[numColId].Value = DataManager.Instance
                                        .InsertNewLTRSignal(Convert.ToInt32(coll[numColSlot].Value),
                                                            Convert.ToInt32(coll[numColChannel].Value),
                                                            Globals.MoveToDouble(coll[numColRange].Value.ToString()),
                                                            Convert.ToBoolean(coll[numColAlone].Value),
                                                            Convert.ToBoolean(coll[numColACDC].Value), "");
      if (Convert.ToInt32(coll[numColId].Value) != 0)
      {
        RowSaved(rowIndex);
        return true;
      }
      return false;
    }

    private void RowSaved(int rowIndex)
    {
      foreach (DataGridViewCell cell in dgvSignals.Rows[rowIndex].Cells)
        cell.Style.BackColor = frmCalibrate.UnchangedColor;
    }

    private void dgvSignals_CellValueChanged(object sender, DataGridViewCellEventArgs e)
    {
      if (((DataGridView) sender).ReadOnly)
        return;
      if (e.RowIndex >= 0)
      {
        ((DataGridView) sender).Rows[e.RowIndex].Cells[e.ColumnIndex].Style.BackColor = Color.Red;
        bSave.Enabled = true;
      }
    }

    private void dgvSignals_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
    {
      if (((DataGridView) sender).ReadOnly)
        return;
      foreach (DataGridViewCell cell in ((DataGridView) sender).Rows[e.RowIndex - 1].Cells)
        cell.Style.BackColor = frmCalibrate.ChangedColor;
    }

    private void bOK_Click(object sender, EventArgs e)
    {
      CurSignal = Convert.ToInt32(dgvSignals.SelectedRows[0].Cells[numColId].Value);
      Close();
    }
  }
}