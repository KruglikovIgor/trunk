using System.IO.Ports;
using Sparc.KPK2.Config.Settings;

namespace Sparc.KPK2.Config.Hardware
{
    public static class SettingsHelper
    {
        public static SerialPort CreatePortFromConfig(SerialPortElement portConfig)
        {
            var sPort = new SerialPort(portConfig.PortName);
            sPort.BaudRate = portConfig.BaudRate;
            sPort.Parity = portConfig.Parity;
            sPort.DataBits = portConfig.DataBits;
            sPort.StopBits = portConfig.StopBits;
            sPort.WriteTimeout = portConfig.WriteTimeout;
            sPort.ReadTimeout = portConfig.ReadTimeout;
            return sPort;
        }
    }
}
