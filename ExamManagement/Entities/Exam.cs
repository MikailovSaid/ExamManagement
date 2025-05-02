namespace ExamManagement.Entities
{
    public class Exam
    {
        public int ExamId { get; set; }
        public string SubjectCode { get; set; } = null!;
        public int StudentId { get; set; }
        public DateTime ExamDate { get; set; }
        public int Grade { get; set; }
        public int ClassLevel { get; set; }
    }
}
