using System;
using System.Collections;
using System.Windows.Forms;
using KPK2.MyClasse;
using NLog;
using Sparc.KPK2.Hardware;
using Sparc.KPK2.Resources.Properties;

namespace KPK2.Common.DissFunctions
{
  public static class Diss15Functions
  {
    private static Logger _logger = LogManager.GetLogger("KPK2Error");

    /// <summary>
    /// Универсальный метод для всех КЗ
    /// </summary>
    /// <param name="state"></param>
    /// <param name="str"></param>
    /// <returns></returns>
    public static bool SetTask(bool state, string str)
    {
      try
      {
        DiscreteOutputs.Instance().SetOutputEx("UNCh_15", state);
        DiscreteOutputs.Instance().SetOutputEx("Task_15", state);
        DiscreteOutputs.Instance().SetOutputEx("Course", state);
        DiscreteOutputs.Instance().SetOutputEx("Pitch", state);
        DiscreteOutputs.Instance().SetOutputEx("Heel", state);
        if (state)
        {
          SetAngleEx(0);
          ControlTask.Instance().SetControlTaskEx(str);
        }
        else
        {
          GenSinCos.Instance().GeneratorOffEx();
          ControlTask.Instance().ControlTaskOffEx();
        }
      }
      catch (Exception ex)
      {
        _logger.Error(ex);
        MessageBox.Show(Properties.Resources.ErrorDoCommand + "\n" + ex.Message, Resources.Error);
        return false;
      }
      return true;
    }

    /// <summary>
    /// Включает и отключает КЗ-4
    /// </summary>
    /// <param name="state"></param>
    /// <returns></returns>
    public static bool SetTask4()
    {
      return SetTask(true, "Task_W_4_15");
    }
     
    internal static bool SetTask5()
    {
      return SetTask(true, "Task_W_5_15");
    }

    internal static bool SetTask6()
    {
      return SetTask(true, "Task_W_6_15");
    }

    internal static bool SetTask1()
    {
      return SetTask(true, "Task_W_1_15");
    }

    public static bool TaskOff()
    {
      return SetTask(false, String.Empty);
    }

    internal static void SetCourseEx(bool state)
    {
      DiscreteOutputs.Instance().SetOutputEx("Course", state);
    }

    internal static bool SetTask2()
    {
      return SetTask(true, "Task_W_2_15");
    }

    internal static bool SetTask3()
    {
      return SetTask(true, "Task_W_3_15");
    }

    public static bool Block8On()
    {
      return DiscreteOutputs.Instance().SetOutput("Block_8", true);
    }

    public static bool Block8Off()
    {
      return DiscreteOutputs.Instance().SetOutput("Block_8", false);
    }

    public static bool DVSOn()
    {
      return DiscreteOutputs.Instance().SetOutput("Task_1_32", true);
    }

    public static bool DVSOff()
    {
      return DiscreteOutputs.Instance().SetOutput("Task_1_32", false);
    }

    internal static bool SetStatorX1()
    {
      return SetReferencedReles(6);
    }

    internal static bool SetStatorX2()
    {
      return SetReferencedReles(7);
    }

    internal static bool SetStatorZ1()
    {
      return SetReferencedReles(8);
    }

    internal static bool SetStatorZ2()
    {
      return SetReferencedReles(9);
    }

    internal static bool SetSAU_Alfa()
    {
      return SetReferencedReles(3);
    }

    internal static bool SetSAU_Wxr()
    {
      return SetReferencedReles(4);
    }

    internal static bool SetSAU_Wzr()
    {
      return SetReferencedReles(5);
    }

    /// <summary>
    /// Включить Тангаж
    /// </summary>
    /// <returns></returns>
    internal static bool SetHeel()
    {
      try
      {
        DiscreteOutputs.Instance().SetOutputEx("Pitch", false);
        DiscreteOutputs.Instance().SetOutputEx("Heel", true);
      }
      catch
      {
        return false;
      }
      return true;
    }

    /// <summary>
    /// Включить Крен
    /// </summary>
    /// <returns></returns>
    internal static bool SetPitch()
    {
      try
      {
        DiscreteOutputs.Instance().SetOutputEx("Pitch", true);
        DiscreteOutputs.Instance().SetOutputEx("Heel", false);
      }
      catch
      {
        return false;
      }
      return true;
    }

    private static bool SetReferencedReles(int numRele)
    {
      Hashtable ht = new Hashtable();
      for (int i= 3;i<=9;i++)
        ht.Add(i, i == numRele);
      return DO176X.Instance.SetOutputs(ht);
    }

    /// <summary>
    /// Включает эквивалент вместо антенны
    /// </summary>
    /// <param name="state">True - система работает на эквивалент, False - на антенну</param>
    /// <returns></returns>
    public static bool SetEquivalent(bool state)
    {
      return DiscreteOutputs.Instance().SetOutput("Equivalent", state);
    }

    public static bool SetAngle(double angle)
    {
      return GenSinCos.Instance().GeneratorOn(NormalizeAngle(angle));
    }

    public static void SetAngleEx(double angle)
    {
        GenSinCos.Instance().GeneratorOnEx(NormalizeAngle(angle));
    }

    public static ushort NormalizeAngle(double angle)
    {
        angle = 180 - angle;
        while (angle >= 360)
            angle -= 360;
        while (angle < 0)
            angle += 360;
        return (ushort)angle;
    }
  }
}
