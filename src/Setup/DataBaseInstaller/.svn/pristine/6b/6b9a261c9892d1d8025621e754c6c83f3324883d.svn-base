using System;
using System.Collections.Generic;
using Sparc.DatabaseInstaller.Sources.DatabaseInstaller.Scripts;
using Sparc.DatabaseInstaller.Sources.ScriptLoader;

namespace Sparc.DatabaseInstaller.Sources.DatabaseInstaller.DbCreator.ScriptSets
{
  /// <summary>
  ///   Набор скриптов для создания новой и Обновления существующей БД.
  /// </summary>
  internal class ScriptSet
  {
    //--------------------------------------------------------------------------------------------------------------
    /// <summary>
    ///   Набор скриптов для создания новой БД.
    /// </summary>
    private readonly CreateScriptsSet _createSet;

    //--------------------------------------------------------------------------------------------------------------
    /// <summary>
    ///   Набор скриптов для обновления существующей БД.
    /// </summary>
    private readonly UpdateScriptsSet _updateSet;


    //--------------------------------------------------------------------------------------------------------------
    /// <summary>
    ///   Имена Баз данных.
    /// </summary>
    public IEnumerable<string> DbNames => _createSet.DbNames;

    //--------------------------------------------------------------------------------------------------------------
    /// <summary>
    ///   Версия новой БД после отработки инсталлятора.
    /// </summary>
    public Version NewDbVersion => _updateSet.HighVersion;

    //--------------------------------------------------------------------------------------------------------------
    /// <summary>
    ///   Конструктор.
    /// </summary>
    /// <param name="createLoader">Загрузчик скриптов для Создания БД.</param>
    /// <param name="updateLoader">Загрузчик скриптов для Обновления БД.</param>
    public ScriptSet(
      IScriptLoader createLoader,
      IScriptLoader updateLoader
    )
    {
      if (createLoader == null)
        throw new ArgumentNullException(nameof(createLoader));
      if (updateLoader == null)
        throw new ArgumentNullException(nameof(updateLoader));

      _createSet = new CreateScriptsSet(createLoader);
      _updateSet = new UpdateScriptsSet(updateLoader);
    }

    //--------------------------------------------------------------------------------------------------------------
    /// <summary>
    ///   Получить все скрипты для обновления БД от заданной версии.
    /// </summary>
    /// <param name="isNewDb">Установить новую пустую БД?</param>
    /// <param name="curVersion">Версия установленной БД.</param>
    /// <returns>Скрипты обновления БД.</returns>
    public IEnumerable<IScript> Get_Scripts(bool isNewDb, Version curVersion)
    {
      if (isNewDb)
        return _createSet.Scripts;

      if (curVersion == null)
        throw new ArgumentNullException(nameof(curVersion));

      return _updateSet.Get_Scripts_for_Update_From(curVersion);
    }
  }
}