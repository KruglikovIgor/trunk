using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Authentication.ExtendedProtection;
using System.Text;
using KPK2.Common.MeasureVoltage;
using KPK2.GeneralForms;
using KPK2.GeneralForms.ServiceForm;
using NUnit.Framework;
using Sparc.Hardware.NAkip1142;

namespace Sparc.KPK2.Tests
{
  public class LoadMeasureVoltageTest
  {
    [Test]
    public void LoadTest()
    {
      var form = new frmMeasVoltage(new MeasuredChannel() {Name = "Wx ПКВ", Code = "Wx_PKV"});
      form.ShowDialog();
      Assert.DoesNotThrow(() => form.ShowDialog());
    }
  }
}
