using USBVoltmeter;

namespace KPK2.Common.MeasureVoltage
{
  public class Voltmeter : IVoltmeter
  {
    private bool _isReady;
    private bool _isUSBVoltmeter;

    private bool IsUSBVoltmeter
    {
      set
      {
        _isUSBVoltmeter = value;
        if (_isUSBVoltmeter)
          _isReady = true;
      }
    }

    public bool Init()
    {
      if (_isReady)
        return true;
      V7_VoltmeterManagement.SearchVoltmetersOnce();
      if (V7_VoltmeterManagement.VoltmetersCount > 0)
      {
        _isUSBVoltmeter = true;
        _isReady = true;
      }

      return _isReady;
    }

    public bool IsReady()
    {
      return _isReady;
    }

    public void SetMeasureVoltageDC()
    {
      if (!_isReady)
        return;
      if (_isUSBVoltmeter)
          V7_VoltmeterManagement.GetVoltmeter(0).SetMeasureVoltageDC();
    }

    public bool CheckMeasureVoltageDC()
    {
      if (!_isReady)
        return false;
      if (_isUSBVoltmeter)
        return true;
      return false;
    }

    public double Measure()
    {
      if (!_isReady)
        return double.NaN;
      return V7_VoltmeterManagement.Measure(0);
    }

    public void Release()
    {
      if (!_isReady)
        return;
      if (V7_VoltmeterManagement.VoltmetersCount > 0)
          V7_VoltmeterManagement.GetVoltmeter(0).Release();
      _isReady = false;
    }
  }
}