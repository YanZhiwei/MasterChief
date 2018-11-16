namespace MasterChief.DotNet4.Utilities.Common
{
    /// <summary>
    /// Bit 帮助类
    /// </summary>
    public static class BitHelper
    {
        #region Methods

        /// <summary>
        /// Clears the bit.
        /// <para>eg:ByteHelper.ClearBit(24, 4);==>3</para>
        /// </summary>
        /// <param name="data">The data.</param>
        /// <param name="index">The index.</param>
        /// <returns></returns>
        public static byte Clear(this byte data, int index)
        {
            return (byte)(data & (byte.MaxValue - (1 << index)));
        }

        /// <summary>
        /// Gets the bit.
        /// <para>eg:ByteHelper.GetBit(8,3);==>1</para>
        /// </summary>
        /// <param name="data">The data.</param>
        /// <param name="index">The index.</param>
        /// <returns></returns>
        public static int Get(this byte data, int index)
        {
            return ((data & (1 << index)) > 0) ? 1 : 0;
        }

        /// <summary>
        /// Reverses the bit.
        /// <para>eg:ByteHelper.ReverseBit(24, 4);==>8</para>
        /// </summary>
        /// <param name="data">The data.</param>
        /// <param name="index">The index.</param>
        /// <returns></returns>
        public static byte Reverse(this byte data, int index)
        {
            return (byte)(data ^ (byte)(1 << index));
        }

        /// <summary>
        /// Sets the bit.
        ///<para>eg: ByteHelper.SetBit(8, 4);==>24</para>
        /// </summary>
        /// <param name="data">The data.</param>
        /// <param name="index">The index.</param>
        /// <returns></returns>
        public static byte Set(this byte data, int index)
        {
            return (byte)(data | (1 << index));
        }

        #endregion Methods
    }
}