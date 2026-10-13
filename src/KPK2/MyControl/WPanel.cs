using System;
using System.Windows.Forms;
using KPK2.Properties;
using UserControlsKPK2;

namespace KPK2.MyControl
{
  public partial class WPanel : UserControl
  {
    private Timer tmrTimePast = new Timer();
    private DateTime _startTime;
    public Action<int> TaskChanged;

    public WPanel(bool isDiss32, bool bortSystems)
    {
      InitializeComponent();
      grpBox_W.Text = Resources.OutputZeroVoltages;
      rbTask1.Visible = rbTask2.Visible = bortSystems;
      lTime.Visible = siTime.Visible = lbl_W.Visible = !bortSystems;
      si1_4.Visible = !isDiss32 || bortSystems;
      si2_1.Visible = si2_2.Visible = si2_3.Visible = isDiss32;
      si1_5.Visible = si1_6.Visible = bortSystems;
      si2_4.Visible = si2_5.Visible = si2_6.Visible = bortSystems;
    }

    private void WPanel_EnabledChanged(object sender, EventArgs e)
    {
      if (!Enabled)
      {
        foreach (Control ctrl in grpBox_W.Controls)
        {
          if (ctrl.Visible)
            if (ctrl is SegmentIndication)
              ((SegmentIndication) ctrl).Inactive = true;
        }
        tmrTimePast.Stop();
        siTime.Inactive = true;
      }
    }

    private void WPanel_Load(object sender, EventArgs e)
    {
      foreach (Control ctrl in grpBox_W.Controls)
      {
        if (ctrl.Visible)
          if (ctrl is SegmentIndication)
            ((SegmentIndication) ctrl).Inactive = true;
      }
      StartTimer();
    }

    private void StartTimer()
    {
      _startTime = DateTime.Now;
      tmrTimePast.Tick += TmrTimePastOnTick;
      tmrTimePast.Interval = 1000;
      tmrTimePast.Start();
    }

    private void TmrTimePastOnTick(object sender, EventArgs eventArgs)
    {
      TimeSpan ts = DateTime.Now - _startTime;
      siTime.ValueString = string.Format("{0:00}:{1:00}:{2:00}", ts.Hours, ts.Minutes, ts.Seconds);
    }

    private void rbTask2_CheckedChanged(object sender, EventArgs e)
    {
      rbTask1.Checked = !rbTask2.Checked;
      
            Action<int> handler = TaskChanged;
      if (handler != null)
        handler(rbTask2.Checked ? 2 : 1);
    }

    private void rbTask1_CheckedChanged(object sender, EventArgs e)
    {
      rbTask2.Checked = !rbTask1.Checked;
    }
  }
}
