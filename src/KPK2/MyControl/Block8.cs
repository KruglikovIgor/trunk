using System;
using System.Drawing;
using System.Windows.Forms;
using KPK2.Properties;
using Sparc.KPK2.Hardware;
using Sparc.KPK2.NFunctions;
using Res = Sparc.KPK2.Resources.Properties.Resources;
using Timer = System.Windows.Forms.Timer;

namespace KPK2.MyControl
{
  public partial class Block8 : UserControl
  {
    Timer tmrBlock8;
#if (!emulation)
    private readonly TimeSpan _totalTimeSpan = new TimeSpan(0, 15, 0);
#else
    private readonly TimeSpan _totalTimeSpan = new TimeSpan(0, 1, 0);
#endif
    private DateTime _stopTime;
    public static readonly TimeSpan WaitBeforeCheckDirection = TimeSpan.FromSeconds(10);
    public delegate bool SetReverseDirectionDelegate();
    public SetReverseDirectionDelegate SetReverseDirection;
    public delegate double GetMeasValueDelegate(string parameter);
    public GetMeasValueDelegate GetMeasValue;
    public bool _diss32_90 = false;

    /// <summary>
    /// № новой проверки для записи в БД
    /// </summary>
    private int blockID;

    public int BlockId
    {
      set { blockID = value; }
    }

    public Block8()
    {
      InitializeComponent();
    }

    private double minPathGeo = 0;
    private double maxPathGeo = 0;


    private double minAngleGeo = 0;
    private double maxAngleGeo = 0;


    public Block8(bool showAddElements)
    {
      InitializeComponent();
      btn_8_On_6.Text = Res.Block8On;
      btn_8_Off_6.Text = Res.Block8Off;
      _diss32_90 = showAddElements;
      gbBlock8Geo.Visible = lVisirBlock10.Visible = cbVisirBlock10.Visible = _diss32_90;
      DataManager.Instance.GetParamLimitAndId(PathGeoParam, ref minPathGeo, ref maxPathGeo);
      DataManager.Instance.GetParamLimitAndId(AngleGeoParam, ref minAngleGeo, ref maxAngleGeo);
    }

    private void btn_8_On_6_Click(object sender, EventArgs e)
    {
      if (btn_8_On_6.FlatStyle == FlatStyle.Standard)
      {
        Save();
        MessageBox.Show(Resources.Block8OrtWarningSimple, Resources.Attention);
        if (_diss32_90)
          MessageBox.Show(Resources.PrepareBlock10, Resources.Attention);
        //_isBlock8OrtCheck = true;
        SetDirectionFunc();
        CheckDirection();
        DiscreteOutputs.Instance().SetOutput("Block_8", true);
        //DiscreteOutputs.Instance().SetOutput("Course", true);
        btn_8_On_6.FlatStyle = FlatStyle.Flat;
        btn_8_On_6.BackColor = Color.FromArgb(153, 249, 164);
        btn_8_Off_6.FlatStyle = FlatStyle.Standard;
        btn_8_Off_6.BackColor = Color.Transparent;
        btn_8_Off_6.Enabled = true;

        progressBar_6.Value = 0;
        progressBar_6.Maximum = 900;
        progressBar_6.Visible = true;
        _stopTime = DateTime.Now + _totalTimeSpan;

        lbl_6_Time.Visible = true;
        lbl_6_Time.Text = string.Format("{0}: {1} {2} 0 {3}", Resources.TimeLeft, _totalTimeSpan.TotalMinutes, Resources.Minutes,
                                        Resources.Seconds);

        txtBox_6_1.Text = txtBox_6_2.Text = "";
        txtBox_6_1.ForeColor =
        txtBox_6_2.ForeColor = SystemColors.WindowText;
        cmbBox_6_1.Items.Clear();
        cmbBox_6_1.Items.AddRange(new[] { Resources.Forward.ToUpper(), Resources.Back.ToUpper() });
        cmbBox_6_2.Items.Clear();
        cmbBox_6_2.Items.AddRange(new[] { Resources.Right.ToUpper(), Resources.Left.ToUpper() });
        cbPathGeo.Items.Clear();
        cbPathGeo.Items.AddRange(new[] { Resources.Forward.ToUpper(), Resources.Back.ToUpper() });
        cbAngleGeo.Items.Clear();
        cbAngleGeo.Items.AddRange(new[] { Resources.Right.ToUpper(), Resources.Left.ToUpper() });
        EnableControls(false);
        tmrBlock8.Start();
      }
    }

    private bool ReverseDirectionFunc()
    {
      var handler = SetReverseDirection;
      if (handler != null)
        return handler();
      return false;
    }

    private double GetMeasValueFunc(string parameter)
    {
      var handler = GetMeasValue;
      if (handler != null)
        return handler(parameter);
      return double.NaN;
    }

    private void CheckDirection()
    {
        return;
      //int channel = DataManager.Instance.GetChannelByName(DataManager.Advantech1760, "Block8_32");
      //Board176X.Instance.CloseRelay(channel);
      //Thread.Sleep(1000);
      //Board176X.Instance.OpenRelay(channel);
      //DiscreteOutputs.Instance().SetOutput("Block_8", true);
      //Thread.Sleep(WaitBeforeCheckDirection);
      //double result = GetMeasValueFunc(frmWork32.ZTaskStr);
      //if (!double.IsNaN(result) && result < 0)
      //  if (ReverseDirectionFunc())
      //    Thread.Sleep(WaitBeforeCheckDirection);
      //DiscreteOutputs.Instance().SetOutput("Block_8", false);
    }

    private void btn_8_Off_6_Click(object sender, EventArgs e)
    {
      if (btn_8_Off_6.FlatStyle == FlatStyle.Standard)
      {
        tmrBlock8.Stop();
        btn_8_Off_6.FlatStyle = FlatStyle.Flat;
        btn_8_Off_6.BackColor = Color.FromArgb(153, 249, 164);
        btn_8_On_6.FlatStyle = FlatStyle.Standard;
        btn_8_On_6.BackColor = Color.Transparent;

        DiscreteOutputs.Instance().SetOutput("Block_8", false);
        //DiscreteOutputs.Instance().SetOutput("Course", false);
        GenSinCos.Instance().GeneratorOff();
        if (DateTime.Now > _stopTime)
          EnableControls(true);
      }
    }

    private void EnableControls(bool state)
    {
      if (!state)
      {
        txtBox_6_1.Enabled =
          txtBox_6_2.Enabled =
            cmbBox_6_1.Enabled = cmbBox_6_2.Enabled = cbVisirBlock10.Enabled = false;
        tPathGeo.Enabled = tAngleGeo.Enabled = cbPathGeo.Enabled = cbAngleGeo.Enabled = false;
      }
      else
      {
        cmbBox_6_1.Enabled = cmbBox_6_2.Enabled = true;
        cbPathGeo.Enabled = cbAngleGeo.Enabled = true;
        if (_diss32_90)
          cbVisirBlock10.Enabled = true;
      }

    }

    internal bool Save()
    {
      bool result = true;
      double value;
      if (!String.IsNullOrEmpty(txtBox_6_1.Text))
      {
        value = Functions.MoveToDouble(txtBox_6_1.Text);
        if (cmbBox_6_1.Text == Resources.Back.ToUpper())
          value *= -1;
        result &= ManualRecord("Point24_X", value.ToString());
      }
      if (!String.IsNullOrEmpty(txtBox_6_2.Text))
      {
        value = Functions.MoveToDouble(txtBox_6_2.Text);
        if (cmbBox_6_2.Text == Resources.Right.ToUpper())
          value *= -1;
        result &= ManualRecord("Point24_Z", value.ToString());
      }

      if (!String.IsNullOrEmpty(tPathGeo.Text))
      {
        value = Functions.MoveToDouble(tPathGeo.Text);
        if (cbPathGeo.Text == Resources.Back.ToUpper())
          value *= -1;
        result &= ManualRecord(PathGeoParam, value.ToString());
      }
      if (!String.IsNullOrEmpty(tAngleGeo.Text))
      {
        value = Functions.MoveToDouble(tAngleGeo.Text);
        if (cbAngleGeo.Text == Resources.Right.ToUpper())
          value *= -1;
        result &= ManualRecord(AngleGeoParam, value.ToString());
      }
      if (cbVisirBlock10.Visible)
        if (cbVisirBlock10.SelectedIndex != -1)
          result &= ManualRecord("B10_VisirPosition", cbVisirBlock10.SelectedIndex == 0);
      return result;
    }

    const string PathGeoParam = "Point24_X_2";
    const string AngleGeoParam = "Point24_Z_2";

    private bool ManualRecord(string param, string value)
    {
      double result = Functions.MoveToDouble(value);
      if (double.IsNaN(result))
        return false;
      return ManualRecord(param, result);
    }

    private bool ManualRecord(string param, double value)
    {
      if (double.IsNaN(value))
        return true;
      return DataManager.Instance.SaveParameter(blockID, param, value);
    }

    private bool ManualRecord(string param, bool value)
    {
      double val = value ? 1 : 0;
      return ManualRecord(param, val);
    }

    private void Block8_Load(object sender, EventArgs e)
    {
      tmrBlock8 = new Timer();
      tmrBlock8.Interval = 1000;
      tmrBlock8.Tick += TmrBlock8OnTick;
    }

    private void TmrBlock8OnTick(object sender, EventArgs eventArgs)
    {
      TimeSpan timeLeft = _stopTime - DateTime.Now;
      if (timeLeft.TotalSeconds <= 0)
        timeLeft = new TimeSpan(0);
      lbl_6_Time.Text = string.Format("{0}: {1} {2} {3} {4}", Resources.TimeLeft, timeLeft.Minutes, Resources.Minutes, timeLeft.Seconds, Resources.Seconds);
      if (progressBar_6.Value != progressBar_6.Maximum)
        progressBar_6.Value = (int)(progressBar_6.Maximum - timeLeft.TotalSeconds / _totalTimeSpan.TotalSeconds * progressBar_6.Maximum);
      if (timeLeft.TotalSeconds <= 0) // время кончилось 
      {
        if (btn_8_On_6.FlatStyle == FlatStyle.Flat)
          btn_8_Off_6.PerformClick();
        //else
        //  b8GeoOff.PerformClick();
      }
    }

    public MethodInvoker SetDirection;

    private void SetDirectionFunc()
    {
      var handler = SetDirection;
      if (handler != null)
        handler();
    }

    private void cmbBox_6_1_SelectedIndexChanged(object sender, EventArgs e)
    {
      txtBox_6_1.Enabled = true;
      txtBox_6_1.Text = "";
    }

    private void cmbBox_6_2_SelectedIndexChanged(object sender, EventArgs e)
    {
      txtBox_6_2.Enabled = true;
      txtBox_6_2.Text = "";
    }

    private void txtBox_6_1_TextChanged(object sender, EventArgs e)
    {
      txtBox_6_1.ForeColor = SystemColors.WindowText;
      txtBox_6_1.Text = txtBox_6_1.Text.Trim();
    }

    private void txtBox_6_2_TextChanged(object sender, EventArgs e)
    {
      txtBox_6_2.ForeColor = SystemColors.WindowText;
      txtBox_6_2.Text = txtBox_6_2.Text.Trim();
    }

    private void cbPathGeo_SelectedIndexChanged(object sender, EventArgs e)
    {
      tPathGeo.Enabled = true;
      tPathGeo.Text = "";
    }

    private void cbAngleGeo_SelectedIndexChanged(object sender, EventArgs e)
    {
      tAngleGeo.Enabled = true;
      tAngleGeo.Text = "";
    }

    private void tPathGeo_Validating(object sender, System.ComponentModel.CancelEventArgs e)
    {
      tPathGeo.ForeColor = SystemColors.WindowText;
      if (String.IsNullOrEmpty(tPathGeo.Text.Trim()))
        return;
      if (cbPathGeo.Text == Resources.Back.ToUpper())
      {
        tPathGeo.ForeColor = Color.Red;
        return;
      }
      double value = Functions.MoveToDouble(tPathGeo.Text);
      if (value >= minPathGeo && value <= maxPathGeo)
        tPathGeo.ForeColor = Color.LimeGreen;
      else
        tPathGeo.ForeColor = Color.Red;
    }

    private void tAngleGeo_Validating(object sender, System.ComponentModel.CancelEventArgs e)
    {
      tAngleGeo.ForeColor = SystemColors.WindowText;
      if (String.IsNullOrEmpty(tAngleGeo.Text.Trim()))
        return;
      if (cbAngleGeo.Text == Resources.Right.ToUpper())
      {
        tAngleGeo.ForeColor = Color.Red;
        return;
      }
      double value = Functions.MoveToDouble(tAngleGeo.Text);
      if (value >= minAngleGeo && value <= maxAngleGeo)
        tAngleGeo.ForeColor = Color.LimeGreen;
      else
        tAngleGeo.ForeColor = Color.Red;
    }
  }
}
