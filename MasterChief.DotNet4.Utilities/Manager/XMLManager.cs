namespace MasterChief.DotNet4.Utilities.Manager
{
    using Operator;
    using System.Collections.Generic;
    using System.IO;
    using System.Text;
    using System.Xml;
    using System.Xml.Serialization;

    /// <summary>
    /// XML文件序列化与反序列化
    /// </summary>
    /// 时间：2016/8/25 13:12
    /// 备注：
    public static class XmlManager
    {
        #region Methods

        /// <summary>
        /// 将XML文件反序列化成集合
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="path">XML文件路径</param>
        /// <returns>集合</returns>
        /// 时间：2016/8/25 13:11
        /// 备注：
        public static IEnumerable<T> Deserialize<T>(string path)
        where T : class
        {
            ValidateOperator.Begin().NotNullOrEmpty(path, "XML文件").IsFilePath(path).CheckFileExists(path);
            using(Stream stream = new FileStream(path, FileMode.Open, FileAccess.Read))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(IEnumerable<T>));
                return (IEnumerable<T>)serializer.Deserialize(stream);
            }
        }

        /// <summary>
        /// 将XML文件读取成字符串
        /// </summary>
        /// <param name="path">路径</param>
        /// <returns>XML字符串</returns>
        public static string DeserializeToString(string path)
        {
            ValidateOperator.Begin().NotNullOrEmpty(path, "XML文件").IsFilePath(path).CheckFileExists(path);
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(path);
            StringBuilder builder = new StringBuilder();
            StringWriter xmlSw = new StringWriter(builder);
            XmlTextWriter xmlWriter = new XmlTextWriter(xmlSw);
            xmlWriter.Formatting = Formatting.Indented;
            xmlDoc.WriteContentTo(xmlWriter);
            return builder.ToString();
        }

        /// <summary>
        /// 将集合序列化到XML文件
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="data">集合</param>
        /// <param name="path">XML路径</param>
        /// 时间：2016/8/25 13:14
        /// 备注：
        public static void Serialize<T>(IEnumerable<T> data, string path)
        where T : class
        {
            ValidateOperator.Begin().NotNull(data, "需要序列化集合").NotNullOrEmpty(path, "XML文件").IsFilePath(path);
            using(Stream stream = new FileStream(path, FileMode.Create, FileAccess.Write))
            {
                XmlTextWriter xmlTextWriter = new XmlTextWriter(stream, new UTF8Encoding(false));
                xmlTextWriter.Formatting = Formatting.Indented;
                XmlSerializer xmlSerializer = new XmlSerializer(data.GetType());
                xmlSerializer.Serialize(xmlTextWriter, data);
                xmlTextWriter.Flush();
                xmlTextWriter.Close();
            }
        }

        /// <summary>
        /// 将对象序列化到XML文件
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="model">实体类</param>
        /// <param name="path">XML路径</param>
        /// 时间：2016/8/25 13:16
        /// 备注：
        public static void Serialize<T>(T model, string path)
        where T : class
        {
            ValidateOperator.Begin().NotNull(model, "需要序列化对象").NotNullOrEmpty(path, "XML文件").IsFilePath(path);
            using(Stream stream = new FileStream(path, FileMode.Create, FileAccess.Write))
            {
                XmlTextWriter xmlTextWriter = new XmlTextWriter(stream, new UTF8Encoding(false));
                xmlTextWriter.Formatting = Formatting.Indented;
                XmlSerializer xmlSerializer = new XmlSerializer(model.GetType());
                xmlSerializer.Serialize(xmlTextWriter, model);
                xmlTextWriter.Flush();
                xmlTextWriter.Close();
            }
        }

        #endregion Methods
    }
}