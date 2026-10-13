using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using KPK2.Common.DissFunctions;
using Sparc.KPK2.Imitator;
using Sparc.KPK2.NFunctions;
using Globals = KPK2.Common.Globals;
using Res = Sparc.KPK2.Resources.Properties.Resources;

namespace KPK2.MyControl
{
  public partial class ControlDVS_24 : UserControl
  {
    Timer tBlock8 = new Timer();
    private DateTime _startTime;
#if (!emulation)
    private TimeSpan _waitTime = new TimeSpan(0,5,0);
#else
    private TimeSpan _waitTime = new TimeSpan(0,0,5);
#endif
    private List<KeyValuePair<string, double[]>> paramLimits = new List<KeyValuePair<string, double[]>>();
    private const string Block8_Forward = "DVS_B8_Forward";
    private const string Block8_ToSide = "DVS_B8_ToSide";
    private const string Block10_Up = "DVS_B10_Up";
    private const string Block10_ToSide = "DVS_B10_ToSide";
    public const string MapCorner = "DVS_MapCorner";
    private bool timePast;


    public delegate void SaveParameterDelegate(string paramName, double value);

    public event SaveParameterDelegate SaveParameter;
    public event MethodInvoker CheckAndSaveMapCorner;

    public ControlDVS_24()
    {
      InitializeComponent();
      double min = 0, max = 0;
      DataManager.Instance.GetParamLimitAndId(Block8_Forward, ref min, ref max);
      paramLimits.Add(new KeyValuePair<string, double[]>(tbB8_Forward.Name, new[] {min, max}));
      DataManager.Instance.GetParamLimitAndId(Block8_ToSide, ref min, ref max);
      paramLimits.Add(new KeyValuePair<string, double[]>(tbB8_ToSide.Name, new[] {min, max}));
      DataManager.Instance.GetParamLimitAndId(Block10_Up, ref min, ref max);
      paramLimits.Add(new KeyValuePair<string, double[]>(tbB10_Up.Name, new[] {min, max}));
      DataManager.Instance.GetParamLimitAndId(Block10_ToSide, ref min, ref max);
      paramLimits.Add(new KeyValuePair<string, double[]>(tbB10_ToSide.Name, new[] {min, max}));
    }

    private void bKZ2_Click(object sender, EventArgs e)
    {
      bKZ2.Enabled = false;
      bool result = bKZ2.FlatStyle == FlatStyle.Standard;
      bKZ2.FlatStyle = result ? FlatStyle.Flat : FlatStyle.Standard;
      bKZ2.Text = string.Format(result? Res.TaskNOff : Res.TaskNOn, string.Format(Res.KZN, 2));
      if (result)
      {
        MessageBox.Show(Res.DVS24_KZ2Message);
        Diss15Functions.Block8On();
        Diss15Functions.SetTask2();
        Diss15Functions.DVSOn();
        timePast = false;
        StartTimer();
        bCheckMapCorner.Enabled = false;
      }
      else
      {
        gbBlock8.Enabled = gbBlock10.Enabled = false;
        StopTimer();
        Diss15Functions.Block8Off();
        Diss15Functions.TaskOff();
        Diss15Functions.DVSOff();
        if (timePast)
          SaveParameters();
        bCheckMapCorner.Enabled = true;
      }
      bKZ2.Enabled = true;
    }

    private void SaveParameters()
    {
      SaveParamFunc(Block8_Forward, Globals.MoveToDouble(tbB8_Forward.Text));
      SaveParamFunc(Block8_ToSide, Globals.MoveToDouble(tbB8_ToSide.Text));
      SaveParamFunc(Block10_Up, Globals.MoveToDouble(tbB10_Up.Text));
      SaveParamFunc(Block10_ToSide, Globals.MoveToDouble(tbB10_ToSide.Text));
    }

    private void SaveParamFunc(string parameter, double value)
    {
      try
      {
        if (SaveParameter != null)
          SaveParameter(parameter, value);
      }
      catch{}
    }

    private void StartTimer()
    {
      tBlock8.Interval = 1000;
      tBlock8.Tick += TBlock8OnTick;
      lTime.Visible = pbTime.Visible = true;
      _startTime = DateTime.Now;
      tBlock8.Start();
    }

    private void TBlock8OnTick(object sender, EventArgs eventArgs)
    {
      TimeSpan _timepast = DateTime.Now - _startTime;
      TimeSpan _timeLeft = _waitTime - _timepast;
      if (_timeLeft.TotalSeconds < 0)
        _timeLeft = new TimeSpan(0);
      if (_timepast <= _waitTime)
        DisplayTime(_timeLeft);
      if (_timepast >= _waitTime)
      {
        timePast = true;
        StopTimer();
        Diss15Functions.Block8Off();
        gbBlock8.Enabled = gbBlock10.Enabled = true;
        ShowMessage(Res.DVS_EnterValuesMessage);
      }
    }

    private void StopTimer()
    {
      if (tBlock8.Enabled)
        tBlock8.Stop();
      pbTime.Value = pbTime.Maximum;
      bCheckMapCorner.Enabled = true;
    }

    private void ShowMessage(string mess)
    {
      if (InvokeRequired)
      {
        Invoke(new Action<string>(ShowMessage), mess);
        return;
      }
      MessageBox.Show(mess);
    }

    private void DisplayTime(TimeSpan timeLeft)
    {
      if (InvokeRequired)
      {
        Invoke(new Action<TimeSpan>(DisplayTime), timeLeft);
        return;
      }
      lTime.Text = string.Format(Res.TimeLeftNN, timeLeft.Minutes, timeLeft.Seconds);
      pbTime.Value = (int)(((_waitTime - timeLeft).Ticks / (double)_waitTime.Ticks) * pbTime.Maximum);
    }

    private void bCheckMapCorner_Click(object sender, EventArgs e)
    {
      bCheckMapCorner.Enabled = false;
#if(emulation)
      Imitator.Diss15.CurrentInstance.SetMapCorner(true);
#endif
      MessageBox.Show(Res.DVS_CheckMapCornerMessage);
      CheckAndSaveMapCornerFunc();
#if(emulation)
      Imitator.Diss15.CurrentInstance.SetMapCorner(false);
#endif
      bCheckMapCorner.Enabled = true;
    }

    private void CheckAndSaveMapCornerFunc()
    {
      try
      {
        if (CheckAndSaveMapCorner != null)
          CheckAndSaveMapCorner();
      }
      catch
      {
        
      }
    }

    private void TextBox_Validated(object sender, EventArgs e)
    {
      TextBox tbCurrent = (TextBox) sender;
      tbCurrent.Text = tbCurrent.Text.Trim();
      double result;
      if (!String.IsNullOrEmpty(tbCurrent.Text))
      {
        result = Globals.MoveToDouble(tbCurrent.Text);
        if (double.IsNaN(result))
          ErrorTextBox(tbCurrent);
        else
        {
          bool successLight = LightTextBoxByValue(tbCurrent.Name, result);
          tbCurrent.ForeColor = successLight ? Globals.SuccessTbColor : Globals.FailColor;
        }
      }
      else
      {
        result = 0;
        tbCurrent.ForeColor = Color.Black;
      }
      if (!double.IsNaN(result))
        tbCurrent.BackColor = Color.White;

    }

    private bool LightTextBoxByValue(string bName, double result)
    {
      foreach (KeyValuePair<string, double[]> key in paramLimits)
      {
        if (key.Key == bName)
          return result >= key.Value[0] && result <= key.Value[1];
      }
      return false;
    }

    private void ErrorTextBox(TextBox tbCurrent)
    {
      tbCurrent.BackColor = Globals.FailColor;
      MessageBox.Show(Res.NotANumberMessage, Res.Error);
      tbCurrent.Focus();
    }

    internal void StopCurrentTask()
    {
      if (bKZ2.FlatStyle == FlatStyle.Flat)
        bKZ2.PerformClick();
    }
  }
}
