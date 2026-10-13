using System;
using System.Data.Common;

namespace Sparc.DatabaseInstaller.Sources.DatabaseInstaller.Scripts
{
  /// <summary>
  ///   Скрипт для проверки наличия БД.
  /// </summary>
  internal class ExistDbScript : SimpleScript
  {
    //--------------------------------------------------------------------------------------------------------------
    /// <summary>
    ///   БД существует?
    /// </summary>
    public bool IsExist { get; private set; }

    //--------------------------------------------------------------------------------------------------------------
    /// <summary>
    ///   Конструктор.
    /// </summary>
    /// <param name="dbName">Имя БД.</param>
    public ExistDbScript(string dbName)
    {
      if (string.IsNullOrEmpty(dbName))
        throw new ArgumentException("db_name");

      IsExist = false;
      Sql = $@"SELECT COUNT (*) FROM master.dbo.sysdatabases WHERE [name] = N'{dbName}'";
    }

    //--------------------------------------------------------------------------------------------------------------
    /// <summary>
    ///   Выполнить команду БД.
    /// </summary>
    /// <param name="command">Команда БД.</param>
    protected override void ExecuteCommand(DbCommand command)
    {
      var count = (int) command.ExecuteScalar();
      IsExist = count > 0;
    }
  }
}