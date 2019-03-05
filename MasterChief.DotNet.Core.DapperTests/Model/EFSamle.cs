using Dapper.Contrib.Extensions;
using MasterChief.DotNet.Core.Contract;
using System;

namespace MasterChief.DotNet.Core.DapperTests.Model
{
    [Table("EFSample")]
    public sealed class EFSample : ModelBase
    {
        [ExplicitKey]//不是自动增长主键时使用ExplicitKey
        public override Guid ID { get; set; }
        public EFSample()
        {
            ID = Guid.NewGuid();
            CreateTime = DateTime.Now;
            ModifyTime = DateTime.Now;
            Available = true;
        }

        public string UserName { get; set; } // nvarchar(20), null
    }
}