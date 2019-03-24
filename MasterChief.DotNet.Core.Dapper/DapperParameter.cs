using System.Collections;
using System.Collections.Generic;
using System.Data;
using Dapper;

namespace MasterChief.DotNet.Core.Dapper
{
    internal sealed class DapperParameter : SqlMapper.IDynamicParameters,
        IEnumerable<IDbDataParameter>
    {
        private readonly List<IDbDataParameter> _parameters =
            new List<IDbDataParameter>();

        void SqlMapper.IDynamicParameters.AddParameters(IDbCommand command,
            SqlMapper.Identity identity)
        {
            foreach (var parameter in _parameters)
                command.Parameters.Add(parameter);
        }

        public IEnumerator<IDbDataParameter> GetEnumerator()
        {
            return _parameters.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void Add(IDbDataParameter value)
        {
            _parameters.Add(value);
        }
    }
}