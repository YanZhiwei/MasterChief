using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace MasterChief.DotNet.Infrastructure.Serializer
{
    /// <summary>
    ///     中国时间转换器
    ///     <para>
    ///         [JsonConverter(typeof(ChinaDateTimeConverter))]
    ///         public DateTime Birthday { get; set; }
    ///     </para>
    /// </summary>
    /// 时间：2016/7/8 11:39
    /// 备注：
    public class ChinaDateTimeConverter : DateTimeConverterBase
    {
        private static readonly IsoDateTimeConverter DtConverter = new IsoDateTimeConverter
            {DateTimeFormat = "yyyy-MM-dd HH:mm:ss"};

        /// <summary>
        ///     Reads the JSON representation of the object.
        /// </summary>
        /// <param name="reader">The <see cref="T:Newtonsoft.Json.JsonReader" /> to read from.</param>
        /// <param name="objectType">Type of the object.</param>
        /// <param name="existingValue">The existing value of object being read.</param>
        /// <param name="serializer">The calling serializer.</param>
        /// <returns>
        ///     The object value.
        /// </returns>
        /// 时间：2016/7/8 11:40
        /// 备注：
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue,
            Newtonsoft.Json.JsonSerializer serializer)
        {
            return DtConverter.ReadJson(reader, objectType, existingValue, serializer);
        }

        /// <summary>
        ///     Writes the JSON representation of the object.
        /// </summary>
        /// <param name="writer">The <see cref="T:Newtonsoft.Json.JsonWriter" /> to write to.</param>
        /// <param name="value">The value.</param>
        /// <param name="serializer">The calling serializer.</param>
        /// 时间：2016/7/8 11:40
        /// 备注：
        public override void WriteJson(JsonWriter writer, object value, Newtonsoft.Json.JsonSerializer serializer)
        {
            DtConverter.WriteJson(writer, value, serializer);
        }
    }
}