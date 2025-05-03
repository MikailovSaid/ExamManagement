using System.ComponentModel.DataAnnotations;

namespace ExamManagement.Entities
{
    public class ExamStatistics
    {
        public int StatId { get; set; }
        [MaxLength(20)]
        public string SubjectCode { get; set; } = null!;
        public int ClassLevel { get; set; }
        public double AverageGrade { get; set; }
        public int MaxGrade { get; set; }
        public int MinGrade { get; set; }
        public int ExamCount { get; set; }
        public DateTime CalculatedAt { get; set; }
    }
}
