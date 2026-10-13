using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Sparc.KPK2.Hardware.LTR;

namespace Sparc.KPK2.Tests
{
    [TestFixture]
    public class CorrectRangeLTRMeasureTest
    {
        [Test]
        public void TestUnCorrectRangeLTR22()
        {
            double[] signal = { 1, 1.5, 2, 1.2 };
            double[] ranges = { 1, 10, 3, 0.1, 0.3, 0.03 };
            CorrectRangeLTRMeasure helper = new CorrectRangeLTRMeasure(ranges, 10);

            var res = helper.CheckCorrectRange(signal);

            Assert.AreEqual(false, res);
            Assert.AreEqual(3, helper.Range);
        }

        [Test]
        public void TestCorrectRangeLTR22()
        {
            double[] signal = { 1, 1.5, 2, 1.2, 2.2, 2.5, 2.9, 3.6 };
            double[] ranges = { 1, 10, 3, 0.1, 0.3, 0.03 };
            CorrectRangeLTRMeasure helper = new CorrectRangeLTRMeasure(ranges, 10);

            var res = helper.CheckCorrectRange(signal);

            Assert.AreEqual(true, res);
            Assert.AreEqual(10, helper.Range);
        }

        [Test]
        public void TestUnCorrectRangeLTR11()
        {
            double[] signal = { 1, 1.5, 2, 1.2 };
            double[] ranges = { 10,2.5,0.15,0.6 };
            CorrectRangeLTRMeasure helper = new CorrectRangeLTRMeasure(ranges, 2.5);

            var res = helper.CheckCorrectRange(signal);

            Assert.AreEqual(false, res);
            Assert.AreEqual(10, helper.Range);
        }

        [Test]
        public void TestCorrectRangeLTR11()
        {
            double[] signal = { 1, 1.5, 2, 1.2 };
            double[] ranges = { 10, 2.5, 0.15, 0.6 };
            CorrectRangeLTRMeasure helper = new CorrectRangeLTRMeasure(ranges, 10);

            var res = helper.CheckCorrectRange(signal);

            Assert.AreEqual(true, res);
            Assert.AreEqual(10, helper.Range);
        }

        [Test]
        public void TestUnCorrectRangeLTR114()
        {
            double[] signal = { 1, 1.1, 1.3, 1.2 };
            double[] ranges = { 10, 0.4, 2 };
            CorrectRangeLTRMeasure helper = new CorrectRangeLTRMeasure(ranges, 10);

            var res = helper.CheckCorrectRange(signal);

            Assert.AreEqual(false, res);
            Assert.AreEqual(2, helper.Range);
        }

        [Test]
        public void TestCorrectRangeLTR114()
        {
            double[] signal = { 1, 1.1, 1.3, 1.2 };
            double[] ranges = { 10, 0.4, 2 };
            CorrectRangeLTRMeasure helper = new CorrectRangeLTRMeasure(ranges, 2);

            var res = helper.CheckCorrectRange(signal);

            Assert.AreEqual(true, res);
            Assert.AreEqual(2, helper.Range);
        }
    }
    
}
