using ExamManagement.Entities;

namespace ExamManagement.Interfaces
{
    public interface IExamRepository : IRepository<Exam>
    {
        Task<Exam?> GetByCompositeKeyAsync(string subjectCode, int studentId, DateTime examDate);
    }
}
