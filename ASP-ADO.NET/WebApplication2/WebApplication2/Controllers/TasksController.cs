using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class TasksController : ControllerBase
{
    private readonly TaskService _taskService;

    public TasksController(TaskService taskService)
    {
        _taskService = taskService;
    }

    [HttpGet]
    public ActionResult<List<TaskItem>> GetAll([FromQuery] TaskItemStatus? status = null)
    {
        return Ok(_taskService.GetAll(status));
    }

    [HttpPost]
    public ActionResult<TaskItem> Create(TaskItem task)
    {
        var created = _taskService.Add(task);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    [HttpGet("{id}")]
    public ActionResult<TaskItem> GetById(int id)
    {
        var task = _taskService.GetById(id);
        if (task == null) return NotFound();
        return Ok(task);
    }

    [HttpPatch("{id}/status")]
    public IActionResult UpdateStatus(int id, [FromBody] TaskItemStatus status)
    {
        if (!_taskService.UpdateStatus(id, status))
            return NotFound();
        return NoContent();
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        if (!_taskService.Delete(id))
            return NotFound();
        return NoContent();
    }
}