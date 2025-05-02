namespace ExamManagement.Entities
{
    public class Student
    {
        public int StudentId { get; set; }
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public int ClassLevel { get; set; }
    }
}
