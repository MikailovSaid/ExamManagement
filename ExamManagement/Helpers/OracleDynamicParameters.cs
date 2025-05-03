using Dapper;
using Oracle.ManagedDataAccess.Client;
using System.Data;

namespace ExamManagement.Helpers
{
    public class OracleDynamicParameters : SqlMapper.IDynamicParameters
    {
        private readonly List<OracleParameter> _parameters = new();

        public void Add(string name, object? value = null, OracleDbType? dbType = null, ParameterDirection direction = ParameterDirection.Input, int? size = null)
        {
            var param = new OracleParameter
            {
                ParameterName = name,
                Value = value ?? DBNull.Value,
                Direction = direction
            };

            if (dbType.HasValue)
                param.OracleDbType = dbType.Value;

            if (size.HasValue)
                param.Size = size.Value;

            _parameters.Add(param);
        }

        public void AddParameters(IDbCommand command, SqlMapper.Identity identity)
        {
            if (command is not OracleCommand oracleCommand)
                throw new InvalidOperationException("This class only works with OracleCommand.");

            foreach (var param in _parameters)
            {
                oracleCommand.Parameters.Add(param);
            }
        }
    }
}
