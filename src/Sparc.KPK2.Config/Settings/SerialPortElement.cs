using System.Configuration;
using System.IO.Ports;
using System.Xml.Serialization;

namespace Sparc.KPK2.Config.Settings
{
    public sealed class SerialPortElement
    {
        [XmlAttribute] public string PortName { get; set; }

        [XmlAttribute] public int BaudRate { get; set; }

        [XmlAttribute] public StopBits StopBits { get; set; }

        [XmlAttribute] public int DataBits { get; set; }

        [XmlAttribute] public Parity Parity { get; set; }

        [XmlAttribute] public int WriteTimeout { get; set; }

        [XmlAttribute] public int ReadTimeout { get; set; }

    }
}
