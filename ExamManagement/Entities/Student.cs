using System.ComponentModel.DataAnnotations;

namespace ExamManagement.Entities
{
    public class Student
    {
        public int StudentId { get; set; }
        [MaxLength(50)]
        public string FirstName { get; set; } = null!;
        [MaxLength(50)]
        public string LastName { get; set; } = null!;
        [Range(1, 11, ErrorMessage = "Value must be between 1 and 11.")]
        public int ClassLevel { get; set; }
    }
}
