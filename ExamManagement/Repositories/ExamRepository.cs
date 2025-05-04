using ExamManagement.Entities;
using ExamManagement.Interfaces;
using Dapper;
using ExamManagement.Data;
using System.Data;
using ExamManagement.Helpers;
using Oracle.ManagedDataAccess.Client;

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

            var parameters = new OracleDynamicParameters();
            parameters.Add("p_result", dbType: OracleDbType.RefCursor, direction: ParameterDirection.Output);

            var result = await db.QueryAsync<Exam>(
                "CRUD_EXAMS.GetAll",
                param: parameters,
                commandType: CommandType.StoredProcedure);

            return result;
        }

        public async Task<Exam?> GetByIdAsync(object id)
        {
            using IDbConnection db = _connectionFactory.CreateConnection();

            var parameters = new OracleDynamicParameters();
            parameters.Add("p_exam_id", id, OracleDbType.Int32, ParameterDirection.Input);
            parameters.Add("p_result", dbType: OracleDbType.RefCursor, direction: ParameterDirection.Output);

            var result = await db.QueryAsync<Exam>(
                "CRUD_EXAMS.GetById",
                param: parameters,
                commandType: CommandType.StoredProcedure);

            return result.FirstOrDefault();
        }

        public async Task<Exam?> GetByCompositeKeyAsync(int? examId, string subjectCode, int studentId, DateTime examDate)
        {
            using IDbConnection db = _connectionFactory.CreateConnection();

            var parameters = new OracleDynamicParameters();
            parameters.Add("p_exam_id", examId, OracleDbType.Int32, ParameterDirection.Input);
            parameters.Add("p_subject_code", subjectCode, OracleDbType.Varchar2, ParameterDirection.Input);
            parameters.Add("p_student_id", studentId, OracleDbType.Int32, ParameterDirection.Input);
            parameters.Add("p_exam_date", examDate.ToString("yyyy-MM-dd"), OracleDbType.Varchar2, ParameterDirection.Input);
            parameters.Add("p_result", dbType: OracleDbType.RefCursor, direction: ParameterDirection.Output);

            var result = await db.QueryAsync<Exam>(
                "CRUD_EXAMS.GetByCompositeKey",
                param: parameters,
                commandType: CommandType.StoredProcedure);

            return result.FirstOrDefault();
        }

        public async Task AddAsync(Exam exam)
        {
            using IDbConnection db = _connectionFactory.CreateConnection();

            var parameters = new OracleDynamicParameters();
            parameters.Add("p_subject_code", exam.SubjectCode, OracleDbType.Varchar2, ParameterDirection.Input);
            parameters.Add("p_student_id", exam.StudentId, OracleDbType.Int32, ParameterDirection.Input);
            parameters.Add("p_exam_date", exam.ExamDate, OracleDbType.Date, ParameterDirection.Input);
            parameters.Add("p_grade", exam.Grade, OracleDbType.Int32, ParameterDirection.Input);

            await db.ExecuteAsync(
                "CRUD_EXAMS.AddExam",
                param: parameters,
                commandType: CommandType.StoredProcedure);
        }

        public async Task UpdateAsync(Exam exam)
        {
            using IDbConnection db = _connectionFactory.CreateConnection();

            var parameters = new OracleDynamicParameters();
            parameters.Add("p_subject_code", exam.SubjectCode, OracleDbType.Varchar2, ParameterDirection.Input);
            parameters.Add("p_student_id", exam.StudentId, OracleDbType.Int32, ParameterDirection.Input);
            parameters.Add("p_exam_id", exam.ExamId, OracleDbType.Int32, ParameterDirection.Input);
            parameters.Add("p_grade", exam.Grade, OracleDbType.Int32, ParameterDirection.Input);

            await db.ExecuteAsync(
                "CRUD_EXAMS.UpdateExam",
                param: parameters,
                commandType: CommandType.StoredProcedure);
        }

        public async Task DeleteAsync(object id)
        {
            using IDbConnection db = _connectionFactory.CreateConnection();

            var parameters = new OracleDynamicParameters();
            parameters.Add("p_exam_id", id, OracleDbType.Int32, ParameterDirection.Input);

            await db.ExecuteAsync(
                "CRUD_EXAMS.DeleteExam",
                param: parameters,
                commandType: CommandType.StoredProcedure);
        }
    }
}
