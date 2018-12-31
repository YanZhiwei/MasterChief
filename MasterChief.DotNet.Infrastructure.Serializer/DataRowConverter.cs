using Newtonsoft.Json;
using System;
using System.Data;

namespace MasterChief.DotNet.Infrastructure.Serializer
{
    /// <summary>
    /// DataRowConverter转换器
    /// </summary>
    internal class DataRowConverter : JsonConverter
    {
        /// <summary>
        /// Writes the json.
        /// </summary>
        /// <param name="writer">JsonWriter</param>
        /// <param name="dataRow">dataRow</param>
        /// <param name="ser">JsonSerializer</param>
        public override void WriteJson(JsonWriter writer, object dataRow, Newtonsoft.Json.JsonSerializer ser)
        {
            DataRow row = dataRow as DataRow;
            writer.WriteStartObject();
            foreach (DataColumn column in row.Table.Columns)
            {
                writer.WritePropertyName(column.ColumnName);
                ser.Serialize(writer, row[column]);
            }
            writer.WriteEndObject();
        }

        /// <summary>
        /// override CanConvert
        /// </summary>
        /// <param name="valueType"></param>
        /// <returns></returns>
        public override bool CanConvert(Type valueType)
        {
            return typeof(DataRow).IsAssignableFrom(valueType);
        }

        /// <summary>
        /// override ReadJson
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="objectType"></param>
        /// <param name="existingValue"></param>
        /// <param name="ser"></param>
        /// <returns></returns>
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, Newtonsoft.Json.JsonSerializer ser)
        {
            throw new NotImplementedException();
        }
    }
}