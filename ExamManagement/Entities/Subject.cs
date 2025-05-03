using System.ComponentModel.DataAnnotations;

namespace ExamManagement.Entities
{
    public class Subject
    {
        [MaxLength(20)]
        public string SubjectCode { get; set; } = null!;
        [MaxLength(100)]
        public string SubjectName { get; set; } = null!;
        [Range(1, 11, ErrorMessage = "Value must be between 1 and 11.")]
        public int ClassLevel { get; set; }
        [MaxLength(50)]
        public string TeacherFirstName { get; set; } = null!;
        [MaxLength(50)]
        public string TeacherLastName { get; set; } = null!;
    }
}
