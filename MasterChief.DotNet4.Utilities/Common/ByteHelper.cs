using System;
using System.Text;

namespace MasterChief.DotNet4.Utilities.Common
{
    /// <summary>
    /// Byte 辅助类
    /// </summary>
    public static class ByteHelper
    {
        #region Methods

        /// <summary>
        /// 将Byte换算成百分比
        ///<para>eg: Assert.AreEqual(50.20m, ByteHelper.CalcPercentage(128));</para>
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static decimal CalcPercentage(byte value)
        {
            return DecimalHelper.CalcPercentage(value, byte.MaxValue);
        }

        /// <summary>
        /// 两个byte合并
        /// <para>0x10,0x20==>0x1020</para>
        /// </summary>
        /// <param name="b1">第一个byte</param>
        /// <param name="b2">第二个byte</param>
        /// <returns>合并byte</returns>
        public static int Combine(byte b1, byte b2)
        {
            return b1 << 8 | b2;
        }

        /// <summary>
        /// 三个byte合并
        ///<para>0x10,0x20,0x30==>0x102030</para>
        /// </summary>
        /// <param name="b1">第一个byte</param>
        /// <param name="b2">第二个byte</param>
        /// <param name="b3">第三个byte</param>
        /// <returns></returns>
        public static int Combine(byte b1, byte b2, byte b3)
        {
            int combined = b1 << 8 | b2;
            combined = combined << 8 | b3;
            return combined;
        }

        /// <summary>
        /// 获取高位
        /// <para>eg: 0x0A==>10101101==>1010</para>
        /// <para>eg: Assert.AreEqual(0x0A, ByteHelper.GetHigh(0xAD));</para>
        /// </summary>
        /// <param name="data">The data.</param>
        /// <returns></returns>
        public static byte GetHigh(this byte data)
        {
            return (byte)(data >> 4);
        }

        /// <summary>
        /// 获取低位
        /// <para>eg: 0x0A==>10101101==>1101</para>
        /// <para>eg: Assert.AreEqual(0x0D, ByteHelper.GetLow(0xAD));</para>
        /// </summary>
        /// <param name="data">The data.</param>
        /// <returns></returns>
        public static byte GetLow(this byte data)
        {
            return (byte)(data & 0x0F);
        }

        /// <summary>
        ///将二进制字符串转换成byte
        ///<para>eg:Assert.AreEqual(0xFF, ByteHelper.ParseBinaryString("11111111"));</para>
        /// </summary>
        /// <param name="binaryString">二进制字符串</param>
        /// <returns></returns>
        public static byte ParseBinaryString(this string binaryString)
        {
            binaryString = binaryString.Replace(" ", "");
            return Convert.ToByte(binaryString, 2);
        }

        /// <summary>
        /// 将二进制字符串转换成byte数组
        ///<para> eg:  CollectionAssert.AreEqual(new byte[2] { 0xFF, 0xFF }, ByteHelper.ParseBinaryStringToBytes("1111111111111111"));
        ///</para>
        /// </summary>
        /// <param name="binaryString">The binary string.</param>
        /// <returns></returns>
        public static byte[] ParseBinaryStringToBytes(this string binaryString)
        {
            binaryString = binaryString.Replace(" ", "");
            int count = binaryString.Length / 8;
            byte[] array = new byte[count];

            for (int i = 0; i < count; ++i)
            {
                array[i] = Convert.ToByte(binaryString.Substring(8 * i, 8), 2);
            }

            return array;
        }

        /// <summary>
        /// 将十六进制字符串转换成byte数组
        /// <para>eg: CollectionAssert.AreEqual(new byte[2] { 0xFF, 0xFE }, ByteHelper.ParseHexString("FFFE"));</para>
        /// </summary>
        /// <param name="hexString">The hexadecimal string.</param>
        /// <returns></returns>
        public static byte[] ParseHexString(this string hexString)
        {
            hexString = hexString.Replace(" ", "");
            int count = hexString.Length;
            byte[] array = new byte[count / 2];

            for (int i = 0; i < count; i += 2)
            {
                array[i / 2] = Convert.ToByte(hexString.Substring(i, 2), 16);
            }

            return array;
        }

        /// <summary>
        /// 将带符号的十六进制字符串转成成Byte数组
        /// <para>
        /// eg: CollectionAssert.AreEqual(new byte[2] { 0xFF, 0xFE },
        ///     ByteHelper.ParseHexStringWithDelimiter("FF-FE", "-"));
        /// </para>
        /// </summary>
        /// <param name="hexString">The hexadecimal string.</param>
        /// <param name="delimiter">The delimiter.</param>
        /// <returns></returns>
        public static byte[] ParseHexStringWithDelimiter(this string hexString, string delimiter)
        {
            hexString = hexString.Replace(delimiter, "");
            byte[] array = new byte[hexString.Length / 2];

            for (int i = 0; i < hexString.Length; i += 2)
            {
                array[i / 2] = Convert.ToByte(hexString.Substring(i, 2), 16);
            }

            return array;
        }

        /// <summary>
        /// 将百分比转成byte
        /// <para>eg: Assert.AreEqual(128, ByteHelper.ParsePercent(50));</para>
        /// </summary>
        /// <param name="percent">The percent.</param>
        /// <returns></returns>
        public static byte ParsePercent(this ushort percent)
        {
            double actualPer = ((double)percent) / 100;
            double number = Math.Round(actualPer * byte.MaxValue, 0);
            string hexPercent = string.Format("{0:x}", (ushort)number).PadLeft(2, '0');
            return Convert.ToByte(hexPercent, 16);
        }

        /// <summary>
        /// 将byte数组转换成二进制字符串
        /// <para>
        /// eg: Assert.AreEqual("1111111111111111", ByteHelper.ToBinaryString(new byte[2] { 0xFF,
        ///     0xFF }));
        /// </para>
        /// </summary>
        /// <param name="data">The bytes.</param>
        /// <returns></returns>
        public static string ToBinaryString(this byte[] data)
        {
            string hexString = ToHexStringByLoop(data);
            return Convert.ToString(HexHelper.ToUInt(hexString), 2);
        }

        /// <summary>
        /// 将byte数组转换成二进制字符串
        ///<para>eg: Assert.AreEqual("11111111", ByteHelper.ToBinaryString(0xFF));</para>
        /// </summary>
        /// <param name="data">数组.</param>
        /// <returns>二进制字符串</returns>
        public static string ToBinaryString(this byte data)
        {
            return Convert.ToString(data, 2).PadLeft(8, '0');
        }

        /// <summary>
        /// 把Uint(2个字节)类型转化为字节数组（可高低位反转）
        ///  <para>eg: CollectionAssert.AreEqual(new byte[2] { 0x01, 0x02 }, ByteHelper.ToBytes(258));</para>
        /// </summary>
        /// <param name="data">UShort类型</param>
        /// <param name="reverse">是不是需要把得到的字节数组反转</param>
        /// <returns>Byte数组</returns>
        public static byte[] ToBytes(this ushort data, bool reverse)
        {
            byte[] array = BitConverter.GetBytes(data);

            if (reverse)
            {
                Array.Reverse(array);
            }

            return array;
        }

        /// <summary>
        ///把short(2个字节)类型转化为字节数组（可高低位反转）
        /// </summary>
        /// <param name="data">Short类型</param>
        /// <param name="reverse">是不是需要把得到的字节数组反转</param>
        /// <returns>Byte数组</returns>
        public static byte[] ToBytes(this short data, bool reverse)
        {
            byte[] array = BitConverter.GetBytes(data);

            if (reverse)
            {
                Array.Reverse(array);
            }

            return array;
        }

        /// <summary>
        /// 把uint转换成字节数组
        /// <para>uint _data = 255;</para>
        ///<para> CollectionAssert.AreEqual(new byte[4] { 0xff, 0x00, 0x00, 0x00 }, ByteHelper.ToBytes(_data, false));</para>
        /// <para> CollectionAssert.AreEqual(new byte[4] { 0x00, 0x00, 0x00, 0xff }, ByteHelper.ToBytes(_data, true));</para>
        /// </summary>
        /// <param name="value">uint</param>
        /// <param name="reverse">是不是需要把得到的字节数组反转</param>
        /// <returns>Byte数组</returns>
        /// 时间：2016/6/22 9:35
        /// 备注：
        public static byte[] ToBytes(this uint value, bool reverse)
        {
            byte[] array = BitConverter.GetBytes(value);

            if (reverse)
            {
                Array.Reverse(array);
            }

            return array;
        }

        /// <summary>
        /// 把ulong转换成字节数组
        /// <para> ulong _data2 = 255;</para>
        ///<para> CollectionAssert.AreEqual(new byte[8] { 0xff, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 }, ByteHelper.ToBytes(_data2, false));</para>
        /// <para> CollectionAssert.AreEqual(new byte[8] { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0xff }, ByteHelper.ToBytes(_data2, true));</para>
        /// </summary>
        /// <param name="value">ulong</param>
        /// <param name="reverse">if set to <c>true</c> [reverse].</param>
        /// <returns></returns>
        /// 时间：2016/6/22 9:46
        /// 备注：
        public static byte[] ToBytes(this ulong value, bool reverse)
        {
            byte[] array = BitConverter.GetBytes(value);

            if (reverse)
            {
                Array.Reverse(array);
            }

            return array;
        }

        /// <summary>
        /// 把ushort(2个字节)类型转化为字节数组
        ///  <para>eg: CollectionAssert.AreEqual(new byte[2] { 0x01, 0x02 }, ByteHelper.ToBytes(258));</para>
        /// </summary>
        /// <param name="value">UShort类型</param>
        /// <returns>Byte数组</returns>
        public static byte[] ToBytes(this ushort value)
        {
            return value.ToBytes(true);
        }

        /// <summary>
        /// 将byte转换成十六进制字符串
        /// </summary>
        /// <param name="data">The data.</param>
        /// <returns></returns>
        public static string ToHexString(this byte data)
        {
            return data.ToString("X2");
        }

        /// <summary>
        /// 将byte数组转换成十六进制字符串
        /// </summary>
        /// <para>
        /// eg: Assert.AreEqual("FFFE", ByteHelper.ToHexString(new byte[2] { 255, 254 }));
        /// </para>
        /// <param name="data">数组.</param>
        /// <returns>十六进制字符串</returns>
        /// 时间：2016-04-13 15:48
        /// 备注：
        public static string ToHexString(this byte[] data)
        {
            return ToHexStringByLoop(data);
        }

        /// <summary>
        /// 将byte数组转换成十六进制字符串
        /// <para>
        /// eg: Assert.AreEqual("FF FE", ByteHelper.ToHexStringWithBlank(new byte[2] { 255, 254 }));
        /// </para>
        /// </summary>
        /// <param name="data">数组</param>
        /// <returns>十六进制字符串</returns>
        public static string ToHexStringWithBlank(byte[] data)
        {
            string hexString = BitConverter.ToString(data);
            return hexString.Replace("-", " ");
        }

        /// <summary>
        ///将byte数组转换成带符号的十六进制字符串
        /// <para>
        /// eg: Assert.AreEqual("FF-FE", ByteHelper.ToHexStringWithDelimiter(new byte[2] { 255, 254
        ///     }, "-"));
        /// </para>
        /// </summary>
        /// <param name="bytes">The bytes.</param>
        /// <param name="delimiter">The delimiter.</param>
        /// <returns></returns>
        public static string ToHexStringWithDelimiter(byte[] bytes, string delimiter)
        {
            string hexString = string.Empty;

            if (bytes != null)
            {
                for (int i = 0; i < bytes.Length; i++)
                {
                    hexString += bytes[i].ToString("X2");

                    if (i != bytes.Length - 1)
                    {
                        hexString += delimiter;
                    }
                }
            }

            return hexString;
        }

        /// <summary>
        ///将Byte数组转换为INT32
        /// </summary>
        /// <param name="data">byte数组</param>
        /// <returns>INT32</returns>
        public static int ToInt32(byte[] data)
        {
            return BitConverter.ToInt32(data, 0);
        }

        /// <summary>
        ///将Byte数组转换为long
        /// </summary>
        /// <param name="data">byte数组</param>
        /// <returns>long</returns>
        public static long ToInt64(byte[] data)
        {
            return BitConverter.ToInt64(data, 0);
        }

        /// <summary>
        ///将Byte数组转换为UInt16
        /// </summary>
        /// <param name="data">byte数组</param>
        /// <returns>UInt16</returns>
        public static ushort ToUInt16(byte[] data)
        {
            return BitConverter.ToUInt16(data, 0);
        }

        /// <summary>
        ///将Byte数组转换Int16
        /// </summary>
        /// <param name="data">byte数组</param>
        /// <returns>UInt16</returns>
        public static short ToInt16(byte[] data)
        {
            return BitConverter.ToInt16(data, 0);
        }

        private static string ToHexStringByLoop(byte[] data)
        {
            StringBuilder builder = new StringBuilder(data.Length * 2);

            foreach (byte b in data)
            {
                builder.AppendFormat("{0:X2}", b);
            }

            return builder.ToString();
        }

        #endregion Methods
    }
}