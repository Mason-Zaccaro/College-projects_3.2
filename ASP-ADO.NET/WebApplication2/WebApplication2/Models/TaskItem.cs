namespace WebApplication2.Models
{
    public class TaskItem
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public TaskStatus Status { get; set; } = TaskStatus.New;
    }

    public enum TaskStatus
    {
        New,
        InProgress,
        Completed
    }
}
