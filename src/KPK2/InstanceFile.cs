using System;
using System.IO;
using System.Xml;
using KPK2.Properties;
using Sparc.KPK2.NFunctions;

namespace KPK2
{
    public class InstanceFile
    {
        private XmlDocument doc = new XmlDocument();
        public string path = Environment.GetFolderPath(Environment.SpecialFolder.CommonDocuments) + @"\Sparc\KPK2\SQLInstanceName.xml";

        public string ReadInstanceFromFile()
        {
            doc.Load(path);

            XmlElement root = doc.DocumentElement;
            XmlNode attr = root.Attributes.GetNamedItem("insName");

            return attr.Value;
        }

        public void DeleteInstanceFile()
        {
            File.Delete(path);
        }
    }
}
