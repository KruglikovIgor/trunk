using System;
using System.Data.Common;

namespace Sparc.DatabaseInstaller.Sources.DatabaseInstaller.Scripts
{
  /// <summary>
  ///   Простой скрипт БД.
  /// </summary>
  internal class SimpleScript : IScript
  {
    //--------------------------------------------------------------------------------------------------------------
    /// <summary>
    ///   Имя скрипта.
    /// </summary>
    public string Name { get; protected set; }

    //--------------------------------------------------------------------------------------------------------------
    /// <summary>
    ///   SQL-код скрипта.
    /// </summary>
    public string Sql { get; protected set; }


    //--------------------------------------------------------------------------------------------------------------
    /// <summary>
    ///   Конструктор.
    /// </summary>
    public SimpleScript()
      :
      this(string.Empty, string.Empty)
    {
    }

    //--------------------------------------------------------------------------------------------------------------
    /// <summary>
    ///   Конструктор.
    /// </summary>
    /// <param name="name">Имя скрипта.</param>
    /// <param name="sql">SQL-код скрипта.</param>
    public SimpleScript(string name, string sql)
    {
      if (name == null)
        throw new ArgumentNullException(nameof(name));
      if (sql == null)
        throw new ArgumentNullException(nameof(sql));

      Name = name;
      Sql = sql;
    }

    //--------------------------------------------------------------------------------------------------------------
    /// <summary>
    ///   Выполнить скрипт.
    /// </summary>
    /// <param name="connection">Соединение с БД на котором должен быть выполнен скрипт.</param>
    /// <param name="transaction">Транзакция в которой должен быть выполнен скрипт.</param>
    public void Execute(DbConnection connection, DbTransaction transaction)
    {
      if (connection == null)
        throw new ArgumentNullException(nameof(connection));

      string[] batches = Sql.Split(new[] {"GO\r\n"}, StringSplitOptions.RemoveEmptyEntries);

      foreach (string b in batches)
        ExecuteBatch(connection, transaction, b);
    }

    //--------------------------------------------------------------------------------------------------------------
    private void ExecuteBatch(DbConnection connection, DbTransaction transaction, string batch_sql)
    {
      using (DbCommand command = connection.CreateCommand())
      {
        command.CommandText = batch_sql;
        command.Transaction = transaction;

        ExecuteCommand(command);
      }
    }

    //--------------------------------------------------------------------------------------------------------------
    /// <summary>
    ///   Выполнить команду БД.
    /// </summary>
    /// <param name="command">Команда БД.</param>
    protected virtual void ExecuteCommand(DbCommand command)
    {
        command.ExecuteNonQuery();
    }
  }
}