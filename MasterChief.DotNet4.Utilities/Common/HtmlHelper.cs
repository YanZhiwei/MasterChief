namespace MasterChief.DotNet4.Utilities.Common
{
    using System.Text.RegularExpressions;

    /// <summary>
    /// Html Tag 帮助类
    /// </summary>
    public static class HtmlHelper
    {
        #region Fields

        /*
         * 参考：
         * 1. http://www.dotnetperls.com/remove-html-tags
         */
        private static Regex htmlRegex = new Regex("<.*?>", RegexOptions.Compiled);

        #endregion Fields

        #region Methods

        /// <summary>
        /// 移除HTML Tag标记
        /// </summary>
        /// <param name="data">操作字符串</param>
        /// <param name="type">移除Html Tag方式</param>
        /// <returns>操作后的字符串</returns>
        public static string StripTags(string data, StripHtmlType type)
        {
            if (!string.IsNullOrEmpty(data))
            {
                switch (type)
                {
                    case StripHtmlType.CharArray:
                        data = StripTagsCharArray(data);
                        break;

                    case StripHtmlType.Regex:
                        data = StripTagsRegex(data);
                        break;

                    case StripHtmlType.RegexCompiled:
                        data = StripTagsRegexCompiled(data);
                        break;

                    default:
                        break;
                }
            }

            return data;
        }

        private static string StripTagsCharArray(string data)
        {
            char[] _array = new char[data.Length];
            int _arrayIndex = 0;
            bool _inside = false;

            for (int i = 0; i < data.Length; i++)
            {
                char _let = data[i];

                if (_let == '<')
                {
                    _inside = true;
                    continue;
                }

                if (_let == '>')
                {
                    _inside = false;
                    continue;
                }

                if (!_inside)
                {
                    _array[_arrayIndex] = _let;
                    _arrayIndex++;
                }
            }

            return new string(_array, 0, _arrayIndex);
        }

        private static string StripTagsRegex(string source)
        {
            return Regex.Replace(source, "<.*?>", string.Empty);
        }

        private static string StripTagsRegexCompiled(string source)
        {
            return htmlRegex.Replace(source, string.Empty);
        }

        #endregion Methods
    }
}