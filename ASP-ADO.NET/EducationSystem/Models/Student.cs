using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EducationSystem.Models
{
    public class Student
    {
        public Student()
        {
            // Инициализируем коллекцию при создании объекта
            Courses = new List<Course>();
        }
        
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [Required]
        [StringLength(100)]
        [EmailAddress]
        public string Email { get; set; }

        // Навигационное свойство для отношения многие-ко-многим с Course
        public ICollection<Course> Courses { get; set; }
    }
}
