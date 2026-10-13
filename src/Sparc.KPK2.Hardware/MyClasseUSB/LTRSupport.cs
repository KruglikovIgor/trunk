using System;
using Sparc.KPK2.Config.Hardware;
using Sparc.KPK2.Config.Settings;
using Sparc.KPK2.Hardware.Enums;

namespace Sparc.KPK2.Hardware.MyClasseUSB
{
    public static class LTRSupport
    {
        public static uint GetLTRSlotFromConfig(string moduleName)
        {
            foreach (ModbusElement item in KPK2HardwareSettings.GetSettings().LTRUnits)
            {
                if (item.Name == moduleName)
                    return item.Address - (uint)1;
            }

            throw new Exception(string.Format("Incorrect module name: {0}. Module address not found!", moduleName));
        }

        public static uint GetLTRSlotFromConfigByNum(int slotNum)
        {
            uint address = 0;
            switch (slotNum)
            {
                case 11:
                    address = GetLTRSlotFromConfig("LTR114");
                    break;
                case 12:
                    address = GetLTRSlotFromConfig("LTR11_1");
                    break;
                case 13:
                    address = GetLTRSlotFromConfig("LTR11_2");
                    break;
                case 14:
                    address = GetLTRSlotFromConfig("LTR11_3");
                    break;
                case 15:
                    address = GetLTRSlotFromConfig("LTR22_1");
                    break;
                case 16:
                    address = GetLTRSlotFromConfig("LTR22_2");
                    break;
            }

            if(address == 0)
                throw new Exception(string.Format("Incorrect module name: {0}. Module address not found!", slotNum));

            return address;
        }
    }
}
