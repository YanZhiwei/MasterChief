using MasterChief.DotNet.Core.EF;
using System.Data.Entity;
using System.Data.SqlClient;
using MasterChief.DotNet.Core.EFTests.Model;

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

        public DbSet<EfSample> EfSample { get; set; }
    }
}