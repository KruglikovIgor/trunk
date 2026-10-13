using System.Windows.Forms;

namespace KPK2.Common.MeasureVoltage
{
  /// <summary>
  ///   позволяет получить название измерительного канала
  ///   согласно выбранного значения на панели управления
  /// </summary>
  public static class NameIk
  {
    /// <summary>
    ///   Получить имя канала
    /// </summary>
    /// <param name="groupBox">объект контейнера GroupBox</param>
    /// <returns>возврат имени канала, указанного на выбранной кнопке переключения</returns>
    public static MeasuredChannel Get(GroupBox groupBox)
    {
      MeasuredChannel mc = new MeasuredChannel();
      foreach (Control control in groupBox.Controls)
        if (control is RadioButton && ((RadioButton) control).Checked)
        {
          mc.Name = ((RadioButton) control).Text;
          mc.Code = ((RadioButton) control).Tag.ToString();
          return mc;
        }

      return null;
    }
  }
}