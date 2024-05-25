using System.Net.NetworkInformation;
using Todo.Core;
using Todo.MVVM.Model.Enums;
using TaskStatus = Todo.MVVM.Model.Enums.TaskStatus;

namespace Todo.MVVM.Model
{
    public class Task : ObservableObject
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public TaskPriority Priority { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime Deadline { get; set; }

        private TaskStatus _status = TaskStatus.InProgress;
        public TaskStatus Status
        {
            get => _status;
            set
            {
                if (_status != value)
                {
                    _status = value;
                    OnPropertyChanged();
                }
            }
        } 

        public virtual ICollection<SubTask> SubTasks { get; set; }
        public virtual ICollection<TaskCategory> Categories { get; set; }

    }
}
