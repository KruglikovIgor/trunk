using System;
using System.Collections.Generic;

namespace Sparc.KPK2.Hardware.PS115V
{
    public class PS115DeviceSelector
    {
        private List<IPower115V> _deviceList = new List<IPower115V>();

        public void Add(IPower115V item)
        {
            _deviceList.Add(item);
        }

        public IPower115V GetLastOrConnected()
        {
            foreach (var ps in _deviceList)
            {
                ps.Connect();
                if (ps.IsConnected())
                    return ps;
            }

            return _deviceList[_deviceList.Count-1];
        }
    }
}
