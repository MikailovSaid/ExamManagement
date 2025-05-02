using ExamManagement.Entities;
using ExamManagement.Interfaces;
using Dapper;
using ExamManagement.Data;
using System.Data;

namespace ExamManagement.Repositories
{
    public class LogRepository : ILogRepository
    {
        private readonly DbConnectionFactory _connectionFactory;

        public LogRepository(DbConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task AddAsync(Log log)
        {
            using IDbConnection db = _connectionFactory.CreateConnection();
            string sql = @"
                INSERT INTO Logs (ServiceName, RunAt, Status, Message)
                VALUES (:ServiceName, :RunAt, :Status, :Message)";
            await db.ExecuteAsync(sql, log);
        }

        public async Task<IEnumerable<Log>> GetLogsAsync(string? serviceName = null)
        {
            using IDbConnection db = _connectionFactory.CreateConnection();
            string sql = "SELECT * FROM Logs";

            if (!string.IsNullOrEmpty(serviceName))
            {
                sql += " WHERE ServiceName = :ServiceName";
                return await db.QueryAsync<Log>(sql, new { ServiceName = serviceName });
            }

            return await db.QueryAsync<Log>(sql);
        }
    }
}
