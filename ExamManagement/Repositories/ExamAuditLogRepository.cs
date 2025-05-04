using ExamManagement.Entities;
using ExamManagement.Interfaces;
using Dapper;
using ExamManagement.Data;
using System.Data;

namespace ExamManagement.Repositories
{
    public class ExamAuditLogRepository : IExamAuditLogRepository
    {
        private readonly DbConnectionFactory _connectionFactory;

        public ExamAuditLogRepository(DbConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task<IEnumerable<ExamAuditLog>> GetAllAsync(int? examId)
        {
            using IDbConnection db = _connectionFactory.CreateConnection();
            var checkId = examId != null ? $"where ExamId = {examId}" : "";
            string sql = $"SELECT * FROM ExamAuditLog {checkId} order by LogId desc";
            return await db.QueryAsync<ExamAuditLog>(sql);
        }

        public async Task AddAsync(ExamAuditLog log)
        {
            using IDbConnection db = _connectionFactory.CreateConnection();
            string sql = @"
                INSERT INTO ExamAuditLog (ExamId, OldGrade, NewGrade, ChangedBy, ChangedAt)
                VALUES (:ExamId, :OldGrade, :NewGrade, :ChangedBy, :ChangedAt)";
            await db.ExecuteAsync(sql, log);
        }

        public async Task<IEnumerable<ExamAuditLog>> GetLogsByExamIdAsync(int examId)
        {
            using IDbConnection db = _connectionFactory.CreateConnection();
            return await db.QueryAsync<ExamAuditLog>(
                "SELECT * FROM ExamAuditLog WHERE ExamId = :ExamId",
                new { ExamId = examId });
        }
    }
}
