using System;
using System.Collections.Generic;
using System.Linq;
using Sparc.DatabaseInstaller.Sources.DatabaseInstaller.Scripts;
using Sparc.DatabaseInstaller.Sources.ScriptLoader;

namespace Sparc.DatabaseInstaller.Sources.DatabaseInstaller.DbCreator.ScriptSets
{
  /// <summary>
  ///   Набор скриптов для создания новой БД.
  /// </summary>
  internal class CreateScriptsSet
  {
    //--------------------------------------------------------------------------------------------------------------
    /// <summary>
    ///   Имена Баз данных.
    /// </summary>
    public IEnumerable<string> DbNames { get; }

    //--------------------------------------------------------------------------------------------------------------
    /// <summary>
    ///   Скрипты.
    /// </summary>
    public IEnumerable<IScript> Scripts { get; }

    //--------------------------------------------------------------------------------------------------------------
    /// <summary>
    ///   Конструктор.
    /// </summary>
    /// <param name="scriptLoader">Загрузчик скриптов БД.</param>
    public CreateScriptsSet(IScriptLoader scriptLoader)
    {
      if (scriptLoader == null)
        throw new ArgumentNullException(nameof(scriptLoader));

      IList<ScriptItem> scriptItems = scriptLoader.Load();

      DbNames = scriptItems.Select(si => si.Name);
      Scripts = scriptItems.Select(si => new SimpleScript("Create database " + si.Name, si.Text));
    }
  }
}