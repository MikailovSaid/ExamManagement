using ExamManagement.Entities;

namespace ExamManagement.Interfaces
{
    public interface IStudentRepository : IRepository<Student>
    {
        Task<IEnumerable<Student>> GetByClassLevelAsync(int classLevel);
    }
}
