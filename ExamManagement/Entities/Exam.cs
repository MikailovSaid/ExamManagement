using System.ComponentModel.DataAnnotations;

namespace ExamManagement.Entities
{
    public class Exam
    {
        public int ExamId { get; set; }
        [MaxLength(20)]
        public string SubjectCode { get; set; } = null!;
        public int StudentId { get; set; }
        [Required]
        public DateTime ExamDate { get; set; }
        [Range(1, 5, ErrorMessage = "Value must be between 1 and 5.")]
        public int Grade { get; set; }
        [Range(1, 11, ErrorMessage = "Value must be between 1 and 11.")]
        public int? ClassLevel { get; set; }
        public string? SubjectName { get; set; }
        public string? StudentName { get; set; }
        public string? StudentSurname { get; set; }
    }
}
