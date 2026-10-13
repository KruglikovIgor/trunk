using System;
using System.Threading;
using Hardware.SpectrumAnalyzer.SignalHound.Sa;
using Hardware.SpectrumAnalyzer.SignalHound.Sa.SignalHound;
using NLog;

namespace Sparc.KPK2.Hardware
{
  public class SpectrumAnalyzer
  {
    private SpectrumAnalyzerSignalHoundSa _saDevice;

    private static bool _isConnected = false;

    private static Logger _logger = LogManager.GetLogger("KPK2Info");

        public static bool IsConnected
    {
      get { return _isConnected; }
    }

    private static SpectrumAnalyzer _instance;

    public static SpectrumAnalyzer Instance
    {
      get { return _instance ?? (_instance = new SpectrumAnalyzer()); }
    }

    private SpectrumAnalyzer()
    {
      try
      {
        _saDevice = new SpectrumAnalyzerSignalHoundSa();
        //if (!Globals.Emulation)
          _saDevice.Connect();
        _isConnected = true;
        LogVersion();
      }
      catch (Exception ex)
      {
          //var logger = LogManager.GetCurrentClassLogger();
          _logger.Error(ex.ToString());
          _isConnected = false;
      }
    }

    private void LogVersion()
    {
        //var logger = LogManager.GetCurrentClassLogger();
        _logger.Info(string.Format("Анализатор спектра подключён: {0} {1}", _saDevice.GetDeviceName(), _saDevice.GetFirmwareString()));
    }

    public void Disconnect()
    {
        try
        {
            if (_saDevice != null)
            {
                _saDevice.Disconnect();
                _saDevice.Dispose();
                _saDevice = null;
            }

            _instance = null;

            _isConnected = false;
        }
        catch
        {
            _isConnected = false;
        }
    }

    /// <summary>
    /// Центральная частота обзора
    /// </summary>
    public double Center
    {
      get { return _saDevice.Center; }
      set { _saDevice.Center = value; }
    }

    /// <summary>
    /// Ширина полосы обзора
    /// </summary>
    public double Span
    {
      get { return _saDevice.Span; }
      set { _saDevice.Span = value; }
    }

    /// <summary>
    /// Опорный уровень (предполагаемый уровень сигнала)
    /// </summary>
    public double RefLevel
    {
      get { return _saDevice.ReferenceLevel; }
      set { _saDevice.ReferenceLevel = value; }
    }

    /// <summary>
    /// Resolution Bandwidth
    /// </summary>
    public double RBW
    {
      get { return _saDevice.ResolutionBandwidth; }
      set { _saDevice.ResolutionBandwidth = value; }
    }

    //private double sweepStartFreqEmulation;

    public double SweepStartFreq
    {
      get
      {
        //if (Globals.Emulation)
        //  return sweepStartFreqEmulation;
        return _saDevice.SweepStartFreq;
      }
    }

    //private double sweepBinSizeEmulation;

    public double SweepBinSize
    {
      get
      {
        //if (Globals.Emulation)
        //  return sweepBinSizeEmulation;
        return _saDevice.SweepBinSize;
      }
    }

    public double[] GetSweep()
    {
      if (!_isConnected)
        throw new Exception("Not connected!");
      //if (Globals.Emulation)
      //{
      //  sweepStartFreqEmulation = Center - Span / 2;
      //  sweepBinSizeEmulation = 28571;
      //  double[] result = new double[700];
      //  Random rnd = new Random(Environment.TickCount);
      //  for (int i = 0; i < result.Length; i++)
      //    result[i] = rnd.Next(-60, 2);
      //  return result;
      //}
      _saDevice.GetSweep();
      return _saDevice.MaxSamples;
    }

    public void Init()
    {
      //_saDevice.Connect();
      _saDevice.Detector = DetectorType.Average;
      _saDevice.Scale = SweepScales.LogScale;
      _saDevice.Units = UnitsType.dBm;
      _saDevice.IqBandwidth = 10e3;
      _saDevice.IqDecimation = 1;
      _saDevice.RBWShape = RBWShape.FlatTop;
      //if (Globals.Emulation)
      //  return;
      _saDevice.ConfigRBWShape();
      _saDevice.GetTemperature();
    }


    public void SetCommonParams()
    {
      //if (Globals.Emulation)
      //  return;
      _saDevice.SetCommonState();
    }

    public void SetWorkModeSweep()
    {
      _saDevice.SetWorkMode(WorkModes.Sweeping);
      Thread.Sleep(2000);
    }
  }
}
