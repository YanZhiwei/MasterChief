using System;
namespace MasterChief.DotNet.Core.Dapper
{
    /// <summary>
    /// Dapper table name attribute.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class DapperTableNameAttribute : Attribute
    {
        private readonly string _tableName = null;
        private readonly string _primarykey = null;
        public DapperTableNameAttribute(string tableName, string primarykey)
        {
            _tableName = tableName;
            _primarykey = primarykey;
        }

        /// <summary>
        /// 获取表名称
        /// </summary>
        /// <value>表名称</value>
        public string TableName => _tableName;

        /// <summary>
        /// 获取表主键
        /// </summary>
        /// <value>主键</value>
        public string PrimaryKey => _primarykey;
    }
}
