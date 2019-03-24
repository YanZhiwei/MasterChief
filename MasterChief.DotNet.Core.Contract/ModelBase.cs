using System;

namespace MasterChief.DotNet.Core.Contract
{
    /// <summary>
    ///     实体基类
    /// </summary>
    public class ModelBase
    {
        /// <summary>
        /// 默认构造函数
        /// </summary>
        public ModelBase()
        {
#pragma warning disable RECS0021 // Warns about calls to virtual member functions occuring in the constructor
            Id = Guid.NewGuid();
            CreateTime = DateTime.Now;
            ModifyTime = DateTime.Now;
            Available = true;
#pragma warning restore RECS0021 // Warns about calls to virtual member functions occuring in the constructor
        }

        /// <summary>
        ///     主键ID
        /// </summary>
        public virtual Guid Id
        {
            get; // int, not null
            protected set;
        }

        /// <summary>
        ///     记录创建时间
        /// </summary>
        public virtual DateTime CreateTime
        {
            get; // datetime, not null
            protected set;
        }

        /// <summary>
        ///     记录修改时间
        /// </summary>
        public virtual DateTime ModifyTime
        {
            get; // datetime, not null
            protected set;
        }

        /// <summary>
        ///     记录是否可用
        /// </summary>
        public virtual bool Available { get; protected set; }
    }
}