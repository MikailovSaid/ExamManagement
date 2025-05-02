using ExamManagement.Entities;

namespace ExamManagement.Interfaces
{
    public interface ISubjectRepository : IRepository<Subject>
    {
        Task<IEnumerable<Subject>> GetByClassLevelAsync(int classLevel);
    }
}
