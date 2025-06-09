using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EducationSystem.Models
{
    public class Course
    {
        public Course()
        {
            // Инициализируем коллекцию при создании объекта
            Students = new List<Student>();
        }
        
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Title { get; set; }

        [StringLength(500)]
        public string Description { get; set; }

        // Внешний ключ для связи с Teacher
        public int TeacherId { get; set; }

        // Навигационное свойство для отношения многие-к-одному с Teacher
        [ForeignKey("TeacherId")]
        public Teacher Teacher { get; set; }

        // Навигационное свойство для отношения многие-ко-многим со Student
        public ICollection<Student> Students { get; set; }
    }
}
