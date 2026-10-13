using Sparc.KPK2.Hardware;

namespace KPK2.Common.DissFunctions
{
  public static class Diss32Functions
  {
    /// <summary>
    /// Устанавливает канал для одноканального ДИСС-32
    /// </summary>
    /// <param name="channel"></param>
    /// <returns></returns>
    public static bool SetMeasChannel(int channel)
    {
      if (channel >= 3 || channel < 0)
        return false;
      bool result = DiscreteOutputs.Instance().SetOutput(15, channel != 2);
      result &= DiscreteOutputs.Instance().SetOutput(14, channel != 1);
      return result;
    }

    public static bool SetMeasChannelOff()
    {
      bool result = DiscreteOutputs.Instance().SetOutput(15, false);
      result &= DiscreteOutputs.Instance().SetOutput(14, false);
      return result;
    }

    /// <summary>
    /// Включаем КЗ
    /// </summary>
    /// <param name="strCT">команда на включение меандров</param>
    /// <param name="strDOutput">команда на включение КЗ</param>
    /// <returns></returns>
    public static bool TaskOn(string strCT, string strDOutput)
    {
      bool result = true;
      DiscreteOutputs.Instance().SetOutput("High_32", false); // включаем "Высокое" 
      result &= TaskRele(true);
      result &= ControlTask.Instance().SetControlTask(strCT);
      result &= DiscreteOutputs.Instance().SetOutput(strDOutput, true);
      return result;
    }

    private static bool TaskRele(bool state)
    {
      bool result = true;
      result &= DiscreteOutputs.Instance().SetOutput("UNCh_32", state);
      result &= DiscreteOutputs.Instance().SetOutput("Pitch", state);
      result &= DiscreteOutputs.Instance().SetOutput("Heel", state);
      return result;
    }

    /// <summary>
    /// Выключаем КЗ
    /// <param name="str">команда на выключение КЗ</param>
    /// </summary>
    public static bool TaskOff(string str)
    {
      bool result = true;
      result &= DiscreteOutputs.Instance().SetOutput(str, false);
      result &= ControlTask.Instance().SetControlTask(0, 0, 0);
      result &= GenSinCos.Instance().GeneratorOff();
      result &= TaskRele(false);
      return result;
    }

    public static bool Task1On()
    {
      return TaskOn("Task_W_1_32", "Task_1_32");
    }

    public static bool Task1Off()
    {
      return TaskOff("Task_1_32");
    }

    public static bool Task2On()
    {
      return TaskOn("Task_W_1_32", "Task_2_32");
    }

    public static bool Task2Off()
    {
      return TaskOff("Task_2_32");
    }

    public static bool Task3On()
    {
      return TaskOn("Task_W_1_32", "Task_3_32");
    }

    public static bool Task3Off()
    {
      return TaskOff("Task_3_32");
    }

      public static bool SetAngle(ushort angle)
      {
          angle = NormalizeAngle(angle);
          return GenSinCos.Instance().GeneratorOn(angle);
      }

      public static ushort NormalizeAngle(double angle)
      {
          while (angle >= 360)
              angle -= 360;
          while (angle < 0)
              angle += 360;
          return (ushort)angle;
      }
  }
}
