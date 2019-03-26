using System.Data;
using System.Data.SQLite;
using System.IO;

namespace MasterChief.DotNet.Dapper.Utilities
{
    /// <summary>
    ///     基于Dapper的SQLite数据库操作类
    /// </summary>
    public sealed class DapperSQLiteManager : DapperDataManager
    {
        /// <summary>
        ///     SQLite 文件位置
        /// </summary>
        public readonly string DbFile;

        /// <summary>
        ///     构造函数
        /// </summary>
        /// <param name="sqlLiteDbFile">SQLite 文件位置</param>
        public DapperSQLiteManager(string sqlLiteDbFile) : base(
            $@"Data Source={sqlLiteDbFile};Pooling=true;FailIfMissing=false;Version=3")
        {
            DbFile = sqlLiteDbFile;
            if (!File.Exists(DbFile)) SQLiteConnection.CreateFile(DbFile);
        }

        /// <summary>
        ///     创建SqlConnection连接对象，需要打开
        /// </summary>
        /// <returns>
        ///     IDbConnection
        /// </returns>
        /// 时间：2016-01-19 16:22
        /// 备注：
        public override IDbConnection CreateConnection()
        {
            return new SQLiteConnection(ConnectString);
        }
    }
}