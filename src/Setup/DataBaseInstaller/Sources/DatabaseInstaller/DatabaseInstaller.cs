using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Reflection;
using DBInstaller.Interface;
using Sparc.DatabaseInstaller.Properties;
using Sparc.DatabaseInstaller.Sources.DatabaseInstaller.DbBackuper;
using Sparc.DatabaseInstaller.Sources.DatabaseInstaller.DbCreator;
using Sparc.DatabaseInstaller.Sources.DatabaseInstaller.DbCreator.ScriptSets;
using Sparc.DatabaseInstaller.Sources.DatabaseInstaller.Enum;
using Sparc.DatabaseInstaller.Sources.Logger;
using Sparc.DatabaseInstaller.Sources.ScriptLoader;
using Sparc.DatabaseInstaller.Sources.Tools;

namespace Sparc.DatabaseInstaller.Sources.DatabaseInstaller
{
  /// <summary>
  ///   Установщик Базы данных.
  /// </summary>
  public class DatabaseInstaller
  {
////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    #region PROPERTIES

    private string _dbNameOrKeyPath;
    private string _versionTableNameOrParamName;

    private ScriptName _scriptName;
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

    //--------------------------------------------------------------------------------------------------------------
    /// <summary>
    ///   Строка подключения к БД.
    /// </summary>
    public string ConnectionString
    {
      get => _connectionString;

      set => _connectionString = value ?? throw new ArgumentNullException(nameof(value));
    }

    private string _connectionString = Settings.Default.ConnectionString;

    //--------------------------------------------------------------------------------------------------------------
    /// <summary>
    ///   Загрузчик скриптов для Создания БД.
    /// </summary>
    public IScriptLoader CreateScriptLoader
    {
      get => _createScriptLoader;

      set => _createScriptLoader = value ?? throw new ArgumentNullException(nameof(value));
    }

    private IScriptLoader _createScriptLoader;

    //--------------------------------------------------------------------------------------------------------------
    /// <summary>
    ///   Загрузчик скриптов для Обновления БД.
    /// </summary>
    public IScriptLoader UpdateScriptLoader
    {
      get => _updateScriptLoader;

      set => _updateScriptLoader = value ?? throw new ArgumentNullException(nameof(value));
    }

    private IScriptLoader _updateScriptLoader;

    #endregion

////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    #region CHECK

    //--------------------------------------------------------------------------------------------------------------
    /// <summary>
    ///   Получить информацию об установленной БД.
    /// </summary>
    /// <returns>Информация об установленной Базе данных.</returns>
    public DbInfo Check(string dbName, string versionTableName, ScriptName scriptName)
    {
      DbInfo info = null;
      _dbNameOrKeyPath = dbName;
      _versionTableNameOrParamName = versionTableName;
      _scriptName = scriptName;
      Execute
      (
        () => { info = Check_Command(); }
      );

      return info;
    }

    //--------------------------------------------------------------------------------------------------------------
    private DbInfo Check_Command()
    {
      _logger.Write("CHECK");
      LogTimeMark();

      var scriptSet = new ScriptSet(_createScriptLoader, _updateScriptLoader);

      DbInfo dbInfo = Check_Database(scriptSet);
      return dbInfo;
    }

    //--------------------------------------------------------------------------------------------------------------
    private DbInfo Check_Database(ScriptSet scriptSet)
    {
      var dbInfo = new DbInfo();

      dbInfo.Installed_Databases = Find_Installed_Databases(scriptSet.DbNames);
      dbInfo.Is_Overall_Installed = Is_Db_Overall_Installed(dbInfo.Installed_Databases);
      dbInfo.Current_Version = Read_Version(_connectionString);

      return dbInfo;
    }

    //--------------------------------------------------------------------------------------------------------------
    private IEnumerable<string> Find_Installed_Databases(IEnumerable<string> dbNames)
    {
      var dbCheсker = new DbChecker
      {
        Logger = _logger
      };

      return dbCheсker.Find_Installed(dbNames, _connectionString);
    }

    //--------------------------------------------------------------------------------------------------------------
    private bool Is_Db_Overall_Installed(IEnumerable<string> installedDbs)
    {
      bool isInstalled = installedDbs.Any();

      string msg = isInstalled ? "FOUND" : "NOT found";
      _logger.Write($"Database OVERALL: {msg}");

      return isInstalled;
    }

    //--------------------------------------------------------------------------------------------------------------
    private Version Read_Version(string connStr)
    {
        Version version;

        if (_scriptName.Version_InDB == EnumVersionPlace.VersionDB)
            version = DbVersionInDb.Read(connStr, _dbNameOrKeyPath, _versionTableNameOrParamName);
        else
            version = DbVersion.Read(_dbNameOrKeyPath, _versionTableNameOrParamName);

        _logger.Write("Database current version: " + BackupIndex.Version_String(version));
        return version;
    }

    #endregion

////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    #region INSTALL

    //--------------------------------------------------------------------------------------------------------------
    /// <summary>
    ///   Установить БД.
    /// </summary>
    /// <param name="options">Опции установки Базы данных.</param>
    public void Install(InstallOptions options, ScriptName scriptName)
    {
      _scriptName = scriptName;
      Execute
      (
        () => Install_Command(options)
      );
    }

    //--------------------------------------------------------------------------------------------------------------
    private void Install_Command(InstallOptions options)
    {
      if (options == null)
        throw new ArgumentNullException(nameof(options));

      _logger.Write("INSTALL");
      LogTimeMark();
      Show_Parameters(options);

      var scriptSet = new ScriptSet(_createScriptLoader, _updateScriptLoader);
      DbInfo dbInfo = Check_Database(scriptSet);

      bool isNewDb = Is_New_Db(options.Is_Reinstall, dbInfo.Is_Overall_Installed);

      Check_Conditions(isNewDb, scriptSet.NewDbVersion, dbInfo.Current_Version);

      if (options.Is_Backup)
        BackUp_Database(dbInfo.Installed_Databases, dbInfo.Current_Version, options.Backup_Folder, true);

      Install_Database(scriptSet, isNewDb, dbInfo.Current_Version);
    }

    //--------------------------------------------------------------------------------------------------------------
    private void Show_Parameters(InstallOptions options)
    {
      var paramMsg = string.Empty;

      if (options.Is_Backup)
        paramMsg += " backup";

      if (options.Is_Reinstall)
        paramMsg += " reinstall";

      if (paramMsg == string.Empty)
        paramMsg += " <no parameters>";

      _logger.Write("Parameters:" + paramMsg);
    }

    //--------------------------------------------------------------------------------------------------------------
    private bool Is_New_Db(bool isReinstall, bool isInstalled)
    {
      bool isNewDb = !isInstalled || isReinstall;
      return isNewDb;
    }

    //--------------------------------------------------------------------------------------------------------------
    private void Check_Conditions(bool isNewDb, Version newDbVer, Version curRegDbVersion)
    {
      if (isNewDb)
        return;

      if (curRegDbVersion == null)
        throw new Exception("Update fail: Missing the version number of the installed database");

      if (curRegDbVersion > newDbVer)
      {
        const string msg = "Update fail: The version of the database above version of the update patches. " +
                           "In this case you can only fully replace the current database with an older one. " +
                           "To do this, select the option to 'Reinstall database'";
        throw new Exception(msg);
      }
    }

    //--------------------------------------------------------------------------------------------------------------
    private void Install_Database(ScriptSet scriptSet, bool isNewDb, Version curRegDbVersion)
    {

      var installer = new DbCreator.DbCreator(scriptSet, _connectionString)
      {
        Logger = _logger
      };

      installer.Run(isNewDb, curRegDbVersion, _dbNameOrKeyPath);
    }

    #endregion

////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    #region BACKUP

    //--------------------------------------------------------------------------------------------------------------
    /// <summary>
    ///   Создать резервную копию БД.
    /// </summary>
    /// <param name="folderPath">Путь к резервной копии БД.</param>
    public void BackUp(string folderPath, ScriptName scriptName)
    {
        _scriptName = scriptName;
      Execute
      (
        () => BackUp_Command(folderPath)
      );
    }

    //--------------------------------------------------------------------------------------------------------------
    private void BackUp_Command(string folderPath)
    {
      if (folderPath == null)
        throw new ArgumentNullException(nameof(folderPath));

      _logger.Write("BACKUP");
      LogTimeMark();

      var scriptSet = new ScriptSet(_createScriptLoader, _updateScriptLoader);
      DbInfo dbInfo = Check_Database(scriptSet);

      BackUp_Database(dbInfo.Installed_Databases, dbInfo.Current_Version, folderPath, false);
    }

    //--------------------------------------------------------------------------------------------------------------
    private void BackUp_Database(IEnumerable<string> dbNames, Version version, string folderPath, bool useQuestions)
    {
      var folderName = DateTime.Now.ToString("yyyy.MM.dd  HH-mm-ss");

      var backuper = new DbBackuper.DbBackuper(_connectionString)
      {
        Logger = _logger
      };

      backuper.Backup(dbNames, version, folderPath, folderName, useQuestions);
    }

    #endregion

////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    #region RESTORE

    //--------------------------------------------------------------------------------------------------------------
    /// <summary>
    ///   Восстановить БД из резервной копии.
    /// </summary>
    /// <param name="folder">Путь к резервной копии БД.</param>
    public void Restore(string folder)
    {
      Execute
      (
        () => Restore_Command(folder)
      );
    }

    //--------------------------------------------------------------------------------------------------------------
    private void Restore_Command(string folder)
    {
      if (folder == null)
        throw new ArgumentNullException(nameof(folder));

      _logger.Write("RESTORE");
      LogTimeMark();

      var backuper = new DbBackuper.DbBackuper(_connectionString)
      {
        Logger = _logger
      };

      backuper.Restore(folder);
    }

    #endregion

////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    #region EXECUTE Command with Log

    //--------------------------------------------------------------------------------------------------------------
    private void Execute(Action command)
    {
      try
      {
        command();
        _logger.Write("Completed SUCCESSFULLY" + Environment.NewLine);
      }
      catch (Exception exc)
      {
        var excMsg = new ExceptionMessage(exc);
        _logger.Write(string.Format("{0}{1}{0}", Environment.NewLine, excMsg.Full));

        throw;
      }
    }

    //--------------------------------------------------------------------------------------------------------------
    private void LogTimeMark()
    {
      string msg = string.Format("Timestamp: {0:dd-MM-yyyy HH:mm:ss}", DateTime.Now);
      _logger.Write(msg);
    }

    #endregion

////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
  }
}