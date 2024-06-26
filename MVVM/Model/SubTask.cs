﻿using TaskStatus = Todo.MVVM.Model.Enums.TaskStatus;

namespace Todo.MVVM.Model
{
    public class SubTask
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int TaskId { get; set; }
        public TaskStatus Status { get; set; } = TaskStatus.InProgress;

        public virtual Task Task { get; set; }
    }
}
