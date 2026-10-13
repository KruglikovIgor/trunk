using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace Sparc.KPK2.NFunctions
{
  public static class SettingsSaver
  {
    public const string NewSettingsFileName = "settings.rec";
    public const string MainSettingsSection = "NewSettings";
    public const string TypeSettingsAttribute = "type";
    public const string ValueSettingsAttribute = "value";

    public static void Save(object obj, string path)
    {
      using (XmlWriter xWriter = new XmlTextWriter(path, Encoding.UTF8))
      {
        XmlSerializer serializer = new XmlSerializer(obj.GetType());
        serializer.Serialize(xWriter, obj);
      }
    }

    public static T Load<T>(string path)
    {
      if (!File.Exists(path))
        throw new FileNotFoundException();
      using (XmlTextReader xReader = new XmlTextReader(path))
      {
        XmlSerializer serializer = new XmlSerializer(typeof(T));
        T result = (T) serializer.Deserialize(xReader);
        return result;
      }
    }

    public static void Save(XmlDocument doc, string path)
    {
      path = Path.GetFullPath(path) + "\\" + NewSettingsFileName;
      doc.Save(path);
    }

    public static XmlDocument Load(string path)
    {
      XmlDocument doc = new XmlDocument();
      doc.Load(path);
      return doc;
    }

  }
}
