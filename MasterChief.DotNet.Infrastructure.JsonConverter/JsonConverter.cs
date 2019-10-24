using System.Data;
using System.IO;
using System.Text;
using System.Xml.Linq;
using MasterChief.DotNet.Infrastructure.JsonConverter.Helper;
using Newtonsoft.Json;

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
            return JsonHelper.ToDataTable(jsonText);
        }

        public static void ToXmlFile(string jsonText, string xmlFile)
        {
            XNode node = JsonConvert.DeserializeXNode(jsonText, "Root");
            var xmlText = node.ToString();
            File.WriteAllText(xmlFile, xmlText, Encoding.UTF8);
        }
    }
}