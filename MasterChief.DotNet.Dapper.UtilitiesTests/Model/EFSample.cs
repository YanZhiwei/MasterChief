using System;

namespace MasterChief.DotNet.Dapper.UtilitiesTests.Model
{
    public sealed class EFSample
    {  /// <summary>
       /// 主键ID
       /// </summary>
        public Guid ID
        {
            get;    // int, not null
            set;
        }

        /// <summary>
        /// 记录创建时间
        /// </summary>
        public DateTime CreateTime
        {
            get;    // datetime, not null
            set;
        }

        /// <summary>
        /// 记录修改时间
        /// </summary>
        public DateTime ModifyTime
        {
            get;    // datetime, not null
            set;
        }

        /// <summary>
        /// 记录是否可用
        /// </summary>
        public bool Available
        {
            get;
            set;
        }

        public string UserName { get; set; }
    }
}