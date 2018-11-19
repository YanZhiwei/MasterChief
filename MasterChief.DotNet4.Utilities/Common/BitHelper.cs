namespace MasterChief.DotNet4.Utilities.Common
{
    /// <summary>
    /// Bit 帮助类
    /// </summary>
    public static class BitHelper
    {
        #region Methods

        /// <summary>
        /// 按位清除
        /// <para>eg:ByteHelper.ClearBit(24, 4);==>3</para>
        /// </summary>
        /// <param name="data">Byte</param>
        /// <param name="index">索引</param>
        /// <returns>Byte</returns>
        public static byte Clear(this byte data, int index)
        {
            return (byte)(data & (byte.MaxValue - (1 << index)));
        }

        /// <summary>
        /// 按位获取
        /// <para>eg:ByteHelper.GetBit(8,3);==>1</para>
        /// </summary>
        /// <param name="data">Byte</param>
        /// <param name="index">索引</param>
        /// <returns>数值</returns>
        public static int Get(this byte data, int index)
        {
            return ((data & (1 << index)) > 0) ? 1 : 0;
        }

        /// <summary>
        /// 按位设置
        ///<para>eg: ByteHelper.SetBit(8, 4);==>24</para>
        /// </summary>
        /// <param name="data">Byte</param>
        /// <param name="index">索引</param>
        /// <returns>Byte</returns>
        public static byte Set(this byte data, int index)
        {
            return (byte)(data | (1 << index));
        }

        #endregion Methods
    }
}