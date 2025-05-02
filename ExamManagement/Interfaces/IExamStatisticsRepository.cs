using ExamManagement.Entities;

namespace ExamManagement.Interfaces
{
    public interface IExamStatisticsRepository
    {
        Task AddAsync(ExamStatistics stat);
        Task<IEnumerable<ExamStatistics>> GetByDateAsync(DateTime date);
    }
}
