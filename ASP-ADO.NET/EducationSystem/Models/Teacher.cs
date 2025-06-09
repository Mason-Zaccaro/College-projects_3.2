using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EducationSystem.Models
{
    public class Teacher
    {
        public int Id { get; set; }
        
        public required string FirstName { get; set; }
        
        public required string LastName { get; set; }
        
        public required string Email { get; set; }
        
        // Навигационное свойство для курсов, которые ведет преподаватель
        public ICollection<Course> Courses { get; set; } = new List<Course>();
        
        // Вычисляемое свойство для полного имени
        public string FullName => $"{FirstName} {LastName}";
    }
}
