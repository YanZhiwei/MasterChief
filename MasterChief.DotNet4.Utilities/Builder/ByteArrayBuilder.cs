namespace MasterChief.DotNet4.Utilities.Builder
{
    using Common;
    using System;
    using System.IO;

    /// <summary>
    /// Byte数组构建器
    /// </summary>
    public class ByteArrayBuilder : IDisposable
    {
        /// <summary>
        /// False
        /// </summary>
        private const byte streamFalse = 0;

        /// <summary>
        /// True
        /// </summary>
        private const byte streamTrue = 1;

        /// <summary>
        /// MemoryStream
        /// </summary>
        private MemoryStream _store = new MemoryStream();

        /// <summary>
        /// Initializes a new instance of the <see cref="ByteArrayBuilder"/> class.
        /// </summary>
        public ByteArrayBuilder()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ByteArrayBuilder"/> class.
        /// </summary>
        /// <param name="data">初始化byte数组</param>
        public ByteArrayBuilder(byte[] data)
        {
            _store.Close();
            _store.Dispose();
            _store = new MemoryStream(data);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ByteArrayBuilder"/> class.
        /// </summary>
        /// <param name="base64">base64字符串</param>
        public ByteArrayBuilder(string base64)
        {
            _store.Close();
            _store.Dispose();
            _store = new MemoryStream(Convert.FromBase64String(base64));
        }

        /// <summary>
        /// Gets the Length
        /// 当前Byte数组长度
        /// </summary>
        public int Length => (int)_store.Length;

        /// <summary>
        /// 附加byte数组类型数值
        /// </summary>
        /// <param name="b">byte数组类型数值</param>
        public void Append(byte[] b)
        {
            _store.Write(b, 0, b.Length);
        }

        /// <summary>
        /// 附加一个bool数值
        /// </summary>
        /// <param name="b">bool数值</param>
        public void Append(bool b)
        {
            _store.WriteByte(b ? streamTrue : streamFalse);
        }

        /// <summary>
        /// 附加byte类型数值
        /// </summary>
        /// <param name="b">byte类型数值</param>
        public void Append(byte b)
        {
            _store.WriteByte(b);
        }

        /// <summary>
        /// 附加int类型数值
        /// </summary>
        /// <param name="i">int类型数值</param>
        public void Append(int i)
        {
            Append(BitConverter.GetBytes(i));
        }

        /// <summary>
        /// 附加long类型数值
        /// </summary>
        /// <param name="l">long类型数值</param>
        public void Append(long l)
        {
            Append(BitConverter.GetBytes(l));
        }

        /// <summary>
        /// 附加int类型数值
        /// </summary>
        /// <param name="i">int类型数值</param>
        public void Append(short i)
        {
            Append(BitConverter.GetBytes(i));
        }

        /// <summary>
        /// 附加uint类型数值
        /// </summary>
        /// <param name="ui">uint类型数值</param>
        public void Append(uint ui)
        {
            Append(BitConverter.GetBytes(ui));
        }

        /// <summary>
        /// 附加ulong类型数值
        /// </summary>
        /// <param name="ul">ulong类型数值</param>
        public void Append(ulong ul)
        {
            Append(BitConverter.GetBytes(ul));
        }

        /// <summary>
        /// 附加ushort类型数值
        /// </summary>
        /// <param name="us">ushort类型数值</param>
        public void Append(ushort us)
        {
            Append(BitConverter.GetBytes(us));
        }

        /// <summary>
        /// 清除
        /// </summary>
        public void Clear()
        {
            _store.Close();
            _store.Dispose();
            _store = new MemoryStream();
        }

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        public void Dispose()
        {
            _store.Close();
            _store.Dispose();
        }

        /// <summary>
        /// 转换为数组
        /// </summary>
        /// <returns>数组</returns>
        public byte[] ToArray()
        {
            byte[] data = new byte[Length];
            Array.Copy(_store.GetBuffer(), data, Length);
            return data;
        }

        /// <summary>
        /// 返回带空格的十六进制字符串
        /// </summary>
        /// <returns>The <see cref="string"/></returns>
        public override string ToString()
        {
            return ByteHelper.ToHexStringWithBlank(ToArray());
        }
    }
}