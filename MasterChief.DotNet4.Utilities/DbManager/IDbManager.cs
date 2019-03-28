using System.Data;
using System.Data.Common;

namespace MasterChief.DotNet4.Utilities.DbManager
{
    /// <summary>
    ///     SQL操作接口
    /// </summary>
    /// 时间：2015-12-31 10:09
    /// 备注：
    public interface IDbManager
    {
        /// <summary>
        ///     ExecuteNonQuery
        /// </summary>
        /// <param name="sql">sql语句</param>
        /// <param name="parameters">参数</param>
        /// <returns>影响行数</returns>
        /// 时间：2015-12-31 10:09
        /// 备注：
        int ExecuteNonQuery(string sql, DbParameter[] parameters);

        /// <summary>
        ///     Executes the reader.ExecuteReader
        /// </summary>
        /// <param name="sql">sql语句</param>
        /// <param name="parameters">参数</param>
        /// <returns>IDataReader</returns>
        /// 时间：2015-12-31 10:09
        /// 备注：
        IDataReader ExecuteReader(string sql, DbParameter[] parameters);

        /// <summary>
        ///     StoreExecuteDataReader
        /// </summary>
        /// <param name="proName">存储过程名称</param>
        /// <param name="parameters">参数</param>
        /// <returns></returns>
        /// 时间：2015-12-31 10:09
        /// 备注：
        IDataReader StoreExecuteDataReader(string proName, DbParameter[] parameters);

        /// <summary>
        ///     ExecuteDataTable
        /// </summary>
        /// <param name="sql">sql语句</param>
        /// <param name="parameters">参数</param>
        /// <returns></returns>
        /// 时间：2015-12-31 10:10
        /// 备注：
        DataTable ExecuteDataTable(string sql, DbParameter[] parameters);

        /// <summary>
        ///     ExecuteScalar
        /// </summary>
        /// <param name="sql">sql语句</param>
        /// <param name="parameters">参数</param>
        /// <returns>Object</returns>
        /// 时间：2015-12-31 10:10
        /// 备注：
        object ExecuteScalar(string sql, DbParameter[] parameters);
    }
}