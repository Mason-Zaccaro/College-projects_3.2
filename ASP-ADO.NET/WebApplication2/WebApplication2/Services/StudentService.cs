using System.Xml.Linq;
using WebApplication2.Models;

namespace WebApplication2.Services
{
    public class StudentService
    {
        private readonly List<Student> _students = new();
        private int _nextId = 1;

        public List<Student> GetAll() => _students;

        public Student? GetById(int id) => _students.FirstOrDefault(s => s.Id == id);

        public Student Add(Student student)
        {
            student.Id = _nextId++;
            _students.Add(student);
            return student;
        }

        public bool Update(int id, Student student)
        {
            var existing = GetById(id);
            if (existing == null) return false;

            existing.Name = student.Name;
            existing.Email = student.Email;
            existing.Age = student.Age;
            return true;
        }

        public bool Delete(int id)
        {
            var student = GetById(id);
            if (student == null) return false;

            _students.Remove(student);
            return true;
        }
    }
}
