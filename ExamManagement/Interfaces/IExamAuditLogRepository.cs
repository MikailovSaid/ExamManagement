using ExamManagement.Entities;

namespace ExamManagement.Interfaces
{
    public interface IExamAuditLogRepository
    {
        Task<IEnumerable<ExamAuditLog>> GetAllAsync(int? examId);
        Task AddAsync(ExamAuditLog log);
        Task<IEnumerable<ExamAuditLog>> GetLogsByExamIdAsync(int examId);
    }
}
