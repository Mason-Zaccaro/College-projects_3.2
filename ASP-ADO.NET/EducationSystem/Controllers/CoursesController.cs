using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using EducationSystem.Data;
using EducationSystem.Models;

namespace EducationSystem.Controllers
{
    public class CoursesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CoursesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Courses
        public async Task<IActionResult> Index()
        {
            var courses = await _context.Courses
                .Include(c => c.Teacher)
                .ToListAsync();
            return View(courses);
        }

        // GET: Courses/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var course = await _context.Courses
                .Include(c => c.Teacher)
                .Include(c => c.Students)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (course == null)
            {
                return NotFound();
            }

            return View(course);
        }

        // GET: Courses/Create
        public async Task<IActionResult> Create()
        {
            // Проверяем наличие преподавателей
            var teachers = await _context.Teachers.ToListAsync();
            if (!teachers.Any())
            {
                ViewBag.NoTeachers = true;
                return View();
            }
            
            ViewBag.TeacherId = new SelectList(teachers, "Id", "Name");
            return View();
        }

        // POST: Courses/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,Description,TeacherId")] Course course)
        {
            // Проверяем, есть ли преподаватель с указанным ID
            var teacher = await _context.Teachers.FindAsync(course.TeacherId);
            if (teacher == null)
            {
                ModelState.AddModelError("TeacherId", "Необходимо выбрать преподавателя");
            }
            
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Add(course);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", $"Ошибка при сохранении: {ex.Message}");
                }
            }
            
            ViewBag.TeacherId = new SelectList(await _context.Teachers.ToListAsync(), "Id", "Name", course.TeacherId);
            return View(course);
        }

        // GET: Courses/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var course = await _context.Courses
                .Include(c => c.Students)
                .FirstOrDefaultAsync(c => c.Id == id);
            
            if (course == null)
            {
                return NotFound();
            }
            
            ViewBag.TeacherId = new SelectList(_context.Teachers, "Id", "Name", course.TeacherId);
            ViewBag.AllStudents = await _context.Students.ToListAsync();
            
            return View(course);
        }

        // POST: Courses/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Description,TeacherId")] Course course, int[] selectedStudents)
        {
            if (id != course.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var courseToUpdate = await _context.Courses
                        .Include(c => c.Students)
                        .FirstOrDefaultAsync(c => c.Id == id);

                    if (courseToUpdate == null)
                    {
                        return NotFound();
                    }

                    courseToUpdate.Title = course.Title;
                    courseToUpdate.Description = course.Description;
                    courseToUpdate.TeacherId = course.TeacherId;

                    // Очищаем существующих студентов
                    courseToUpdate.Students.Clear();

                    // Добавляем выбранных студентов
                    if (selectedStudents != null)
                    {
                        var selectedStudentsToAdd = await _context.Students
                            .Where(s => selectedStudents.Contains(s.Id))
                            .ToListAsync();
                        
                        foreach (var student in selectedStudentsToAdd)
                        {
                            courseToUpdate.Students.Add(student);
                        }
                    }

                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CourseExists(course.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewBag.TeacherId = new SelectList(_context.Teachers, "Id", "Name", course.TeacherId);
            ViewBag.AllStudents = await _context.Students.ToListAsync();
            return View(course);
        }

        // GET: Courses/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var course = await _context.Courses
                .Include(c => c.Teacher)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (course == null)
            {
                return NotFound();
            }

            return View(course);
        }

        // POST: Courses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var course = await _context.Courses.FindAsync(id);
            if (course != null)
            {
                _context.Courses.Remove(course);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        private bool CourseExists(int id)
        {
            return _context.Courses.Any(e => e.Id == id);
        }
    }
}
