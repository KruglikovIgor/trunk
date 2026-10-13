using System;
using Sparc.Hardware.NAkip1142;
using Sparc.Hardware.NAkip1142.Transport;

namespace KPK2.Common.MeasureVoltage
{
  /// <summary>
  /// Класс реализации методов действий источника питания АКИП-1142
  /// </summary>
  public class AkipPS : IPowerSupply
  {
    private IPSAkip1142 _ps;
    private readonly Channel _channel;

    public AkipPS(Channel channel, string namesInterface = "USB")
    {
      _channel = channel;
      if (namesInterface == "USB") _ps = new PSAkip1142(new UsbTransport());
      else throw new Exception("Не предусмотрен COM интерфейс");
    }
    public void Connect()
    {
      _ps.Connect(); 
    }

    public void SetVoltage(double voltage)
    {
      _ps.SetVoltage(_channel, voltage);
    }

    public void SetCurrent(double current)
    {
      _ps.SetCurrent(_channel, current);
    }

        public void SendCommand(string command)
    {
      _ps.SendCommand(command);
    }

    public void SetOutState(bool state)
    {
      _ps.SetOutputState(state);
    }

    public double MaxVoltage()
    {
       return 60;
    }

    public void SetAddVoltage(double setVoltage)
    {
        Channel curChannel = _channel == Channel.C1 ? Channel.C2 : Channel.C1;
        _ps.SetVoltage(curChannel, setVoltage);
    }
  }
}