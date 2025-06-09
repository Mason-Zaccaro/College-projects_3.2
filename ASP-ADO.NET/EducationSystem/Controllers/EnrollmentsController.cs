using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EducationSystem.Data;
using EducationSystem.Models;
using System.Linq;

namespace EducationSystem.Controllers
{
    public class EnrollmentsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public EnrollmentsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Enrollments
        public async Task<IActionResult> Index(int? studentId, int? courseId)
        {
            IQueryable<Course> courses = _context.Courses
                .Include(c => c.Teacher);

            IQueryable<Student> students = _context.Students;

            if (studentId != null)
            {
                // Показать курсы, на которые записан студент
                var student = await _context.Students
                    .Include(s => s.Courses)
                    .FirstOrDefaultAsync(s => s.Id == studentId);
                
                if (student != null)
                {
                    ViewBag.Student = student;
                    var enrolledCourseIds = student.Courses.Select(c => c.Id);
                    courses = courses.Where(c => !enrolledCourseIds.Contains(c.Id));
                    
                    ViewBag.EnrolledCourses = await _context.Courses
                        .Where(c => enrolledCourseIds.Contains(c.Id))
                        .Include(c => c.Teacher)
                        .ToListAsync();
                }
            }
            else if (courseId != null)
            {
                // Показать студентов, записанных на курс
                var course = await _context.Courses
                    .Include(c => c.Students)
                    .FirstOrDefaultAsync(c => c.Id == courseId);
                
                if (course != null)
                {
                    ViewBag.Course = course;
                    var enrolledStudentIds = course.Students.Select(s => s.Id);
                    students = students.Where(s => !enrolledStudentIds.Contains(s.Id));
                    
                    ViewBag.EnrolledStudents = await _context.Students
                        .Where(s => enrolledStudentIds.Contains(s.Id))
                        .ToListAsync();
                }
            }

            ViewBag.Courses = await courses.ToListAsync();
            ViewBag.Students = await students.ToListAsync();
            
            return View();
        }

        // POST: Enrollments/Enroll
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Enroll(int studentId, int courseId)
        {
            var student = await _context.Students
                .Include(s => s.Courses)
                .FirstOrDefaultAsync(s => s.Id == studentId);
                
            var course = await _context.Courses.FindAsync(courseId);
            
            if (student != null && course != null)
            {
                if (!student.Courses.Any(c => c.Id == courseId))
                {
                    student.Courses.Add(course);
                    await _context.SaveChangesAsync();
                }
            }
            
            return RedirectToAction(nameof(Index), new { studentId });
        }
        
        // POST: Enrollments/Unenroll
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Unenroll(int studentId, int courseId)
        {
            var student = await _context.Students
                .Include(s => s.Courses)
                .FirstOrDefaultAsync(s => s.Id == studentId);
                
            var course = await _context.Courses.FindAsync(courseId);
            
            if (student != null && course != null)
            {
                student.Courses.Remove(course);
                await _context.SaveChangesAsync();
            }
            
            return RedirectToAction(nameof(Index), new { studentId });
        }
    }
}
