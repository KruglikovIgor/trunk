using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Text;
using KPK2.Common.DissFunctions;
using KPK2.GeneralForms.AddForms;
using KPK2.Properties;
using KPK2.Reports;
using Sparc.KPK2.NFunctions;
using Sparc.KPK2.NFunctions.Enums;
using Res = Sparc.KPK2.Resources.Properties.Resources;
using Globals = Sparc.KPK2.NFunctions.Globals;

namespace KPK2.GeneralForms
{
  /// <summary>
  /// Добвляем новую проверку ДИСС-ов
  /// </summary>
  public partial class frmNewCheck : Form
  {
    /// <summary>
    /// Связанный список: ID - Ф.И.О. (для записи ID в БД
    /// по выбранному Ф.И.О.)
    /// </summary>
    private Dictionary<int, string> lstFullName;

    /// <summary>
    /// флаг-существует запись в БД 
    /// </summary>
    private bool isInDb;

    /// <summary>
    /// показывать форму 
    /// </summary>
    public bool isShow = true;

    /// <summary>
    /// Тип ДИСС-а - через конструктор 
    /// </summary>
    private string strType = "";

    private DevicesID _deviceType;

    private int _deviceTypeId;

    private int _colTSN = 3;
    private int _colTSO = 4;

    private int _testId;
    private bool _existingCheck = false;

    private bool _changed = false;

    /// <summary>
    /// Модификация ДИСС
    /// </summary>
    private Modification _dissMod;

    /// <summary>
    /// Тип блока 5 для ДИСС-15
    /// </summary>
    private Block5Type _curBlock5Type = Block5Type.None;

    private ModeApplication _modeApplication;

    private int _testIdDataMigration;
    private int _flagsNewCheckWithDataMigration;

        //private int _block5Id = 3;

        public frmNewCheck()
    {
      InitializeComponent();
    }

    public frmNewCheck(DevicesID device, Modification mod, ModeApplication modeApplication, int testId = 0)
      : this()
    {
      _existingCheck = true;
      _dissMod = mod;
      _deviceType = device;
      _deviceTypeId = (int)device;
      _testId = testId;
      strType = Globals.GetDissCaption(device, mod);
      _modeApplication = modeApplication;
      LoadCommon();
    }

    private void LoadCommon()
    {
            if(Mode.GetMode().ModeApplication == ModeApplication.RepairCheck)
            {
                tbNote.Visible = false;
                lbl_Note.Visible = false;
                label1.Visible = false;
                cbFormTO.Visible = false;
                label4.Visible = false;
                dgvBlocks.Visible = false;
                lbl.Location = new Point(lbl.Location.X, cbFormTO.Location.Y + cbFormTO.Size.Height + 10);

            }
      if (_deviceTypeId == 0)
      {
        MessageBox.Show(Resources.ErrorCaption);
        return;
      }
      dateTimePicker.MaxDate = DateTime.Now; // Max дата - текущая
      try
      {
        tbProductNumber.AutoCompleteCustomSource.AddRange(
          DataManager.Instance.GetUniqueDeviceNumbers(_deviceTypeId).ToArray());

        // Марки, №№ и Опозновательные знаки вертолетов для подсказки - из всей БД);
        FillACTypes();
        tbHelicopterNumber.AutoCompleteCustomSource.AddRange(
          DataManager.Instance.GetUniqueHelicopterNumbers(_deviceTypeId).ToArray());
        tbHelicopterSign.AutoCompleteCustomSource.AddRange(
          DataManager.Instance.GetUniqueHelicopterSigns(_deviceTypeId).ToArray());

        // Ф.И.О. исполнителя и его ID 

        lstFullName = DataManager.Instance.GetWorkingEmployees();
                foreach (KeyValuePair<int, string> rec in lstFullName)
                {
                    cbEmployee.Items.Add(rec.Value);
                    cbMestre.Items.Add(rec.Value);
                    cbMestreBTK.Items.Add(rec.Value);
                }
        if (lstFullName.Count == 0)
          MessageBox.Show(Resources.CantInitiateNewCheckNoExecutors, Resources.ApplicationName, MessageBoxButtons.OK,
                          MessageBoxIcon.Warning);
      }
      catch
      {
        isShow = false;
        MessageBox.Show(Resources.ErrorLoadFromDb, Resources.ErrorCaption);
      } // не будем открывать форму 
      tbProductNumber.Focus();
    }

    public frmNewCheck(DevicesID device, Modification mod, ModeApplication modeApplication, List<string> dataMigration, int testIdDataMigration, int flagsNewCheckWithDataMigration, DataTable dtDataMigrationNew)
        : this()
    {
        _existingCheck = true;
        _dissMod = mod;
        _deviceType = device;
        _deviceTypeId = (int)device;
        _testId = 0;
        _flagsNewCheckWithDataMigration = flagsNewCheckWithDataMigration;
        _testIdDataMigration = testIdDataMigration;

        strType = Globals.GetDissCaption(device, mod);
        _modeApplication = modeApplication;
        LoadCommon();

        tbProductNumber.Text = dataMigration[0];
        dateTimePicker.Text = dataMigration[1];
        cbEmployee.Text = dataMigration[3];
        cbHelicopterType.Text = dataMigration[4];
        tbHelicopterNumber.Text = dataMigration[5];
        tbHelicopterSign.Text = dataMigration[6];
        DataTable values = DataManager.Instance.GetPassportValues(_testIdDataMigration);
        if (values.Rows.Count > 0)
            bShowPassportData.Visible = true;
        bWork.Enabled = true;
        DataTable dtDataMigration = new DataTable();
        DataManager.Instance.FillCompositionWithIdBlocks(_testIdDataMigration, dtDataMigration);
        foreach (DataRow dr in dtDataMigration.Rows)
            dgvBlocks.Rows.Add(dr[0], dr[1], dr[2], dr[3], dr[4], "X");
    }

        // Закрываем форму + 
        private void btnClose_Click(object sender, EventArgs e)
    {
      //isOpenForm = false;
      if (ParentForm != null)
        ((frmMain)ParentForm).EnableMenu(true);
      Close();
    }

    private bool IsDataExists()
    {
      return tbProductNumber.Text != "" || tbHelicopterNumber.Text != "" ||
             cbHelicopterType.Text != "" || tbHelicopterSign.Text != "" ||
             tbNote.Text != "" || cbEmployee.SelectedIndex != -1 || cbFormTO.Text != "" ||
             dateTimePicker.Text != "01.01.1970" || _changed;
    }

    // Закрытие формы + 
    private void frmNewCheck_FormClosing(object sender, FormClosingEventArgs e)
    {
      if (!_existingCheck)
        e.Cancel = CheckUnsavedData();
    }

    private bool CheckUnsavedData()
    {
      // Есть какие-то данные
      bool bl = IsDataExists() && !isInDb;
      if (_existingCheck)
        bl = _changed;
      if (bl) // В БД ничего не писали и есть какие-то данные
      {
        if (
          MessageBox.Show(Resources.CloseFormWithUnsavedData, Resources.ApplicationName, MessageBoxButtons.YesNo,
                          MessageBoxIcon.Question) ==
          DialogResult.Yes)
        {
          MenuItemEnabled();
          return false; // закрываем форму
        }
        return true; // оставляем форму открытой
      }
      MenuItemEnabled();
      return false;
    }

    /// <summary>
    /// Разблокируем Меню на родительской форме
    /// </summary>
    private void MenuItemEnabled()
    {
      if (ParentForm != null)
      {
        ((frmMain)ParentForm).EnableMenu(true);
      }
    }

    const int TotalRunColumn = 3;
    const int LastRunColumn = 4;

    // Сохранить в БД + 
    private void btnSave_Click(object sender, EventArgs e)
    {
      int i = 0;
      bSave.Enabled = false;
      bClose.Text = Resources.Close;
      errProvider.Clear();
      if (cbEmployee.Text == "" || tbProductNumber.Text == "")
      {
        MessageBox.Show(Resources.FillFieldRequired, Resources.ApplicationName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
        if (cbEmployee.Text == "")
          errProvider.SetError(cbEmployee, Resources.ErrorEmployeeNotSelected);
        if (tbProductNumber.Text == "")
          errProvider.SetError(tbProductNumber, Resources.ErrorProductNumberIsEmpty);
        //if (IsBlock5TypeShow() && cbBlock5Type.SelectedIndex == -1)
        //  errProvider.SetError(cbBlock5Type, Resources.Block5TypeMissing);
        bSave.Enabled = true;
        return;
      }
      
      int employeeId = 0;
            int? mestreNameId = null;
            int? mestreBTKNameId = null;
            foreach (KeyValuePair<int, string> rec in lstFullName)
            {
                if (cbEmployee.Text == rec.Value)
                {
                    employeeId = rec.Key;
                }

                if(cbMestre.Text == rec.Value)
                {
                    mestreNameId = rec.Key;
                }

                if(cbMestreBTK.Text == rec.Value)
                {
                    mestreBTKNameId = rec.Key;
                }
            }

      try
      {
        SaveTest(_testId != 0, employeeId, mestreNameId, mestreBTKNameId);
      }
      catch
      {
        MessageBox.Show(Resources.CantConnectDb, Resources.ErrorCaption, MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
        Close();
        bSave.Enabled = true;
        return;
      }

        DataTable values = DataManager.Instance.GetPassportValues(_testId);
        if (values.Rows.Count > 0)
        {
            foreach (DataRow dr in values.Rows)
            {
                if (dr.ItemArray[2].ToString() != "") i++;
            }
            if (i < values.Rows.Count)
            {
                MessageBox.Show(Res.EnterPassportData, Res.Requirement);
                bShowPassportData.Visible = true;
            }
        }
        else
        bWork.Enabled = true; // разрешаем начать проверку
        _changed = false;
        isInDb = true;
    }

    private void SaveTypeBlock5(int testId)
    {
      if (!IsBlock5TypeShow())
        return;
      if (!DataManager.Instance.SetAddParam(testId, DataManager.AddParameters.TypeBlock5, _curBlock5Type.ToString()))
        MessageBox.Show(Resources.CantSaveDbParam);
    }

    private void SaveTest(bool isUpdate, int employeeId, int? mestreId = null, int? mestreBTKId = null)
    {
      int? result;
      if (isUpdate)
      {
        if (!DataManager.Instance.UpdateTesting(_testId, tbProductNumber.Text, dateTimePicker.Value,
                                                cbHelicopterType.Text, tbHelicopterSign.Text,
                                                tbHelicopterNumber.Text, dtpCheckDate.Value,
                                                employeeId, tbNote.Text, cbFormTO.Text,
                                                (int)nudTotalRun.Value,
                                                (int)nudLastRun.Value,
                                                mestreId, mestreBTKId))
          throw new Exception("Can't update testing in database!");
      }
      else
      {
        result = DataManager.Instance.InsertTesting(_deviceTypeId, tbProductNumber.Text,
                                                             dateTimePicker.Value,
                                                             cbHelicopterType.Text,
                                                             tbHelicopterSign.Text,
                                                             tbHelicopterNumber.Text, DateTime.Now,
                                                             employeeId, tbNote.Text, cbFormTO.Text,
                                                             (int)nudTotalRun.Value, (int)nudLastRun.Value, _modeApplication,
                                                             mestreId, mestreBTKId);
        if (result == null)
          throw new Exception("Can't insert testing in database!");
        _testId = (int)result;
        SaveDissModification();
      }

      SaveTypeBlock5(_testId);

      foreach (DataGridViewRow dr in dgvBlocks.Rows)
      {
        int? totalRun = null;
        object gridValue = dr.Cells[TotalRunColumn].Value;
        if (gridValue != null)
          totalRun = string.IsNullOrEmpty(gridValue.ToString()) ? null : (int?)Convert.ToInt32(gridValue);
        int? lastRun = null;
        gridValue = dr.Cells[LastRunColumn].Value;
        if (gridValue != null)
          lastRun = String.IsNullOrEmpty(gridValue.ToString()) ? null : (int?)Convert.ToInt32(gridValue);
        string blockNumber = String.Empty;
        if (dr.Cells[2].Value != null)
          blockNumber = dr.Cells[2].Value.ToString();
        if (isUpdate)
        {
          if (
            !DataManager.Instance.UpdateComposition(_testId,
                                                    Convert.ToInt32(dr.Cells[0].Value),
                                                    blockNumber,
                                                    totalRun,
                                                    lastRun))
            throw new Exception("Can't update composition in database");
        }
        else
        {
          if (
            !DataManager.Instance.InsertComposition(_testId,
                                                    Convert.ToInt32(dr.Cells[0].Value),
                                                    blockNumber,
                                                    totalRun,
                                                    lastRun))
            throw new Exception("Can't insert composition in database");
        }
      }
    }

    private void SaveDissModification()
    {
      if (_dissMod == Modification.Diss32_90A)
        DataManager.Instance.SetAddParam(_testId, DataManager.AddParameters.Modification, _dissMod.ToString());
      if (_dissMod == Modification.Diss15G)
        DataManager.Instance.SetAddParam(_testId, DataManager.AddParameters.Modification, _dissMod.ToString());
      //if(_dissMod == Modification.Diss15D) 
      //    DataManager.Instance.SetAddParam(_testId, DataManager.AddParameters.Modification, _dissMod.ToString());
    }

    // Запускаем проверку + 
    private void bWork_Click(object sender, EventArgs e)
    {
      if (_existingCheck)
        if (CheckUnsavedData())
          return;

      int id;
      try
      {
        id = _testId;
      }
      catch
      {
        MessageBox.Show(Resources.CantReadDb, Resources.ErrorCaption);
        return;
      }
      Form frm = null;
      frm = KPK2.Common.Globals.GetDeviceForm(_deviceType, _dissMod, id, tbProductNumber.Text, _curBlock5Type);
      if (frm != null)
      {
        frm.MdiParent = null;
        frm.MdiParent = Globals.MainForm;
        frm.Show();
        frm.WindowState = FormWindowState.Maximized;
        Close();
      }
      else
        MessageBox.Show(Res.CantFindForm, Res.Error);
    }

    private void txtBox_Validated(object sender, EventArgs e)
    {
      ((TextBox)sender).Text = ((TextBox)sender).Text.Trim();
    }

    //private int[] _excludeBlocks5Diss15 = new[] { 3,4 };
    private int[] _excludeBloksDiss15 = new[] { 9, 10, 11 };
    //private int[] _excludeBloksDiss15G = new[] { 11 };
    private int[] _excludeBloksDiss32 = new[] { 7, 9, 12, 13 };

    private void frmNewCheck_Load(object sender, EventArgs e)
    {
        dtpCheckDate.Value = DateTime.Now;

        if (Mode.GetMode().ModeApplication == ModeApplication.RepairCheck)
        {
            lblFullNameMestre.Visible = true;
            lblFullNameMestreBTK.Visible = true;
            cbMestre.Visible = true;
            cbMestreBTK.Visible = true;
        }

        Text = string.Format(_existingCheck ? Resources.CheckOf : Resources.NewCheckOf, strType);
      cbFormTO.Items.AddRange(DataManager.Instance.GetTOForms().ToArray());
     // lBlock5Type.Visible = cbBlock5Type.Visible = IsBlock5TypeShow();
      if (_testId != 0)
      {
        LoadExisting();
        return;
      }
            if (_flagsNewCheckWithDataMigration == 0)
            {
                DataTable dt = new DataTable();
                DataManager.Instance.FillCompositionReference(_deviceTypeId, dt);
                foreach (DataRow dr in dt.Rows)
                {
                    if (_deviceType == DevicesID.Diss15 && _dissMod != Modification.Diss15G)
                        if (ArrContains(_excludeBloksDiss15, Convert.ToInt32(dr[0])))
                            continue;

                    //if (_deviceType == DevicesID.Diss15 && _dissMod == Modification.Diss15G)
                    //    if (ArrContains(_excludeBloksDiss15G, Convert.ToInt32(dr[0])))
                    //        continue;
                    //if (_deviceType == DevicesID.Diss15)
                    //  if (ArrContains(_excludeBlocks5Diss15, Convert.ToInt32(dr[0])))
                    //    continue;
                    if (_deviceType == DevicesID.Diss32_90 && _dissMod == Modification.Diss32_90A)
                        if (ArrContains(_excludeBloksDiss32, Convert.ToInt32(dr[0])))
                            continue;
                    dgvBlocks.Rows.Add(dr[0], dr[1], String.Empty, String.Empty, String.Empty, "X");
                }

                if (_deviceType == DevicesID.Diss15 && _dissMod != Modification.Diss15G)

                //if (_deviceType == DevicesID.Diss15 && _dissMod == Modification.Diss15G)
                //{
                //    DataManager.Instance.RemoveComposition(_testId, 11);
                //}

                //if (_deviceType == DevicesID.Diss15 && _dissMod == Modification.None)
                {
                    for (int i = 8; i < 10; i++)
                        DataManager.Instance.RemoveComposition(_testId, i);
                }

                if (_deviceType == DevicesID.Diss32 ||
                    _deviceType == DevicesID.Diss32_90 && _dissMod == Modification.Diss32_90A)
                {
                    DataManager.Instance.RemoveComposition(_testId, 7);
                    DataManager.Instance.RemoveComposition(_testId, 9);
                    for (int i = 11; i < 13; i++)
                        DataManager.Instance.RemoveComposition(_testId, i);
                }
            }
            else
            {
                //DataTable dt = new DataTable();
                //DataManager.Instance.FillCompositionWithIdBlocks(_testId, dt);
                DataManager.Instance.FillTesting(_testIdDataMigration, (int)Mode.GetMode().ModeApplication, _headerDataTable);
                if (_headerDataTable[0]["FormTO"] != DBNull.Value)
                    SetComboBoxValue(cbFormTO, _headerDataTable[0].FormTO);
                if (_headerDataTable[0]["TotalRun"] != DBNull.Value)
                    nudTotalRun.Value = Convert.ToDecimal(_headerDataTable[0].TotalRun);
                if (_headerDataTable[0]["LastRun"] != DBNull.Value)
                    nudLastRun.Value = Convert.ToDecimal(_headerDataTable[0].LastRun);
            }
    }

    private bool IsBlock5TypeShow()
    {
      return _deviceType == DevicesID.Diss15;
    }

    private dsReport.HeaderDataTable _headerDataTable = new dsReport.HeaderDataTable();

    private void LoadExisting()
    {
      DataTable dt = new DataTable();
      DataManager.Instance.FillCompositionWithIdBlocks(_testId, dt);
      foreach (DataRow dr in dt.Rows)
        dgvBlocks.Rows.Add(dr[0], dr[1], dr[2], dr[3], dr[4], "X");
      DataManager.Instance.FillTesting(_testId, (int)Mode.GetMode().ModeApplication, _headerDataTable);
      if (_headerDataTable[0]["FormTO"] != DBNull.Value)
        SetComboBoxValue(cbFormTO, _headerDataTable[0].FormTO);
      tbProductNumber.Text = _headerDataTable[0].ProductNumber;
      if (_headerDataTable[0]["EmployeeName"] != DBNull.Value)
        SetComboBoxValue(cbEmployee, _headerDataTable[0].EmployeeName);
      cbHelicopterType.Text = _headerDataTable[0].HelicopterMark;
      tbHelicopterNumber.Text = _headerDataTable[0].HelicopterNumber;
      tbHelicopterSign.Text = _headerDataTable[0].HelicopterSign;
      tbNote.Text = _headerDataTable[0].Note;
      if (_headerDataTable[0]["TotalRun"] != DBNull.Value)
        nudTotalRun.Value = Convert.ToDecimal(_headerDataTable[0].TotalRun);
      if (_headerDataTable[0]["LastRun"] != DBNull.Value)
        nudLastRun.Value = Convert.ToDecimal(_headerDataTable[0].LastRun);
            if (_headerDataTable[0]["MestreName"] != DBNull.Value)
            {
                SetComboBoxValue(cbMestre, _headerDataTable[0].MestreName);
            }
            if (_headerDataTable[0]["MestreBTKName"] != DBNull.Value)
            {
                SetComboBoxValue(cbMestreBTK, _headerDataTable[0].MestreBTKName);
            }
      bSave.Enabled = false;
      DataTable values = DataManager.Instance.GetPassportValues(_testId);
      if (values.Rows.Count > 0)
        bShowPassportData.Visible = true;
      bWork.Enabled = true;
    }

    private void FillACTypes()
    {
      DataTable tb = DataManager.Instance.GetACTypes();
      List<string> acTypes = new List<string>();
      if (tb.Columns.Count > 1)
        for (int i = 0; i < tb.Rows.Count; i++)
          acTypes.Add(tb.Rows[i][1].ToString());
      cbHelicopterType.Items.Clear();
      cbHelicopterType.Items.AddRange(acTypes.ToArray());
      cbHelicopterType.AutoCompleteSource = AutoCompleteSource.ListItems;
    }

    private void SetComboBoxValue(ComboBox cb, string str)
    {
      if (!String.IsNullOrEmpty(str))
        for (int i = 0; i < cb.Items.Count; i++)
          if (cb.Items[i].ToString() == str)
          {
            cb.SelectedIndex = i;
            return;
          }
    }

    private bool ArrContains(int[] arr, int value)
    {
      for (int i = 0; i < arr.Length; i++)
        if (arr[i] == value)
          return true;
      return false;
    }

    private const int colDelete = 5;

    private void dgvBlocks_CellClick(object sender, DataGridViewCellEventArgs e)
    {
      if (e.RowIndex == -1)
        return;
      if (e.ColumnIndex == colDelete)
        if (MessageBox.Show(Resources.DeleteBlockRow, Resources.Attention, MessageBoxButtons.YesNo) ==
            DialogResult.Yes)
        {
          if (_testId != 0)
            if (
              !DataManager.Instance
                          .RemoveComposition(_testId,
                                             Convert.ToInt32(dgvBlocks.Rows[e.RowIndex].Cells[0].Value)))
            {
              MessageBox.Show(Resources.ErrorDeleteBlockFromDb);
              return;
            }
          dgvBlocks.Rows.RemoveAt(e.RowIndex);
        }
    }

    private void cbFormTO_Leave(object sender, EventArgs e)
    {
      cbFormTO.Text = cbFormTO.Text.Trim();
    }

    private void bShowPassportData_Click(object sender, EventArgs e)
    {
        if (_flagsNewCheckWithDataMigration == 0)
        {
            frmPassportData frm = new frmPassportData(_testId);
            frm.ShowDialog();
            bWork.Enabled = true;
            // разрешаем начать проверку
        }
        else
        {
            frmPassportData frm = new frmPassportData(_testIdDataMigration);
            frm.ShowDialog();
        }
    }

    private void dgvBlocks_CellValueChanged(object sender, DataGridViewCellEventArgs e)
    {
      if ((e.RowIndex == -1) || (e.ColumnIndex == -1))
        return;
      DataGridView dgv = (DataGridView)sender;
      if (dgv.Rows[e.RowIndex].Cells[e.ColumnIndex].Value != null)
      {
        string curValue = dgv.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString();
        if (curValue != curValue.Trim())
          dgv.Rows[e.RowIndex].Cells[e.ColumnIndex].Value =
            curValue.Trim();
        CheckGridValues(e);
      }
      _changed = true;
      bSave.Enabled = true;
    }

    private void CheckGridValues(DataGridViewCellEventArgs e)
    {
      if ((e.ColumnIndex == _colTSN) || (e.ColumnIndex == _colTSO))
      {
        if (String.IsNullOrEmpty(dgvBlocks.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString().Trim()))
          return;
        int value;
        if (!int.TryParse(dgvBlocks.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString(), out value))
        {
          MessageBox.Show(Resources.ErrorNumber, Resources.Error);
          dgvBlocks.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = String.Empty;
        }
        else
        {
          if (value <= 0)
          {
            MessageBox.Show(Resources.TimeCantBeNegative, Resources.ErrorCaption);
            dgvBlocks.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = String.Empty;
            return;
          }

          if (e.ColumnIndex == _colTSN)
          {
            if (!String.IsNullOrEmpty(dgvBlocks.Rows[e.RowIndex].Cells[_colTSO].Value.ToString()))
              if (Convert.ToInt32(dgvBlocks.Rows[e.RowIndex].Cells[_colTSN].Value) <
                  Convert.ToInt32(dgvBlocks.Rows[e.RowIndex].Cells[_colTSO].Value))
              {
                MessageBox.Show(Resources.TSNLessThanTSO, Resources.ErrorCaption);
                dgvBlocks.Rows[e.RowIndex].Cells[e.ColumnIndex].Value =
                  dgvBlocks.Rows[e.RowIndex].Cells[_colTSO].Value;
              }
          }
          else
          {
            if (!String.IsNullOrEmpty(dgvBlocks.Rows[e.RowIndex].Cells[_colTSN].Value.ToString()))
              if (Convert.ToInt32(dgvBlocks.Rows[e.RowIndex].Cells[_colTSN].Value) <
                  Convert.ToInt32(dgvBlocks.Rows[e.RowIndex].Cells[_colTSO].Value))
              {
                MessageBox.Show(Resources.TSNLessThanTSO, Resources.ErrorCaption);
                dgvBlocks.Rows[e.RowIndex].Cells[e.ColumnIndex].Value =
                  dgvBlocks.Rows[e.RowIndex].Cells[_colTSN].Value;
              }
          }
        }
      }
    }

    private void control_ValueChanged(object sender, EventArgs e)
    {
        errProvider.Clear();
        bSave.Enabled = true;
    }

    private void anyBox_ValueChanged(object sender, EventArgs e)
    {
      if (sender is NumericUpDown)
      {
        if (nudTotalRun.Value < nudLastRun.Value)
        {
          MessageBox.Show(Resources.TSNLessThanTSO, Resources.ErrorCaption);
          if (sender.Equals(nudTotalRun))
            nudTotalRun.Value = nudLastRun.Value;
          else
            nudLastRun.Value = nudTotalRun.Value;
        }
      }
      control_ValueChanged(sender, e);
    }

    private void cb_ValueChanged(object sender, EventArgs e)
     {
        bSave.Enabled = true;
     }

    private void nudTotalRun_Leave(object sender, EventArgs e)
    {
      if (String.IsNullOrEmpty(nudTotalRun.Text))
        nudTotalRun.Value = 0;
    }

    private void nudLastRun_Leave(object sender, EventArgs e)
    {
      if (String.IsNullOrEmpty(nudLastRun.Text))
        nudLastRun.Value = 0;
    }

    private void cbHelicopterType_Leave(object sender, EventArgs e)
    {
      if (String.IsNullOrEmpty(cbHelicopterType.Text))
        return;
      string newACType = cbHelicopterType.Text;
      cbHelicopterType.SelectedItem = newACType;
      if (cbHelicopterType.SelectedIndex != -1)
        return;
      frmAddACType frm = new frmAddACType(newACType);
      frm.ShowDialog();
      newACType = frm.NewACType;
      FillACTypes();
      cbHelicopterType.Text = newACType;
      if (cbHelicopterType.SelectedIndex == -1)
        cbHelicopterType.Text = String.Empty;
    }

        private void dtpCheckDate_ValueChanged(object sender, EventArgs e)
        {
            errProvider.Clear();
            bSave.Enabled = true;
        }
    }
}