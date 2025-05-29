public class TaskService
{
    private readonly List<TaskItem> _tasks = new();
    private int _nextId = 1;

    public List<TaskItem> GetAll(TaskItemStatus? status = null)
    {
        if (status.HasValue)
            return _tasks.Where(t => t.Status == status.Value).ToList();
        return _tasks;
    }

    public TaskItem? GetById(int id) => _tasks.FirstOrDefault(t => t.Id == id);

    public TaskItem Add(TaskItem task)
    {
        task.Id = _nextId++;
        _tasks.Add(task);
        return task;
    }

    public bool UpdateStatus(int id, TaskItemStatus status)
    {
        var task = GetById(id);
        if (task == null) return false;

        task.Status = status;
        return true;
    }

    public bool Delete(int id)
    {
        var task = GetById(id);
        if (task == null) return false;

        _tasks.Remove(task);
        return true;
    }
}