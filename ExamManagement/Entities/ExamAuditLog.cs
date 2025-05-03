using System.ComponentModel.DataAnnotations;

namespace ExamManagement.Entities
{
    public class ExamAuditLog
    {
        public int LogId { get; set; }
        public int ExamId { get; set; }
        [Range(1, 5, ErrorMessage = "Value must be between 1 and 5.")]
        public int OldGrade { get; set; }
        [Range(1, 5, ErrorMessage = "Value must be between 1 and 5.")]
        public int NewGrade { get; set; }
        [MaxLength(50)]
        public string ChangedBy { get; set; } = null!;
        public DateTime ChangedAt { get; set; } = DateTime.Now;
    }
}
