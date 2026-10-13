using System;
using System.IO;
using DBInstaller.Interface;

namespace Sparc.DatabaseInstaller.Sources.Logger
{
  /// <summary>
  ///   Файловый Логгер.
  /// </summary>
  public class FileLogger : ILogger
  {
    //--------------------------------------------------------------------------------------------------------------
    private readonly string _path;

    //--------------------------------------------------------------------------------------------------------------

    #region Конструктор

    public FileLogger(string path)
    {
      if (string.IsNullOrEmpty(path))
        throw new ArgumentException(nameof(path));

      _path = path;

      // Создаем каталог
      CreateLogFolder(path);
    }

    private void CreateLogFolder(string path)
    {
      try
      {
        string logFolder = Path.GetDirectoryName(path);

        if (!string.IsNullOrWhiteSpace(logFolder))
          Directory.CreateDirectory(logFolder);
      }
      catch
      {
        // do nothing
      }
    }

    #endregion

    //--------------------------------------------------------------------------------------------------------------
    /// <summary>
    ///   Записать сообщение в Лог.
    /// </summary>
    /// <param name="message">Сообщение.</param>
    public void Write(string message)
    {
      try
      {
        File.AppendAllText(_path, message + Environment.NewLine);
      }
      catch
      {
        // do nothing
      }
    }
  }
}