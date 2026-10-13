using System;
using System.Collections.Generic;
using System.Windows.Forms;
using KPK2.Common.DissFunctions;
using Res = Sparc.KPK2.Resources.Properties.Resources;

namespace KPK2.MyControl
{
  public partial class ControlAist : UserControl
  {
    List<Button> buttonList = new List<Button>();

    public bool? StateAMove
    {
      get
      {
        if (cbAMove.SelectedIndex == -1)
          return null;
        return cbAMove.SelectedIndex == 0;
      }
    }

    public bool? StateAClose
    {
      get
      {
        if (cbAClose.SelectedIndex == -1)
          return null;
        return cbAClose.SelectedIndex == 1;
      }
    }

    public enum Task
    {
      Task4,
      Task5,
      Task6,
      SKTZero
    }

    public event Action<Task> TaskActivated;
    public event Action<Task> TaskDeactivated;

    public ControlAist()
    {
      InitializeComponent();
      buttonList.Add(bKZ4);
      buttonList.Add(bKZ5);
      buttonList.Add(bKZ6);
      buttonList.Add(bSKTZero);
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
      catch {}
    }

    private void bKZ_Click(object sender, EventArgs e)
    {
      Button bCurrent = (Button) sender;
      bool pressB = PressButton(bCurrent);
      if (pressB)
      {
        if (bCurrent.Name == bSKTZero.Name)
          MessageBox.Show(Res.AistZeroMessage);
        SetTask(bCurrent.Name);
      }
      
      TaskFunc(GetTaskByButton(bCurrent.Name), pressB);
      if (bCurrent.Name == bSKTZero.Name)
        cbAClose.Enabled = cbAMove.Enabled = pressB;
      if (!pressB)
      {
        SetInactiveIndicators(bCurrent.Name);
        Diss15Functions.TaskOff();
      }
    }

    //private void DisableControls(string bName)
    //{
    //  if (bName == bKZ4.Name)
    //    siKZ4A.Inactive = siKZ4W.Inactive = true;
    //  else if (bName == bKZ5.Name)
    //    siKZ5A.Inactive = true;
    //  else if (bName == bKZ6.Name)
    //    siKZ6A.Inactive = siKZ6W.Inactive = true;
    //}

    private void SetTask(string bName)
    {
      if (bName == bKZ4.Name)
        Diss15Functions.SetTask4();
      else if (bName == bKZ5.Name)
        Diss15Functions.SetTask5();
      else if (bName == bKZ6.Name)
        Diss15Functions.SetTask6();
      else if (bName == bSKTZero.Name)
        Diss15Functions.SetTask1();
    }

    private Task GetTaskByButton(string bName)
    {
      if (bName == bKZ4.Name)
        return Task.Task4;
      if (bName == bKZ5.Name)
        return Task.Task5;
      if (bName == bKZ6.Name)
        return Task.Task6;
      return Task.SKTZero;
    }

    private void SetInactiveIndicators(string bName)
    {
      if (bName == bKZ4.Name)
        siKZ4A.Inactive = siKZ4W.Inactive = true;
      else if (bName == bKZ5.Name)
        siKZ5A.Inactive = true;
      else if (bName == bKZ6.Name)
        siKZ6A.Inactive = siKZ6W.Inactive = true;
      else
        siZero.Inactive = true;
    }

    private bool PressButton(Button bCurrent)
    {
      bool result = bCurrent.FlatStyle == FlatStyle.Standard;
      EnableControls(!result, bCurrent);
      bCurrent.FlatStyle = result ? FlatStyle.Flat : FlatStyle.Standard;
      if (bCurrent.Name != bSKTZero.Name)
        bCurrent.Text = GetButtonText(bCurrent.Name, result);
      return result;
    }

    private string GetButtonText(string bName, bool state)
    {
      int taskNumber = 0;
      if (bName == bKZ4.Name)
        taskNumber = 4;
      else if (bName == bKZ5.Name)
        taskNumber = 5;
      else if (bName == bKZ6.Name)
        taskNumber = 6;
      return string.Format(state ? Res.TaskNOff : Res.TaskNOn, string.Format(Res.KZN, taskNumber));
    }

    private void EnableControls(bool state, Button except)
    {
      foreach (Button b in buttonList)
        if (b.Name != except.Name)
          b.Enabled = state;
    }

    public void StopCurrentTask()
    {
      foreach (Button b in buttonList)
        if (b.FlatStyle == FlatStyle.Flat)
        {
          b.PerformClick();
          return;
        }
    }

    private void ProductAist_Load(object sender, EventArgs e)
    {
      siKZ4A.Inactive = siKZ4W.Inactive = siKZ5A.Inactive = siKZ6A.Inactive = siKZ6W.Inactive = siZero.Inactive = true;
    }
  }
}
