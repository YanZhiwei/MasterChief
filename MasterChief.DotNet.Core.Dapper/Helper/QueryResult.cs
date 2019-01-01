using System;
namespace MasterChief.DotNet.Core.Dapper.Helper
{
    /// <summary>
    /// Query result.
    /// Code reference:http://www.bradoncode.com/blog/2012/12/creating-data-repository-using-dapp
    /// </summary>
    public class QueryResult
    {
        /// <summary>
        /// The _result
        /// </summary>
        private readonly Tuple<string, dynamic> _result;

        /// <summary>
        /// Gets the SQL.
        /// </summary>
        /// <value>
        /// The SQL.
        /// </value>
        public string Sql
        {
            get
            {
                return _result.Item1;
            }
        }

        /// <summary>
        /// Gets the param.
        /// </summary>
        /// <value>
        /// The param.
        /// </value>
        public dynamic Param
        {
            get
            {
                return _result.Item2;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="QueryResult" /> class.
        /// </summary>
        /// <param name="sql">The SQL.</param>
        /// <param name="param">The param.</param>
        public QueryResult(string sql, dynamic param)
        {
            _result = new Tuple<string, dynamic>(sql, param);
        }
    }
}
