using ExamManagement.Entities;
using ExamManagement.Interfaces;
using Dapper;
using ExamManagement.Data;
using System.Data;

namespace ExamManagement.Repositories
{
    public class StudentRepository : IStudentRepository
    {
        private readonly DbConnectionFactory _connectionFactory;

        public StudentRepository(DbConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task<IEnumerable<Student>> GetAllAsync()
        {
            using IDbConnection db = _connectionFactory.CreateConnection();
            var data = await db.QueryAsync<Student>("SELECT * FROM Students");
            return data;
        }

        public async Task<Student?> GetByIdAsync(object id)
        {
            using IDbConnection db = _connectionFactory.CreateConnection();
            return await db.QueryFirstOrDefaultAsync<Student>(
                "SELECT * FROM Students WHERE StudentId = :Id",
                new { Id = id });
        }

        public async Task<IEnumerable<Student>> GetByClassLevelAsync(int classLevel)
        {
            using IDbConnection db = _connectionFactory.CreateConnection();
            return await db.QueryAsync<Student>(
                "SELECT * FROM Students WHERE ClassLevel = :ClassLevel",
                new { ClassLevel = classLevel });
        }

        public async Task AddAsync(Student student)
        {
            using IDbConnection db = _connectionFactory.CreateConnection();
            string sql = @"
                INSERT INTO Students (FirstName, LastName, ClassLevel)
                VALUES (:FirstName, :LastName, :ClassLevel)";
            await db.ExecuteAsync(sql, student);
        }

        public async Task UpdateAsync(Student student)
        {
            using IDbConnection db = _connectionFactory.CreateConnection();
            string sql = @"
                UPDATE Students
                SET FirstName = :FirstName,
                    LastName = :LastName,
                    ClassLevel = :ClassLevel
                WHERE StudentId = :StudentId";
            await db.ExecuteAsync(sql, student);
        }

        public async Task DeleteAsync(object id)
        {
            using IDbConnection db = _connectionFactory.CreateConnection();
            await db.ExecuteAsync("DELETE FROM Students WHERE StudentId = :Id", new { Id = id });
        }

        public async Task<IEnumerable<Student>> FindAsync(System.Linq.Expressions.Expression<Func<Student, bool>> predicate)
        {
            throw new NotImplementedException("Predicate-based query is not supported with Dapper.");
        }
    }
}
