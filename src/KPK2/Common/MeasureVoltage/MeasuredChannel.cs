namespace KPK2.Common.MeasureVoltage
{
  /// <summary>
  ///   Класс содержит имя и код измерительного канала
  /// </summary>
  public class MeasuredChannel
  {
    /// <summary>
    ///   Имя отображаемого рядом с кнопкой выбранного канала
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    ///   Код согласно tag Radiobutton выбранного канала
    /// </summary>
    public string Code { get; set; }
  }
}