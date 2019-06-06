using System.Data;
using System.Xml;
using System.Xml.Linq;

namespace MasterChief.DotNet4.Utilities.Common
{
    /// <summary>
    ///     XML 辅助类
    /// </summary>
    public static class XmlHelper
    {
        #region Methods

        /// <summary>
        ///     格式化xml内容显示
        /// </summary>
        /// <param name="xmlText">xml内容</param>
        /// <returns>string</returns>
        public static string FormatXml(string xmlText)
        {
            if (string.IsNullOrEmpty(xmlText)) return xmlText;
            xmlText = xmlText.Trim();
            var xDocument = XDocument.Parse(xmlText);
            return xDocument.ToString().Trim();
        }

        /// <summary>
        ///     运行XPath语法
        /// </summary>
        /// <param name="xmlText">xml内容</param>
        /// <param name="xpath">xpath语法</param>
        /// <returns>XmlNodeList</returns>
        public static XmlNodeList ExecXPath(string xmlText, string xpath)
        {
            var doc = new XmlDocument();
            doc.LoadXml(xmlText);
            //eg://bus
            return doc.SelectNodes(xpath);
        }

        /// <summary>
        ///     将XML文件读取返回成DataSet
        /// </summary>
        /// <param name="xmlFilePath">xml路径</param>
        /// <returns>返回DataSet，若发生异常则返回NULL</returns>
        public static DataSet ParseXmlFile(string xmlFilePath)
        {
            var xmlDocument = new XmlDocument();
            xmlDocument.Load(xmlFilePath);
            var nodeReader = new XmlNodeReader(xmlDocument);
            var dataSet = new DataSet();
            dataSet.ReadXml(nodeReader);
            return dataSet;
        }

        #endregion Methods
    }
}