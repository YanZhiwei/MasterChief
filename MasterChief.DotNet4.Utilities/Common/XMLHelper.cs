namespace MasterChief.DotNet4.Utilities.Common
{
    using System.Data;
    using System.IO;
    using System.Text;
    using System.Xml;

    /// <summary>
    /// XML 辅助类
    /// </summary>
    public static class XmlHelper
    {
        #region Methods

        /// <summary>
        /// 格式化xml内容显示
        /// </summary>
        /// <param name="xmlString">xml内容</param>
        /// <param name="encoding">encode编码</param>
        /// <returns>string</returns>
        public static string FormatXml(string xmlString, Encoding encoding)
        {
            using (MemoryStream stream = new MemoryStream())
            {
                using (XmlTextWriter writer = new XmlTextWriter(stream, null))
                {
                    XmlDocument xmlDocument = new XmlDocument();
                    writer.Formatting = Formatting.Indented;
                    xmlDocument.LoadXml(xmlString);
                    xmlDocument.WriteTo(writer);
                    return encoding.GetString(stream.ToArray());
                }
            }
        }

        /// <summary>
        /// 将XML文件读取返回成DataSet
        /// </summary>
        /// <param name="xmlFilePath">xml路径</param>
        /// <returns>返回DataSet，若发生异常则返回NULL</returns>
        public static DataSet ParseXmlFile(string xmlFilePath)
        {
            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.Load(xmlFilePath);
            XmlNodeReader nodeReader = new XmlNodeReader(xmlDocument);
            DataSet dataSet = new DataSet();
            dataSet.ReadXml(nodeReader);
            return dataSet;
        }

        #endregion Methods
    }
}