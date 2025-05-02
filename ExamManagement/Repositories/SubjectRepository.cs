using ExamManagement.Entities;
using ExamManagement.Interfaces;
using Dapper;
using ExamManagement.Data;
using System.Data;

namespace ExamManagement.Repositories
{
    public class SubjectRepository : ISubjectRepository
    {
        private readonly DbConnectionFactory _connectionFactory;

        public SubjectRepository(DbConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task<IEnumerable<Subject>> GetAllAsync()
        {
            using IDbConnection db = _connectionFactory.CreateConnection();
            return await db.QueryAsync<Subject>("SELECT * FROM Subjects");
        }

        public async Task<Subject?> GetByIdAsync(object id)
        {
            using IDbConnection db = _connectionFactory.CreateConnection();
            return await db.QueryFirstOrDefaultAsync<Subject>(
                "SELECT * FROM Subjects WHERE SubjectCode = :Code",
                new { Code = id });
        }

        public async Task AddAsync(Subject subject)
        {
            using IDbConnection db = _connectionFactory.CreateConnection();
            string sql = @"
                INSERT INTO Subjects (SubjectCode, SubjectName, ClassLevel, TeacherFirstName, TeacherLastName)
                VALUES (:SubjectCode, :SubjectName, :ClassLevel, :TeacherFirstName, :TeacherLastName)";
            await db.ExecuteAsync(sql, subject);
        }

        public async Task UpdateAsync(Subject subject)
        {
            using IDbConnection db = _connectionFactory.CreateConnection();
            string sql = @"
                UPDATE Subjects
                SET SubjectName = :SubjectName,
                    ClassLevel = :ClassLevel,
                    TeacherFirstName = :TeacherFirstName,
                    TeacherLastName = :TeacherLastName
                WHERE SubjectCode = :SubjectCode";
            await db.ExecuteAsync(sql, subject);
        }

        public async Task DeleteAsync(object id)
        {
            using IDbConnection db = _connectionFactory.CreateConnection();
            await db.ExecuteAsync("DELETE FROM Subjects WHERE SubjectCode = :Code", new { Code = id });
        }

        public async Task<IEnumerable<Subject>> FindAsync(System.Linq.Expressions.Expression<Func<Subject, bool>> predicate)
        {
            throw new NotImplementedException("Predicate-based query is not supported directly with Dapper.");
        }

        public async Task<IEnumerable<Subject>> GetByClassLevelAsync(int classLevel)
        {
            using IDbConnection db = _connectionFactory.CreateConnection();
            return await db.QueryAsync<Subject>(
                "SELECT * FROM Subjects WHERE ClassLevel = :ClassLevel",
                new { ClassLevel = classLevel });
        }
    }
}
