namespace MasterChief.DotNet4.Utilities.Common
{
    using System.Collections;
    using System.Text;

    /// <summary>
    /// BitArray 帮助类
    /// </summary>
    public static class BitArrayHelper
    {
        #region Methods

        /// <summary>
        /// 逆序
        /// </summary>
        /// <param name="array">BitArray</param>
        /// <returns>逆序后的BitArray</returns>
        public static BitArray Reverse(this BitArray array)
        {
            int count = array.Length;
            int middle = count / 2;

            for (int i = 0; i < middle; i++)
            {
                bool tmp = array[i];
                array[i] = array[count - i - 1];
                array[count - i - 1] = tmp;
            }

            return array;
        }

        /// <summary>
        /// 转换成十六进制字符串
        /// </summary>
        /// <param name="array">需要操作的BitArray</param>
        /// <param name="trueValue">当条件成立的值</param>
        /// <param name="falseValue">当条件不成立的值</param>
        /// <returns>十六进制字符串</returns>
        public static string ToBinaryString(this BitArray array, char trueValue, char falseValue)
        {
            var builder = new StringBuilder();

            for (int i = 0; i < array.Length; i++)
            {
                if (array[i])
                {
                    builder.Append(trueValue);
                }
                else
                {
                    builder.Append(falseValue);
                }
            }

            return builder.ToString();
        }

        /// <summary>
        /// 转成是十六进制字符串
        /// </summary>
        /// <param name="array">需要操的在BitArray</param>
        /// <returns>十六进制字符串</returns>
        public static string ToBinaryString(this BitArray array)
        {
            return array.ToBinaryString('1', '0');
        }

        /// <summary>
        /// 转换为byte
        /// </summary>
        /// <param name="array">需要操的在BitArray</param>
        /// <returns>byte</returns>
        /// 时间：2016/6/7 15:21
        /// 备注：
        public static byte ToByte(this BitArray array)
        {
            return array.ToBytes()[0];
        }

        /// <summary>
        /// 转成成byte数组
        /// </summary>
        /// <param name="array">需要操的在BitArray</param>
        /// <returns>byte数组</returns>
        public static byte[] ToBytes(this BitArray array)
        {
            byte[] data = new byte[((array.Length - 1) / 8) + 1];
            array.CopyTo(data, 0);
            return data;
        }

        #endregion Methods
    }
}