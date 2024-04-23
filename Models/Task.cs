using Todo.Models.Enums;
using TaskStatus = Todo.Models.Enums.TaskStatus;

namespace Todo.Models
{
    public class Task
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public TaskPriority Priority { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime Deadline { get; set; }
        public TaskStatus Status { get; set; }

        public virtual ICollection<SubTask> SubTasks { get; set; }
        public virtual ICollection<TaskCategory> Categories { get; set; }
    }
}
