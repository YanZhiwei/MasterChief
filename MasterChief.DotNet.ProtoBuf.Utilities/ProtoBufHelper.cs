namespace MasterChief.DotNet.ProtoBuf.Utilities
{
    using global::ProtoBuf;
    using MasterChief.DotNet4.Utilities.Operator;
    using System.IO;

    /// <summary>
    /// 利用ProtoBuf序列化与反序列化对象
    /// </summary>
    public static class ProtoBufHelper
    {
        #region Methods

        /// <summary>
        /// 反序列化对象
        /// </summary>
        /// <param name="data">二进制流</param>
        /// <returns>对象</returns>
        public static T Deserialize<T>(byte[] data)
        {
            ValidateOperator.Begin().NotNull(data, "需要反序列化二进制流");

            using (MemoryStream stream = new MemoryStream(data))
            {
                return Serializer.Deserialize<T>(stream);
            }
        }

        /// <summary>
        /// 反序列化BIN文件
        /// </summary>
        /// <param name="path">BIN文件路径</param>
        /// <returns>对象</returns>
        public static T Deserialize<T>(string path)
        {
            ValidateOperator.Begin().IsFilePath(path).CheckFileExists(path);

            using (FileStream file = File.OpenRead(path))
            {
                return Serializer.Deserialize<T>(file);
            }
        }

        /// <summary>
        /// 序列化二进制流
        /// </summary>
        /// <param name="value">需要序列化对象</param>
        /// <returns>二进制流</returns>
        public static byte[] Serialize(object value)
        {
            ValidateOperator.Begin().NotNull(value, "需要序列化对象");
            byte[] buffer = null;

            using (MemoryStream stream = new MemoryStream())
            {
                Serializer.Serialize(stream, value);
                buffer = stream.ToArray();
            }

            return buffer;
        }

        /// <summary>
        /// 序列化成BIN文件
        /// </summary>
        /// <param name="value">需要序列化对象</param>
        /// <param name="path">bin文件存储路径</param>
        public static void Serialize<T>(T value, string path)
        {
            ValidateOperator.Begin().NotNull(value, "需要序列化对象").IsFilePath(path);

            using (FileStream file = File.Create(path))
            {
                Serializer.Serialize<T>(file, value);
            }
        }

        #endregion Methods
    }
}