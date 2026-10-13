using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sparc.KPK2.Hardware.PS27V
{
    public class PS27DeviceSelector
    {
        private List<IPower27V> _deviceList = new List<IPower27V>();

        public void Add(IPower27V item)
        {
            _deviceList.Add(item);
        }

        public IPower27V GetLastOrConnected()
        {
            foreach (var ps in _deviceList)
            {
                if (ps.Init())
                    return ps;
            }

            return _deviceList[_deviceList.Count - 1];
        }
    }
}
