namespace MasterChief.DotNet.Json.Utilities
{
    using global::Newtonsoft.Json;
    using System;
    using System.Data;
    using System.IO;

    /// <summary>
    /// 使用Newtonsoft处理Json序列化反序列化
    /// </summary>
    public sealed class NewtonsoftJsonProvider : IJsonProvider
    {
        #region Methods

        /// <summary>
        /// 反序列化Json数据格式
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="jsonText">Json文本</param>
        /// <returns>反序列化</returns>
        public T Deserialize<T>(string jsonText)
        {
            T deserializedType = default(T);
            if (!string.IsNullOrEmpty(jsonText))
            {
                JsonSerializer serializer = new JsonSerializer();
                Initialize(serializer);

                using (StringReader reader = new StringReader(jsonText))
                {
                    using (JsonTextReader jsonReader = new JsonTextReader(reader))
                    {
                        deserializedType = serializer.Deserialize<T>(jsonReader);
                    }
                }
            }
            return deserializedType;
        }

        /// <summary>
        /// 序列化数据为Json数据格式.支持DataRow,DataTable,DataSet
        /// <para>说明 [JsonProperty("姓名")]重命名属性名称</para>
        /// <para>说明 [JsonIgnore]忽略属性</para>
        /// </summary>
        /// <param name="serializeObject">需要序列化对象</param>
        /// <returns>Jsonz字符串</returns>
        public string Serialize(object serializeObject)
        {
            if (serializeObject == null)
            {
                return null;
            }

            Type type = serializeObject.GetType();
            JsonSerializer serializer = new JsonSerializer();
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

        private static void Initialize(JsonSerializer jsonSerializer)
        {
            if (jsonSerializer != null)
            {
                jsonSerializer.NullValueHandling = NullValueHandling.Ignore;
                jsonSerializer.ObjectCreationHandling = ObjectCreationHandling.Replace;
                jsonSerializer.MissingMemberHandling = MissingMemberHandling.Ignore;
                jsonSerializer.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            }
        }

        #endregion Methods
    }
}