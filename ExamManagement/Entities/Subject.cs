namespace ExamManagement.Entities
{
    public class Subject
    {
        public string SubjectCode { get; set; } = null!;
        public string SubjectName { get; set; } = null!;
        public int ClassLevel { get; set; }
        public string TeacherFirstName { get; set; } = null!;
        public string TeacherLastName { get; set; } = null!;
    }
}
