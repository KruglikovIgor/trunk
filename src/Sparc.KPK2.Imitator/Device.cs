using System;

namespace Sparc.KPK2.Imitator
{
    public partial class Imitator
    {
        public abstract class Device : IDisposable
        {
            protected Device()
            {
                Hardware.HImitator.PowerSupply40.SetExtResistance(54);
                Hardware.HImitator.PowerSupply40.SetCurrentLimit(2);
                Hardware.HImitator.PowerSupply115.SetExtResistance(100);
                Hardware.HImitator.PowerSupply115.SetCurrentLimit(5);
            }

            public virtual void Dispose()
            {
                //DiscretInputs.ResetAll();
                //DAC.ResetAll();
                //ADC.ResetAll();
            }
        }
    }
}
