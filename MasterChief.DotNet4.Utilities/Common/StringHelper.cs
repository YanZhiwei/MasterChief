using System;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;

namespace MasterChief.DotNet4.Utilities.Common
{
    /// <summary>
    ///     字符串辅助类
    /// </summary>
    public static class StringHelper
    {
        /// <summary>
        ///     对字符串遍历分割
        ///     <para>eg: StringHelper.BuilderDelimiter("Yan", '-');==>"Y-a-n"</para>
        /// </summary>
        /// <param name="data">需要分割的字符串</param>
        /// <param name="delimiter">每个字符分割符号</param>
        /// <returns>分割好的字符串</returns>
        public static string BuilderDelimiter(this string data, char delimiter)
        {
            var array = data.ToCharArray();
            var builder = new StringBuilder();

            foreach (var c in array) builder.AppendFormat("{0}{1}", c, delimiter);

            var unHaleString = builder.ToString();
            var dIndex = unHaleString.LastIndexOf(delimiter);

            return dIndex != -1 ? unHaleString.Substring(0, dIndex) : unHaleString;
        }

        /// <summary>
        ///     清除字符串内空格
        ///     <para>eg:StringHelper.ClearBlanks(" 11 22 33 44  ");==>11223344</para>
        /// </summary>
        /// <param name="data">需要处理的字符串</param>
        /// <returns>处理的字符串</returns>
        public static string ClearBlanks(this string data)
        {
            var count = data.Length;
            var builder = new StringBuilder(count);

            for (var i = 0; i < count; i++)
            {
                var tmp = data[i];

                if (!char.IsWhiteSpace(tmp)) builder.Append(tmp);
            }

            return builder.ToString();
        }

        /// <summary>
        ///     忽略大小写比较
        /// </summary>
        /// <param name="data">字符串</param>
        /// <param name="compareData">比较字符串</param>
        /// <returns>是否相等</returns>
        /// 时间：2016/8/29 9:14
        /// 备注：
        public static bool CompareIgnoreCase(this string data, string compareData)
        {
            return string.Equals(data, compareData, StringComparison.OrdinalIgnoreCase);
        }

        /// <summary>
        ///     对字符串进行编码
        /// </summary>
        /// <param name="data">需要编码的字符串</param>
        /// <returns>编码后的字符串</returns>
        /// 时间:2016/10/16 13:02
        /// 备注:
        public static string Escape(this string data)
        {
            if (!string.IsNullOrEmpty(data))
            {
                var builder = new StringBuilder();

                foreach (var c in data)
                    builder.Append(char.IsLetterOrDigit(c)
                                   || c == '-' || c == '_' || c == '\\'
                                   || c == '/' || c == '.'
                        ? c.ToString()
                        : Uri.HexEscape(c));

                return builder.ToString();
            }

            return data;
        }

        /// <summary>
        ///     为指定格式的字符串填充相应对象来生成字符串
        /// </summary>
        /// <param name="format">字符串格式，占位符以{n}表示</param>
        /// <param name="args">用于填充占位符的参数</param>
        /// <returns>格式化后的字符串</returns>
        public static string FormatWith(this string format, params object[] args)
        {
            return string.Format(CultureInfo.CurrentCulture, format, args);
        }

        /// <summary>
        ///     截取字符串,超过最大长度则以'...'表示
        /// </summary>
        /// <param name="data">需要截取的字符串</param>
        /// <param name="maxLen">字符串最大长度，超过最大长度则以'...'表示</param>
        /// <returns>截取后字符串</returns>
        public static string GetFriendly(this string data, int maxLen)
        {
            if (maxLen <= 0) return data;

            if (data.Length > maxLen)
                return data.Substring(0, maxLen) + "...";
            return data;
        }

        /// <summary>
        ///     将字符串第一位置为小写
        /// </summary>
        /// <param name="data">需要操作的字符串</param>
        /// <returns>操作后的字符串</returns>
        public static string LowerFirstChar(this string data)
        {
            if (!string.IsNullOrEmpty(data)) return data.ToLower().Substring(0, 1) + data.Substring(1, data.Length - 1);

            return data;
        }

        /// <summary>
        ///     将千分位字符串转换成数字
        ///     <para>eg:StringHelper.ParseThousandthString("111,222,333");==>111222333</para>
        /// </summary>
        /// <param name="data">需要转换的千分位</param>
        /// <returns>数字;若转换失败则返回-1</returns>
        public static long ParseThousandthString(this string data)
        {
            return long.Parse(data,
                NumberStyles.AllowThousands | NumberStyles.AllowDecimalPoint | NumberStyles.AllowLeadingSign);
        }

        /// <summary>
        ///     去除文本中的html代码。
        /// </summary>
        public static string RemoveHtml(string data)
        {
            return string.IsNullOrEmpty(data) ? data : Regex.Replace(data, @"<[^>]+>", "");
        }

        /// <summary>
        ///     移除Json字符串诸如“{”,“}”符号
        /// </summary>
        /// <param name="data">Json字符串</param>
        /// <returns>Json字符串</returns>
        /// 时间：2016/6/29 16:31
        /// 备注：
        public static string RemoveJsonStringSymbol(string data)
        {
            return data.Replace("{", "").Replace("}", "").Replace("\"", "");
        }

        /// <summary>
        ///     字符串逆转
        ///     <para>eg:StringHelper.Reverse("YanZhiwei");</para>
        /// </summary>
        /// <param name="data">需要逆转的字符串</param>
        /// <returns>逆转后的字符串</returns>
        public static string Reverse(this string data)
        {
            var array = data.ToCharArray();
            Array.Reverse(array);
            return new string(array);
        }

        /// <summary>
        ///     按照符号截取字符串
        /// </summary>
        /// <param name="data">字符串</param>
        /// <param name="delimiter">第一个分隔符</param>
        /// <returns>截取后的字符串</returns>
        /// 日期：2015-09-18 11:40
        /// 备注：
        public static string SubString(this string data, char delimiter)
        {
            var index = data.IndexOf(delimiter);

            return index != -1 ? data.Substring(0, index) : data;
        }

        /// <summary>
        ///     按照最后符号截取字符串
        /// </summary>
        /// <param name="data">字符串</param>
        /// <param name="delimiter">最后一个分隔符</param>
        /// <returns>截取后的字符串</returns>
        public static string SubStringFromLast(this string data, char delimiter)
        {
            var index = data.LastIndexOf(delimiter);

            return index != -1 ? data.Substring(0, index) : data;
        }

        /// <summary>
        ///     对字符串进行解码
        /// </summary>
        /// <param name="data">需要解码的字符串</param>
        /// <returns>解码后的字符串</returns>
        /// 时间:2016/10/16 13:06
        /// 备注:
        public static string UnEscape(this string data)
        {
            if (!string.IsNullOrEmpty(data))
            {
                var builder = new StringBuilder();
                var count = data.Length;
                var index = 0;

                while (index != count)
                    builder.Append(Uri.IsHexEncoding(data, index) ? Uri.HexUnescape(data, ref index) : data[index++]);

                return builder.ToString();
            }

            return data;
        }

        /// <summary>
        ///     获取全局唯一值
        /// </summary>
        /// <returns>全局唯一值</returns>
        public static string Unique()
        {
            return Guid.NewGuid().ToString().Replace("-", string.Empty);
        }

        /// <summary>
        ///     将字符串第一位置为大写
        ///     <para>eg:studentName==>StudentName</para>
        /// </summary>
        /// <param name="data">需要操作的字符串</param>
        /// <returns>操作后的字符串</returns>
        public static string UpperFirstChar(this string data)
        {
            return data.ToUpper().Substring(0, 1) + data.Substring(1, data.Length - 1);
        }

        /// <summary>
        ///     文字换行
        ///     <para>eg:StringHelper.WrapText("YanZhiwei", 3);==>"Yan\r\nZhi\r\nwei"</para>
        /// </summary>
        /// <param name="data">需要换行的文字</param>
        /// <param name="maxWidth">多少长度换行</param>
        /// <returns>换行好的文字</returns>
        public static string WrapText(this string data, int maxWidth)
        {
            var count = data.Length;

            if (maxWidth > 0 && count > maxWidth)
            {
                var builder = new StringBuilder(data);
                var wrapIndex = builder.Length / maxWidth;

                for (var i = 0; i < wrapIndex; i++)
                {
                    var position = i * maxWidth;

                    if (position != 0)
                    {
                        var offset = (i - 1) * 2;
                        builder.Insert(position + offset, Environment.NewLine);
                    }
                }

                return builder.ToString();
            }

            return data;
        }

        /// <summary>
        ///     获取中英文长度，中文长度2，其他1
        /// </summary>
        /// <param name="data">需要判断字符串</param>
        /// <returns>长度</returns>
        public static int Calculate(string data)
        {
            if (string.IsNullOrEmpty(data)) return 0;

            var total = 0;
            var encoding = new ASCIIEncoding();
            //将字符串转换为ASCII编码的字节数字
            var array = encoding.GetBytes(data);
            for (var i = 0; i <= array.Length - 1; i++)
            {
                if (array[i] == 63) //中文都将编码为ASCII编码63,即"?"号
                    total++;

                total++;
            }

            return total;
        }
    }
}