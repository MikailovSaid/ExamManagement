using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExamManagement.Entities
{
    public class User
    {
        public int UserId { get; set; }
        [MaxLength(50)]
        public string Username { get; set; } = null!;
        [Range(8, 16, ErrorMessage = "Value must be between 8 and 16.")]
        public string Password { get; set; } = null!;
    }
}
