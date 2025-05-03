using ExamManagement.Entities;
using ExamManagement.Interfaces;
using Dapper;
using ExamManagement.Data;
using System.Data;
using ExamManagement.Helpers;
using Oracle.ManagedDataAccess.Client;

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
            var parameters = new OracleDynamicParameters();
            parameters.Add("p_Result", dbType: OracleDbType.RefCursor, direction: ParameterDirection.Output);

            return await db.QueryAsync<Subject>(
                "CRUD_SUBJECTS.GetAllSubjects",
                param: parameters,
                commandType: CommandType.StoredProcedure
            );
        }

        public async Task<Subject?> GetByIdAsync(object id)
        {
            using IDbConnection db = _connectionFactory.CreateConnection();
            var parameters = new OracleDynamicParameters();
            parameters.Add("p_SubjectCode", id, OracleDbType.Varchar2, ParameterDirection.Input);
            parameters.Add("p_Result", dbType: OracleDbType.RefCursor, direction: ParameterDirection.Output);

            return await db.QueryFirstOrDefaultAsync<Subject>(
                "CRUD_SUBJECTS.GetSubjectByCode",
                param: parameters,
                commandType: CommandType.StoredProcedure
            );
        }

        public async Task AddAsync(Subject subject)
        {
            using IDbConnection db = _connectionFactory.CreateConnection();
            var parameters = new OracleDynamicParameters();
            parameters.Add("p_SubjectCode", subject.SubjectCode);
            parameters.Add("p_SubjectName", subject.SubjectName);
            parameters.Add("p_ClassLevel", subject.ClassLevel);
            parameters.Add("p_TeacherFirstName", subject.TeacherFirstName);
            parameters.Add("p_TeacherLastName", subject.TeacherLastName);

            await db.ExecuteAsync(
                "CRUD_SUBJECTS.AddSubject",
                param: parameters,
                commandType: CommandType.StoredProcedure
            );
        }

        public async Task UpdateAsync(Subject subject)
        {
            using IDbConnection db = _connectionFactory.CreateConnection();
            var parameters = new OracleDynamicParameters();
            parameters.Add("p_SubjectCode", subject.SubjectCode);
            parameters.Add("p_SubjectName", subject.SubjectName);
            parameters.Add("p_ClassLevel", subject.ClassLevel);
            parameters.Add("p_TeacherFirstName", subject.TeacherFirstName);
            parameters.Add("p_TeacherLastName", subject.TeacherLastName);

            await db.ExecuteAsync(
                "CRUD_SUBJECTS.UpdateSubject",
                param: parameters,
                commandType: CommandType.StoredProcedure
            );
        }

        public async Task DeleteAsync(object id)
        {
            using IDbConnection db = _connectionFactory.CreateConnection();
            var parameters = new OracleDynamicParameters();
            parameters.Add("p_SubjectCode", id);

            await db.ExecuteAsync(
                "CRUD_SUBJECTS.DeleteSubject",
                param: parameters,
                commandType: CommandType.StoredProcedure
            );
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
