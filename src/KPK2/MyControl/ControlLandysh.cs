using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using KPK2.Common.DissFunctions;
using Sparc.KPK2.Hardware;
using UserControlsKPK2;
using Res = Sparc.KPK2.Resources.Properties.Resources;

namespace KPK2.MyControl
{
  public partial class ControlLandysh : UserControl
  {
    private Timer tSpy;
    private DateTime _startTime;
    private TimeSpan _stator1;
    private TimeSpan _stator2;

    public delegate void SaveParameterDelegate(string pName, double value);
    

    public event Action<Task> TaskActivated;
    public event Action<Task> TaskDeactivated;
    public event SaveParameterDelegate SaveParameter;

    List<Control> taskControlList = new List<Control>();
    private StatorState _curStatorState;

    private delegate double ReturnDoubleDelegate();

    public double StatorVoltage
    {
      get
      {
        if (InvokeRequired)
          return (double)Invoke(new ReturnDoubleDelegate(() => StatorVoltage));
        SegmentIndication siCurrent = GetIndicatorByStatorStep(_curStatorState);
        return siCurrent.Value;
      }

      set
      {
        if (InvokeRequired)
        {
          Invoke(new Action<double>(d => StatorVoltage = d));
          return;
        }
        SegmentIndication siCurrent = GetIndicatorByStatorStep(_curStatorState);
        siCurrent.Value = value;
      }
    }

    private SegmentIndication GetIndicatorByStatorStep(StatorState _curStatorState)
    {
      SegmentIndication siCurrent;
      switch (_curStatorState)
      {
        case StatorState.StatorX1:
          siCurrent = siStatorX1;
          break;
        case StatorState.StatorX2:
          siCurrent = siStatorX2;
          break;
        case StatorState.StatorZ1:
          siCurrent = siStatorZ1;
          break;
        default:
          siCurrent = siStatorZ2;
          break;
      }
      return siCurrent;
    }

    public enum Task
    {
      Task2,
      Task4,
      Task5,
      Task6,
      Task4_ExceptB10
    }

    public enum StatorState
    {
      StatorX1,
      StatorX2,
      StatorZ1,
      StatorZ2
    }

    public ControlLandysh()
    {
      InitializeComponent();
      taskControlList.Add(bKZ2);
      taskControlList.Add(bKZ4);
      taskControlList.Add(bKZ5);
      taskControlList.Add(bKZ6);
      taskControlList.Add(gbExceptBlock10);
      taskControlList.Add(cbBlock10Exist);
    }

    private void bKZ_Click(object sender, EventArgs e)
    {
      Button bCurrent = (Button)sender;
      bCurrent.Enabled = false;
      bool pressB = PressTaskButton(bCurrent);
      if (pressB)
        ActivateTaskByButton(bCurrent.Name);
      else
      {
        DeactivateTaskByButton(bCurrent.Name);
        DisableControls(bCurrent.Name);
      }
      if (bCurrent == bKZ4 && pressB && !cbBlock10Exist.Checked)
      {
        MessageBox.Show(Res.WaitTask);
        const int angle = 315;
        if (!GenSinCos.Instance().GeneratorOn(angle) || !Diss15Functions.Block8On())
          MessageBox.Show(Res.Error);
        SetStatorX1();
        StartSpyTimer();
      }
      TaskFunc(GetTaskByButton(bCurrent.Name), pressB);
      bCurrent.Enabled = true;
    }

    private void DisableControls(string bName)
    {
      if (bName == bKZ4.Name)
        siStatorX1.Inactive =
          siStatorX2.Inactive = siStatorZ1.Inactive = siStatorZ2.Inactive = siTimeX.Inactive = siTimeZ.Inactive = siKZ4Wxr.Inactive = siKZ4Wzr.Inactive = true;
      else if (bName == bKZ5.Name)
        siKZ5Wzr.Inactive = true;
      else if (bName == bKZ6.Name)
        siKZ6Wxr.Inactive = siKZ6Wzr.Inactive = true;
      else if (bName == bKZ2.Name)
        siKZ2Wzr.Inactive = siKZ2Wxr.Inactive = true;
    }

    private void SetStatorX1()
    {
      if (!Diss15Functions.SetStatorX1())
        MessageBox.Show(Res.SetTaskError);
      _curStatorState = StatorState.StatorX1;
    }

    private void SetStatorX2()
    {
      if (!Diss15Functions.SetStatorX2())
        MessageBox.Show(Res.SetTaskError);
      _curStatorState = StatorState.StatorX2;
      siStatorX1.Inactive = true;
    }

    private void SetStatorZ1()
    {
      if (!Diss15Functions.SetStatorZ1())
        MessageBox.Show(Res.SetTaskError);
      _curStatorState = StatorState.StatorZ1;
      siStatorX2.Inactive = siTimeX.Inactive = true;
    }

    private void SetStatorZ2()
    {
      if (!Diss15Functions.SetStatorZ2())
        MessageBox.Show(Res.SetTaskError);
      _curStatorState = StatorState.StatorZ2;
      siStatorZ1.Inactive = true;
    }

    private void StartSpyTimer()
    {
      tSpy = new Timer();
      tSpy.Interval = 100;
      tSpy.Tick += TSpyOnTick;
      tSpy.Start();
    }

    public const string LandyshStatorXParam = "Landysh_StatorX";
    public const string LandyshStatorZParam = "Landysh_StatorZ";

    private void TSpyOnTick(object sender, EventArgs eventArgs)
    {
      tSpy.Stop();
      bool statorZeroOrLess = StatorVoltage <= 0;
      switch (_curStatorState)
      {
        case StatorState.StatorX1:
          SetIndicatorValue(siStatorX1, statorZeroOrLess? 0 : StatorVoltage);
          if (!statorZeroOrLess)
            return;
          _startTime = DateTime.Now;
          SetStatorX2();
          break;
        case StatorState.StatorX2:
          SetIndicatorValue(siStatorX2, statorZeroOrLess ? 0 : StatorVoltage);
          _stator1 = DateTime.Now - _startTime;
          SetTime(siTimeX, _stator1);
          if (statorZeroOrLess)
          {
            SetStatorZ1();
            SaveParameterFunc(LandyshStatorXParam, _stator1.TotalSeconds);
          }
          break;
        case StatorState.StatorZ1:
          SetIndicatorValue(siStatorZ1, statorZeroOrLess ? 0 : StatorVoltage);
          if (!statorZeroOrLess)
            return;
          _startTime = DateTime.Now;
          SetStatorZ2();
          break;
        case StatorState.StatorZ2:
          SetIndicatorValue(siStatorZ2, statorZeroOrLess ? 0 : StatorVoltage);
          _stator2 = DateTime.Now - _startTime;
          SetTime(siTimeZ, _stator2);
          if (statorZeroOrLess)
          {
            StopSpyTimer();
            siStatorZ2.Inactive = siTimeZ.Inactive = true;
            SaveParameterFunc(LandyshStatorZParam, _stator1.TotalSeconds);
            StopKZ4_2Step();
            return;
          }
          break;
      }
      tSpy.Start();
    }

    private void SetIndicatorValue(SegmentIndication indicator, double voltage)
    {
      if (InvokeRequired)
      {
        Invoke(new Action<SegmentIndication, double>(SetIndicatorValue), indicator, voltage);
        return;
      }
     // indicator.Inactive = false;
      indicator.Value = voltage;
    }

    private void StopKZ4_2Step()
    {
      if (InvokeRequired)
      {
        Invoke(new MethodInvoker(StopKZ4_2Step));
        return;
      }
      bKZ4.PerformClick();
    }

    private void SaveParameterFunc(string pName, double value)
    {
      try
      {
        if (SaveParameter != null)
          SaveParameter(pName, value);
      }
      catch {}
    }

    private void StopSpyTimer()
    {
      if (tSpy.Enabled)
        tSpy.Stop();
    }

    private delegate void SetTimeDelegate(SegmentIndication siControl, TimeSpan time);

    private void SetTime(SegmentIndication siControl, TimeSpan time)
    {
      if (InvokeRequired)
      {
        Invoke(new SetTimeDelegate(SetTime), siControl, time);
        return;
      }
      siControl.Value = time.TotalSeconds;
    }

    private Task GetTaskByButton(string bName)
    {
      if (bName == bKZ2.Name)
        return Task.Task2;
      if (bName == bKZ4.Name)
      {
        if(cbBlock10Exist.Checked)
          return Task.Task4;
        return Task.Task4_ExceptB10;
      }

      if (bName == bKZ5.Name)
        return Task.Task5;
      if (bName == bKZ6.Name)
        return Task.Task6;
      return Task.Task4_ExceptB10;
    }

    private void DeactivateTaskByButton(string bName)
    {
      if (bName == bKZ4.Name && !cbBlock10Exist.Checked)
      {
        StopSpyTimer();
        Diss15Functions.Block8Off();
        GenSinCos.Instance().GeneratorOff();
      }
      Diss15Functions.TaskOff();
    }

    private void ActivateTaskByButton(string bName)
    {
      if (bName == bKZ2.Name)
      {
        if (!Diss15Functions.SetTask2())
          MessageBox.Show(Res.SetTaskError);
      }
      else if ((bName == bKZ4.Name))
      {
        if (!Diss15Functions.SetTask4())
          MessageBox.Show(Res.SetTaskError);
      }
      else if (bName == bKZ5.Name)
      {
        if (!Diss15Functions.SetTask5())
          MessageBox.Show(Res.SetTaskError);
      }
      else if (bName == bKZ6.Name)
      {
        if (!Diss15Functions.SetTask6())
          MessageBox.Show(Res.SetTaskError);
      }
    }

    private bool PressTaskButton(Button bCurrent)
    {
      bool result = bCurrent.FlatStyle == FlatStyle.Standard;
      if (bCurrent == bKZ4 && !cbBlock10Exist.Checked)
        EnableControls(!result, gbExceptBlock10.Name);
      else
        EnableControls(!result, bCurrent.Name);
      bCurrent.FlatStyle = result ? FlatStyle.Flat : FlatStyle.Standard;
      bCurrent.Text = GetButtonText(bCurrent.Name, result);
      return result;
    }

    private void EnableControls(bool state, string bName)
    {
      foreach (Control b in taskControlList)
        if (b.Name != bName)
          b.Enabled = state;
    }

    private string GetButtonText(string bName, bool state)
    {
      int taskNumber;
      if (bName == bKZ4.Name && !cbBlock10Exist.Checked)
        taskNumber = 4;
      else
        taskNumber = Convert.ToInt32(bName.Substring(bName.Length - 1, 1));
      return string.Format(state ? Res.TaskNOff : Res.TaskNOn, string.Format(Res.KZN, taskNumber));
    }

    private void TaskFunc(Task inTask, bool activate)
    {
      try
      {
        if (activate)
        {
          if (TaskActivated != null)
            TaskActivated(inTask);
        }
        else if (TaskDeactivated != null)
          TaskDeactivated(inTask);
      }
      catch { }
    }

    private void ControlLandysh_Load(object sender, EventArgs e)
    {
      DisableSIControls(Controls);
      RenewBlock10State();
    }

    private void RenewBlock10State()
    {
      gbExceptBlock10.Visible = !cbBlock10Exist.Checked;
      gbWithBlock10.Visible = cbBlock10Exist.Checked;
      gbWithBlock10.Location = new Point(gbExceptBlock10.Location.X+gbExceptBlock10.Width/2 - gbWithBlock10.Width/2, gbExceptBlock10.Location.Y);
    }

    private void DisableSIControls(ControlCollection ctrls)
    {
      foreach (Control c in ctrls)
      {
        SegmentIndication si = c as SegmentIndication;
        if (si != null)
          si.Inactive = true;
        if (c.HasChildren)
          DisableSIControls(c.Controls);
      }
    }

    public void StopCurrentTask()
    {
      foreach (Control c in taskControlList)
      {
        Button btn = c as Button;
        if (btn != null)
          if (btn.FlatStyle == FlatStyle.Flat)
            btn.PerformClick();
        if (c.Name == gbExceptBlock10.Name)
          if (c.Enabled)
            if (bKZ4.FlatStyle == FlatStyle.Flat)
              bKZ4.PerformClick();
      }
    }

    private void cbBlock10Exist_CheckedChanged(object sender, EventArgs e)
    {
      RenewBlock10State();
    }
  }
}
