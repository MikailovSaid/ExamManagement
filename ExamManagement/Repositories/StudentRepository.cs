using ExamManagement.Entities;
using ExamManagement.Interfaces;
using Dapper;
using ExamManagement.Data;
using System.Data;
using Oracle.ManagedDataAccess.Client;
using ExamManagement.Helpers;

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

            var parameters = new OracleDynamicParameters();
            parameters.Add("p_Result", dbType: OracleDbType.RefCursor, direction: ParameterDirection.Output);

            var result = await db.QueryAsync<Student>(
                "CRUD_STUDENTS.GetAllStudents",
                param: parameters,
                commandType: CommandType.StoredProcedure);

            return result;
        }

        public async Task<Student?> GetByIdAsync(object id)
        {
            using IDbConnection db = _connectionFactory.CreateConnection();

            var parameters = new OracleDynamicParameters();
            parameters.Add("p_StudentId", id, OracleDbType.Int32, ParameterDirection.Input);
            parameters.Add("p_Result", dbType: OracleDbType.RefCursor, direction: ParameterDirection.Output);

            var result = await db.QueryAsync<Student>(
                "CRUD_STUDENTS.GetStudentById",
                param: parameters,
                commandType: CommandType.StoredProcedure);

            return result.FirstOrDefault();
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

            var parameters = new OracleDynamicParameters();
            parameters.Add("p_FirstName", student.FirstName, OracleDbType.Varchar2, ParameterDirection.Input);
            parameters.Add("p_LastName", student.LastName, OracleDbType.Varchar2, ParameterDirection.Input);
            parameters.Add("p_ClassLevel", student.ClassLevel, OracleDbType.Int32, ParameterDirection.Input);

            await db.ExecuteAsync(
                "CRUD_STUDENTS.InsertStudent",
                param: parameters,
                commandType: CommandType.StoredProcedure);
        }

        public async Task UpdateAsync(Student student)
        {
            using IDbConnection db = _connectionFactory.CreateConnection();

            var parameters = new OracleDynamicParameters();
            parameters.Add("p_StudentId", student.StudentId, OracleDbType.Int32, ParameterDirection.Input);
            parameters.Add("p_FirstName", student.FirstName, OracleDbType.Varchar2, ParameterDirection.Input);
            parameters.Add("p_LastName", student.LastName, OracleDbType.Varchar2, ParameterDirection.Input);
            parameters.Add("p_ClassLevel", student.ClassLevel, OracleDbType.Int32, ParameterDirection.Input);

            await db.ExecuteAsync(
                "CRUD_STUDENTS.UpdateStudent",
                param: parameters,
                commandType: CommandType.StoredProcedure);
        }

        public async Task DeleteAsync(object id)
        {
            using IDbConnection db = _connectionFactory.CreateConnection();

            var parameters = new OracleDynamicParameters();
            parameters.Add("p_StudentId", id, OracleDbType.Int32, ParameterDirection.Input);

            await db.ExecuteAsync(
                "CRUD_STUDENTS.DeleteStudent",
                param: parameters,
                commandType: CommandType.StoredProcedure);
        }
    }

}
