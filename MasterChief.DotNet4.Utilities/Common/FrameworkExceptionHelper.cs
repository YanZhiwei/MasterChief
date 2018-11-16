namespace MasterChief.DotNet4.Utilities.Common
{
    using Collection;
    using System.Collections;
    using System.IO;
    using System.Xml;

    /// <summary>
    /// FrameworkException 辅助类
    /// </summary>
    /// 时间：2016-02-26 13:16
    /// 备注：
    public static class FrameworkExceptionHelper
    {
        #region Methods

        /// <summary>
        /// 将InnerException.Data转换为Xml字符串
        /// <para>不支持Data中Value是集合数组形式存储</para>
        /// </summary>
        /// <param name="frameworkException">FrameworkException</param>
        /// <returns>Xml字符串</returns>
        /// 时间：2016-02-26 13:19
        /// 备注：
        public static string ParseInnerDataToXmlString(this FrameworkException frameworkException)
        {
            ValidateOperator.Begin().NotNull(frameworkException, "FrameworkException");
            string _xmlString = string.Empty;

            if (frameworkException.InnerException != null)
            {
                if (frameworkException.InnerException.Data != null)
                {
                    SerializableDictionary<string, string> _seriableDic = new SerializableDictionary<string, string>();

                    foreach (DictionaryEntry data in frameworkException.InnerException.Data)
                    {
                        _seriableDic.Add(data.Key.ToStringOrDefault(string.Empty), data.Value.ToStringOrDefault(string.Empty));
                    }

                    using (StringWriter sw = new StringWriter())
                    {
                        using (XmlTextWriter writer = new XmlTextWriter(sw))
                        {
                            writer.Formatting = Formatting.Indented;
                            _seriableDic.WriteXml(writer);
                            writer.Flush();
                            _xmlString = sw.ToString().Trim();
                        }
                    }
                }
            }

            return _xmlString;
        }

        #endregion Methods
    }
}