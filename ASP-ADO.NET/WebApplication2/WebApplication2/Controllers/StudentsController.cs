using Microsoft.AspNetCore.Mvc;
using WebApplication2.Models;
using WebApplication2.Services;

[ApiController]
[Route("api/[controller]")]
public class StudentsController : ControllerBase
{
    private readonly StudentService _studentService;

    public StudentsController(StudentService studentService)
    {
        _studentService = studentService;
    }

    [HttpGet]
    public ActionResult<List<Student>> GetAll()
    {
        return Ok(_studentService.GetAll());
    }

    [HttpGet("{id}")]
    public ActionResult<Student> GetById(int id)
    {
        var student = _studentService.GetById(id);
        if (student == null) return NotFound();
        return Ok(student);
    }

    [HttpPost]
    public ActionResult<Student> Create(Student student)
    {
        var created = _studentService.Add(student);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    [HttpPut("{id}")]
    public IActionResult Update(int id, Student student)
    {
        if (!_studentService.Update(id, student))
            return NotFound();
        return NoContent();
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        if (!_studentService.Delete(id))
            return NotFound();
        return NoContent();
    }
}