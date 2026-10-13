namespace Sparc.KPK2.NFunctions
{
    /// <summary>
    /// Методы расширений
    /// </summary>
    public static class Extensions
    {
        private const int IntBitsCount = 8 * sizeof(int);

        /// <summary>
        /// Реверсирование порядка бит в заданной зоне
        /// </summary>
        /// <param name="value">Исходное значение</param>
        /// <param name="offset">Смещение до начала реверсируемой зоны</param>
        /// <param name="count">Количество реверсируемых бит</param>
        /// <returns>Результат с реверсированной зоной бит</returns>
        public static int ReverseBits(this int value, int offset = 0, int count = IntBitsCount)
        {
            if (offset < 0 || offset > IntBitsCount - 2 || count < 2)
                return value;

            if (offset + count > IntBitsCount)
                count = IntBitsCount - offset;

            var reversed = 0;
            var direct = value >> offset;

            for (var i = 0; i < count; i++)
            {
                reversed = (reversed << 1) | (direct & 1);
                direct >>= 1;
            }

            if (count >= IntBitsCount)
                return reversed;

            var cleaningMask = ~(((1 << count) - 1) << offset);
            var result = (value & cleaningMask) | (reversed << offset);
            return result;
        }
    }
}
