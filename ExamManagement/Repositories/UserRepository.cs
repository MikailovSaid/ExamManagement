using ExamManagement.Entities;
using ExamManagement.Interfaces;
using Dapper;
using ExamManagement.Data;

namespace ExamManagement.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly DbConnectionFactory _connectionFactory;

        public UserRepository(DbConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task<User?> GetByUsernameAsync(string username)
        {
            using var db = _connectionFactory.CreateConnection();
            return await db.QueryFirstOrDefaultAsync<User>(
                "SELECT * FROM Users WHERE Username = :Username",
                new { Username = username });
        }

        public async Task AddAsync(User user)
        {
            using var db = _connectionFactory.CreateConnection();
            string sql = @"
                INSERT INTO Users (Username, Password)
                VALUES (:Username, :Password)";
            await db.ExecuteAsync(sql, user);
        }
    }
}
