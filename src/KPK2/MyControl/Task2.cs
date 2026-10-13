using System;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;
using KPK2.DB;
using KPK2.MyClasse;
using KPK2.Properties;
using Sparc.KPK2.Hardware;
using Sparc.KPK2.Hardware.LTR;
using Sparc.KPK2.NFunctions;
using Globals = KPK2.Common.Globals;
using Timer = System.Windows.Forms.Timer;

namespace KPK2.MyControl
{
  public partial class Task2: UserControl
  {
    private Timer tmrBlock8 = new Timer();
    //private int countBlock8;
    //private int strDelta;
    private Timer StopTimer = new Timer();
    private Measure _measure;
    private MeasuringValueInfo _measValueInfo;
    private DataBaseKPK2 _dbKPK2;

    private readonly TimeSpan ts5Min = TimeSpan.FromMinutes(5);
    private readonly TimeSpan ts39Sec = TimeSpan.FromSeconds(39);
    private TimeSpan _currentTs;
    private DateTime _startTime;
    public Block8.SetReverseDirectionDelegate SetReverseDirection;

    public EventHandler EventInitializeZTask;
    public Action<object, string> EventErrorDetected;
    public EventHandler EventInitializeTask2;
    public EventHandler EventInitializeZPNPTask;
    public EventHandler EventInitializeZZeroTask;



    public Task2(Measure measure, MeasuringValueInfo measuringValueInfo, DataBaseKPK2 dbKPK2)
    {
      InitializeComponent();
      tmrBlock8.Interval = 1000;
      tmrBlock8.Tick += tmrBlock8_Tick;
      StopTimer.Tick += StopTimerOnTick;
      _measure = measure;
      _measValueInfo = measuringValueInfo;
      _dbKPK2 = dbKPK2;
    }

    private void StopTimerOnTick(object sender, EventArgs eventArgs)
    {
      tmrBlock8.Stop();
      Button btn = btn_T2_Task.FlatStyle == FlatStyle.Flat ? btn_T2_Task : btn_T2_Z;
      //countBlock8 = 0;
      btn.PerformClick();
      lbl_T2_Time.Visible = false;
    }

    private void btn_T2_Task_Click(object sender, EventArgs e)
    {
      btn_T2_Task.Enabled = false;
      btn_T2_Z.Enabled = btn_T2_Zero.Enabled = false;
      if (btn_T2_Task.FlatStyle == FlatStyle.Standard)
      {
        _measure.StopMeasuring(false);
        // скинуть индикаторы 
        siT2_1.Value = 0;
        siT2_2.Value = 0;
        siT2_1.Inactive = siT2_2.Inactive = true;
        lbl_T2_Time.Text = Resources.TimeLeft + ": " + _currentTs.Minutes + Resources.Minutes + " " +
                           _currentTs.Seconds + Resources.Seconds;
        lbl_T2_Time.Visible = true;
        label46.Text = Resources.Zopt;
        label45.Text = Resources.ZoptPNP;

        PressBtn_T2(btn_T2_Task);

        try
        {
          StartTimeCount();
          int channel = DataManager.Instance.GetChannelByName(DataManager.Advantech1760, "Block8_32");
          DO176X.Instance.SetRelay(channel);
          Thread.Sleep(1000);
          DO176X.Instance.ClearRelay(channel);
          DiscreteOutputs.Instance().SetOutput("Block_8", true);

          EventInitializeZTask?.Invoke(this, EventArgs.Empty);
          CheckDirection();
        }
        catch
        {
          EventErrorDetected?.Invoke(this, Globals.GetDeviceErrorString(Globals.Device.Advantech176X,
                                                          Globals.Operation.SetValue));
          return;
        }
      }
      else
      {
        StopTimeCount();
        lbl_T2_Time.Visible = false;
        WrungBtn_T2(btn_T2_Task);
        TimeSpan timeLeft = DateTime.Now - _startTime; //выждали нужное время
        timeLeft = _currentTs - timeLeft;
        
        _measure.StopMeasuring( timeLeft.Seconds <= 0 && _dbKPK2.SaveResults);
        EventInitializeTask2?.Invoke(this, EventArgs.Empty);
        try
        {
          DiscreteOutputs.Instance().SetOutput("Block_8", false);
        }
        catch
        {
        }
        btn_T2_Z.Enabled = btn_T2_Zero.Enabled = true;
      }
      btn_T2_Task.Enabled = true;
    }

    private void StopTimeCount()
    {
      tmrBlock8.Stop();
      StopTimer.Stop();
    }

    private void CheckDirection()
    {
        return;
      //Thread.Sleep(Block8.WaitBeforeCheckDirection);
      //double result = _frm._measure.GetValue(frmWork32.ZTaskStr);
      //if (double.IsNaN(result))
      //  result = _frm._measure.GetValue(frmWork32.ZPNPStr);
      //if (!double.IsNaN(result) && result < 0)
      //  if (ReverseDirectionFunc())
      //    _startTime = DateTime.Now;
    }

    private bool ReverseDirectionFunc()
    {
      var handler = SetReverseDirection;
      if (handler != null)
        return handler();
      return false;
    }

    private void StartTimeCount()
    {
      _startTime = DateTime.Now;
      tmrBlock8.Start();
      StopTimer.Interval = (int)_currentTs.TotalMilliseconds;
      StopTimer.Start();
    }

    private void tmrBlock8_Tick(object sender, EventArgs e)
    {
      //countBlock8--;
      //int s = countBlock8 % 60; // кол-во секунд
      //int m = countBlock8 / 60; // кол-во минут 
      TimeSpan timeLeft = DateTime.Now - _startTime;
      timeLeft = _currentTs - timeLeft;
      if (timeLeft.Seconds < 0)
        timeLeft = TimeSpan.FromSeconds(0);
      lbl_T2_Time.Text = Resources.TimeLeft + ": " + timeLeft.Minutes + Resources.Minutes + " " + timeLeft.Seconds + Resources.Seconds;
      if (Math.Abs(timeLeft.TotalSeconds - 0) < double.Epsilon)
        tmrBlock8.Stop();
      //if (countBlock8 == 0) // прошло 5 минут 
      //{
      //  tmrBlock8.Stop();
      //  Button btn = btn_T2_Task.FlatStyle == FlatStyle.Flat ? btn_T2_Task : btn_T2_Z;
      //  btn.PerformClick();
      //  lbl_T2_Time.Visible = false;
      //}
      //return;

    }

    private void WrungBtn_T2(Button btn)
    {
      btn.FlatStyle = FlatStyle.Standard;
      btn.BackColor = Color.Transparent;
      siT2_1.Inactive = siT2_2.Inactive = true;
    }

    private void btn_T2_Z_Click(object sender, EventArgs e)
    {
      btn_T2_Z.Enabled = false;
      btn_T2_Task.Enabled = btn_T2_Zero.Enabled = false;
      if (btn_T2_Z.FlatStyle == FlatStyle.Standard)
      {
        _measure.StopMeasuring(false);
        // скинуть индикаторы 
        siT2_1.Value = 0;
        siT2_2.Value = 0;
        siT2_1.Inactive = siT2_2.Inactive = true;
        lbl_T2_Time.Text = Resources.TimeLeft + ": " + _currentTs.Minutes + Resources.Minutes + " " + _currentTs.Seconds + Resources.Seconds;
        lbl_T2_Time.Visible = true;
        label46.Text = Resources.Zopt;
        label45.Text = Resources.ZoptPNP;

        PressBtn_T2(btn_T2_Z);
        try
        {
          StartTimeCount();
          DiscreteOutputs.Instance().SetOutput("Block_8", true);
          DO176X.Instance.SetRelay(DataManager.Instance.GetChannelByName(DataManager.Advantech1760,
                                                                                 "Block8_32"));
          Thread.Sleep(1000);
          DO176X.Instance.ClearRelay(DataManager.Instance.GetChannelByName(DataManager.Advantech1760,
                                                                         "Block8_32"));
          EventInitializeZPNPTask?.Invoke(this, EventArgs.Empty); //Todo переписать всё!
          CheckDirection();
        }
        catch
        {
          EventErrorDetected?.Invoke(this, Globals.GetDeviceErrorString(Globals.Device.Advantech176X, Globals.Operation.SetValue));
          return;
        }
      }
      else
      {
        StopTimeCount();
          double pnpValue = _measure.GetValue("Z_PNP_32") - _measValueInfo.ZoptPNPStartValue;
        lbl_T2_Time.Visible = false;
        WrungBtn_T2(btn_T2_Z);
        TimeSpan timeLeft = DateTime.Now - _startTime; //выждали нужное время
        timeLeft = _currentTs - timeLeft;
        if (timeLeft.Seconds <= 0)
          if (_dbKPK2.SaveResults)
              _dbKPK2.ManualRecordDouble("Z_PNP_32", pnpValue);
        _measure.StopMeasuring(false);
        EventInitializeTask2(this, EventArgs.Empty);
        btn_T2_Task.Enabled = btn_T2_Zero.Enabled = true;
        try
        {
          DiscreteOutputs.Instance().SetOutput("Block_8", false);
        }
        catch { }
      }
      btn_T2_Z.Enabled = true;
    }

    private void btn_T2_Zero_Click(object sender, EventArgs e)
    {
      btn_T2_Zero.Enabled = false;
      btn_T2_Z.Enabled = btn_T2_Task.Enabled = false;
      if (btn_T2_Zero.FlatStyle == FlatStyle.Standard)
      {
        _measure.StopMeasuring(false);
        // скинуть индикаторы 
        siT2_1.Value = 0;
        siT2_2.Value = 0;
        siT2_1.Inactive = siT2_2.Inactive = true;
        lbl_T2_Time.Visible = false;
        label46.Text = Resources.Zopt;
        label45.Text = Resources.ZoptPNP;

        PressBtn_T2(btn_T2_Zero);
        try
        {
            //StartTimeCount();
            int channel = DataManager.Instance.GetChannelByName(DataManager.Advantech1760, "Block8_32");
            DO176X.Instance.SetRelay(channel);
            Thread.Sleep(1000);
            DO176X.Instance.ClearRelay(channel);
            DiscreteOutputs.Instance().SetOutput("Block_8", true);
          EventInitializeZZeroTask?.Invoke(this, EventArgs.Empty);
        }
        catch
        {
          EventErrorDetected(this, Globals.GetDeviceErrorString(Globals.Device.Advantech176X,
                                                          Globals.Operation.SetValue));
          return;
        }
      }
      else
      {
                //StopTimeCount();
                lbl_T2_Time.Visible = false;
                WrungBtn_T2(btn_T2_Zero);

                //TimeSpan timeLeft = DateTime.Now - _startTime; //выждали нужное время
                //timeLeft = _currentTs - timeLeft;
                //_measure.StopMeasuring(timeLeft.Seconds <= 0 && _dbKPK2.SaveResults);
                _measure.StopMeasuring(_dbKPK2.SaveResults);
                EventInitializeTask2?.Invoke(this, EventArgs.Empty);
        try
        {
          DiscreteOutputs.Instance().SetOutput("Block_8", false);
        }
        catch { }
        btn_T2_Task.Enabled = btn_T2_Z.Enabled = true;
      }
      btn_T2_Zero.Enabled = true;
    }

    /// Блокируем все кнопки на panel_Task2 
    /// Z обнуление  
    /// <param name="btn">кнопка, которую нажали</param>
    private void PressBtn_T2(Button btn)
    {
      btn.FlatStyle = FlatStyle.Flat;
      btn.BackColor = Color.FromArgb(153, 249, 164);
      _currentTs = btn == btn_T2_Task ? ts5Min : btn == btn_T2_Z ? ts39Sec : TimeSpan.FromSeconds(0);
      //countBlock8 = btn == btn_T2_Task ? 300 :
      //    btn == btn_T2_Z ? 37 : 0;
    }

    private void Task2_EnabledChanged(object sender, EventArgs e)
    {
      if (!Enabled)
        siT2_1.Inactive = siT2_2.Inactive = true;
    }

    private void Task2_Load(object sender, EventArgs e)
    {
      siT2_1.Inactive = siT2_2.Inactive = true;
    }

    internal void Deactivate()
    {
      if (btn_T2_Task.FlatStyle == FlatStyle.Flat)
        btn_T2_Task.PerformClick();
      if (btn_T2_Zero.FlatStyle == FlatStyle.Flat)
        btn_T2_Zero.PerformClick();
      if (btn_T2_Z.FlatStyle == FlatStyle.Flat)
        btn_T2_Z.PerformClick();
    }
  }
}
