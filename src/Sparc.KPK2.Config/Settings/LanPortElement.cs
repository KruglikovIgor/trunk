using System.Configuration;
using System.IO.Ports;
using System.Xml.Serialization;

namespace Sparc.KPK2.Config.Settings
{
    public sealed class LanPortElement
    {
        [XmlAttribute] public string IPAddress { get; set; }
    }
}