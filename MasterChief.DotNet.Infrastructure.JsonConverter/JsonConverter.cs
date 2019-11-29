using System.Data;
using System.IO;
using System.Text;
using System.Xml.Linq;
using MasterChief.DotNet.Infrastructure.JsonConverter.Helper;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace MasterChief.DotNet.Infrastructure.JsonConverter
{
    public sealed class JsonConverter
    {
        public static void ToCsvFile(string jsonText, string csvFile)
        {
            var csvText = JsonHelper.ToCsv(jsonText, ",");
            File.WriteAllText(csvFile, csvText, Encoding.UTF8);
        }

        public static DataTable ToDataTable(string jsonText)
        {
            var jToken = JToken.Parse(jsonText);
            if (jToken is JArray) return JsonConvert.DeserializeObject<DataSet>(jsonText)?.Tables[0];

            var jArray = new JArray {jToken};
            return JsonConvert.DeserializeObject<DataTable>(jArray.ToString());
        }

        public static void ToXmlFile(string jsonText, string xmlFile)
        {
            XNode node = JsonConvert.DeserializeXNode(jsonText, "Root");
            var xmlText = node.ToString();
            File.WriteAllText(xmlFile, xmlText, Encoding.UTF8);
        }
    }
}