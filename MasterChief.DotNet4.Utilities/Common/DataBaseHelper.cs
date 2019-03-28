using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text.RegularExpressions;

namespace MasterChief.DotNet4.Utilities.Common
{
    /// <summary>
    ///     数据辅助操作类
    /// </summary>
    /// 时间：2016-02-25 12:00
    /// 备注：
    public static class DataBaseHelper
    {
        #region Methods

        /// <summary>
        ///     创建Sql Server身份认证连接字符串
        /// </summary>
        /// <param name="server">服务器</param>
        /// <param name="database">数据库</param>
        /// <param name="userName">用户名</param>
        /// <param name="password">密码</param>
        /// <returns>Sql Server身份认证连接字符串</returns>
        public static string CreateSqlServerConnectString(string server, string database, string userName,
            string password)
        {
            return string.Format(@"Server={0};DataBase={1};uid={2};pwd={3};", server, database, userName, password);
        }

        /// <summary>
        ///     创建Sql Server Windows身份认证连接字符串
        /// </summary>
        /// <param name="server">服务器</param>
        /// <param name="datatabase">数据库</param>
        /// <returns>Sql Server Windows身份认证连接字符串</returns>
        /// 时间：2016-02-26 9:21
        /// 备注：
        public static string CreateSqlServerConnectString(string server, string datatabase)
        {
            return string.Format(@"Server={0}; Database={1}; Integrated Security=True;", server, datatabase);
        }

        /// <summary>
        ///     过滤HTML标记
        /// </summary>
        /// <param name="data">包括HTML，脚本，数据库关键字，特殊字符的源码 </param>
        /// <returns>已经去除标记后的文字</returns>
        public static string FilterHtmlTag(this string data)
        {
            if (!string.IsNullOrEmpty(data))
            {
                data = Regex.Replace(data, @"<script[^>]*?>.*?</script>", string.Empty, RegexOptions.IgnoreCase);
                data = Regex.Replace(data, @"<(.[^>]*)>", string.Empty, RegexOptions.IgnoreCase);
                data = Regex.Replace(data, @"([\r\n])[\s]+", string.Empty, RegexOptions.IgnoreCase);
                data = Regex.Replace(data, @"-->", string.Empty, RegexOptions.IgnoreCase);
                data = Regex.Replace(data, @"<!--.*", string.Empty, RegexOptions.IgnoreCase);
                data = Regex.Replace(data, @"&(quot|#34);", "\"", RegexOptions.IgnoreCase);
                data = Regex.Replace(data, @"&(amp|#38);", "&", RegexOptions.IgnoreCase);
                data = Regex.Replace(data, @"&(lt|#60);", "<", RegexOptions.IgnoreCase);
                data = Regex.Replace(data, @"&(gt|#62);", ">", RegexOptions.IgnoreCase);
                data = Regex.Replace(data, @"&(nbsp|#160);", " ", RegexOptions.IgnoreCase);
                data = Regex.Replace(data, @"&(iexcl|#161);", "\xa1", RegexOptions.IgnoreCase);
                data = Regex.Replace(data, @"&(cent|#162);", "\xa2", RegexOptions.IgnoreCase);
                data = Regex.Replace(data, @"&(pound|#163);", "\xa3", RegexOptions.IgnoreCase);
                data = Regex.Replace(data, @"&(copy|#169);", "\xa9", RegexOptions.IgnoreCase);
                data = Regex.Replace(data, @"&#(\d+);", string.Empty, RegexOptions.IgnoreCase);
                data = Regex.Replace(data, "xp_cmdshell", string.Empty, RegexOptions.IgnoreCase);
            }

            return data;
        }

        /// <summary>
        ///     过滤特殊字符
        /// </summary>
        /// <param name="data">字符串</param>
        /// <returns>过滤后的字符串</returns>
        public static string FilterSpecial(this string data)
        {
            if (!string.IsNullOrEmpty(data))
            {
                data = data.Replace("<", string.Empty);
                data = data.Replace(">", string.Empty);
                data = data.Replace("*", string.Empty);
                data = data.Replace("-", string.Empty);
                data = data.Replace("?", string.Empty);
                data = data.Replace("'", "''");
                data = data.Replace(",", string.Empty);
                data = data.Replace("/", string.Empty);
                data = data.Replace(";", string.Empty);
                data = data.Replace("*/", string.Empty);
                data = data.Replace("\r\n", string.Empty);
            }

            return data;
        }

        /// <summary>
        ///     过滤SQL语句字符串中的注入脚本
        /// </summary>
        /// <param name="data">传入的字符串</param>
        /// <returns>过滤后的字符串</returns>
        public static string FilterSqlInject(this string data)
        {
            if (!string.IsNullOrEmpty(data))
            {
                data = data.Replace("'", "''");
                data = data.Replace(";", "；");
                data = data.Replace("(", "（");
                data = data.Replace(")", "）");
                data = data.Replace("Exec", string.Empty);
                data = data.Replace("Execute", string.Empty);
                data = data.Replace("xp_", "x p_");
                data = data.Replace("sp_", "s p_");
                data = data.Replace("0x", "0 x");
                data = data.Replace(";", string.Empty);
                data = data.Replace("'", string.Empty);
                data = data.Replace("%20", string.Empty);
                data = data.Replace("-", string.Empty);
                data = data.Replace("=", string.Empty);
                data = data.Replace("==", string.Empty);
                data = data.Replace("%", string.Empty);
                data = data.Replace(" or", string.Empty);
                data = data.Replace("or ", string.Empty);
                data = data.Replace(" and", string.Empty);
                data = data.Replace("and ", string.Empty);
                data = data.Replace(" not", string.Empty);
                data = data.Replace("not ", string.Empty);
                data = data.Replace("!", string.Empty);
                data = data.Replace("{", string.Empty);
                data = data.Replace("}", string.Empty);
                data = data.Replace("[", string.Empty);
                data = data.Replace("]", string.Empty);
                data = data.Replace("(", string.Empty);
                data = data.Replace(")", string.Empty);
                data = data.Replace("|", string.Empty);
                data = data.Replace("_", string.Empty);
                data = data.Replace("&", "&amp");
                data = data.Replace("<", "&lt");
                data = data.Replace(">", "&gt");
                data = Regex.Replace(data, "select", string.Empty, RegexOptions.IgnoreCase);
                data = Regex.Replace(data, "insert", string.Empty, RegexOptions.IgnoreCase);
                data = Regex.Replace(data, "delete from", string.Empty, RegexOptions.IgnoreCase);
                data = Regex.Replace(data, "count''", string.Empty, RegexOptions.IgnoreCase);
                data = Regex.Replace(data, "drop table", string.Empty, RegexOptions.IgnoreCase);
                data = Regex.Replace(data, "truncate", string.Empty, RegexOptions.IgnoreCase);
                data = Regex.Replace(data, "asc", string.Empty, RegexOptions.IgnoreCase);
                data = Regex.Replace(data, "mid", string.Empty, RegexOptions.IgnoreCase);
                data = Regex.Replace(data, "char", string.Empty, RegexOptions.IgnoreCase);
                data = Regex.Replace(data, "xp_cmdshell", string.Empty, RegexOptions.IgnoreCase);
                data = Regex.Replace(data, "exec master", string.Empty, RegexOptions.IgnoreCase);
                data = Regex.Replace(data, "net localgroup administrators", string.Empty, RegexOptions.IgnoreCase);
                data = Regex.Replace(data, "and", string.Empty, RegexOptions.IgnoreCase);
                data = Regex.Replace(data, "net user", string.Empty, RegexOptions.IgnoreCase);
                data = Regex.Replace(data, "or", string.Empty, RegexOptions.IgnoreCase);
                data = Regex.Replace(data, "net", string.Empty, RegexOptions.IgnoreCase);
                data = Regex.Replace(data, "-", string.Empty, RegexOptions.IgnoreCase);
                data = Regex.Replace(data, "delete", string.Empty, RegexOptions.IgnoreCase);
                data = Regex.Replace(data, "drop", string.Empty, RegexOptions.IgnoreCase);
                data = Regex.Replace(data, "script", string.Empty, RegexOptions.IgnoreCase);
            }

            return data;
        }

        /// <summary>
        ///     过滤字符串【HTML标记，敏感SQL操作关键，特殊字符】
        /// </summary>
        /// <param name="data">字符串</param>
        /// <returns>处理后的字符串</returns>
        public static string FilterString(this string data)
        {
            if (!string.IsNullOrEmpty(data))
            {
                data = FilterHtmlTag(data);
                data = FilterSpecial(data);
            }

            return data;
        }

        /// <summary>
        ///     由错误码返回指定的自定义SqlException异常信息
        /// </summary>
        /// <param name="sqlException">SqlException</param>
        /// <returns>SqlException异常信息</returns>
        public static string GetSqlExceptionMessage(this SqlException sqlException)
        {
            var msg = GetSqlExceptionMessage(sqlException.Number);

            return string.IsNullOrEmpty(msg) ? string.Empty : sqlException.Message.Trim();
        }

        /// <summary>
        ///     由错误码返回指定的自定义SqlException异常信息
        ///     <para>DataHelper.GetSqlExceptionMessage(sqlEx.Number);</para>
        /// </summary>
        /// <param name="number">错误标识数字 </param>
        /// <returns>描述信息 </returns>
        public static string GetSqlExceptionMessage(int number)
        {
            var msg = string.Empty;

            switch (number)
            {
                case 2:
                    msg = "连接数据库超时，请检查网络连接或者数据库服务器是否正常。";
                    break;

                case 17:
                    msg = "SqlServer服务不存在或拒绝访问。";
                    break;

                case 17142:
                    msg = "SqlServer服务已暂停，不能提供数据服务。";
                    break;

                case 2812:
                    msg = "指定存储过程不存在。";
                    break;

                case 208:
                    msg = "指定名称的表不存在。";
                    break;

                case 4060: //数据库无效。
                    msg = "所连接的数据库无效。";
                    break;

                case 18456: //登录失败
                    msg = "使用设定的用户名与密码登录数据库失败。";
                    break;

                case 547:
                    msg = "外键约束，无法保存数据的变更。";
                    break;

                case 2627:
                    msg = "主键重复，无法插入数据。";
                    break;

                case 2601:
                    msg = "未知错误。";
                    break;
            }

            return msg;
        }

        /// <summary>
        ///     DataTable的列求和
        /// </summary>
        /// <param name="datatable">DataTable</param>
        /// <param name="sumColumnName">sum的列</param>
        /// <returns>计算的值</returns>
        public static object GetSumByColumn(this DataTable datatable, string sumColumnName)
        {
            object result = null;

            if (datatable != null && !string.IsNullOrEmpty(sumColumnName))
                result = datatable.Compute("Sum(" + sumColumnName + ")", string.Empty);

            return result;
        }

        /// <summary>
        ///     DataTable的group by sum计算
        ///     <para>eg:eg:DBHelper.GroupByToSum(_dt, "CTLampType", "钠灯- 100W", "CTLastMonthCount");</para>
        /// </summary>
        /// <param name="datatable">DataTable</param>
        /// <param name="gColumnName">分组字段名称</param>
        /// <param name="gValue">分组数值</param>
        /// <param name="sColumnName">求和字段名称</param>
        /// <returns>object</returns>
        public static object GetSumByGroup(this DataTable datatable, string gColumnName, string gValue,
            string sColumnName)
        {
            object result = null;

            if (datatable != null && !string.IsNullOrEmpty(gColumnName) && !string.IsNullOrEmpty(sColumnName))
                result = datatable.Compute("Sum(" + sColumnName + ")", " " + gColumnName + "='" + gValue + "'");

            return result;
        }

        /// <summary>
        ///     将连接字符串转换成字典
        /// </summary>
        /// <param name="connectString">
        ///     The connect string.eg:IP=127.0.0.1;
        ///     DataSource=YanZhiwei-PC;User=sa;Password=sasa;DataBase=NorthWind
        /// </param>
        /// <returns>字典IDictionary</returns>
        public static IDictionary<string, string> ParseConnnectString(this string connectString)
        {
            var groups = connectString.Split(';');
            IDictionary<string, string> valuePairs = new Dictionary<string, string>();

            foreach (var group in groups)
            {
                var groupOk = group.Trim();

                if (string.IsNullOrEmpty(groupOk)) continue;

                var keyVal = groupOk.Split('=');
                valuePairs.Add(keyVal[0].Trim(), keyVal[1].Trim());
            }

            return valuePairs;
        }

        #endregion Methods
    }
}