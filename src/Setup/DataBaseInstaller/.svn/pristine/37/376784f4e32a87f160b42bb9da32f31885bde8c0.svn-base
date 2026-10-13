using System;
using System.Collections.Generic;
using DBInstaller.Interface;

namespace Sparc.DatabaseInstaller.Sources.Logger
{
  /// <summary>
  ///   Многоканальный Логгер: Объединяет несколько Логгеров.
  /// </summary>
  public class MultiLogger : ILogger
  {
    //--------------------------------------------------------------------------------------------------------------
    private readonly List<ILogger> _loggers = new List<ILogger>();

    //--------------------------------------------------------------------------------------------------------------
    /// <summary>
    ///   Добавить логгер.
    /// </summary>
    /// <param name="logger">Добавляемый логгер.</param>
    public void AddLogger(ILogger logger)
    {
      if (logger == null)
        throw new ArgumentNullException(nameof(logger));

      _loggers.Add(logger);
    }

    //--------------------------------------------------------------------------------------------------------------
    /// <summary>
    ///   Записать сообщение в Лог.
    /// </summary>
    /// <param name="message">Сообщение.</param>
    public void Write(string message)
    {
      _loggers.ForEach
      (
        l => l.Write(message)
      );
    }
  }
}