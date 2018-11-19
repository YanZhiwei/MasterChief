using System.Data;

namespace MasterChief.DotNet4.Utilities.Common
{
    /// <summary>
    /// IDbCommand帮助类
    /// </summary>
    /// 时间：2016-03-30 10:44
    /// 备注：
    public static class IDbCommandHelper
    {
        /// <summary>
        /// 从IDbCommand获取完整Sql
        /// </summary>
        /// <param name="cmd">IDbCommand</param>
        /// <returns>sql语句</returns>
        /// 时间：2016-03-30 10:45
        /// 备注：
        public static string GetGeneratedQuery(this IDbCommand cmd)
        {
            string sqlText = cmd.CommandText;

            for (int i = 0; i < cmd.Parameters.Count; i++)
            {
                IDbDataParameter _parameter = cmd.Parameters[i] as IDbDataParameter;
                sqlText = sqlText.Replace(_parameter.ParameterName, _parameter.Value.ToStringOrDefault(string.Empty));
            }

            return sqlText;
        }
    }
}