using System;
using NLog;
using ILogger = DBInstaller.Interface.ILogger;

namespace Sparc.DatabaseInstaller.Sources.Logger
{
  /// <summary>
  ///   NLog Логгер.
  /// </summary>
  public class NLogLogger : ILogger
  {
    //--------------------------------------------------------------------------------------------------------------
    private readonly NLog.Logger _logger;
    private readonly LogLevel _logLevel;

    //--------------------------------------------------------------------------------------------------------------
    /// <summary>
    ///   Конструктор.
    /// </summary>
    /// <param name="name">Название логгера NLog.</param>
    /// <param name="logLevel">Уровень логгирования для всех сообщений.</param>
    public NLogLogger(string name, LogLevel logLevel)
    {
      if (name == null)
        throw new ArgumentNullException(nameof(name));
      _logger = LogManager.GetLogger(name);
      _logLevel = logLevel ?? throw new ArgumentNullException(nameof(logLevel));
    }

    //--------------------------------------------------------------------------------------------------------------
    /// <summary>
    ///   Записать сообщение в Лог.
    /// </summary>
    /// <param name="message">Сообщение.</param>
    public void Write(string message)
    {
      _logger.Log(_logLevel, message);
    }
  }
}