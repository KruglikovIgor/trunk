using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Windows.Forms;
using CrystalDecisions.CrystalReports.Engine;
using KPK2.Common;
using KPK2.GeneralForms.DissForms;
using KPK2.Properties;
using KPK2.Reports;
using Sparc.KPK2.NFunctions;
using Sparc.KPK2.NFunctions.Enums;
using Globals = Sparc.KPK2.NFunctions.Globals;
using Res = Sparc.KPK2.Resources.Properties.Resources;

namespace KPK2.GeneralForms 
{
  /// <summary>
  /// История проведения проверок ДИСС-ов 
  /// </summary>
  public partial class frmViewTest : Form
  {
    #region Определения

    /// <summary>
    /// true - показывать форму 
    /// </summary>
    public bool isShow = true;

    /// <summary>
    /// Для блокировки меню при запуске frmNewCheck 
    /// </summary>
    private bool isClose = true;

    /// <summary>
    /// При загрузке все отрубить
    /// </summary>
    private bool isLoad = true;

    /// <summary>
    /// Отмена фильтрации 
    /// </summary>
    private bool isCancel;

    /// <summary>
    /// ID проверки 
    /// </summary>
    private int testID = -1;

    /// <summary>
    /// Тип ДИСС - из конструктора
    /// </summary>
    private DevicesID _dissType;

    /// <summary>
    /// Модификация ДИСС
    /// </summary>
    private Modification _dissMod;

    private int _deviceTypeId;
    private Dictionary<int, string> lstFullName;
    private DataTable dtView;
    private DataTable dtViewTemp;
    private DataTable dtDataMigration;
    private DateTime _dtMin = DateTime.MinValue.AddYears(1980), _dtMax;
    private ModeApplication _modeApplication;
    #endregion

    public frmViewTest(ModeApplication modeApplication)
    {
      InitializeComponent();
      dtpFrom.Checked = false;
      dtpFrom.MaxDate = DateTime.Today.AddDays(-1);
      dtpTo.Checked = false;
      dtpTo.MaxDate = DateTime.Today;
      _modeApplication = modeApplication;
    }

    public frmViewTest(DevicesID device, ModeApplication modeApplication = ModeApplication.DefaultCheck)
      : this(modeApplication)
    {
      _dissType = device;
      _deviceTypeId = (int) device;
      if (!isShow) return;
    }

    private void FillMainTable(DataTable dtView)
    {
        dtViewTemp = new DataTable();
        dtViewTemp = dtView.Copy();
        dtViewTemp.Columns.Add("NumberOfVerification").SetOrdinal(0);
        for (int i = 0; i < dtView.Rows.Count; i++)
        {
            dtViewTemp.Rows[i][0] = i + 1;
        }
        dgvChecks.DataSource = dtViewTemp;
        }

    private DataTable GetTestings()
    {
      dtView = new DataTable();
      for (int i = 1; i < 12; i++)
      {
          dtView.Columns.Add(dgvChecks.Columns[i].DataPropertyName);
      }
      DataManager.Instance.GetTestings(_deviceTypeId, _dissMod, _modeApplication , ref dtView);
      return dtView;
    }

    private void LockComboBoxes(bool state)
    {
      cbNumber.Enabled = !state;
      cbPerformer.Enabled = !state;
      dtpFrom.Enabled = !state;
      dtpTo.Enabled = !state;
    }

    private void FillComboBoxes(DataTable dtView)
    {
      LockComboBoxes(true);

      cbNumber.Items.Clear();
      cbNumber.Items.Add(Resources.All);

      List<string> devNums =  GetUniqueDeviceNumbers(dtView);
      //cbNumber.Items.AddRange(DataManager.Instance.GetUniqueDeviceNumbers(_deviceTypeId).ToArray());
      cbNumber.Items.AddRange(devNums.ToArray());
      cbNumber.SelectedItem = Resources.All;

      cbPerformer.Items.Clear();
      cbPerformer.Items.Add(Resources.All);
      lstFullName = DataManager.Instance.GetEmployeesForFilter();
      foreach (KeyValuePair<int, string> rec in lstFullName)
        cbPerformer.Items.Add(rec.Value);
      cbPerformer.SelectedItem = Resources.All;

      dtpTo.Checked = false;
      dtpFrom.Checked = false;

      LockComboBoxes(false);
    }

    private const int DeviceTypeIdColumn = 9;

    private List<string> GetUniqueDeviceNumbers(DataTable dtView)
    {
      List<string> result = new List<string>();
      for (int i = 0; i < dtView.Rows.Count; i++)
      {
        string devNum = dtView.Rows[i][0].ToString();
        if (!result.Contains(devNum))
          result.Add(devNum);
      }
            return result;
        }

    private void frmViewTest_Load(object sender, EventArgs e)
    {
      // заголовок формы
      Text = string.Format(Resources.TestHistory, Globals.GetDissCaption(_dissType, _dissMod));
      Modification mod;
       DevicesID id = (DevicesID)DataManager.Instance.GetLastCheckDissId(out mod, (int)Mode.GetMode().ModeApplication);
      FillDissTypes(id, mod);
      // при запущенной Аттестации или работе по проверке - 
      // запрет на добавление новой проверки
      if (ParentForm != null)
        for (int i = 0; i < ParentForm.MdiChildren.Length; i++)
          if (ParentForm.MdiChildren[i] is frmStandAttest ||
              ParentForm.MdiChildren[i] is frmWork15 ||
              ParentForm.MdiChildren[i] is frmWork32)
            bAdd.Enabled = false;
      if(Mode.GetMode().ModeApplication == ModeApplication.RepairCheck) 
      { 
        bRepair.Text = "Карта ремонта";
      }
      isLoad = false;
    }

    Dictionary<string, KeyValuePair<int, Modification>> listDevices = new Dictionary<string, KeyValuePair<int, Modification>>();

    private void FillDissTypes(DevicesID id, Modification mod)
    {
      cbDissType.Enabled = false;
      Dictionary<int, string> devices = DataManager.Instance.GetDevicesShort();

      var devIds = Enum.GetValues(typeof(DevicesID));

      foreach (var item in devIds)
      {
          if (((DevicesID)item) == DevicesID.None)
              continue;
          var mods = Globals.GetAvailableModifications((DevicesID)item);
          int mainId = FindIdIn(devices, Globals.GetDissCaption((DevicesID)item, Modification.None));
          if (mainId == -1) continue; // пропускаем устройство, которого нет в БД

          foreach (var modItem in mods)
          {
              if (CommonSettings.Default.GetCheckEnable(new DeviceAndMod { DeviceId = (DevicesID)item, Modification = modItem }))
                  listDevices.Add(Globals.GetDissCaption((DevicesID)item, modItem), new KeyValuePair<int, Modification>(mainId, modItem));
          }
      }

      //foreach (KeyValuePair<int, string> kp in devices)
      //  listDevices.Add(kp.Value, new KeyValuePair<int, Modification>(kp.Key, Modification.None));
      
      //string diss32_90A = Globals.GetDissCaption(DevicesID.Diss32_90, Modification.Diss32_90A);
      //string diss32_90 = Globals.GetDissCaption(DevicesID.Diss32_90, Modification.None);

      //listDevices.Add(diss32_90A,
      //  new KeyValuePair<int, Modification>(listDevices[diss32_90].Key, Modification.Diss32_90A));

      //if (CommonSettings.Default.IsNSK)
      //{
      //  string diss15G = Globals.GetDissCaption(DevicesID.Diss15, Modification.Diss15G);
      //  string diss15 = Globals.GetDissCaption(DevicesID.Diss15, Modification.None);

      //  listDevices.Add(diss15G,
      //    new KeyValuePair<int, Modification>(listDevices[diss15].Key, Modification.Diss15G));
      //}

      List<string> deviceNames = new List<string>();
      foreach (var item in listDevices)
        deviceNames.Add(item.Key);

      deviceNames.Sort((str, str2) => str2.CompareTo(str));
      
      cbDissType.Items.AddRange(deviceNames.ToArray());
      
      //if (!CommonSettings.Default.IsSilkway)
      //  cbDissType.Items.Remove(Resources.Diss13);
      //if (!CommonSettings.Default.IsNSK)
      //{
      //  cbDissType.Items.Remove(Res.Diss3228);
      //  cbDissType.Items.Remove(diss32_90);
      //}
      //if (!CommonSettings.Default.ShowDiss32)
      //  cbDissType.Items.Remove(Res.Diss32);
      cbDissType.Enabled = true;
      cbDissType.SelectedItem = Globals.GetDissCaption(id == DevicesID.None? _dissType : id, mod);
    }

    private int FindIdIn(Dictionary<int, string> devices, string value)
    {
        foreach (var item in devices)
            if (item.Value == value)
                return item.Key;
        return -1; // вместо throw new NotImplementedException();
    }

        // закрываемся + 
        private void frmCurStatus_FormClosing(object sender, FormClosingEventArgs e)
    {
      if (isClose) // не будем открывать добавление новой проверки 
        if (ParentForm != null)
          ((frmMain) ParentForm).EnableMenu(true);
    }

    // Закрыть форму + 
    private void btnCurStatusClose_Click(object sender, EventArgs e)
    {
      Close();
    }

    // Отчет + 
    private void btn_Repair_Click(object sender, EventArgs e)
    {
      if (dgvChecks.RowCount == 0)
        return;
      RenewDissMod();
      frmReport frm = new frmReport(testID, _dissType, _dissMod);
      //frm.MdiParent = MdiParent;
      if (frm.isShow)
        frm.ShowDialog();
      else
      {
        Globals.CantOpenForm(frm);
      }
    }

    // Удалить выделенную проверку + 
    private void btn_Delete_Click(object sender, EventArgs e)
    {
      if (dgvChecks.RowCount == 0)
        return;
      if (dgvChecks.CurrentRow == null)
        return;
      string str = string.Format(Resources.DeleteConfirmMessage, Globals.GetDissCaption(_dissType, _dissMod),
                                 dgvChecks.CurrentRow.Cells["productionNumber"].Value,
                                 dgvChecks.CurrentRow.Cells["dateMaking"].Value,
                                 dgvChecks.CurrentRow.Cells["dateRepair"].Value,
                                 dgvChecks.CurrentRow.Cells["fullName"].Value);

      if (MessageBox.Show(str, Resources.ApplicationName, MessageBoxButtons.YesNo, MessageBoxIcon.Warning) ==
          DialogResult.No)
        return;

      if (!DataManager.Instance.DeleteTest(Convert.ToInt32(dgvChecks.CurrentRow.Cells[NumColTestId + 1].Value)))
      {
        MessageBox.Show(Resources.Error);
        return;
      }
      dgvChecks.Rows.Remove(dgvChecks.CurrentRow);
    }

    // Добавить новую проверку + 
    private void btn_Add_Click(object sender, EventArgs e)
    {
        List<string> dataMigration = new List<string>();
        DialogResult results = MessageBox.Show("Перенести данные из текущей проверки в новую проверку?", "ВНИМАНИЕ!",
            MessageBoxButtons.YesNo);
        if (results == DialogResult.Yes)
        {

            for (int i = 1; i < 8; i++)
            {
                dataMigration.Add(dgvChecks.CurrentRow.Cells[i].Value.ToString());
            }

            ;
            RenewDissMod();
            dtDataMigration = new DataTable();
            DataManager.Instance.FillCompositionWithIdBlocks(testID, dtDataMigration);
            frmNewCheck frm = new frmNewCheck(_dissType, _dissMod, _modeApplication, dataMigration, testID, 1,
                dtDataMigration);
            MdiParent = null;
            frm.MdiParent = Globals.MainForm;
            if (frm.isShow)
            {
                isClose = false;
                Close();
                frm.Show();
                frm.WindowState = FormWindowState.Maximized;
            }
            else
            {
                isClose = true;
                Globals.CantOpenForm(frm);
            }
        }
        else
        {
            RenewDissMod();
            frmNewCheck frm = new frmNewCheck(_dissType, _dissMod, _modeApplication);
            MdiParent = null;
            frm.MdiParent = Globals.MainForm;
            if (frm.isShow)
            {
                isClose = false;
                Close();
                frm.Show();
                frm.WindowState = FormWindowState.Maximized;
            }
            else
            {
                isClose = true;
                Globals.CantOpenForm(frm);
            }
        };
    }

    private const int NumColTestId = 9;

    // ID выделенной проверки + 
    private void dgv_SelectionChanged(object sender, EventArgs e)
    {
      try
      {
          bDelete.Enabled = bAdd.Enabled = bRepair.Enabled = bContinue.Enabled = dgvChecks.SelectedRows.Count > 0;
          if (dgvChecks.CurrentRow != null)
              if (dgvChecks.SelectedRows.Count > 0)
                testID = int.Parse(dgvChecks.CurrentRow.Cells[NumColTestId+1].Value.ToString());
      }
      catch (NullReferenceException)
      {
      }
    }

    #region Фильтрация

    // Отмена фильтра + 
    private void btn_Cancel_Click(object sender, EventArgs e)
    {
      LockComboBoxes(true);
      isCancel = true;
      dtpFrom.MaxDate = DateTime.MaxValue.AddYears(-2);
      dtpTo.MinDate = _dtMin;
      dtpFrom.Value = dtpFrom.MinDate;
      dtpTo.Value = dtpTo.MaxDate;
      dtpFrom.Checked = false;
      dtpTo.Checked = false;
      cbNumber.SelectedItem = cbPerformer.SelectedItem = Resources.All;
      FillMainTable(GetTestings());
      isCancel = false;
      LockComboBoxes(false);
    }

    private void cmbBox_Number_SelectedValueChanged(object sender, EventArgs e)
    {
      if (!((Control) sender).Enabled)
        return;
      if (isLoad || isCancel) return;
      if (sender is DateTimePicker)
        if (((DateTimePicker)sender).Checked)
        {
          if (sender.Equals(dtpTo))
            dtpFrom.MaxDate = dtpTo.Value;
          else
            dtpTo.MinDate = dtpFrom.Value;
        }
        else
        {
          if (sender.Equals(dtpTo))
            dtpTo.MinDate = _dtMin;
          else
            dtpFrom.MaxDate = _dtMax;
        }
      FillDataGrid();
    }

    private static string GetNewFilterString(string filter, string addStr)
    {
      const string sAnd = " AND ";
      if (String.IsNullOrEmpty(filter))
        filter = addStr;
      else
        filter += sAnd + addStr;
      return filter;
    }

    private static void FillComboBox(ComboBox cb, DataView dv, int colNum)
    {
      cb.Items.Add(Resources.All);
      foreach (DataRowView dr in dv)
      {
        string add = dr[colNum].ToString();
        if (!cb.Items.Contains(add))
          cb.Items.Add(add);
      }
      cb.SelectedItem = Resources.All;
    }

    private const int NumColProductNumber = 0;
    private const int NumColTestDate = 2;
    private const int NumColEmployee = 3;

    /// <summary>
    /// Фильтруем и заполняем наш DataGridView  
    /// </summary>
    private void FillDataGrid()
    {
            cbNumber.Enabled = false;
            cbPerformer.Enabled = false;
            DataView dv = new DataView(dtViewTemp);
            if (cbNumber.SelectedItem.ToString() != Resources.All)
                dv.RowFilter = GetNewFilterString(dv.RowFilter,
                                                  dgvChecks.Columns[NumColProductNumber + 1].DataPropertyName + "='" +
                                                  cbNumber.SelectedItem + "'");
            else
                cbNumber.Items.Clear();
            if (cbPerformer.SelectedItem.ToString() != Resources.All)
                dv.RowFilter = GetNewFilterString(dv.RowFilter,
                                                  dgvChecks.Columns[NumColEmployee + 1].DataPropertyName + "='" +
                                                  cbPerformer.SelectedItem + "'");
            else
                cbPerformer.Items.Clear();
            if (dtpTo.Checked)
            {
                DateTime dtTo = dtpTo.Value;
                dtTo = dtTo.AddHours(23);
                dtTo = dtTo.AddMinutes(59);
                dtTo = dtTo.AddSeconds(59);
                dv.RowFilter = GetNewFilterString(dv.RowFilter,
                                                  dgvChecks.Columns[NumColTestDate + 1].DataPropertyName + "<=#" +
                                                  dtTo.ToString(CultureInfo.InvariantCulture) + "#");
            }
            if (dtpFrom.Checked)
                dv.RowFilter = GetNewFilterString(dv.RowFilter,
                                                  dgvChecks.Columns[NumColTestDate + 1].DataPropertyName + ">=#" +
                                                  dtpFrom.Value.ToString(CultureInfo.InvariantCulture) + "#");
            if (cbNumber.Items.Count == 0)
                FillComboBox(cbNumber, dv, NumColProductNumber + 1);
            if (cbPerformer.Items.Count == 0)
                FillComboBox(cbPerformer, dv, NumColEmployee + 1);
            if (dv.Count > 0)
            {
                for (int y = 0; y < dv.Count; y++)
                {
                    dv[y][0] = (y + 1).ToString();
                };
            };

            dgvChecks.DataSource = dv;
            cbNumber.Enabled = true;
            cbPerformer.Enabled = true;
    }

    #endregion

    private void cbDissType_SelectedIndexChanged(object sender, EventArgs e)
    {
      ComboBox current = (ComboBox) sender;
      if (!current.Enabled) return;
      string strType = current.SelectedItem.ToString();
      _deviceTypeId = listDevices[strType].Key;
      _dissType = (DevicesID) _deviceTypeId;
      _dissMod = listDevices[strType].Value;
      Text = string.Format(Resources.TestHistory, strType);
      dtView = GetTestings();

      FillDates(dtView);
      FillMainTable(dtView);
      FillComboBoxes(dtView);
      ShowJUPForDevice();
    }

      private void ShowJUPForDevice() // отображает ЖУП для выбранного устройства (на 10.10.2023 нужно отображать только для ДИСС-013)
      {
          if (!Settings.Default.JUPEnable) // Для некоторых заказов ЖУП быть не должно
          {
              btnJupReport.Visible = false;
                return;
          }

          btnJupReport.Visible = _dissType == DevicesID.Diss13;
      }

    private void FillDates(DataTable dtView)
    {
      GetMinAndMaxTestDate(dtView, out _dtMin, out _dtMax);
      TimeSpan ts = _dtMax - _dtMin;
      if (ts.Days < 2)
        _dtMin = _dtMin.AddDays(-2);
      if (_dtMin > dtpFrom.MaxDate || _dtMin > dtpTo.MaxDate)
      {
        dtpFrom.MaxDate = dtpTo.MaxDate = _dtMax;
        dtpFrom.MinDate = dtpTo.MinDate = _dtMin;
      }
      else
      {
        dtpFrom.MinDate = dtpTo.MinDate = _dtMin;
        dtpFrom.MaxDate = dtpTo.MaxDate = _dtMax;
      }
      dtpFrom.Value = _dtMin;
      dtpTo.Value = _dtMax;
    }

    private const int TestDateColumn = 2;

    private void GetMinAndMaxTestDate(DataTable dtView, out DateTime _dtMin, out DateTime _dtMax)
    {
      _dtMin = DateTime.MaxValue;
      _dtMax = DateTime.MinValue;
      for (int i = 0; i < dtView.Rows.Count; i++)
      {
        DateTime _curDate = Convert.ToDateTime(dtView.Rows[i][TestDateColumn]);
        if (_curDate < _dtMin)
          _dtMin = _curDate;
        if (_curDate > _dtMax)
          _dtMax = _curDate;
      }

      if (_dtMin == DateTime.MaxValue)
        _dtMin = DateTime.Now;
      if (_dtMax == DateTime.MinValue)
        _dtMax = DateTime.Now;
    }

    private const string ColumnTestId = "TestId";


    private void bContinue_Click(object sender, EventArgs e)
    {
      ContinueCheck();
    }

    private void ContinueCheck()
    {
      Form frm = null;
      RenewDissMod();
      frm = new frmNewCheck(_dissType, _dissMod, _modeApplication, testID);
      // frm = Common.Globals.GetDeviceForm(_dissType, _dissMod, testID);
      frm.MdiParent = null;
      frm.MdiParent = Globals.MainForm;
      frm.WindowState = FormWindowState.Maximized;
      frm.Show();
      Close();
    }

    private void RenewDissMod()
    {
      if (testID == -1)
        return;
      string mod = DataManager.Instance.GetAddParam(testID, DataManager.AddParameters.Modification);
      _dissMod = Globals.GetModificationByStr(mod);
    }

        private void FillDSJup(ReportClass rep)
        {
            if (dgvChecks.RowCount == 0)
                return;
            if (dgvChecks.CurrentRow == null)
                return;

            DataTable dt = DataManager.Instance.GetReport(testID);

            //Тут нужно как то универсальней сделать потом
            var prodNumber = dgvChecks.CurrentRow.Cells["productionNumber"].Value;
            var prodDate = dgvChecks.CurrentRow.Cells["dateMaking"].Value;
            var prodName = "";
            if (cbDissType.SelectedItem != null)
                 prodName = cbDissType.SelectedItem + "-С2М";

            var bortName = dgvChecks.CurrentRow.Cells["numberHelicopter"].Value;

            var employeeName = dgvChecks.CurrentRow.Cells["fullName"].Value;

            var fullDate = dgvChecks.CurrentRow.Cells["dateRepair"].Value;
            var copy = fullDate;
            DateTime date = DateTime.Parse(copy.ToString());

            var freqPrd = GetValueOfParamWithPrecision(dt, "FreqSVCh");
            var powerPrd = GetValueOfParamWithPrecision(dt, "PowerSVCh");
            var indexModulation = GetValueOfParamWithPrecision(dt, "PhaseIndex");

            rep.SetParameterValue("ProdNumber",prodNumber ?? "");
            rep.SetParameterValue("ProdName", prodName);
            rep.SetParameterValue("BortName", bortName ?? "");
            rep.SetParameterValue("EmployeeName", employeeName ?? "");
            rep.SetParameterValue("DateProverka", fullDate == null ? "" : date.ToString("dd.MM.yy"));

            rep.SetParameterValue("FreqPRD", freqPrd ?? "");
            rep.SetParameterValue("PRDPower", powerPrd ?? "");
            rep.SetParameterValue("IndexModulation", indexModulation ?? "");

        }

        private string GetValueOfParamWithPrecision(DataTable dt, string paramName) 
        {
            for (int i = 0; i < dt.Rows.Count; i++)
			{
                var colParamName = dt.Rows[i]["ParamName"].ToString();

                if (colParamName == paramName) 
                {
                    var precision = Convert.ToInt32(dt.Rows[i]["Precision"]);
                    if (dt.Rows[i]["Result"] != DBNull.Value)
                    {
                        var res = Math.Round(Convert.ToDouble(dt.Rows[i]["Result"]), precision);
                        return res.ToString();
                    }
                }
			}
            return "";
        }

        private void btnJupReport_Click(object sender, EventArgs e)
        {
            ParamsReport rep = new ParamsReport();
            FillDSJup(rep); // пока только для ДИСС-13
            frmForPrint frmJupReport = new frmForPrint(rep, testID);
            frmJupReport.ShowDialog();
        }

        private void dgvChecks_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
    {
      ContinueCheck();
    }
  }
}