using System.Configuration;
using System.Xml.Serialization;

namespace Sparc.KPK2.Config.Settings
{
	[XmlRoot("LTRUnit")]
	public class ModbusElement
    {
        [XmlAttribute("name")] public string Name { get; set; }

        [XmlAttribute("address")] public byte Address { get; set; }
    }
}
