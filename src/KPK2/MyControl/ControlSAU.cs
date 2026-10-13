using System;
using System.Collections.Generic;
using System.Threading;
using System.Windows.Forms;
using KPK2.Common;
using KPK2.Common.DissFunctions;
using Sparc.KPK2.Hardware;
using Res = Sparc.KPK2.Resources.Properties.Resources;


namespace KPK2.MyControl
{
  public partial class ControlSAU : UserControl
  {
    private Thread thCheckSignal;
    private System.Windows.Forms.Timer tChangeTask;
    


    public enum Task
    {
      Task5,
      Task6,
      Task1,
      Task3,
      Task3_2,
      Task2
    }

    public delegate bool ReturnBool();

    public delegate void SaveParameterDelegate(string paramName, bool value);

    public event Action StopMeasuring;
    public event Action<Task> TaskActivated;
    public event Action<Task> TaskDeactivated;
    public event ReturnBool CheckWorkabilityEvent;
    public event SaveParameterDelegate SaveParameterEvent;

    public delegate bool CommandMessageDelegate(string message, string caption);
    public event CommandMessageDelegate CommandMessage;

    List<Button> taskButtonList = new List<Button>();

    public ControlSAU()
    {
      InitializeComponent();
      taskButtonList.Add(bKZ6);
      taskButtonList.Add(bKZ1);
      taskButtonList.Add(bKZ2);
      taskButtonList.Add(bKZ3);
      
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
        if (bCurrent == bKZ5)
          bCurrent = bKZ6;
      }
      TaskFunc(GetTaskByButton(bCurrent.Name) ,pressB);
      bCurrent.Enabled = true;
    }

    private void DisableControls(string bName)
    {
      if (bName == bKZ1.Name)
        siKZ1Wxr.Inactive = siKZ1Wyr.Inactive = true;
      else if (bName == bKZ2.Name)
        siKZ2Wzr.Inactive = true;
      else if (bName == bKZ3.Name)
        siKZ3Wxr.Inactive = siKZ3Wyr.Inactive = siKZ3Wzr.Inactive = true;
      else if (bName == bKZ6.Name || bName == bKZ5.Name)
        siKZ6A.Inactive = true;
    }

    private Task GetTaskByButton(string bName)
    {
      if (bName == bKZ6.Name)
        return Task.Task6;
      if (bName == bKZ5.Name)
        return Task.Task5;
      if (bName == bKZ1.Name)
        return Task.Task1;
      if (bName == bKZ2.Name)
        return Task.Task2;
      return Task.Task3;
    }

    private void DeactivateTaskByButton(string bName)
    {
      if ((bName == bKZ5.Name) || ((bName == bKZ6.Name) && (bKZ5.FlatStyle == FlatStyle.Flat)))
      {
        Diss15Functions.SetCourseEx(false);
        GenSinCos.Instance().GeneratorOff();
        if (bName == bKZ6.Name)
          PressTaskButton(bKZ5);
        if (bName == bKZ5.Name)
          bKZ_Click(bKZ6, new EventArgs());
      }
      if ((bName == bKZ2.Name) && (bCheckSignal.FlatStyle == FlatStyle.Flat))
        PressTaskButton(bCheckSignal);
      Diss15Functions.TaskOff();
    }

    private void ActivateTaskByButton(string bName)
    {
      if (bName == bKZ6.Name)
      {
        if (!Diss15Functions.SetTask6())
          MessageBox.Show(Res.SetTaskError);
        else
          MessageBox.Show(string.Format(Res.WaitWhileTaskNCompleted, 6));
      }
      else if (bName == bKZ5.Name)
      {
        try
        {
          TaskFunc(Task.Task6, false);
          Diss15Functions.SetCourseEx(true);
          GenSinCos.Instance().GeneratorOnEx(0);
          if (!Diss15Functions.SetTask5())
            throw new Exception();
          else
            MessageBox.Show(string.Format(Res.WaitWhileTaskNCompleted, 5));
        }
        catch
        {
          MessageBox.Show(Res.SetTaskError);
        }
      }
      else if (bName == bKZ1.Name)
      {
        if (!Diss15Functions.SetTask1())
          MessageBox.Show(Res.SetTaskError);
        else
          MessageBox.Show(string.Format(Res.WaitWhileTaskNCompleted, 1));
      }
      else if (bName == bKZ2.Name)
      {
        if (!Diss15Functions.SetTask2())
          MessageBox.Show(Res.SetTaskError);
        else
          MessageBox.Show(string.Format(Res.WaitWhileTaskNCompleted, 2));
      }
      else if (bName == bKZ3.Name)
      {
        if (!Diss15Functions.SetTask3() || !Diss15Functions.SetSAU_Wxr())
          MessageBox.Show(Res.SetTaskError);
        else
        {
          MessageBox.Show(string.Format(Res.WaitWhileTaskNCompleted, 3) + Environment.NewLine + String.Format(Res.WaitWhileBottonIndicatorTurnOnAfterNSec, _changeTaskInterval.TotalSeconds));
          StartChangeTaskTimer();
        }
      }
    }

    private TimeSpan _changeTaskInterval = TimeSpan.FromSeconds(30);

    private void StartChangeTaskTimer()
    {
      tChangeTask = new System.Windows.Forms.Timer();
      tChangeTask.Interval = (int)_changeTaskInterval.TotalMilliseconds;
      tChangeTask.Tick += TChangeTaskOnTick;
      tChangeTask.Start();
    }

    private void TChangeTaskOnTick(object sender, EventArgs eventArgs)
    {
      tChangeTask.Stop();
      OnStopMeasuring();
      //TaskFunc(Task.Task3, false);
      siKZ3Wxr.Inactive = true;
      siKZ3Wyr.Inactive = true;
      TaskFunc(Task.Task3_2, true);
    }

    private delegate bool PressTaskButtonDelegate(Button bCurrent);

    private bool PressTaskButton(Button bCurrent)
    {
      if (InvokeRequired)
        return (bool)Invoke(new PressTaskButtonDelegate(PressTaskButton), bCurrent);
      
      bool result = bCurrent.FlatStyle == FlatStyle.Standard;
      if (bCurrent != bKZ5)
      {
        if (bCurrent != bCheckSignal)
          EnableControls(!result, bCurrent);
        if (bCurrent == bKZ6)
          bKZ5.Enabled = result;
      }
      bCurrent.FlatStyle = result ? FlatStyle.Flat : FlatStyle.Standard;
      bCurrent.Text = GetButtonText(bCurrent.Name, result);
      return result;
    }

    private string GetButtonText(string bName, bool state)
    {
      if (bName == bCheckSignal.Name)
        return state ? Res.StopCheck : Res.CheckWorkabilitySignal;
      int taskNumber = Convert.ToInt32(bName.Substring(bName.Length-1, 1));
      return string.Format(state ? Res.TaskNOff : Res.TaskNOn, string.Format(Res.KZN, taskNumber));
    }

    private void EnableControls(bool state, Button except)
    {
      foreach (Button b in taskButtonList)
        if (b.Name != except.Name)
        {
          b.Enabled = state;
        }
      if (except == bKZ2)
        bCheckSignal.Enabled = !state;
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

    private void bCheckSignal_Click(object sender, EventArgs e)
    {
      bCheckSignal.Enabled = false;
      bKZ2.Enabled = false;
      bool pressB = PressTaskButton(bCheckSignal);
      if (pressB)
      {
        ClearCheckState();
        SetVisibleCheckState(true);
        //TaskFunc(Task.Task2, false);
        thCheckSignal = new Thread(CheckSignalThread);
        thCheckSignal.Start();
      }
      else
      {
        //Diss15Functions.SetEquivalent(true);
        thCheckSignal.Abort();
        //Diss15Functions.TaskOff();
        //SetVisibleCheckState(false);
        EndCheckSignal();
        MessageBox.Show(Res.CheckInterruptedByUser);
      }
      bCheckSignal.Enabled = true;
    }

    private void ClearCheckState()
    {
      for (int i=0;i<clbCheckState.Items.Count;i++)
        clbCheckState.SetItemChecked(i, false);
    }

    private void CheckSignalThread(object state)
    {
      Globals.SetThreadCulture(Thread.CurrentThread);
      ShowCheckSignalProgress(true);
      GenSinCos.Instance().GeneratorOn(40);
      Diss15Functions.SetEquivalent(false);
      Thread.Sleep(7000);
      bool workability = CheckWorkabilitySignalFunc();
      if (!workability)
      {
        MessageBox.Show(Res.WorkabilityIsAbsent);
        EndCheckSignal();
        return;
      }

      try
      {
        bool result = true;
        result &= CheckWorkability("Heel",  Res.CheckHeel, true);
        result &= CheckWorkability("Pitch", Res.CheckPitch, true);
        result &= CheckWorkability(CheckWorkabilityManual.RKSwitch);
        result &= CheckWorkability("Equivalent", Res.CheckEquivalent);
        result &= CheckWorkability(ContrTaskChannel, Res.CheckRemoveCT);
        result &= CheckWorkability("High_15", Res.CheckHVOff);
        result &= CheckWorkability(CheckWorkabilityManual.Zero);
        result &= CheckWorkability(CheckWorkabilityManual.PK_KZ1);
        result &= CheckWorkability(CheckWorkabilityManual.PK_KZ2);
        result &= CheckWorkability(CheckWorkabilityManual.PK_KZ3);
        //result &= CheckWorkability("UNCh_32");
        SaveParameterFunc("SAU_Workability", result);

      }
      catch (ThreadAbortException)
      {
      }
      catch (Exception)
      {
        ShowMessage(Res.ReleError);
      }
     EndCheckSignal();
    }

    private void EndCheckSignal()
    {
      ShowCheckSignalProgress(false);
      PressTaskButton(bCheckSignal);
      Diss15Functions.SetEquivalent(true);
      SetVisibleCheckState(false);
      SetControlEnabled(bKZ2);
    }

    private void SetControlEnabled(Button bKZ2)
    {
      if (InvokeRequired)
      {
        Invoke(new Action<Button>(SetControlEnabled), bKZ2);
        return;
      }
      bKZ2.Enabled = true;
    }

    private void SetVisibleCheckState(bool state)
    {
      if (InvokeRequired)
      {
        Invoke(new Action<bool>(SetVisibleCheckState), state);
        return;
      }
      clbCheckState.Visible = state;
    }

    private void ShowMessage(string message)
    {
      if (InvokeRequired)
      {
        Invoke(new Action<string>(ShowMessage), message);
        return;
      }
      MessageBox.Show(message);
    }

    private void ShowCheckSignalProgress(bool visible)
    {
      if (InvokeRequired)
      {
        Invoke(new Action<bool>(ShowCheckSignalProgress), visible);
        return;
      }
      pbCheckSignal.Value = 0;
      pbCheckSignal.Visible = visible;
    }

    private void StepCheckSignalProgress()
    {
      if (InvokeRequired)
      {
        Invoke(new MethodInvoker(StepCheckSignalProgress));
        return;
      }
      pbCheckSignal.Value++;
    }

    private enum CheckWorkabilityManual
    {
        RKSwitch,
        Zero,
        PK_KZ1,
        PK_KZ2,
        PK_KZ3
    }

    TimeSpan _humanization = TimeSpan.FromMilliseconds(400);
    private const string ContrTaskChannel = "Task_15";

    bool CheckWorkability(string dSignal,  string caption, bool invert = false)
    {
        if (dSignal == ContrTaskChannel)
            Diss15Functions.TaskOff();
        else
            DiscreteOutputs.Instance().SetOutputEx(dSignal, !invert);
      ShowCommandMessageFunc(Res.WaitWhileTaskCompletedOrGoodOff, caption);
      bool workability = CheckWorkabilitySignalFunc();
      if (dSignal == ContrTaskChannel)
          Diss15Functions.SetTask2();
      else
        DiscreteOutputs.Instance().SetOutputEx(dSignal, invert);
      Thread.Sleep(_humanization);
      ShowCommandMessageFunc(Res.WaitWhileTaskCompletedOrGoodOn, caption);
      bool checkResult = !workability && CheckWorkabilitySignalFunc();
      SetCheckResult(checkResult);
      StepCheckSignalProgress();
      Thread.Sleep(_humanization);
      return checkResult;
    }

    private void SetCheckResult(bool checkResult)
    {
      if (InvokeRequired)
      {
        Invoke(new Action<bool>(SetCheckResult), checkResult);
        return;
      }
      clbCheckState.SetItemChecked(pbCheckSignal.Value, checkResult);
    }

    bool CheckWorkability(CheckWorkabilityManual step)
    {
      ShowCommandMessageFunc(GetCommandMessage(step, true));
      bool workability = CheckWorkabilitySignalFunc();
      Thread.Sleep(_humanization);
      ShowCommandMessageFunc(GetCommandMessage(step, false));
      bool checkResult = !workability && CheckWorkabilitySignalFunc();
      SetCheckResult(checkResult);
      StepCheckSignalProgress();
      Thread.Sleep(_humanization);
      return checkResult;
    }

    private void ShowCommandMessageFunc(string message, string caption = "")
    {
      CommandMessageDelegate action = CommandMessage;
      if (action != null)
        action(message, caption);
    }

    private string GetCommandMessage(CheckWorkabilityManual step, bool activate)
    {
      switch (step)
      {
        case CheckWorkabilityManual.RKSwitch:
          return string.Format( activate ? Res.SetRKToK : Res.SetRKToR, "OK");
        case CheckWorkabilityManual.PK_KZ1:
          return string.Format(Res.SetPKToNN, activate ? 1 : 5, activate ? Res.Task1Full : Res.WorkPosition) + (activate ? Environment.NewLine + string.Format(Res.WaitWhileGoodOff) : Environment.NewLine + string.Format(Res.WaitWhileBlock6And7DoneAndGoodOn));
        case CheckWorkabilityManual.PK_KZ2:
          return string.Format(Res.SetPKToNN, activate ? 2 : 5, activate ? Res.Task2Full : Res.WorkPosition) + (activate ? Environment.NewLine + string.Format(Res.WaitWhileGoodOff) : Environment.NewLine + string.Format(Res.WaitWhileBlock6And7DoneAndGoodOn));
        case CheckWorkabilityManual.PK_KZ3:
          return string.Format(Res.SetPKToNN, activate ? 3 : 5, activate ? Res.Task3Full : Res.WorkPosition) + (activate ? Environment.NewLine + string.Format(Res.WaitWhileGoodOff) : Environment.NewLine + string.Format(Res.WaitWhileBlock6And7DoneAndGoodOn));
        case CheckWorkabilityManual.Zero:
          return string.Format(Res.ZeroModeMessage, activate ? Res.ZeroOn + Environment.NewLine + Res.WaitWhileBlock6And7DoneAndGoodOff : Res.ZeroOff + Environment.NewLine + Res.WaitWhileBlock6And7DoneAndGoodOn);
      }
      return String.Empty;
    }


    private void SaveParameterFunc(string paramName, bool value)
    {
      try
      {
        if (SaveParameterEvent != null)
          SaveParameterEvent(paramName, value);
      }
      catch
      {
        
      }
    }

    private bool CheckWorkabilitySignalFunc()
    {
      try
      {
        if (CheckWorkabilityEvent != null)
          return CheckWorkabilityEvent();
      }
      catch
      {
        return false;
      }
      return false;
    }

    private void ControlSAU_Load(object sender, EventArgs e)
    {
      siKZ1Wyr.Inactive = siKZ1Wxr.Inactive = siKZ2Wzr.Inactive = siKZ2Wzr.Inactive = siKZ3Wxr.Inactive = siKZ3Wyr.Inactive = siKZ3Wzr.Inactive = siKZ6A.Inactive = true;
    }

    internal void StopCurrentTask()
    {
      foreach (Button b in taskButtonList)
        if (b.FlatStyle == FlatStyle.Flat)
        {
          b.PerformClick();
          return;
        }
    }

      protected virtual void OnStopMeasuring()
      {
          var handler = StopMeasuring;
          if (handler != null)
              handler();
      }
  }
}
