using Oracle.ManagedDataAccess.Client;
using System.Data;

namespace ExamManagement.Data
{
    public class DbConnectionFactory
    {
        private readonly IConfiguration _configuration;

        public DbConnectionFactory(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public IDbConnection CreateConnection()
        {
            string connectionString = _configuration.GetConnectionString("OracleDb")!;
            return new OracleConnection(connectionString);
        }
    }
}
