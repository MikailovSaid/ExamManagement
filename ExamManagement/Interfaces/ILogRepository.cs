using ExamManagement.Entities;

namespace ExamManagement.Interfaces
{
    public interface ILogRepository
    {
        Task AddAsync(Log log);
        Task<IEnumerable<Log>> GetLogsAsync(string? serviceName = null);
    }
}
