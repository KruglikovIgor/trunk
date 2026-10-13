using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using KPK2.GeneralForms.DissForms;
using KPK2.Interfaces;
using KPK2.Properties;
using Sparc.KPK2.Hardware;
using Sparc.KPK2.Imitator;
using Sparc.KPK2.NFunctions;
using Res = Sparc.KPK2.Resources.Properties.Resources;

namespace KPK2.MyControl
{
  public partial class Block8Check : UserControl
  {
    private Timer tmrBlock8;
    private int countBlock8;
    private const int KeepTime = 900;

    /// <summary>
    /// "Режим Море"(КЗ-6) - увеличение путевой скорости 
    /// </summary>
    private double task6_W, task6_W_dif;

    private bool[] arTask_6_2 = new bool[2];
    private IFrmWork15ForBlack8Check _frm;
    private Imitator.Diss15 _device;

    public Block8Check(IFrmWork15ForBlack8Check frm)
    {
      _frm = frm;
      InitializeComponent();
    }

    public Block8Check(IFrmWork15ForBlack8Check frm, Imitator.Device device)
    {
      _frm = frm;
      if (device is Imitator.Diss15)
        _device = (Imitator.Diss15)device;
      InitializeComponent();
    }

    private void grpBox_Task6_Enter(object sender, EventArgs e)
    {

    }

    private void Block8Check_Load(object sender, EventArgs e)
    {
      InitializeTimer();
      AssignHandlers();

      b8.progressBar_6.Value = 0;
      b8.progressBar_6.Visible = b8.lbl_6_Time.Visible = false;
      b8.btn_8_On_6.Enabled = b8.btn_8_Off_6.Enabled = true;
      b8.btn_8_Off_6.PerformClick();
      b8.txtBox_6_1.Enabled = b8.txtBox_6_2.Enabled = b8.cmbBox_6_1.Enabled = b8.cmbBox_6_2.Enabled = false;
    }

    private void AssignHandlers()
    {
      b8.btn_8_On_6.Click += btn_8_On_6_Click;
      b8.btn_8_Off_6.Click += btn_8_Off_6_Click;
      b8.txtBox_6_1.TextChanged += txtBox_TextChanged;
      b8.txtBox_6_2.TextChanged += txtBox_TextChanged;
      b8.txtBox_6_1.Validating += txtBox_6_1_Validating;
      b8.txtBox_6_2.Validating += txtBox_6_2_Validating;
      b8.cmbBox_6_1.SelectedIndexChanged += cmbBox_SelectedIndexChanged;
      b8.cmbBox_6_2.SelectedIndexChanged += cmbBox_SelectedIndexChanged;
    }

    private void InitializeTimer()
    {
      if (tmrBlock8 == null)
        tmrBlock8 = new Timer();
      tmrBlock8.Interval = 1000;
      tmrBlock8.Tick += tmrBlock8_Tick;
    }

    // Время для отработки Блока 8 (через 1сек)
    private void tmrBlock8_Tick(object sender, EventArgs e)
    {
      countBlock8--;
      int s = countBlock8%60; // кол-во секунд
      int m = countBlock8/60; // кол-во минут            
      if (b8.btn_8_On_6.FlatStyle == FlatStyle.Flat) // для КЗ-6
      {
        b8.lbl_6_Time.Text = string.Format("{0}: {1} {2} {3} {4}", Resources.TimeLeft, m, Resources.Minutes, s,
                                           Resources.Seconds);
        b8.progressBar_6.Value++;
        if (countBlock8 == 0) // время кончилось
        {
          b8.btn_8_Off_6.PerformClick();
          b8.lbl_6_Time.Text = string.Format("{0}: {1} {2} {3} {4}", Resources.TimePassed, (KeepTime/60),
                                             Resources.Minutes, (KeepTime%60), Resources.Seconds);
          b8.btn_8_On_6.Enabled = b8.btn_8_Off_6.Enabled = false;
        }
      }
    }

    // Включить блок 8
    private void btn_8_On_6_Click(object sender, EventArgs e)
    {
      if (b8.btn_8_On_6.FlatStyle == FlatStyle.Standard)
      {
        DiscreteOutputs.Instance().SetOutput("Block_8", true);

        b8.btn_8_On_6.FlatStyle = FlatStyle.Flat;
        b8.btn_8_On_6.BackColor = Color.FromArgb(153, 249, 164);
        b8.btn_8_Off_6.FlatStyle = FlatStyle.Standard;
        b8.btn_8_Off_6.BackColor = Color.Transparent;

        b8.progressBar_6.Value = 0;
        b8.progressBar_6.Visible = b8.lbl_6_Time.Visible = true;
        countBlock8 = KeepTime;
        b8.lbl_6_Time.Text = string.Format("{0}: {1} {2} {3} {4}", Resources.TimeLeft, (countBlock8/60),
                                           Resources.Minutes, (countBlock8%60), Resources.Seconds);

        b8.txtBox_6_1.Text = b8.txtBox_6_2.Text = "";
        b8.txtBox_6_1.ForeColor = b8.txtBox_6_2.ForeColor = SystemColors.WindowText;
        b8.cmbBox_6_1.Items.Clear();
        b8.cmbBox_6_1.Items.AddRange(new[] {Resources.Forward, Resources.Back});
        b8.cmbBox_6_2.Items.Clear();
        b8.cmbBox_6_2.Items.AddRange(new[] {Resources.Left, Resources.Right});

        if (arTask_6_2[0] && arTask_6_2[1]) // блокируем, если что-то вводили/выбирали
          b8.cmbBox_6_1.Enabled = b8.cmbBox_6_2.Enabled = b8.txtBox_6_1.Enabled =
                                                          b8.txtBox_6_2.Enabled = false;

        tmrBlock8.Start();
      }
    }

    // Выключить блок 8
    private void btn_8_Off_6_Click(object sender, EventArgs e)
    {
      if (b8.btn_8_Off_6.FlatStyle == FlatStyle.Standard)
      {
        tmrBlock8.Stop();

        b8.btn_8_Off_6.FlatStyle = FlatStyle.Flat;
        b8.btn_8_Off_6.BackColor = Color.FromArgb(153, 249, 164);
        b8.btn_8_On_6.FlatStyle = FlatStyle.Standard;
        b8.btn_8_On_6.BackColor = Color.Transparent;

        arTask_6_2[0] = arTask_6_2[1] = false;

        DiscreteOutputs.Instance().SetOutput("Block_8", false);

        if (countBlock8 == 0) // отработали 15 мин
        {
          b8.cmbBox_6_1.Enabled = b8.cmbBox_6_2.Enabled = true;
          MessageBox.Show(Resources.WriteValuesPathAndAngle, Resources.ApplicationName,
                          MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        else // выключили раньше
        {
          b8.progressBar_6.Visible = b8.lbl_6_Time.Visible =
                                     b8.txtBox_6_1.Enabled = b8.txtBox_6_2.Enabled =
                                                             b8.cmbBox_6_1.Enabled = b8.cmbBox_6_2.Enabled = false;
        }
      }
    }

    private void txtBox_TextChanged(object sender, EventArgs e)
    {
      TextBox current = (TextBox) sender;
      current.ForeColor = SystemColors.WindowText;
      current.Text = current.Text.Trim();
    }

    private double _minPath = double.NaN;
    private double _maxPath = double.NaN;

    private void txtBox_6_1_Validating(object sender, CancelEventArgs e)
    {
      if (b8.txtBox_6_1.Text != "")
      {
        double value = Functions.MoveToDouble(b8.txtBox_6_1.Text);
        if (double.IsNaN(value))
        {
          Globals.ErrorInputTextBox(b8.txtBox_6_1);
          return;
        }
        if (!(0 <= value && value <= 999))
        {
          Globals.ErrorCorridorTextBox(b8.txtBox_6_1, 0, 999, Resources.Path);
          return;
        }
        if (double.IsNaN(_minPath) || double.IsNaN(_maxPath))
          DataManager.Instance.GetParamLimitAndId("Block8_15_Forward", ref _minPath, ref _maxPath);
        // красный/зеленый коридор
        b8.txtBox_6_1.ForeColor = _minPath <= value && value <= _maxPath &&
                                  b8.cmbBox_6_1.SelectedItem.ToString() == Resources.Forward.ToUpper()
                                    ? Color.LimeGreen
                                    : Color.Red;
        arTask_6_2[0] = true;
        if (arTask_6_2[0] && arTask_6_2[1]) // разблокируем "Режим К", "Режим МОРЕ"...
          btn_Mode_K.Enabled = btn_Mode_M.Enabled = b8.btn_8_On_6.Enabled =
                                                    b8.btn_8_Off_6.Enabled = true;
      }
    }

    private double _minLeft = double.NaN;
    private double _maxLeft = double.NaN;

    private void txtBox_6_2_Validating(object sender, CancelEventArgs e)
    {
      if (b8.txtBox_6_2.Text != "")
      {
        double value = Functions.MoveToDouble(b8.txtBox_6_2.Text);
        if (double.IsNaN(value))
        {
          Globals.ErrorInputTextBox(b8.txtBox_6_2);
          return;
        }
        if (!(0 <= value && value <= 999))
        {
          Globals.ErrorCorridorTextBox(b8.txtBox_6_2, 0, 999, "\"Боковое уклонение\"");
          return;
        }
        if (double.IsNaN(_minLeft) || double.IsNaN(_maxLeft))
          DataManager.Instance.GetParamLimitAndId("Block8_15_Left", ref _minLeft, ref _maxLeft);
        // красный/зеленый коридор
        b8.txtBox_6_2.ForeColor = _minLeft <= value && value <= _maxLeft &&
                                  b8.cmbBox_6_2.SelectedItem.ToString() == Resources.Left.ToUpper()
                                    ? Color.LimeGreen
                                    : Color.Red;
        arTask_6_2[1] = true;
        if (arTask_6_2[0] && arTask_6_2[1]) // разблокируем "Режим К", "Режим МОРЕ"...
          btn_Mode_K.Enabled = btn_Mode_M.Enabled = b8.btn_8_On_6.Enabled =
                                                    b8.btn_8_Off_6.Enabled = true;
      }
    }

    private void cmbBox_SelectedIndexChanged(object sender, EventArgs e)
    {
      ComboBox cbCurrent = (ComboBox) sender;
      TextBox tbCurrent = cbCurrent.Name == b8.cmbBox_6_1.Name ? b8.txtBox_6_1 : b8.txtBox_6_2;
      tbCurrent.Enabled = true;
      tbCurrent.Text = "";
    }

    private void btn_Mode_K_Click(object sender, EventArgs e)
    {
      if (btn_Mode_K.FlatStyle == FlatStyle.Standard && btn_Mode_K.Text != Resources.Save)
      {
        if (arTask_6_2[0] && arTask_6_2[1]) // блокируем, если что-то вводили/выбирали
        {
          b8.cmbBox_6_1.Enabled = b8.cmbBox_6_2.Enabled = b8.txtBox_6_1.Enabled =
                                                          b8.txtBox_6_2.Enabled = false;
          b8.progressBar_6.Visible = b8.lbl_6_Time.Visible = false;
        }
        btn_Mode_M.Enabled = b8.btn_8_On_6.Enabled = b8.btn_8_Off_6.Enabled = false;
        MessageBox.Show(string.Format(Sparc.KPK2.Resources.Properties.Resources.SetRKToK, Resources.Save), Resources.ApplicationName,
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
        if (_device != null)
          _device.SetControl(true);
        _frm.SetIndicatorLimits((int)Globals.IndicatorNumber.Speed1, "Alfa_K_15", true);
        _frm.SetIndicatorLimits((int)Globals.IndicatorNumber.Speed2, "W_K_15");
        btn_Mode_K.Text = Resources.Save;
      }
      else // отжали кнопку 
      {
        //домножаем на -1 со слов Борислава т.к. вправо это +, а - это влево домножаем 
        _frm.ManualRecordDouble("Alfa_K_15", _frm.GetMeasuredValue(_frm.alfa_18minus) * -1);
        _frm.ManualRecordDouble("W_K_15", _frm.GetMeasuredValue(_frm.spd_task6_15));

        btn_Mode_M.Enabled = b8.btn_8_On_6.Enabled = b8.btn_8_Off_6.Enabled = true;
        //txtBox_Block_1.Enabled = txtBox_Block_2.Enabled = true;

        btn_Mode_K.FlatStyle = FlatStyle.Standard;
        btn_Mode_K.BackColor = Color.Transparent;

        MessageBox.Show(string.Format(Sparc.KPK2.Resources.Properties.Resources.SetRKToR,"OK"), Resources.ApplicationName,
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
        if (_device != null)
          _device.SetControl(false);
        _frm.SetIndicatorLimits((int)Globals.IndicatorNumber.Speed1, _frm.alfa_18minus);
        _frm.SetIndicatorLimits((int)Globals.IndicatorNumber.Speed2, _frm.spd_task6_15);
        btn_Mode_K.Text = Res.ModeK;
      }
    }

    // Режим МОРЕ 
    private void btn_Mode_M_Click(object sender, EventArgs e)
    {
      const string strWSea = "W_Sea_15";
      if (btn_Mode_M.Text == Res.ModeSea) // 1-й click
      {
        //Save();
        task6_W = _frm.GetMeasuredValue(_frm.spd_task6_15); //Spd_Task6_15.Result;
        if (arTask_6_2[0] && arTask_6_2[1]) // блокируем, если что-то вводили/выбирали
          b8.cmbBox_6_1.Enabled = b8.cmbBox_6_2.Enabled = b8.txtBox_6_1.Enabled =
                                                          b8.txtBox_6_2.Enabled =
                                                          b8.progressBar_6.Visible = b8.lbl_6_Time.Visible = false;
        btn_Mode_M.Text = Res.FindDW;
        btn_Mode_K.Enabled = b8.btn_8_On_6.Enabled = b8.btn_8_Off_6.Enabled = false;
        //txtBox_Block_1.Enabled = txtBox_Block_2.Enabled = false;
        //indN_Speed_2.iLText = "Wм, км/ч";
        MessageBox.Show(string.Format(Resources.SetSMToM, Res.FindDW, Resources.Save), Resources.ApplicationName,
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
        if (_device != null)
          _device.SetSeaMode(true);
        _frm.SetIndicatorLimitsPercent((int)Globals.IndicatorNumber.Speed2, task6_W, task6_W, strWSea);
        return;
      } // 
      if (btn_Mode_M.Text == Res.FindDW) // 2-й click 
      {
        task6_W_dif = _frm.GetMeasuredValue(_frm.spd_task6_15) - task6_W;
        task6_W_dif = task6_W_dif / task6_W * 100; // в %
        _frm.DeletingAll();

        btn_Mode_M.Text = Resources.Save;
        _frm.SetupIndicator((int) Globals.IndicatorNumber.Speed2,
                            _frm._deltaChar + string.Format(Resources.WN, Resources.Percent), null, "");
        //_frm.SetIndicatorLimitsPercent((int)Globals.IndicatorNumber.Speed2, 0, task6_W, strWSea);
        _frm.SetIndicatorLimits((int)Globals.IndicatorNumber.Speed2, strWSea);
        
        _frm.SetIndicatorValue((int) Globals.IndicatorNumber.Speed2, task6_W_dif);
        return;
      }
      if (btn_Mode_M.Text == Resources.Save) // 3-й click
      {
        if (!double.IsNaN(task6_W_dif))
          _frm.ManualRecordDouble(strWSea, task6_W_dif);
        btn_Mode_M.Text = Res.ModeSea;
        btn_Mode_K.Enabled = b8.btn_8_On_6.Enabled = b8.btn_8_Off_6.Enabled = true;
        _frm.InitializTask_6();
        MessageBox.Show(Resources.SetSMToS, Resources.ApplicationName,
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
        if (_device != null)
          _device.SetSeaMode(false);
      }
    }

    public void Save()
    {
      if (b8.txtBox_6_1.Enabled == false && b8.txtBox_6_2.Enabled == false)
        return;
      double value = 0.0; // что ввели 
      bool bl = false;
      if (b8.txtBox_6_1.Enabled)
      {
        value = Functions.MoveToDouble(b8.txtBox_6_1.Text);
        if (double.IsNaN(value))
        {
          bl = true;
          value = 0;
        }
        if (!bl) // ошибка -> уходим 
        {
          value = b8.cmbBox_6_1.SelectedItem.ToString() == Resources.Forward.ToUpper() ? value : -value;
          _frm.ManualRecordDouble("Block8_15_Forward", value);
        }
      }
      bl = false;
      if (b8.txtBox_6_2.Enabled)
      {
        value = Functions.MoveToDouble(b8.txtBox_6_2.Text);
        if (double.IsNaN(value))
        {
          bl = true;
          value = 0;
        }
        if (!bl) // ошибка -> уходим 
        {
          value = b8.cmbBox_6_2.SelectedItem.ToString() == Resources.Left.ToUpper() ? value : -value;
          _frm.ManualRecordDouble("Block8_15_Left", value);
        }
      }
    }

    ~Block8Check()
    {
      Save();
    }
  }
}
