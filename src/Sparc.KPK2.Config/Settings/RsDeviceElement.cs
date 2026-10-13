using System.Configuration;
using System.Xml.Serialization;

namespace Sparc.KPK2.Config.Settings
{
    public class RsDeviceElement
    {
        [XmlIgnore]public byte Address { get; set; }

        [XmlElement("port")] public SerialPortElement Port { get; set; }

        [XmlElement("lanport")] public LanPortElement LanPort { get; set; }
    }
}
