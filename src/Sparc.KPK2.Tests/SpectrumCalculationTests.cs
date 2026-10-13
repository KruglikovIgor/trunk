using System;
using NUnit.Framework;
using Sparc.KPK2.NFunctions;

namespace Sparc.KPK2.Tests
{
  [TestFixture]
  public class SpectrumCalculationTests
  {
    [Test]
    public void ConvertAlfa12()
    {
      double m12 = SpectrumCalculation.ConvertToM12(3.5);
      Assert.AreEqual(2.1, Math.Round(m12,1));
    }

    [Test]
    public void ConvertAlfa10Low()
    {
      double m10 = SpectrumCalculation.ConvertToM10(14, true);
      Assert.AreEqual(2.2, Math.Round(m10, 1));
    }

    [Test]
    public void ConvertAlfa10High()
    {
      double m10 = SpectrumCalculation.ConvertToM10(14, false);
      Assert.AreEqual(2.6, Math.Round(m10, 1));
    }
  }
}
