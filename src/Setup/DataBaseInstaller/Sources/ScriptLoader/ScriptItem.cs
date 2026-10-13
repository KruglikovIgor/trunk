namespace Sparc.DatabaseInstaller.Sources.ScriptLoader
{
  /// <summary>
  ///   Загруженный скрипт БД.
  /// </summary>
  public class ScriptItem
  {
    /// <summary>
    ///   Название скрипта.
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    ///   Sql код скрипта.
    /// </summary>
    public string Text { get; set; }
  }
}