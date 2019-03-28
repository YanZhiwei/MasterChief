using System.Data;

namespace MasterChief.DotNet4.Utilities.Common
{
    /// <summary>
    ///     IDbCommand帮助类
    /// </summary>
    /// 时间：2016-03-30 10:44
    /// 备注：
    public static class DbCommandHelper
    {
        /// <summary>
        ///     从IDbCommand获取完整Sql
        /// </summary>
        /// <param name="cmd">IDbCommand</param>
        /// <returns>sql语句</returns>
        /// 时间：2016-03-30 10:45
        /// 备注：
        public static string GetGeneratedQuery(this IDbCommand cmd)
        {
            var sqlText = cmd.CommandText;

            for (var i = 0; i < cmd.Parameters.Count; i++)
            {
                var parameter = cmd.Parameters[i] as IDbDataParameter;
                sqlText = sqlText.Replace(parameter.ParameterName, parameter.Value.ToStringOrDefault(string.Empty));
            }

            return sqlText;
        }
    }
}