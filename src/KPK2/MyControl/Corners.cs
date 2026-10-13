using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using KPK2.Common.DissFunctions;
using KPK2.GeneralForms.DissForms;
using KPK2.Interfaces;
using KPK2.Properties;
using Sparc.KPK2.Hardware;
using Sparc.KPK2.Imitator;
using Sparc.KPK2.NFunctions;
using Globals = KPK2.Common.Globals;

namespace KPK2.MyControl
{
  public partial class Corners : UserControl
  {
    private IFrmWork15ForCorners _frm;
    private double XValue = double.NaN;
    private double ZValue = double.NaN;
    private Imitator.Diss15 _device;

    internal Corners(IFrmWork15ForCorners frm)
    {
      _frm = frm;
      InitializeComponent();
    }

    internal Corners(IFrmWork15ForCorners frm, Imitator.Device device)
    {
      _frm = frm;
      if (device is Imitator.Diss15)
        _device = (Imitator.Diss15)device;
      InitializeComponent();
    }

    // Обнуление 
    private void rbtn_Zero_Click(object sender, EventArgs e)
    {
      if (rbtn_Zero.FlatStyle == FlatStyle.Standard)
      {
        ManualRecordTask2();
        IncludeRadioButtonCorner(rbtn_Zero);
        txtBox_1.Enabled = txtBox_2.Enabled = txtBox_3.Enabled =
                                              cmbBox_1.Enabled = cmbBox_2.Enabled = cmbBox_3.Enabled =
                                                                                    rbtn_Kren.Enabled =
                                                                                    rbtn_Tangazh.Enabled = false;
        //btn_Corner.Enabled = cmbBox_Corner.Enabled = 
        tiCorner.VisualText = " 0°";
        //txtBox_Block_1.Enabled = txtBox_Block_2.Enabled = true;
        //txtBox_Block_1.Focus();
        cmbBox_Corner.SelectedItem = " 0°";
        if (_device != null)
        {
          _device.SetKren(false, false);
          _device.SetTangazh(false, false);
        }
        DiscreteOutputs.Instance().SetOutput("Pitch", true);
        DiscreteOutputs.Instance().SetOutput("Heel", true);
        Diss15Functions.SetAngle(0);
        _frm.SetIndicatorLimits((int)Globals.IndicatorNumber.Speed1, _frm.strW_Back_15);
        _frm.SetIndicatorLimits((int)Globals.IndicatorNumber.Speed2, _frm.strW_Right_15);
        //ControlSignal("Pitch_On");
        //ControlSignal("Heel_On");
        //ControlSignal("SKT_0");
      }
    }

    /// <summary>
    /// Переключение RadioButton в groupBox на
    /// вкладке "Контрольные задачи" 
    /// </summary>
    /// <param name="rbtn">нажали RadioButton</param>
    private static void IncludeRadioButtonCorner(RadioButton rbtn)
    {
      rbtn.FlatStyle = FlatStyle.Flat;
      rbtn.BackColor = Color.FromArgb(153, 249, 164);
      ControlCollection coll = rbtn.Parent.Controls;
      for (int i = 0; i < coll.Count; i++)
        if (coll[i].GetType() == typeof (RadioButton) &&
            ((RadioButton) coll[i]).FlatStyle == FlatStyle.Flat &&
            coll[i].Name != rbtn.Name)
        {
          ((RadioButton) coll[i]).FlatStyle = FlatStyle.Standard;
          coll[i].BackColor = Color.Transparent;
        }
    }

    /// <summary>
    /// Запись в БД введенных значений курса и тангажа КЗ-2 
    /// </summary>
    internal void ManualRecordTask2()
    {
      // нет новых введенных значений 
      if (tiCorner.VisualText == " 0°" || cmbBox_1.Enabled == false &&
          cmbBox_2.Enabled == false && cmbBox_3.Enabled == false) return;
      double enterValue = 0.0; // что ввели 
      bool bl = false;
      string s; // имя сигнала 
      if (cmbBox_1.Enabled) // X 
      {
        if (cmbBox_1.SelectedItem == null) return;

        // "Увеличилось", "Уменьшилось"

        // во все txtBox вводим на сколько увелич/уменьш. значение 
        enterValue = Functions.MoveToDouble(txtBox_1.Text);
        if (double.IsNaN(enterValue))
        {
          bl = cmbBox_1.SelectedItem.ToString() != Resources.NotChanged;
          enterValue = 0;
        }

        enterValue = !bl && cmbBox_1.SelectedItem.ToString() == Resources.GreatThan
               ? enterValue
               : !bl && cmbBox_1.SelectedItem.ToString() == Resources.LessThan ? -enterValue : 0.0;

        if (!bl)
        {
          s = Is15Degree ? "Tangazh15_X_15" : "Tangazh345_X_15";
          _frm.ManualRecordDouble(s, enterValue);
        }
      }
      bl = false;
      if (cmbBox_2.Enabled) // Z 
      {
        if (cmbBox_2.SelectedItem == null) return;
        enterValue = Functions.MoveToDouble(txtBox_2.Text);
        if (double.IsNaN(enterValue))
        {
          bl = cmbBox_2.SelectedItem.ToString() != Resources.NotChanged;
          enterValue = 0;
        }

        enterValue = !bl && cmbBox_2.SelectedItem.ToString() == Resources.GreatThan
                       ? enterValue
                       : !bl && cmbBox_2.SelectedItem.ToString() == Resources.LessThan ? -enterValue : 0.0;

        if (!bl)
        {
          s = Is15Degree ? "Kren15_Z_15" : "Kren345_Z_15";
          _frm.ManualRecordDouble(s, enterValue);
        }
      }
      bl = false;
      if (cmbBox_3.Enabled) // Y 
      {
        s = rbtn_Kren.FlatStyle == FlatStyle.Flat ? "Kren" : "Tangazh"; // Tangazh345_Y_15
        string s1 = Is15Degree ? "15_Y_15" : "345_Y_15";
        s = s + s1;
        if (cmbBox_3.SelectedItem == null) return;


        enterValue = Functions.MoveToDouble(txtBox_3.Text);
        if (double.IsNaN(enterValue))
        {
          bl = cmbBox_3.SelectedItem.ToString() != Resources.NotChanged;
          enterValue = 0;
        }

        enterValue = !bl && cmbBox_3.SelectedItem.ToString() == Resources.GreatThan
                       ? enterValue
                       : !bl && cmbBox_3.SelectedItem.ToString() == Resources.LessThan ? -enterValue : 0.0;

        if (!bl) _frm.ManualRecordDouble(s, enterValue);
      }
    }

    private string _15DegreeStr = "+15°";
    private string Kren15ZStr = "Kren15_Z_15";
    private string Kren345ZStr = "Kren345_Z_15";
    private string Tangazh15XStr = "Tangazh15_X_15";
    private string Tangazh345XStr = "Tangazh345_X_15";

    private bool Is15Degree
    {
      get { return tiCorner.VisualText == _15DegreeStr; }
    }

    // Крен 
    private void rbtn_Kren_Click(object sender, EventArgs e)
    {
      if (rbtn_Kren.FlatStyle == FlatStyle.Standard)
      {
        ManualRecordTask2();
        DiscreteOutputs.Instance().SetOutput("Pitch", true);
        DiscreteOutputs.Instance().SetOutput("Heel", false);
        //ControlSignal("Pitch_On");
        //ControlSignal("Heel_Off"); // Выключили Крен 
        if (_device != null)
          _device.SetKren(true, Is15Degree);


        _frm.SetIndicatorLimits((int)Globals.IndicatorNumber.Speed2, ZValue, Is15Degree ? Kren15ZStr : Kren345ZStr);
        IncludeRadioButtonCorner(rbtn_Kren);
        LogicWork_2();
      }
    }

    // Тангаж
    private void rbtn_Tangazh_Click(object sender, EventArgs e)
    {
      if (rbtn_Tangazh.FlatStyle == FlatStyle.Standard)
      {
        ManualRecordTask2();
        DiscreteOutputs.Instance().SetOutput("Heel", true);
        DiscreteOutputs.Instance().SetOutput("Pitch", false);
        //ControlSignal("Heel_On");
        //ControlSignal("Pitch_Off"); // Выключили Тангаж  
        if (_device != null)
          _device.SetTangazh(true, Is15Degree);
        _frm.SetIndicatorLimits((int)Globals.IndicatorNumber.Speed1, XValue, Is15Degree ? Tangazh15XStr : Tangazh345XStr);
        IncludeRadioButtonCorner(rbtn_Tangazh);
        LogicWork_2();
      }
    }

    /// <summary>
    /// Логика работы, выдачи упр.команд для ввода углов при КЗ-2 
    /// </summary>
    private void LogicWork_2()
    {
      rbtn_Tangazh.Enabled = rbtn_Kren.Enabled = false;
      txtBox_1.Text = txtBox_2.Text = txtBox_3.Text = "";

      cmbBox_3.Enabled = true; // Y составляющая всегда 
      cmbBox_1.Enabled = rbtn_Tangazh.Checked;
      cmbBox_2.Enabled = rbtn_Kren.Checked;
      MessageBox.Show(Resources.FixChanges, Resources.ApplicationName,
                      MessageBoxButtons.OK, MessageBoxIcon.Information);
      // очистить cmbBox_1... - шоб были чистинькие
      string[] strFill = new string[] {Resources.NotChanged, Resources.GreatThan, Resources.LessThan};
      cmbBox_1.Items.Clear();
      cmbBox_1.Items.AddRange(strFill);
      cmbBox_2.Items.Clear();
      cmbBox_2.Items.AddRange(strFill);
      cmbBox_3.Items.Clear();
      cmbBox_3.Items.AddRange(strFill);
    }

    private void txtBox_TextChanged(object sender, EventArgs e)
    {
      TextBox current = (TextBox) sender;
      current.ForeColor = SystemColors.WindowText;
      current.Text = current.Text.Trim();
    }

    private string CornerPlus15 = "+15°";
    private string CornerMinus15 = "-15°";

    private void txtBox_Validating(object sender, CancelEventArgs e)
    {
      TextBox tbCurrent = (TextBox) sender;
      if (tbCurrent.Text != "")
      {
        double d = Functions.MoveToDouble(tbCurrent.Text);
        if (double.IsNaN(d))
        {
          Globals.ErrorInputTextBox(tbCurrent);
          return;
        }
        int min = 0;
        int max = 10;
        if (tbCurrent.Name == txtBox_3.Name)
          max = 5;

        if (!(min <= d && d <= max))
        {
          string errStr = sender.Equals(txtBox_2)
                            ? Resources.SpeedRight
                            : sender.Equals(txtBox_1) ? Resources.SpeedBackward : Resources.SpeedUp;
          Globals.ErrorCorridorTextBox(tbCurrent, min, max, errStr);
          return;
        }

        ComboBox cbCurrent = tbCurrent.Name == txtBox_1.Name
                               ? cmbBox_1
                               : tbCurrent.Name == txtBox_2.Name ? cmbBox_2 : cmbBox_3;
        bool result = true;
        if (tbCurrent.Name != txtBox_3.Name) // не равен вверх-вниз
        {
          result &= (1.5 <= d && d <= 3.5);
          if (rbtn_Tangazh.Checked)
            result &= cmbBox_Corner.SelectedItem.ToString() == CornerPlus15 &&
                      cbCurrent.SelectedItem.ToString() == Resources.LessThan ||
                      cmbBox_Corner.SelectedItem.ToString() == CornerMinus15 &&
                      cbCurrent.SelectedItem.ToString() == Resources.GreatThan;
          else
            result &= cmbBox_Corner.SelectedItem.ToString() == CornerPlus15 &&
                      cbCurrent.SelectedItem.ToString() == Resources.GreatThan ||
                      cmbBox_Corner.SelectedItem.ToString() == CornerMinus15 &&
                      cbCurrent.SelectedItem.ToString() == Resources.LessThan;

          tbCurrent.ForeColor = result ? Color.LimeGreen : Color.Red;
        }
        else
        {
          result &= 0.8 <= d && d <= 1.7;
          if (rbtn_Tangazh.Checked)
            result &= cmbBox_Corner.SelectedItem.ToString() == CornerPlus15 &&
                      cbCurrent.SelectedItem.ToString() == Resources.GreatThan ||
                      cmbBox_Corner.SelectedItem.ToString() == CornerMinus15 &&
                      cbCurrent.SelectedItem.ToString() == Resources.LessThan;
          else
            result &= cmbBox_Corner.SelectedItem.ToString() == CornerPlus15 &&
                      cbCurrent.SelectedItem.ToString() == Resources.LessThan ||
                      cmbBox_Corner.SelectedItem.ToString() == CornerMinus15 &&
                      cbCurrent.SelectedItem.ToString() == Resources.GreatThan;

          tbCurrent.ForeColor = result ? Color.LimeGreen : Color.Red;
        }
      }
    }


    private void cmbBox_SelectedIndexChanged(object sender, EventArgs e)
    {
      ComboBox cbCurrent = (ComboBox) sender;
      TextBox tbCurrent = cbCurrent.Name == cmbBox_1.Name
                            ? txtBox_1
                            : cbCurrent.Name == cmbBox_2.Name ? txtBox_2 : txtBox_3;
      if (cbCurrent.SelectedItem.ToString() == Resources.NotChanged)
      {
        tbCurrent.Text = "";
        tbCurrent.Enabled = false;
        return; // уходим, т.к. "Не изменилось"
      }
      tbCurrent.Text = "";
      tbCurrent.Enabled = true;
    }

    // выбрали угол в cmbBox_Corner 
    private void cmbBox_Corner_SelectedValueChanged(object sender, EventArgs e)
    {
      if (cmbBox_Corner.SelectedItem.ToString() == " 0°") // при 0° => "Обнуление"
      {
        if (!rbtn_Zero.Checked)
          rbtn_Zero.PerformClick();
        rbtn_Kren.Checked = rbtn_Tangazh.Checked = radioButton1.Checked = false;
        return;
      }
      if (double.IsNaN(XValue))
        XValue = _frm.GetMeasuredValue(_frm.strW_Back_15);
      if (double.IsNaN(ZValue))
        ZValue = _frm.GetMeasuredValue(_frm.strW_Right_15);
      ManualRecordTask2();
      if (_device != null)
      {
        _device.SetKren(false, false);
        _device.SetTangazh(false, false);
      }
      DiscreteOutputs.Instance().SetOutput("Pitch", true);
      DiscreteOutputs.Instance().SetOutput("Heel", true);
      _frm.SetIndicatorLimits((int)Globals.IndicatorNumber.Speed1, _frm.strW_Back_15);
      _frm.SetIndicatorLimits((int)Globals.IndicatorNumber.Speed2, _frm.strW_Right_15);
      //ControlSignal("Pitch_On");
      //ControlSignal("Heel_On");
      //string s = cmbBox_Corner.SelectedItem.ToString() == "+15°" ? "SKT_15" : "SKT_345";
      ushort angle = (ushort) (cmbBox_Corner.SelectedItem.ToString() == "+15°" ? 15 : 345);
      Diss15Functions.SetAngle(angle);
      //ControlSignal(s);

      tiCorner.VisualText = cmbBox_Corner.SelectedItem.ToString();
      txtBox_1.Enabled = txtBox_2.Enabled = txtBox_3.Enabled =
                                            cmbBox_1.Enabled = cmbBox_2.Enabled = cmbBox_3.Enabled = false;
      //txtBox_Block_1.Enabled = txtBox_Block_2.Enabled = false;

      rbtn_Kren.Enabled = rbtn_Tangazh.Enabled = true;
      rbtn_Kren.FlatStyle = rbtn_Tangazh.FlatStyle = FlatStyle.Standard;
      rbtn_Kren.BackColor = rbtn_Tangazh.BackColor = Color.Transparent;
      rbtn_Kren.Checked = rbtn_Tangazh.Checked = false;
    }

    private void Corners_Load(object sender, EventArgs e)
    {
      cmbBox_1.Enabled = cmbBox_2.Enabled = cmbBox_3.Enabled = txtBox_1.Enabled =
                                                               txtBox_2.Enabled = txtBox_3.Enabled = false;
      rbtn_Zero.PerformClick();
    }

    private void rbtn_Kren_CheckedChanged(object sender, EventArgs e)
    {

    }

    internal void Dectivate()
    {
        if (!rbtn_Zero.Checked)
            rbtn_Zero.PerformClick();
    }

    private void radioButton1_CheckedChanged(object sender, EventArgs e)
    {
        
    }

    private void rbtn_Tangazh_CheckedChanged(object sender, EventArgs e)
    {

    }

    private void radioButton1_Click(object sender, EventArgs e)
    {
      Diss15Functions.SetAngle(0);
    }

    private void radioButton2_Click(object sender, EventArgs e)
    {
      Diss15Functions.SetAngle(90);        
    }
  }
}
