using MasterChief.DotNet.Core.EF;
using System.Data.Entity;
using System.Data.SqlClient;

namespace MasterChief.DotNet.Core.EFTests
{
    public sealed class SampleDbContext : EfDbContextBase
    {
        public SampleDbContext() : base(new SqlConnection()
        {
            ConnectionString = "server=localhost;database=Sample;uid=sa;pwd=sasa"
        })
        {
        }

        public DbSet<EFSample> EFSample { get; set; }
    }
}