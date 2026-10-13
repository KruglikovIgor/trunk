using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KPK2.Common.MeasureVoltage
{
  public interface IVoltmeter
  {
      /// <summary>
      /// Инициализация (начальная подготовка) вольтметра, его поиск
      /// </summary>
      /// <returns></returns>
      bool Init();

      /// <summary>
      /// Индикатор готовности вольтметра, т.е. прошёл он инициализацию или нет
      /// </summary>
      /// <returns></returns>
      bool IsReady();

      /// <summary>
      /// Установить измерение постоянного напряжения
      /// </summary>
      void SetMeasureVoltageDC();

      /// <summary>
      /// Проверить установку измерения постоянного напряжения
      /// </summary>
      /// <returns></returns>
      bool CheckMeasureVoltageDC();

      /// <summary>
      /// Измеряет напряжение в вольтах
      /// </summary>
      /// <returns></returns>
      double Measure();

      /// <summary>
      /// Освобождает вольтметр от захвата
      /// </summary>
      void Release();
  }
}
