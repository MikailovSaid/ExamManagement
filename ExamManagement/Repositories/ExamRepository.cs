using ExamManagement.Entities;
using ExamManagement.Interfaces;
using Dapper;
using ExamManagement.Data;
using System.Data;

namespace ExamManagement.Repositories
{
    public class ExamRepository : IExamRepository
    {
        private readonly DbConnectionFactory _connectionFactory;

        public ExamRepository(DbConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task<IEnumerable<Exam>> GetAllAsync()
        {
            using IDbConnection db = _connectionFactory.CreateConnection();
            string sql = @"
                SELECT e.ExamId, e.SubjectCode, e.StudentId, e.ExamDate, e.Grade,
                        s.ClassLevel
                FROM Exams e
                JOIN Students s ON e.StudentId = s.StudentId";

            return await db.QueryAsync<Exam>(sql);
        }

        public async Task<Exam?> GetByIdAsync(object id)
        {
            using IDbConnection db = _connectionFactory.CreateConnection();
            return await db.QueryFirstOrDefaultAsync<Exam>(
                "SELECT * FROM Exams WHERE ExamId = :Id",
                new { Id = id });
        }

        public async Task<Exam?> GetByCompositeKeyAsync(string subjectCode, int studentId, DateTime examDate)
        {
            using IDbConnection db = _connectionFactory.CreateConnection();
            return await db.QueryFirstOrDefaultAsync<Exam>(
                @"SELECT * FROM Exams 
                  WHERE SubjectCode = :SubjectCode AND StudentId = :StudentId AND ExamDate = :ExamDate",
                new { SubjectCode = subjectCode, StudentId = studentId, ExamDate = examDate });
        }

        public async Task AddAsync(Exam exam)
        {
            using IDbConnection db = _connectionFactory.CreateConnection();
            string sql = @"
                INSERT INTO Exams (SubjectCode, StudentId, ExamDate, Grade)
                VALUES (:SubjectCode, :StudentId, :ExamDate, :Grade)";
            await db.ExecuteAsync(sql, exam);
        }

        public async Task UpdateAsync(Exam exam)
        {
            using IDbConnection db = _connectionFactory.CreateConnection();
            string sql = @"
                UPDATE Exams
                SET Grade = :Grade
                WHERE ExamId = :ExamId";
            await db.ExecuteAsync(sql, exam);
        }

        public async Task DeleteAsync(object id)
        {
            using IDbConnection db = _connectionFactory.CreateConnection();
            await db.ExecuteAsync("DELETE FROM Exams WHERE ExamId = :Id", new { Id = id });
        }

        public async Task<IEnumerable<Exam>> FindAsync(System.Linq.Expressions.Expression<Func<Exam, bool>> predicate)
        {
            throw new NotImplementedException("Predicate-based query is not supported with Dapper.");
        }
    }
}
