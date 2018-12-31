using Newtonsoft.Json;
using System;
using System.Data;

namespace MasterChief.DotNet.Infrastructure.Serializer
{
    /// <summary>
    /// DataTableConverter转换器
    /// </summary>
    internal class DataTableConverter : JsonConverter
    {
        /// <summary>
        /// override CanConvert
        /// </summary>
        /// <param name="valueType"></param>
        /// <returns></returns>
        public override bool CanConvert(Type valueType)
        {
            return typeof(DataTable).IsAssignableFrom(valueType);
        }

        /// <summary>
        /// override ReadJson
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="objectType"></param>
        /// <param name="existingValue"></param>
        /// <param name="ser"></param>
        /// <returns></returns>
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer ser)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// override WriteJson
        /// </summary>
        /// <param name="writer"></param>
        /// <param name="dataTable"></param>
        /// <param name="ser"></param>
        public override void WriteJson(JsonWriter writer, object dataTable, JsonSerializer ser)
        {
            DataTable table = dataTable as DataTable;
            DataRowConverter converter = new DataRowConverter();
            writer.WriteStartObject();
            writer.WritePropertyName("Rows");
            writer.WriteStartArray();
            foreach (DataRow row in table.Rows)
            {
                converter.WriteJson(writer, row, ser);
            }
            writer.WriteEndArray();
            writer.WriteEndObject();
        }
    }
}