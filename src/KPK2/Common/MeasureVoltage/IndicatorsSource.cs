using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KPK2.Common.MeasureVoltage
{
  public class IndicatorsSource :ISource
  {
    private readonly List<VoltageValues> _list = new List<VoltageValues>();

    public IEnumerable<VoltageValues> List {
        get { return _list; }
    }

    public void Add(VoltageValues voltages)
    {
      _list.Add(voltages);
    }
  }
}
