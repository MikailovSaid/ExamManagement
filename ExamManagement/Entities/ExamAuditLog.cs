namespace ExamManagement.Entities
{
    public class ExamAuditLog
    {
        public int LogId { get; set; }
        public int ExamId { get; set; }
        public int OldGrade { get; set; }
        public int NewGrade { get; set; }
        public string ChangedBy { get; set; } = null!;
        public DateTime ChangedAt { get; set; }
    }
}
