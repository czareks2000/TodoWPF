﻿using Todo.MVVM.Model.Enums;
using TaskStatus = Todo.MVVM.Model.Enums.TaskStatus;

namespace Todo.MVVM.Model
{
    public class Task
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public TaskPriority Priority { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime Deadline { get; set; } 
        public TaskStatus Status { get; set; } = TaskStatus.InProgress;

        public virtual ICollection<SubTask> SubTasks { get; set; }
        public virtual ICollection<TaskCategory> Categories { get; set; }
    }
}
