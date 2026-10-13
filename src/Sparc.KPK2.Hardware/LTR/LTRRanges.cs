using System.Collections.Generic;
using LCards;

namespace Sparc.KPK2.Hardware.LTR
{
    /// <summary>
    /// Класс с словорями для диапазонов
    /// </summary>
    public  static class LTRRanges
    {
        /// <summary>
        /// Словарь для диапазонов LTR11
        /// </summary>
        public static Dictionary<LTRCards.LTR11.ChannelRange, double> LTR11Ranges = new Dictionary<LTRCards.LTR11.ChannelRange, double>()
         {
             {LTRCards.LTR11.ChannelRange.TenV, 10 },
             {LTRCards.LTR11.ChannelRange.TwoPFiveV, 2.5 },
             {LTRCards.LTR11.ChannelRange.ZeroPOneFiveV, 0.15 },
             {LTRCards.LTR11.ChannelRange.ZeroPSixV, 0.6 }
         };

        /// <summary>
        /// Словарь для диапазонов LTR22
        /// </summary>
        public static Dictionary<LTRCards.LTR22.ChannelRange, double> LTR22Ranges = new Dictionary<LTRCards.LTR22.ChannelRange, double>()
        {
            {LTRCards.LTR22.ChannelRange.One, 1 },
            {LTRCards.LTR22.ChannelRange.Ten, 10 },
            {LTRCards.LTR22.ChannelRange.Three, 3 },
            {LTRCards.LTR22.ChannelRange.ZeroPOne, 0.1 },
            {LTRCards.LTR22.ChannelRange.ZeroPThree, 0.3 },
            { LTRCards.LTR22.ChannelRange.ZeroPZeroThree, 0.03}

        };

        /// <summary>
        /// Словарь для диапазонов LTR114
        /// </summary>
        public static Dictionary<LTRCards.LTR114.ChannelRange, double> LTR114Ranges = new Dictionary<LTRCards.LTR114.ChannelRange, double>()
         {
             {LTRCards.LTR114.ChannelRange.U_2_R_1200, 2 },
             {LTRCards.LTR114.ChannelRange.U_04_R_4000, 0.4 },
             {LTRCards.LTR114.ChannelRange.U_10_R_400, 10 }

         };
    }
}
