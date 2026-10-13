using System;
using System.Collections.Generic;
using System.Data;
using NUnit.Framework;
using Sparc.KPK2.NFunctions;

namespace Sparc.KPK2.Tests
{
    [TestFixture]
    public class MainTests
    {
      private const int numColIdealValue = 3;
      private const int numColValue = 4;
      private const int baseFreq = 3000;
      private const int secondCheckFreq = 400;

      private double[] _answers = new double[360];

      private const int angleIncrease = 1;

      [Test]
      public void TestCorrectAngle()
      {
        FillAnswers();

        for (int angle =0; angle<360; angle += angleIncrease)
          Assert.AreEqual(Math.Round(_answers[angle/angleIncrease],3), Math.Round(CorrectPhase(1, baseFreq, angle),3));
      }

      [Test]
      public void TestCorrectAngleWoTable()
      {
        FillAnswers();

        for (int angle = 0; angle < 360; angle += angleIncrease)
          Assert.AreEqual(angle, Math.Round(CalibrateFunctions.CorrectPhase(new List<CalibratePoint>(), baseFreq, angle, CorrectPhaseByFreq), 3));
      }

      [Test]
      public void TestCorrectAngleOtherFreq()
      {
        FillAnswers();

        for (int angle = 0; angle < 360; angle += angleIncrease)
        {
          double exResult = _answers[angle / angleIncrease];
          if (Math.Abs(exResult - angle) > 180)
          {
            if (exResult > angle)
              exResult -= 360;
            else
              angle -= 360;
          }
          exResult = (exResult - angle) / baseFreq * secondCheckFreq + angle;
          Assert.AreEqual(Math.Round(exResult,3), Math.Round(CorrectPhase(1, secondCheckFreq, angle), 3));
        }
      }

      private void FillAnswers()
      {
        double less90Koef = 90 / 95d;
        double less90B = 9.4736842105;
        double less180Koef = 1;
        double less180B = 5;
        double less270Koef = 90/96d;
        double less270B = 15.9375;
        double less350Koef = 90/79d;
        double less350B = -38.73417721518;

        for (int angle = 0; angle < 360; angle += angleIncrease)
        {
          if (angle <= 85)
            _answers[angle / angleIncrease] = angle * less90Koef + less90B;
          else
            if (angle <= 175)
              _answers[angle / angleIncrease] = angle * less180Koef + less180B;
            else
              if (angle <= 271)
                _answers[angle / angleIncrease] = angle * less270Koef + less270B;
              else
              if (angle <= 350)
                _answers[angle / angleIncrease] = angle * less350Koef + less350B;
              else
                  _answers[angle / angleIncrease] = angle * less90Koef + less90B;
        }
      }

      public const double AngleSaveKoef = 10;

      private double CorrectPhase(int channel, double freq, double phase)
      {
        if (channel == 0)
          return phase; //0 это опорный канал, для него фаза не корректируется

        DataTable dtPhaseCalibration = GetChannelCalibration(channel);

        List<CalibratePoint> calPoints = new List<CalibratePoint>();
        for (int i = 0; i < dtPhaseCalibration.Rows.Count; i++)
          calPoints.Add(new CalibratePoint(Convert.ToDouble(dtPhaseCalibration.Rows[i][numColIdealValue]) / AngleSaveKoef, Convert.ToDouble(dtPhaseCalibration.Rows[i][numColValue]) / AngleSaveKoef));

        if (calPoints.Count == 0)
          return phase;

        return CalibrateFunctions.CorrectPhase(calPoints, freq, phase, CorrectPhaseByFreq);
      }

      private DataTable GetChannelCalibration(int channel)
      {
        DataTable dtResult = new DataTable();
        for (int i=0;i<5;i++)
          dtResult.Columns.Add();
        dtResult.Rows.Add(0, 0, 0, 3500, 0);
        dtResult.Rows.Add(0, 0, 0, 850, 900);
        dtResult.Rows.Add(0, 0, 0, 1750, 1800);
        dtResult.Rows.Add(0, 0, 0, 2710, 2700);
        return dtResult;
      }

      /// <summary>
      /// Коррекитровка фазы в зависимости от частоты
      /// </summary>
      /// <param name="freq">частота сигнала</param>
      /// <param name="phase">фаза</param>
      /// <param name="corrPhase">скорректированная фаза (по таблице калибровки)</param>
      /// <returns></returns>
      private double CorrectPhaseByFreq(double freq, double phase, double corrPhase)
      {
        const int idealFreq = 3000;
        corrPhase = Globals.NormalizeAngle(corrPhase);
        if (Math.Abs(corrPhase - phase) > 180)
        {
          if (corrPhase > phase)
            corrPhase -= 360;
          else
            phase -= 360;
        }
        double result = (corrPhase - phase) / idealFreq * freq + phase;
        result = Globals.NormalizeAngle(result);
        return result;
      }
    }
}
