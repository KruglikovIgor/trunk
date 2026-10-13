using System;
using System.Collections.Generic;
using System.Text;

namespace Sparc.KPK2.NFunctions
{
  public static class CalibrateFunctions
  {
    public delegate double CorrectPhaseByFreqDelegate(double freq, double phase, double corrPhase);

    /// <summary>
    /// Вычисляет максимальную и минимальную точку т.е. интервал в который попадает фаза для дальнейшей апроксимации
    /// </summary>
    /// <param name="points">Точки калибровки первая - значение с прибора, вторая - заданное значение</param>
    /// <param name="phase">Значение фазы для калибровки</param>
    /// <param name="maxIdealValue">верхняя граница интервала с прибора</param>
    /// <param name="maxRealValue">верхняя граница интервала заданное значение</param>
    /// <param name="minIdealValue">нижняя граница интервала с прибора</param>
    /// <param name="minRealValue">нижняя граница интервала заданное значение</param>
    public static void GetMinAndMaxPointByPhase(List<CalibratePoint> points, double phase, ref CalibratePoint maxValue, ref CalibratePoint minValue)
    {
      if (points.Count == 0)
        return;

      for (int i = 0; i < points.Count; i++) //Выбираем максимальную и минимальную точку математически
      {
        double curIdealValue = Convert.ToDouble(points[i].Ideal);
        if (curIdealValue >= phase)
          if (double.IsNaN(maxValue.Ideal) || curIdealValue < maxValue.Ideal)
          {
            maxValue.Ideal = curIdealValue;
            maxValue.Real = Convert.ToInt32(points[i].Real);
          }

        if (curIdealValue < phase)
          if (double.IsNaN(minValue.Ideal) || curIdealValue > minValue.Ideal)
          {
            minValue.Ideal = curIdealValue;
            minValue.Real = Convert.ToInt32(points[i].Real);
          }
      }
      

      if (double.IsNaN(minValue.Ideal) || double.IsNaN(maxValue.Ideal)) //Если максимальная или минимальная точка не найдена, то используем "круговую зависимость" и вместо минимальной точки берём максимальную, а вместо максимальной - минимальную
      {
        if (double.IsNaN(minValue.Ideal))
        {
          minValue.Ideal = Convert.ToDouble(points[0].Ideal);
          minValue.Real = Convert.ToInt32(points[0].Real);
          for (int i = 1; i < points.Count; i++)
          {
            double curIdealValue = Convert.ToDouble(points[i].Ideal);
            if (curIdealValue > minValue.Ideal)
            {
              minValue.Ideal = curIdealValue;
              minValue.Real = Convert.ToInt32(points[i].Real);
            }
          }
        }

        if (double.IsNaN(maxValue.Ideal))
        {
          maxValue.Ideal = Convert.ToDouble(points[0].Ideal);
          maxValue.Real = Convert.ToInt32(points[0].Real);
          for (int i = 1; i < points.Count; i++)
          {
            double curIdealValue = Convert.ToDouble(points[i].Ideal);
            if (curIdealValue < maxValue.Ideal)
            {
              maxValue.Ideal = curIdealValue;
              maxValue.Real = Convert.ToInt32(points[i].Real);
            }
          }
        }
      }
    }

    public static double CorrectPhase(List<CalibratePoint> calPoints, double freq, double phase, CalibrateFunctions.CorrectPhaseByFreqDelegate phaseByFreq)
    {
      phase = Globals.NormalizeAngle(phase);
      double corrPhase;
      if (calPoints.Count == 0)
        return phase;
      if (calPoints.Count == 1)
      {
        corrPhase = calPoints[0].Real / calPoints[0].Ideal * phase;
        if (phaseByFreq != null)
          corrPhase = phaseByFreq(freq, phase, corrPhase);
        return corrPhase;
      }

      if (calPoints.Count == 2)
      {
        int first = 0, second = 1;
        double ideal1 = calPoints[first].Ideal;
        double ideal2 = calPoints[second].Ideal;
        if (ideal1 > ideal2)
        {
          first = 1;
          second = 0;
        }
        ideal1 = calPoints[first].Ideal;
        ideal2 = calPoints[second].Ideal;
        double value1 = calPoints[first].Real;
        double value2 = calPoints[second].Real;

        double k = (value2 - value1) / (ideal2 - ideal1);
        double b = value1 - k * ideal1;
        corrPhase = k * phase + b;
        if (phaseByFreq != null)
          corrPhase = phaseByFreq(freq, phase, corrPhase);
        return corrPhase;
      }

      CalibratePoint minValue = new CalibratePoint();
      CalibratePoint maxValue = new CalibratePoint();

      GetMinAndMaxPointByPhase(calPoints, phase, ref maxValue, ref minValue);

      double bKoef = 0;
      double kKoef = Globals.DiffAngleMin(minValue.Real, maxValue.Real) / Globals.DiffAngleMin(minValue.Ideal, maxValue.Ideal);
      if (Math.Abs(maxValue.Real - phase) < Math.Abs(minValue.Real - phase)) //Рассчитываем коэф B по точке, которая дальше от 0
      {
        if (Math.Abs(maxValue.Real - maxValue.Ideal) > 180)
        {
          if (maxValue.Real > maxValue.Ideal)
            maxValue.Real -= 360;
          else
            maxValue.Ideal -= 360;
        }
        bKoef = maxValue.Real - kKoef * maxValue.Ideal;
      }
      else
      {
        if (Math.Abs(minValue.Real - minValue.Ideal) > 180)
        {
          if (minValue.Real > minValue.Ideal)
            minValue.Real -= 360;
          else
            minValue.Ideal -= 360;
        }
        bKoef = minValue.Real - kKoef * minValue.Ideal;
      }

      corrPhase = kKoef * phase + bKoef;
      if (phaseByFreq != null)
        corrPhase = phaseByFreq(freq, phase, corrPhase);

      corrPhase = Globals.NormalizeAngle(corrPhase);
      return corrPhase;
    }

  }
}
