using System;
using System.Data;
using Newtonsoft.Json;

namespace MasterChief.DotNet.Infrastructure.JsonSerializer
{
    /// <summary>
    ///     DataTableConverter转换器
    /// </summary>
    internal class DataTableConverter : JsonConverter
    {
        /// <summary>
        ///     override CanConvert
        /// </summary>
        /// <param name="valueType"></param>
        /// <returns></returns>
        public override bool CanConvert(Type valueType)
        {
            return typeof(DataTable).IsAssignableFrom(valueType);
        }

        /// <summary>
        ///     override ReadJson
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="objectType"></param>
        /// <param name="existingValue"></param>
        /// <param name="ser"></param>
        /// <returns></returns>
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue,
            Newtonsoft.Json.JsonSerializer ser)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        ///     override WriteJson
        /// </summary>
        /// <param name="writer"></param>
        /// <param name="dataTable"></param>
        /// <param name="ser"></param>
        public override void WriteJson(JsonWriter writer, object dataTable, Newtonsoft.Json.JsonSerializer ser)
        {
            var table = dataTable as DataTable;
            var converter = new DataRowConverter();
            writer.WriteStartObject();
            writer.WritePropertyName("Rows");
            writer.WriteStartArray();
            if (table != null)
            {
                foreach (DataRow row in table.Rows)
                {
                    converter.WriteJson(writer, row, ser);
                }
            }
            writer.WriteEndArray();
            writer.WriteEndObject();
        }
    }
}