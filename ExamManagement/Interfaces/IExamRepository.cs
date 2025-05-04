using ExamManagement.Entities;

namespace ExamManagement.Interfaces
{
    public interface IExamRepository : IRepository<Exam>
    {
        Task<Exam?> GetByCompositeKeyAsync(int? examId, string subjectCode, int studentId, DateTime examDate);
    }
}
