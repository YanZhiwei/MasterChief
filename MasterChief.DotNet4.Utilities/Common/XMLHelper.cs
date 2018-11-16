namespace MasterChief.DotNet4.Utilities.Common
{
    using System.Data;
    using System.IO;
    using System.Text;
    using System.Xml;

    /// <summary>
    /// XML 辅助类
    /// </summary>
    public static class XMLHelper
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
                    XmlDocument _xDoc = new XmlDocument();
                    writer.Formatting = Formatting.Indented;
                    _xDoc.LoadXml(xmlString);
                    _xDoc.WriteTo(writer);
                    return encoding.GetString(stream.ToArray());
                }
            }
        }

        /// <summary>
        /// 将XML文件读取返回成DataSet
        /// </summary>
        /// <param name="xmlFilePath">xml路径</param>
        /// <returns>返回DataSet，若发生异常则返回NULL</returns>
        public static DataSet ParseXMLFile(string xmlFilePath)
        {
            XmlDocument _doc = new XmlDocument();
            _doc.Load(xmlFilePath);
            XmlNodeReader _xmlReader = new XmlNodeReader(_doc);
            DataSet _dataSet = new DataSet();
            _dataSet.ReadXml(_xmlReader);
            return _dataSet;
        }

        #endregion Methods
    }
}