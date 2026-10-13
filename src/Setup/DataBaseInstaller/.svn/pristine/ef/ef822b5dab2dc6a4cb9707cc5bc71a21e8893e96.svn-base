using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Sparc.DatabaseInstaller.Sources.ScriptLoader
{
  /// <summary>
  ///   Загрузчик скриптов БД из Ресурсов сборки.
  /// </summary>
  public class ResourceScriptLoader : IScriptLoader
  {
    //--------------------------------------------------------------------------------------------------------------
    private readonly Assembly _assembly;

    private readonly string _prefix;
    private readonly string _suffix;

    //--------------------------------------------------------------------------------------------------------------
    /// <summary>
    ///   Конструктор.
    /// </summary>
    /// <param name="assembly"></param>
    /// <param name="prefix">Префикс полного имени ресурса.</param>
    /// <param name="suffix">Суффикс полного имени ресурса.</param>
    /// <remarks>
    ///   Полное имя ресурса включает имя самого файла ресурса и имена всех папок проекта в которые он вложен.
    /// </remarks>
    public ResourceScriptLoader(Assembly assembly, string prefix, string suffix)
    {
      if (assembly == null)
        throw new ArgumentNullException(nameof(assembly));
      if (prefix == null)
        throw new ArgumentNullException(nameof(prefix));
      if (suffix == null)
        throw new ArgumentNullException(nameof(suffix));

      _assembly = assembly;
      _prefix = prefix;
      _suffix = suffix;
    }

    //--------------------------------------------------------------------------------------------------------------
    public IList<ScriptItem> Load()
    {
      IEnumerable<string> names = GetResourcesNames();
      return ReadResources(names);
    }

    //--------------------------------------------------------------------------------------------------------------
    private IEnumerable<string> GetResourcesNames()
    {
      string[] names = _assembly.GetManifestResourceNames();
      int prefixSuffixLen = _prefix.Length + _suffix.Length;

      IEnumerable<string> selectedNames = names.Where(n => n.StartsWith(_prefix, StringComparison.Ordinal))
        .Where(n => n.EndsWith(_suffix, StringComparison.Ordinal)).Where(n => n.Length >= prefixSuffixLen).OrderBy(n=>n);
      return selectedNames;
    }

    //--------------------------------------------------------------------------------------------------------------
    private IList<ScriptItem> ReadResources(IEnumerable<string> names)
    {
      var resItems = new List<ScriptItem>();

      foreach (string n in names)
      {
        ScriptItem ri = ReadRes(n);
        resItems.Add(ri);
      }

      return resItems;
    }

    //--------------------------------------------------------------------------------------------------------------
    private ScriptItem ReadRes(string resName)
    {
      Stream resStream = _assembly.GetManifestResourceStream(resName);
      if (resStream == null)
        throw new ArgumentException(string.Format("Cant't get stream for resource '{0}'", resName));

      using (resStream)
      {
        using (var sr = new StreamReader(resStream))
        {
          var ri = new ScriptItem
          {
            Name = ExtractShortName(resName, _prefix, _suffix),
            Text = sr.ReadToEnd()
          };

          return ri;
        }
      }
    }

    //--------------------------------------------------------------------------------------------------------------
    private static string ExtractShortName(string fullName, string prefix, string suffix)
    {
      string shortName = fullName;

      if (prefix.Length > 0)
        shortName = shortName.Remove(0, prefix.Length);

      if (suffix.Length > 0)
        shortName = shortName.Remove(shortName.Length - suffix.Length);

      return shortName;
    }
  }
}