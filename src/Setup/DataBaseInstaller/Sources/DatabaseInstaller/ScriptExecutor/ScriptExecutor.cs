using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using DBInstaller.Interface;
using Sparc.DatabaseInstaller.Sources.DatabaseInstaller.Scripts;
using Sparc.DatabaseInstaller.Sources.Logger;

namespace Sparc.DatabaseInstaller.Sources.DatabaseInstaller.ScriptExecutor
{
  /// <summary>
  ///   Исполнитель скриптов.
  /// </summary>
  internal class ScriptExecutor
  {
    private ILogger _logger = new NullLogger();

    //--------------------------------------------------------------------------------------------------------------
    /// <summary>
    ///   Строка соединения на котором выполняется взаимодействие с БД.
    /// </summary>
    public string ConnectionString { get; set; }

    //--------------------------------------------------------------------------------------------------------------
    /// <summary>
    ///   Выполнять все скрипты в одной транзакции?
    /// </summary>
    public bool UseTransaction { get; set; }

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
    ///   Событие: Все скрипты успешно выполнены.
    /// </summary>
    public event Action OnComplete;

    //--------------------------------------------------------------------------------------------------------------
    /// <summary>
    ///   Событие: Произошёл сбой.
    /// </summary>
    public event Action OnFail;

    //--------------------------------------------------------------------------------------------------------------
    /// <summary>
    ///   Выполнить скрипты.
    /// </summary>
    /// <returns>Версия БД после выполнения скриптов.</returns>
    public void Run(IEnumerable<IScript> scripts)
    {
      if (scripts == null)
        throw new ArgumentNullException(nameof(scripts));
      if (ConnectionString == null)
        throw new InvalidOperationException();

      using (var conn = new SqlConnection(ConnectionString))
      {
        conn.Open();

        DbTransaction tx = UseTransaction
          ? conn.BeginTransaction(IsolationLevel.Serializable)
          : null;
        try
        {
          RunScripts(scripts, conn, tx);

          if (OnComplete != null)
            OnComplete();

          if (tx != null)
            tx.Commit();
        }
        catch (Exception)
        {
          if (OnFail != null)
            OnFail();

          if (tx != null)
            tx.Rollback();

          throw;
        }
      }
    }

    //--------------------------------------------------------------------------------------------------------------
    private void RunScripts(IEnumerable<IScript> scripts, DbConnection conn, DbTransaction tx)
    {
      if (scripts.Any())
        foreach (IScript s in scripts)
          RunScript(s, conn, tx);
      else
        Logger.Write("No scripts to run");
    }

    //--------------------------------------------------------------------------------------------------------------
    private void RunScript(IScript script, DbConnection conn, DbTransaction tx)
    {
      try
      {
        Logger.Write(script.Name);
        script.Execute(conn, tx);
      }
      catch (Exception e)
      {
        throw new Exception("Fail: " + script.Name, e);
      }
    }
  }
}