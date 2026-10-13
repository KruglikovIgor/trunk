using System;

namespace KPK2.MyClasse
{
    public static class BitWork
    {
        /// <summary>
        /// Развернуть первые 4 бита.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static int Revert4Bits(int value)
        {
            int newValue = 0;
            const int bitCount = 4;
            for (int i = 0; i < bitCount; i++)
            {
                bool bit = GetBit(i, value);
                newValue = SetBit(bitCount - i - 1, bit, newValue);
            }

            return newValue;
        }

        public static int SetBit(int bitCount, bool bit, int newValue)
        {
            if (bit)
            {
                var prepValue = 1 << bitCount;
                newValue |= prepValue;
            }
            else
            {
                var prepValue = 1 << bitCount;
                prepValue = ~prepValue;
                newValue &= prepValue;
            }

            return newValue;
        }

        public static bool GetBit(int i, int value)
        {
            int checkValue = (int)Math.Pow(2, i);
            return (value & checkValue) == checkValue;
        }
    }
}
