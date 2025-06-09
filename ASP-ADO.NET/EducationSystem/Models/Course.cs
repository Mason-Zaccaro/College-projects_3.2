using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EducationSystem.Models
{
    public class Course
    {
        public int Id { get; set; }
        
        public required string Title { get; set; }
        
        public string? Description { get; set; }
        
        // Внешний ключ для преподавателя
        public int TeacherId { get; set; }
        
        // Навигационное свойство для преподавателя
        public Teacher Teacher { get; set; } = null!;
        
        // Навигационное свойство для студентов, записанных на курс
        public ICollection<Student> Students { get; set; } = new List<Student>();
    }
}
