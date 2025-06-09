using System.ComponentModel.DataAnnotations;

namespace EducationSystem.Models
{
    public class Teacher
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        public ICollection<Course> Courses { get; set; } = new List<Course>();
    }
}