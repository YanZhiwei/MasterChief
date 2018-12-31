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
        public DapperTableNameAttribute(string tableName)
        {
            _tableName = tableName;
        }

        /// <summary>
        /// 获取表名称
        /// </summary>
        /// <value>表名称</value>
        public string TableName => _tableName;


    }
}
