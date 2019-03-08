using Dapper.Contrib.Extensions;
using MasterChief.DotNet.Core.Contract;
using System;

namespace MasterChief.DotNet.Core.DapperTests.Model
{
    [Table("EFSample")]
    public sealed class EFSample : ModelBase
    {
        /*
         * Table：指定实体对应地数据库表名，可忽略，但是忽略后实体对应地数据库表名会在末尾加个s，Demo对应Demos
         * Key：指定此列为主键（自动增长主键），可忽略，忽略后默认查找
         * ExplicitKey：指定此列为主键（不自动增长类型例如guid）
         * Computed：计算属性，打上此标签，对象地insert，update等操作会忽略此列
         * Write：需穿一个bool值，false时insert，update等操作会忽略此列（和Computed的作用差不多）
         */

        [ExplicitKey]//不是自动增长主键时使用ExplicitKey
        public override Guid ID { get; set; }

        public EFSample()
        {
            ID = Guid.NewGuid();
            CreateTime = DateTime.Now;
            ModifyTime = DateTime.Now;
            Available = true;
        }

        [Write(true)]
        public override DateTime CreateTime { get => base.CreateTime; set => base.CreateTime = value; }

        public string UserName { get; set; } // nvarchar(20), null
    }
}