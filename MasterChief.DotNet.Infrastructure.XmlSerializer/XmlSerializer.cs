using System.IO;
using System.Xml;
using MasterChief.DotNet.Infrastructure.Serializer;

namespace MasterChief.DotNet.Infrastructure.XmlSerializer
{
    /// <summary>
    ///     Xml 序列化与反序列化
    /// </summary>
    public class XmlSerializer : ISerializer
    {
        /// <summary>
        ///     反序列化
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="data">需要反序列化字符串</param>
        /// <returns>反序列化</returns>
        public T Deserialize<T>(string data)
        {
            var type = typeof(T);
            var xmlSerializer = new System.Xml.Serialization.XmlSerializer(type);

            using (var reader = new StringReader(data))
            {
                return (T) xmlSerializer.Deserialize(reader);
            }
        }

        /// <summary>
        ///     序列化
        /// </summary>
        /// <param name="serializeObject">需要序列化对象</param>
        /// <returns>Json字符串</returns>
        public string Serialize(object serializeObject)
        {
            var type = serializeObject.GetType();
            var xmlSerializer = new System.Xml.Serialization.XmlSerializer(type);

            using (var stringWriter = new StringWriter())
            {
                using (var xmlWriter = XmlWriter.Create(stringWriter, new XmlWriterSettings {Indent = true}))
                {
                    xmlSerializer.Serialize(xmlWriter, serializeObject);
                    return stringWriter.ToString();
                }
            }
        }
    }
}