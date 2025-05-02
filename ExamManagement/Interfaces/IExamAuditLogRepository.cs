using ExamManagement.Entities;

namespace ExamManagement.Interfaces
{
    public interface IExamAuditLogRepository
    {
        Task AddAsync(ExamAuditLog log);
        Task<IEnumerable<ExamAuditLog>> GetLogsByExamIdAsync(int examId);
    }
}
