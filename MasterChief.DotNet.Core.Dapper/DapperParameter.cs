using Dapper;
using System.Collections;
using System.Collections.Generic;
using System.Data;

namespace MasterChief.DotNet.Core.Dapper
{
    sealed class DapperParameter : SqlMapper.IDynamicParameters,
                            IEnumerable<IDbDataParameter>
    {
        private readonly List<IDbDataParameter> parameters =
            new List<IDbDataParameter>();

        public IEnumerator<IDbDataParameter> GetEnumerator()
        {
            return parameters.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void Add(IDbDataParameter value)
        {
            parameters.Add(value);
        }

        void SqlMapper.IDynamicParameters.AddParameters(IDbCommand command,
            SqlMapper.Identity identity)
        {
            foreach (IDbDataParameter parameter in parameters)
                command.Parameters.Add(parameter);
        }
    }
}