using System;
using System.Data;
using System.IO;
using Newtonsoft.Json;

namespace MasterChief.DotNet.Infrastructure.Serializer
{
    /// <summary>
    /// Json 序列化与反序列化  
    /// </summary>
    public class JsonSerializer : ISerializer
    {

        public T Deserialize<T>(string data)
        {
            T deserializedType = default(T);
            if (!string.IsNullOrEmpty(data))
            {
                Newtonsoft.Json.JsonSerializer serializer = new Newtonsoft.Json.JsonSerializer();
                Initialize(serializer);

                using (StringReader reader = new StringReader(data))
                {
                    using (JsonTextReader jsonReader = new JsonTextReader(reader))
                    {
                        deserializedType = serializer.Deserialize<T>(jsonReader);
                    }
                }
            }
            return deserializedType;
        }

        public string Serialize(object serializeObject)
        {
            if (serializeObject == null)
            {
                return null;
            }

            Type type = serializeObject.GetType();
            Newtonsoft.Json.JsonSerializer serializer = new Newtonsoft.Json.JsonSerializer();
            Initialize(serializer);

            if (type == typeof(DataRow))
            {
                serializer.Converters.Add(new DataRowConverter());
            }
            else if (type == typeof(DataTable))
            {
                serializer.Converters.Add(new DataTableConverter());
            }
            else if (type == typeof(DataSet))
            {
                serializer.Converters.Add(new DataSetConverter());
            }

            using (StringWriter writer = new StringWriter())
            {
                using (JsonTextWriter jsonWriter = new JsonTextWriter(writer))
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
