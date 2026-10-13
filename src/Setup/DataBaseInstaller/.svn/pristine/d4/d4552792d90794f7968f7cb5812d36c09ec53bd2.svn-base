using System;
using System.Collections.Generic;
using DBInstaller.Interface;
using Sparc.DatabaseInstaller.Sources.DatabaseInstaller.DbCreator.ScriptSets;
using Sparc.DatabaseInstaller.Sources.DatabaseInstaller.Scripts;
using Sparc.DatabaseInstaller.Sources.Logger;
using Sparc.DatabaseInstaller.Sources.Tools;

namespace Sparc.DatabaseInstaller.Sources.DatabaseInstaller.DbCreator
{
  /// <summary>
  ///   Установщик Базы данных.
  /// </summary>
  internal class DbCreator
  {
    //--------------------------------------------------------------------------------------------------------------
    /// <summary>
    ///   Логгер.
    /// </summary>
    public ILogger Logger
    {
      get => _logger;

      set => _logger = value ?? new NullLogger();
    }

    private ILogger _logger = new NullLogger();

    private string _dbName;

    //--------------------------------------------------------------------------------------------------------------
    /// <summary>
    ///   Набор скриптов для Cоздания новой и Обновления существующей БД.
    /// </summary>
    private readonly ScriptSet _scriptSet;

    //--------------------------------------------------------------------------------------------------------------
    /// <summary>
    ///   Строка подключения к БД.
    /// </summary>
    private readonly string _connectionString;


    //--------------------------------------------------------------------------------------------------------------
    /// <summary>
    ///   Конструктор.
    /// </summary>
    /// <param name="scriptSet">Набор скриптов для Cоздания новой и Обновления существующей БД.</param>
    /// <param name="connectionString">Строка подключения к БД.</param>
    public DbCreator(ScriptSet scriptSet, string connectionString)
    {
      if (scriptSet == null)
        throw new ArgumentNullException(nameof(scriptSet));
      if (connectionString == null)
        throw new ArgumentNullException(nameof(connectionString));

      _scriptSet = scriptSet;
      _connectionString = connectionString;
    }

    //--------------------------------------------------------------------------------------------------------------
    /// <summary>
    ///   Запустить Установщик Базы данных.
    /// </summary>
    /// <param name="isNewDb">Установить новую пустую БД?</param>
    /// <param name="curRegDbVersion">Версия установленной БД.</param>
    public void Run(bool isNewDb, Version curRegDbVersion, string dbName)
    {
      IEnumerable<IScript> scripts = _scriptSet.Get_Scripts(isNewDb, curRegDbVersion);
      _dbName = dbName;
          ScriptExecutor.ScriptExecutor executor = CreateScriptExecutor(_scriptSet.NewDbVersion);
      executor.Run(scripts);
    }

    //--------------------------------------------------------------------------------------------------------------
    private ScriptExecutor.ScriptExecutor CreateScriptExecutor(Version newDbVer)
    {
      var executor = new ScriptExecutor.ScriptExecutor
      {
        ConnectionString = _connectionString,
        Logger = Logger
      };
      executor.OnComplete += () => Write_Db_Version(newDbVer);

      return executor;
    }

    //--------------------------------------------------------------------------------------------------------------
    private void Write_Db_Version(Version dbVer)
    {
      _logger.Write($"Write database version: {dbVer}");
      DbVersionInDb.Write(_connectionString, dbVer, _dbName);
    }
  }
}