using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using KPK2.Properties;
using Sparc.KPK2.Hardware;
using Sparc.KPK2.NFunctions;

namespace KPK2.GeneralForms.ServiceForm
{
  public partial class frmGenSinCosAngleCalibrate : Form
  {

    public static readonly Color ChangedColor = Color.Red;
    public static readonly Color UnchangedColor = Color.White;
    private const int numColValue = 3;
    private const int numColMeasure = 4;
    private const int numColIdealValue = 2;
    private const int numColChannel = 1;
    private const int numColId = 0;
    const int baseVoltage = 40000;

    public frmGenSinCosAngleCalibrate()
    {
      InitializeComponent();
    }

    private void cbChannel_SelectedIndexChanged(object sender, EventArgs e)
    {
        // Обнуляем "другой" канал
        int otherChannel = CurrentChannel == 1 ? 2 : 1;
        GenSinCos.Instance().SetRealSignalEx(otherChannel, 0, Convert.ToInt32(nudFrequency.Value), 0);

        // Выставляем опорный и текущий каналы (текущий получит nudVoltagePhase)
        SetAllSignalsParams();

        // Загружаем таблицу
        LoadGridValues();
        }

    private int CurrentDbChannel
    {
      get { return cbChannel.SelectedIndex + 3; }
    }

    private int CurrentChannel
    {
      get { return cbChannel.SelectedIndex + 1; }
    }

    private bool IsSine
    {
      get { return CurrentChannel == 1; }
    }

    private void LoadGridValues()
    {
      DataTable dtResult = DataManager.Instance.GetCalibration(DataManager.GenSinCosModuleId,
                                                                 CurrentDbChannel);
      dgvValues.ReadOnly = true;
      dgvValues.Rows.Clear();
      for (int i = 0; i < dtResult.Rows.Count; i++)
      {
        int channel = Convert.ToInt32(dtResult.Rows[i][2]) - 2;
        double angle = Convert.ToInt32(dtResult.Rows[i][4])/GenSinCos.AngleSaveKoef;
        if (IsSine)
          angle = AngleToRealAngle(angle);
        double idealAngle = Convert.ToDouble(dtResult.Rows[i][3])/GenSinCos.AngleSaveKoef;
        if (IsSine)
          idealAngle = AngleToRealAngle(idealAngle);
        dgvValues.Rows.Add(dtResult.Rows[i][0], channel, idealAngle, angle);
      }
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
      {
        double angle = Functions.MoveToDouble(dgvValues.Rows[e.RowIndex].Cells[numColValue].Value.ToString());
        if (angle > 360)
        {
          MessageBox.Show(Resources.ErrorNumber);
          dgvValues.Focus();
          return;
        }
        SetCurrentSignal(e.RowIndex);
      }
    }

    private double AngleToRealAngle(double angle)
    {
      angle += 180;
      if (angle >= 360)
        angle -= 360;
      return angle;
    }

    private void SetCurrentSignal(int rowIndex)
    {
      try
      {
        double angle = Functions.MoveToDouble(dgvValues.Rows[rowIndex].Cells[numColValue].Value.ToString());
        if (IsSine)
          angle = AngleToRealAngle(angle);
        if (!GenSinCos.Instance().SetRealAmplitudeSignal(CurrentChannel, (int) nudVoltagePhase.Value,
                                                         Convert.ToInt32(nudFrequency.Value), angle))
          MessageBox.Show(string.Format(Resources.ErrorOnDeviceN, Resources.GenSinBoard));
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
      double angle = Convert.ToInt32(coll[3].Value);
      double idealAngle = Functions.MoveToDouble(coll[2].Value.ToString());
      if (IsSine)
        PrepareAngels(ref angle, ref idealAngle);
      coll[numColId].Value = DataManager.Instance.UpdateCalibration(Convert.ToInt32(coll[0].Value),
                                             (int)(idealAngle * GenSinCos.AngleSaveKoef), (int)(angle * GenSinCos.AngleSaveKoef));
      if (Convert.ToInt32(coll[numColId].Value) != 0)
      {
        RowSaved(rowIndex);
        return true;
      }
      return false;
    }

    /// <summary>
    /// Синус изначально развернут на 180 градусов, поэтому приходится хитрить
    /// </summary>
    /// <param name="angle"></param>
    /// <param name="idealAngle"></param>
    private void PrepareAngels(ref double angle, ref double idealAngle)
    {
      angle = AngleToRealAngle(angle);
      idealAngle = AngleToRealAngle(idealAngle);
      if (Math.Abs(angle - idealAngle) > 300)
      {
        if (angle > 300)
          idealAngle = idealAngle + 360;
        else
          idealAngle = idealAngle - 360;
      }
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
      double angle = Convert.ToInt32(coll[3].Value);
      double idealAngle = Functions.MoveToDouble(coll[2].Value.ToString());
      if (IsSine)
        PrepareAngels(ref angle, ref idealAngle);
      coll[numColId].Value = DataManager.Instance.SaveCalibration(DataManager.GenSinCosModuleId, CurrentDbChannel,
                                             (int)(idealAngle*GenSinCos.AngleSaveKoef), (int)(angle*GenSinCos.AngleSaveKoef));
      if (Convert.ToInt32(coll[numColId].Value) != 0)
      {
        RowSaved(rowIndex);
        return true;
      }
      return false;
    }

    private void dgvValues_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
    {
      if (((DataGridView)sender).ReadOnly)
        return;
      foreach (DataGridViewCell cell in ((DataGridView)sender).Rows[e.RowIndex - 1].Cells)
        cell.Style.BackColor = ChangedColor;
      dgvValues.Rows[e.RowIndex - 1].Cells[numColChannel].Value = CurrentChannel;
    }

    private void dgvValues_UserDeletedRow(object sender, DataGridViewRowEventArgs e)
    {
      DataManager.Instance.RemoveCalibration(Convert.ToInt32(e.Row.Cells[0].Value));
    }

    private void frmGenSinCosAngleCalibrate_Load(object sender, EventArgs e)
    {
      // Выбираем первый элемент, если список не пуст и ничего не выбрано
      if (cbChannel.Items.Count > 0 && cbChannel.SelectedIndex < 0) cbChannel.SelectedIndex = 0;
      SetAllSignalsParams();
    }

    private void nudFrequency_ValueChanged(object sender, EventArgs e)
    {
      SetAllSignalsParams();
    }

    private void SetAllSignalsParams()
    {
      try
      {
        int frequency = Convert.ToInt32(nudFrequency.Value);
        GenSinCos.Instance().SetRealSignalEx(0, (int) nudVoltageRef.Value, frequency, 0);
        int channel = CurrentChannel; // 1 для синуса, 2 для косинуса
        int phaseVoltage = (int)nudVoltagePhase.Value;
        GenSinCos.Instance().SetRealSignalEx(channel, phaseVoltage, frequency, 0);

        // Сброс напряжения на "другом" канале в 0
        int otherChannel = channel == 1 ? 2 : 1;
        GenSinCos.Instance().SetRealSignalEx(otherChannel, 0, frequency, 0);
      }

      catch
      {
        MessageBox.Show(string.Format(Resources.ErrorOnDeviceN, Resources.GenSinBoard));
      }
    }

    private void nudVoltageRef_ValueChanged(object sender, EventArgs e)
    {
      SetAllSignalsParams();
    }

    private void nudVoltagePhase_ValueChanged(object sender, EventArgs e)
    {
      SetAllSignalsParams();
    }

    private void dgvValues_CellClick(object sender, DataGridViewCellEventArgs e)
    {
      if (e.ColumnIndex == numColValue)
        SetCurrentSignal(e.RowIndex);
    }

        private void frmGenSinCosAngleCalibrate_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                GenSinCos.Instance().GeneratorOff();
            }
            catch
            {
            }
        }
    }
}
