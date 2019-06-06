using System;
using System.IO;
using MasterChief.DotNet.Infrastructure.Serializer;
using MasterChief.DotNet4.Utilities.Operator;

namespace MasterChief.DotNet.Infrastructure.ProtobufSerializer
{
    /// <summary>
    ///     基于Protobuf持久化实现
    /// </summary>
    public class ProtocolBufferSerializer : ISerializer
    {
        #region Methods

        /// <summary>
        ///     反序列化
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="data">需要反序列化字符串</param>
        /// <returns>反序列化</returns>
        public T Deserialize<T>(string data)
        {
            ValidateOperator.Begin().NotNullOrEmpty(data, "需要反序列化字符串");
            var buffer = Convert.FromBase64String(data);
            using (var stream = new MemoryStream(buffer))
            {
                return ProtoBuf.Serializer.Deserialize<T>(stream);
            }
        }

        /// <summary>
        ///     序列化
        /// </summary>
        /// <param name="serializeObject">需要序列化对象</param>
        /// <returns>Json字符串</returns>
        public string Serialize(object serializeObject)
        {
            ValidateOperator.Begin().NotNull(serializeObject, "需要序列化对象");
            using (var stream = new MemoryStream())
            {
                ProtoBuf.Serializer.Serialize(stream, serializeObject);
                return Convert.ToBase64String(stream.GetBuffer(), 0, (int) stream.Length);
            }
        }

        #endregion Methods
    }
}