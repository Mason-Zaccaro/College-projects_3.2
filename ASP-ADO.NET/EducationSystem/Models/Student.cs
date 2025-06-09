using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EducationSystem.Models
{
    public class Student
    {
        public int Id { get; set; }
        
        public required string FirstName { get; set; }
        
        public required string LastName { get; set; }
        
        public required string Email { get; set; }
        
        // Навигационное свойство для курсов, на которые записан студент
        public ICollection<Course> Courses { get; set; } = new List<Course>();
        
        // Вычисляемое свойство для полного имени
        public string FullName => $"{FirstName} {LastName}";
    }
}
