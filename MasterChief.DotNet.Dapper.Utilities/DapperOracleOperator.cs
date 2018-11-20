using Oracle.ManagedDataAccess.Client;
using System.Data;

namespace MasterChief.DotNet.Dapper.Utilities
{
    /// <summary>
    /// 基于Dapper的Oracle数据库操作类
    /// </summary>
    public sealed class DapperOracleOperator : DapperDataOperator
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="connectString">连接字符串</param>
        /// 时间：2016-01-19 16:21
        /// 备注：
        public DapperOracleOperator(string connectString) : base(connectString)
        {
        }

        /// <summary>
        /// 创建SqlConnection连接对象，需要打开
        /// </summary>
        /// <returns>
        /// IDbConnection
        /// </returns>
        /// 时间：2016-01-19 16:22
        /// 备注：
        public override IDbConnection CreateConnection()
        {
            IDbConnection sqlConnection = new OracleConnection(base.ConnectString);

            if (sqlConnection.State != ConnectionState.Open)
            {
                sqlConnection.Open();
            }

            return sqlConnection;
        }
    }
}