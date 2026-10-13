using System;
using System.Drawing;
using System.Windows.Forms;
using Sparc.KPK2.Resources.Properties;

namespace KPK2.MyControl
{
  public partial class ucIndicatorsState : UserControl
  {

    private bool _blockRenewIndicators = false;
    private TimeSpan _blockDelay = TimeSpan.FromSeconds(15);
    private DateTime _reconfigureTime = DateTime.Now;

    public enum Modes
    {
      ModeA,
      ModeB,
      ModeC
    }

    public class IndicatorsState
    {
      public bool DissOn;
      public bool High;
      public bool Flight;
      public bool Capture;
      public bool Capture1;
      public bool Capture2;
      public bool Capture3;
      public bool Serviceability;
      public bool Memory1;
      public bool Memory2;
    }

    private Modes _currentMode;

    public ucIndicatorsState(Modes mode)
    {
      InitializeComponent();
      SetupMode(mode);
    }

    private void SetupMode(Modes mode)
    {
      _reconfigureTime = DateTime.Now;
      _currentMode = mode;
      ResetIndicators();
      gbUSPS.Visible = cbBPKSearch.Visible = cbBPKServiceability.Visible = true;
      cbCapture.Visible = cbCapture3.Visible = cbServiceability.Visible = true;
      cbCapture1.Text = string.Format(Resources.CaptureN, 1);
      cbCapture2.Text = string.Format(Resources.CaptureN, 2);
      switch (mode)
      {
        case Modes.ModeA:
            gbUSPS.Visible = false;
            cbBPKSearch.Visible = false;
          break;
        case Modes.ModeB:
          cbBPKSearch.Visible = false;
          break;
        default:
          cbCapture.Visible = cbCapture3.Visible = cbServiceability.Visible = cbBPKServiceability.Visible = false;
          cbCapture1.Text = string.Format(Resources.MemoryN, 1);
          cbCapture2.Text = string.Format(Resources.MemoryN, 2);
          break;
      }
    }

    public void RenewIndicators(IndicatorsState states)
    {
      if (_blockRenewIndicators)
        return;
      bool result = true;
      result &= RenewCheckBox(cbDissOn, states.DissOn);
      result &= RenewCheckBox(cbHighVoltage, states.High);
      result &= RenewCheckBox(cbFlight, states.Flight);
      result &= RenewCheckBox(cbCapture, states.Capture);
      if (_currentMode == Modes.ModeC)
        result &= RenewCheckBox(cbCapture1, states.Memory1);
      else
        result &= RenewCheckBox(cbCapture1, states.Capture1);
      if (_currentMode == Modes.ModeC)
        result &= RenewCheckBox(cbCapture2, states.Memory2);
      else
        result &= RenewCheckBox(cbCapture2, states.Capture2);
      result &= RenewCheckBox(cbCapture3, states.Capture3);
      result &= RenewCheckBox(cbServiceability, states.Serviceability);
      //if (result && (DateTime.Now - _reconfigureTime) >= _blockDelay)
      //  BlockRenewIndicators(true);
    }

      /// <summary>
      /// Заблокировать (остановить) обновление на индикаторах
      /// </summary>
      /// <param name="state"></param>
    private void BlockRenewIndicators(bool state)
    {
      _blockRenewIndicators = state;
      lHold.Visible = state;
    }

      /// <summary>
      /// Разблокировать (запустить) обновление на индикаторах
      /// </summary>
    public void UnblockRenewIndicators()
    {
      BlockRenewIndicators(false);
      _reconfigureTime = DateTime.Now;
    }

    private bool RenewCheckBox(CheckBox cb, bool state)
    {
      cb.Checked = state;
      bool tempRes = CheckIndicator(cb);
      if (cb.Visible)
        return tempRes;
      return true;
    }

    private void CheckBox_CheckedChanged(object sender, EventArgs e)
    {
      CheckIndicator((CheckBox) sender);
    }

    private bool CheckIndicator(CheckBox cbCurrent)
    {
      cbCurrent.Font = new Font(cbCurrent.Font, cbCurrent.Checked ? FontStyle.Bold : FontStyle.Regular);
      bool checkState = CheckLightState(cbCurrent);
      cbCurrent.ForeColor =  checkState? Color.DarkGreen : Color.Red;
      return checkState;
    }

    public bool GetCurrentState()
    {
      bool bKPK2;
      bool bBPK;
      switch (_currentMode)
      {
        case Modes.ModeA:
          bKPK2 = cbDissOn.Checked && cbHighVoltage.Checked && cbFlight.Checked && cbCapture.Checked &&
                  cbCapture1.Checked && cbCapture2.Checked && cbCapture3.Checked && cbServiceability.Checked;
          bBPK = cbBPKFlight.Checked && cbBPKServiceability.Checked;

          return bKPK2 && bBPK && cbOff.Checked;
        case Modes.ModeB:
          bKPK2 = cbDissOn.Checked && cbHighVoltage.Checked && cbFlight.Checked && cbCapture.Checked &&
                  cbCapture1.Checked && cbCapture2.Checked && cbCapture3.Checked && cbServiceability.Checked;
          bBPK = cbBPKFlight.Checked && cbBPKServiceability.Checked;
          return bKPK2 && bBPK && cbOff.Checked && !cbP.Checked;
        default:
          bKPK2 = cbDissOn.Checked && cbHighVoltage.Checked && cbFlight.Checked && cbCapture1.Checked &&
                  cbCapture2.Checked;
          bBPK = cbBPKFlight.Checked && cbBPKSearch.Checked;
          return bKPK2 && bBPK && cbOff.Checked && cbP.Checked;
      }
    }

    internal void SetMode(Modes mode)
    {
      SetupMode(mode);
    }

    Timer tTime = new Timer();
    private DateTime _dtStart = DateTime.Now;

    public void StartTime()
    {
      siTime.Visible = true;
      tTime = new Timer();
      tTime.Interval = 300;
      tTime.Tick += TTimeOnTick;
      _dtStart = DateTime.Now;
      tTime.Start();
    }

    private void TTimeOnTick(object sender, EventArgs eventArgs)
    {
      TimeSpan curTime = DateTime.Now - _dtStart;
      siTime.ValueString = string.Format("{0:00}:{1:00}:{2:00}", curTime.Hours, curTime.Minutes, curTime.Seconds);
    }

    internal void StopTime()
    {
      tTime.Stop();
      siTime.Visible = false;
    }

    internal void ResetIndicators()
    {
      cbBPKFlight.Checked = cbBPKSearch.Checked = cbBPKServiceability.Checked = cbP.Checked = cbOff.Checked = false;
      cbBPKFlight.ForeColor =
        cbBPKSearch.ForeColor =
        cbBPKServiceability.ForeColor = cbP.ForeColor = cbOff.ForeColor = SystemColors.ControlText;
    }

    private bool CheckLightState(CheckBox cb)
    {
      bool result = false;
      if (AnyOfControl(cb, cbDissOn, cbHighVoltage, cbFlight, cbCapture1, cbCapture2,cbBPKFlight, cbOff, cbServiceability))
        result = cb.Checked;

      if (AnyOfControl(cb, cbCapture, cbCapture3, cbBPKServiceability))
        result = (_currentMode == Modes.ModeA || _currentMode == Modes.ModeB) && cb.Checked;

      if (cb.Name == cbBPKSearch.Name)
        result = _currentMode == Modes.ModeC && cb.Checked;

      if (cb.Name == cbP.Name)
        result = (_currentMode == Modes.ModeC && cb.Checked) || (_currentMode == Modes.ModeB && !cb.Checked);
      return result;
    }

    /// <summary>
    /// По имени проверяет является ли контрол одним из контролов из списка
    /// </summary>
    /// <param name="current"></param>
    /// <param name="controls"></param>
    /// <returns></returns>
    private bool AnyOfControl(Control current, params Control[] controls)
    {
      foreach (Control iControl in controls)
      {
        if (current.Name == iControl.Name)
          return true;
      }
      return false;
    }
  }
}
