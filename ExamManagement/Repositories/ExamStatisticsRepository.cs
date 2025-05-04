using ExamManagement.Entities;
using ExamManagement.Interfaces;
using Dapper;
using ExamManagement.Data;
using System.Data;

namespace ExamManagement.Repositories
{
    public class ExamStatisticsRepository : IExamStatisticsRepository
    {
        private readonly DbConnectionFactory _connectionFactory;

        public ExamStatisticsRepository(DbConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task AddAsync(ExamStatistics stat)
        {
            using IDbConnection db = _connectionFactory.CreateConnection();
            string sql = @"
                INSERT INTO ExamStatistics (SubjectCode, ClassLevel, AverageGrade, MaxGrade, MinGrade, ExamCount, CalculatedAt)
                VALUES (:SubjectCode, :ClassLevel, :AverageGrade, :MaxGrade, :MinGrade, :ExamCount, :CalculatedAt)";
            await db.ExecuteAsync(sql, stat);
        }

        public async Task<IEnumerable<ExamStatistics>> GetByDateAsync(DateTime date)
        {
            using IDbConnection db = _connectionFactory.CreateConnection();
            return await db.QueryAsync<ExamStatistics>(
                "SELECT * FROM ExamStatistics WHERE TRUNC(CalculatedAt) = TRUNC(:TargetDate)",
                new { TargetDate = date });
        }
    }
}
