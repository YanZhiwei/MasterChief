using System.Data;
using System.IO;
using MasterChief.DotNet.Infrastructure.Serializer;
using Newtonsoft.Json;

namespace MasterChief.DotNet.Infrastructure.JsonSerializer
{
    /// <summary>
    ///     Json 序列化与反序列化
    /// </summary>
    public class JsonSerializer : ISerializer
    {
        /// <summary>
        ///     反序列化
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="data">需要反序列化字符串</param>
        /// <returns>反序列化</returns>
        public T Deserialize<T>(string data)
        {
            T deserializedType = default;
            if (!string.IsNullOrEmpty(data))
            {
                var serializer = new Newtonsoft.Json.JsonSerializer();
                Initialize(serializer);

                using (var reader = new StringReader(data))
                {
                    using (var jsonReader = new JsonTextReader(reader))
                    {
                        deserializedType = serializer.Deserialize<T>(jsonReader);
                    }
                }
            }

            return deserializedType;
        }

        /// <summary>
        ///     序列化
        /// </summary>
        /// <param name="serializeObject">需要序列化对象</param>
        /// <returns>Json字符串</returns>
        public string Serialize(object serializeObject)
        {
            if (serializeObject == null) return null;

            var type = serializeObject.GetType();
            var serializer = new Newtonsoft.Json.JsonSerializer();
            Initialize(serializer);

            if (type == typeof(DataRow))
                serializer.Converters.Add(new DataRowConverter());
            else if (type == typeof(DataTable))
                serializer.Converters.Add(new DataTableConverter());
            else if (type == typeof(DataSet)) serializer.Converters.Add(new DataSetConverter());

            using (var writer = new StringWriter())
            {
                using (var jsonWriter = new JsonTextWriter(writer))
                {
                    jsonWriter.Formatting = Formatting.None;
                    jsonWriter.QuoteChar = '"';
                    serializer.Serialize(jsonWriter, serializeObject);
                    return writer.ToString();
                }
            }
        }

        private static void Initialize(Newtonsoft.Json.JsonSerializer jsonSerializer)
        {
            if (jsonSerializer != null)
            {
                jsonSerializer.NullValueHandling = NullValueHandling.Ignore;
                jsonSerializer.ObjectCreationHandling = ObjectCreationHandling.Replace;
                jsonSerializer.MissingMemberHandling = MissingMemberHandling.Ignore;
                jsonSerializer.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            }
        }
    }
}