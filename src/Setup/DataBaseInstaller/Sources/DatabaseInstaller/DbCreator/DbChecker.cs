using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using DBInstaller.Interface;
using Sparc.DatabaseInstaller.Sources.DatabaseInstaller.Scripts;
using Sparc.DatabaseInstaller.Sources.Logger;

namespace Sparc.DatabaseInstaller.Sources.DatabaseInstaller.DbCreator
{
  /// <summary>
  ///   Детектор наличия Баз данных.
  /// </summary>
  internal class DbChecker
  {
    private ILogger _logger = new NullLogger();

    //--------------------------------------------------------------------------------------------------------------
    /// <summary>
    ///   Логгер.
    /// </summary>
    public ILogger Logger
    {
      get => _logger;

      set => _logger = value ?? new NullLogger();
    }

    //--------------------------------------------------------------------------------------------------------------
    /// <summary>
    ///   Определить какие из указанных БД установлены.
    /// </summary>
    /// <param name="dbNames">Имена искомых БД.</param>
    /// <param name="connStr">Строка подключения к БД.</param>
    /// <returns></returns>
    public IEnumerable<string> Find_Installed(IEnumerable<string> dbNames, string connStr)
    {
      if (connStr == null)
        throw new ArgumentNullException(nameof(connStr));
      if (dbNames == null)
        throw new ArgumentNullException(nameof(dbNames));
      if (dbNames.Any(string.IsNullOrEmpty))
        throw new ArgumentException(nameof(dbNames));

      var installedDbs = new List<string>();

      using (var conn = new SqlConnection(connStr))
      {
        conn.Open();

        foreach (string nm in dbNames)
        {
          bool isExist = Is_Db_Exists(nm, conn);
          if (isExist)
            installedDbs.Add(nm);
        }

        return installedDbs;
      }
    }

    //--------------------------------------------------------------------------------------------------------------
    private bool Is_Db_Exists(string dbName, DbConnection conn)
    {
      var script = new ExistDbScript(dbName);
      script.Execute(conn, null);

      string foundStr = script.IsExist ? "present" : "absent";
      Logger.Write($"{dbName} ... {foundStr}");

      return script.IsExist;
    }
  }
}