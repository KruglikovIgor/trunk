using System;
using System.Data.SqlClient;

namespace Sparc.DatabaseInstaller.Sources.Tools
{
  /// <summary>
  ///   Читатель/Писатель Версии БД.
  /// </summary>
  internal static class DbVersionInDb
  {
    //private const string VersionTableName = "Version";
    //private const string DbName = "KPK6";

    //--------------------------------------------------------------------------------------------------------------
    public static Version Read(string connStr, string DbName, string VersionTableName)
    {
      try
      {
        using (var sConnection = new SqlConnection(connStr))
        {
          sConnection.Open();

          var checkDbExistsCommand = new SqlCommand($"use {DbName}", sConnection);
          try
          {
            checkDbExistsCommand.ExecuteNonQuery();
          }
          catch
          {
            return null;
          }

          var checkTbCommand =
            new SqlCommand(
              $"use {DbName} select count(*) from INFORMATION_SCHEMA.TABLES where TABLE_NAME = '{VersionTableName}'", sConnection);
          var tableCount = (int) checkTbCommand.ExecuteScalar();
          if (tableCount == 0)
              return new Version("1.0.0");

          var sqlCommand = new SqlCommand($"use {DbName} select DBVersion from Version", sConnection);
          object verVal = sqlCommand.ExecuteScalar();

          if (verVal == null)
            return null;

          if (string.IsNullOrWhiteSpace((string)verVal))
              verVal = "0.0";

          Version ver = new Version((string)verVal);

          return ver;
        }
      }
      catch (Exception e)
      {
        throw new Exception("Can't read database version", e);
      }
    }

    //--------------------------------------------------------------------------------------------------------------
    public static void Write(string connStr, Version ver, string DbName)
    {
      try
      {
        using (var sConnection = new SqlConnection(connStr))
        {
          sConnection.Open();
          const string verParam = "@Version";
          var sqlCommand =
            new SqlCommand($"use {DbName} update Version Set DBVersion = {verParam} where Id = 1",
              sConnection);
          sqlCommand.Parameters.AddWithValue(verParam, ver.ToString());
          sqlCommand.ExecuteNonQuery();
        }
      }
      catch (Exception e)
      {
        throw new Exception("Can't write database version", e);
      }
    }
  }
}