using System;
using System.Collections.Generic;
using System.Windows.Forms;
using KPK2.Common.DissFunctions;
using Res = Sparc.KPK2.Resources.Properties.Resources;

namespace KPK2.MyControl
{
  public partial class ControlSU : UserControl
  {

    public enum Task
    {
      Task1,
      Task2,
      Task3,
      Task4,
      Task5,
      Task1_1
    }

    public event Action<Task> TaskActivated;
    public event Action<Task> TaskDeactivated;

    private List<Button> taskButtonList = new List<Button>(); 

    public ControlSU()
    {
      InitializeComponent();
      taskButtonList.Add(bKZ1);
      taskButtonList.Add(bKZ2);
      taskButtonList.Add(bKZ6);
      taskButtonList.Add(bKZ4);
      taskButtonList.Add(bKZ5);
    }

    private void ControlSU_Load(object sender, EventArgs e)
    {
      siKZ1Wxr.Inactive = siKZ2Wzr.Inactive = siKZ1_1Wyr.Inactive = siKZ4Wyr.Inactive = siKZ5Wxr.Inactive = siKZ5Wzr.Inactive = siKZ1Wyr.Inactive = true;
    }

    private void bKZ_Click(object sender, EventArgs e)
    {
      Button bCurrent = (Button)sender;
      bCurrent.Enabled = false;
      int taskNumber = GetTaskNumberByButtonName(bCurrent.Name);
      bool pressB = PressTaskButton(bCurrent);
      if (pressB)
        ActivateTaskByButton(taskNumber);
      else
      {
        DeactivateTask();
        DisableControls(bCurrent.Name);
      }
      TaskFunc((Task)(taskNumber-1), pressB);
      bCurrent.Enabled = true;
    }

    private void DisableControls(string bName)
    {
      if (bName == bKZ1.Name)
        siKZ1Wyr.Inactive = siKZ1Wxr.Inactive = true;
      else if (bName == bKZ2.Name)
        siKZ2Wzr.Inactive = true;
      else if (bName == bKZ6.Name)
        siKZ1_1Wyr.Inactive = true;
      else if (bName == bKZ4.Name)
        siKZ4Wyr.Inactive = true;
      else
        siKZ5Wxr.Inactive = siKZ5Wzr.Inactive = true;
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

    private void DeactivateTask()
    {
      Diss15Functions.TaskOff();
    }

    private void ActivateTaskByButton(int taskNumber)
    {
      switch (taskNumber)
      {
        case 6:
        case 1:
          Diss15Functions.SetTask1();
          break;
        case 2:
          Diss15Functions.SetTask2();
          break;
        case 3:
          Diss15Functions.SetTask3();
          break;
        case 4:
          Diss15Functions.SetTask4();
          break;
        default:
          Diss15Functions.SetTask5();
          break;
      }
    }

    private bool PressTaskButton(Button bCurrent)
    {
      bool result = bCurrent.FlatStyle == FlatStyle.Standard;
      EnableControls(!result, bCurrent);
      bCurrent.FlatStyle = result ? FlatStyle.Flat : FlatStyle.Standard;
      bCurrent.Text = GetButtonText(bCurrent.Name, result);
      return result;
    }

    private void EnableControls(bool state, Button except)
    {
      foreach (Button b in taskButtonList)
        if (b.Name != except.Name)
          b.Enabled = state;
    }

    private string GetButtonText(string bName, bool state)
    {
      int taskNumber = GetTaskNumberByButtonName(bName);
      if (bName == bKZ6.Name)
        taskNumber = 1;
      return string.Format(state ? Res.TaskNOff : Res.TaskNOn, string.Format(Res.KZN, taskNumber));
    }

    private int GetTaskNumberByButtonName(string bName)
    {
      return Convert.ToInt32(bName.Substring(bName.Length - 1, 1));
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

    private void label4_Click(object sender, EventArgs e)
    {

    }

    private void siKZ4Wyr_Load(object sender, EventArgs e)
    {

    }
  }
}
