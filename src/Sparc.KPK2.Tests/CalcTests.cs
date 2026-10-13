using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Sparc.KPK2.NFunctions;

namespace Sparc.KPK2.Tests
{
    [TestFixture]
    public class CalcTests
    {
        [Test]

        public void TestRMS()
        {
            var sinPoints = GetSinPoints(10, 20, 78125, 78125);
            Assert.AreEqual(Math.Round(10/Math.Sqrt(2), 4), Math.Round(Functions.CalcRMS(sinPoints), 4));
        }

        private double[] GetSinPoints(int vpp, int frequency, int freqDiscrete, int pointsCount)
        {
            double[] result = new double[pointsCount];
            double signalT = 1d / frequency;
            double minPart = 1d / freqDiscrete;
            double angleStep = 2 * Math.PI / (signalT / minPart);
            double curAngle = 0;
            for (int i = 0; i < pointsCount; i++)
            {
                result[i] = vpp * Math.Sin(curAngle);
                curAngle += angleStep;
            }

            return result;
        }
        [Test]
        public void TestSpectrumCalculation()
        {
            double result = 0;
            while (!double.IsNaN(result))
            {
                int mainFreq = 8796;
                Random rnd = new Random(Environment.TickCount);
                int multip = 1000;
                double mainAmplitude = rnd.Next(4 * multip) / (double)multip;

                double mainFreqPlus1 = rnd.Next(4 * multip) / (double)multip;
                double mainFreqPlusAmp1 = rnd.Next(2 * multip) / (double)multip;

                double mainFreqPlus2 = rnd.Next(4 * multip) / (double)multip;
                double mainFreqPlusAmp2 = rnd.Next(2 * multip) / (double)multip;

                double mainFreqMinus1 = rnd.Next(4 * multip) / (double)multip;
                double mainFreqMinusAmp = rnd.Next(2 * multip) / (double)multip;

                double mainFreqMinus2 = rnd.Next(4 * multip) / (double)multip;
                double mainFreqMinusAmp2 = rnd.Next(2 * multip) / (double)multip;

                //double mainAmplitude = 0.27;

                //double mainFreqPlus1 = 3.603;
                //double mainFreqPlusAmp1 = 0.21;

                //double mainFreqPlus2 = 1.882;
                //double mainFreqPlusAmp2 = 1.672;

                //double mainFreqMinus1 = 3.632;
                //double mainFreqMinusAmp = 0.659;

                //double mainFreqMinus2 = 1.033;
                //double mainFreqMinusAmp2 = 0.08;

                SpectrumCalculation.PeakResult pResult = new SpectrumCalculation.PeakResult();
                pResult.MainFreq = new SpectrumCalculation.PeakPair(mainFreq, mainAmplitude);
                pResult.MainFreqPlus = new List<SpectrumCalculation.PeakPair>();
                pResult.MainFreqPlus.Add(new SpectrumCalculation.PeakPair(mainFreq + mainFreqPlus1,
                    mainAmplitude + mainFreqPlusAmp1));
                pResult.MainFreqPlus.Add(new SpectrumCalculation.PeakPair(mainFreq + mainFreqPlus1 + mainFreqPlus2,
                    mainAmplitude + mainFreqPlusAmp1 - mainFreqPlusAmp2));
                pResult.MainFreqMinus = new List<SpectrumCalculation.PeakPair>();
                pResult.MainFreqMinus.Add(new SpectrumCalculation.PeakPair(mainFreq - mainFreqMinus1,
                    mainAmplitude + mainFreqMinusAmp));
                pResult.MainFreqMinus.Add(new SpectrumCalculation.PeakPair(mainFreq - mainFreqMinus1 - mainFreqMinus2,
                    mainAmplitude + mainFreqMinusAmp - mainFreqMinusAmp2));
                result = SpectrumCalculation.CalculatePhaseIndex(pResult);
                if (double.IsNaN(result))
                    Console.WriteLine("Yes!");
            }

            Assert.AreEqual(double.NaN, result);
        }
    }
}
