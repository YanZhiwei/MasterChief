using System;
using System.Data;
using System.IO;
using CsvHelper;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace MasterChief.DotNet.Infrastructure.JsonConverter.Helper
{
    public sealed class JsonHelper
    {
        public static DataTable ToDataTable(string jsonText)
        {
            return JsonConvert.DeserializeObject<DataSet>(jsonText)?.Tables[0];
        }

        public static string ToCsv(string jsonText, string delimiter)
        {
            var csvString = new StringWriter();
            using (var csv = new CsvWriter(csvString))
            {
                csv.Configuration.Delimiter = delimiter;

                using (var table = ToDataTable(jsonText))
                {
                    foreach (DataColumn column in table.Columns)
                        csv.WriteField(column.ColumnName);
                    csv.NextRecord();

                    foreach (DataRow row in table.Rows)
                    {
                        for (var i = 0; i < table.Columns.Count; i++)
                            csv.WriteField(row[i]);
                        csv.NextRecord();
                    }
                }
            }

            return csvString.ToString();
        }

        public static bool IsValidJsonText(string jsonText)
        {
            if (string.IsNullOrEmpty(jsonText)) return false;
            jsonText = jsonText.Trim();
            var result = false;
            if (jsonText.StartsWith("{") && jsonText.EndsWith("}") ||
                jsonText.StartsWith("[") && jsonText.EndsWith("]"))
                try
                {
                    JToken.Parse(jsonText);
                    result = true;
                }
                catch (Exception)
                {
                    result = false;
                }

            return result;
        }
    }
}