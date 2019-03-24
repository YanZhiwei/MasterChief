using MasterChief.DotNet.Core.Dapper;
using System.Data;
using System.Data.SqlClient;

namespace MasterChief.DotNet.Core.DapperTests
{
    public sealed class SampleDbContext : DapperDbContextBase
    {
        public SampleDbContext() : base("server=localhost;database=Sample;uid=sa;pwd=sasa")
        {
        }

        public override IDbConnection CreateConnection()
        {
            IDbConnection sqlConnection = new SqlConnection(base.ConnectString);

            if (sqlConnection.State != ConnectionState.Open)
            {
                sqlConnection.Open();
            }

            return sqlConnection;
        }
    }
}