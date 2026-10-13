using System;
using System.Collections.Generic;

namespace Sparc.DatabaseInstaller.Sources.Tools.LinesNavigator
{
  /// <summary>
  ///   Навигатор по коллекции строк.
  /// </summary>
  internal class LinesNavigator
  {
    //--------------------------------------------------------------------------------------------------------------
    private const int PosBefore = -1;

    private readonly List<string> _lines;
    private int _currentLine;

    //--------------------------------------------------------------------------------------------------------------
    /// <summary>
    ///   Предикат для вычисления признака Пропуска строки (например, пустой строки или комментария).
    /// </summary>
    public Predicate<string> SkipCondition { get; set; }


    //--------------------------------------------------------------------------------------------------------------
    public LinesNavigator(IEnumerable<string> lines)
    {
      if (lines == null)
        throw new ArgumentNullException(nameof(lines));

      _lines = new List<string>(lines);
      _currentLine = PosBefore;
    }

    //--------------------------------------------------------------------------------------------------------------
    /// <summary>
    ///   Начать перебор строк сначала.
    /// </summary>
    public void Reset()
    {
      _currentLine = PosBefore;
    }

    //--------------------------------------------------------------------------------------------------------------
    /// <summary>
    ///   Перейти к следующей строке.
    /// </summary>
    /// <remarks>
    ///   Часть строк может быть пропущена.
    ///   Пропускаемые строки определяются с помощью предиката SkipCondition.
    /// </remarks>
    /// <returns>
    ///   true	-	Следующая строка есть и к ней произведён переход.
    ///   false	-	Достигнут конец последовательности строк.
    /// </returns>
    public bool Next()
    {
      if (_currentLine == _lines.Count)
        return false;

      ++_currentLine;

      if (SkipCondition != null)
        while (_currentLine < _lines.Count && SkipCondition(_lines[_currentLine]))
          ++_currentLine;

      return _currentLine < _lines.Count;
    }

    //--------------------------------------------------------------------------------------------------------------
    /// <summary>
    ///   Текущая строка.
    /// </summary>
    public string Current
    {
      get
      {
        if (_currentLine < 0 || _currentLine >= _lines.Count)
          throw new InvalidOperationException();

        return _lines[_currentLine];
      }
    }
  }
}