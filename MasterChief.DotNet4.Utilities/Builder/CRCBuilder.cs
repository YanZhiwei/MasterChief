namespace MasterChief.DotNet4.Utilities.Builder
{
    using System;

    /// <summary>
    /// CRC 辅助类
    /// </summary>
    /// 时间：2016/11/23 15:43
    /// 备注：
    public static class CrcBuilder
    {
        #region Methods

        /// <summary>
        /// CRC-16 MODBUS实现
        /// 冗余循环校验码含2个字节
        /// </summary>
        /// <param name="data">需要计算得数组</param>
        /// <returns>CRC数值</returns>
        /// 时间：2016/11/23 15:43
        /// 备注：
        public static ushort Calu16Modbus(byte[] data)
        {
            ushort ax = 0xFFFF;
            ushort lsb;

            for (int i = 0; i < data.Length; i++)
            {
                ax ^= data[i];

                for (int j = 0; j < 8; j++)
                {
                    lsb = Convert.ToUInt16(ax & 0x0001);
                    ax = Convert.ToUInt16(ax >> 1);

                    if (lsb != 0)
                    {
                        ax ^= 0xA001;
                    }
                }
            }

            return ax;
        }

        /// <summary>
        /// CRC 累加和计算
        /// </summary>
        /// <param name="data">需要计算得数组</param>
        /// <returns>CRC数值</returns>
        public static byte CaluSum(byte[] data)
        {
            byte cal;
            uint totol = 0;
            foreach (byte item in data)
            {
                totol += item;
            }

            unchecked
            {
                cal = (byte)totol;
            }

            return cal;
        }

        #endregion Methods
    }
}