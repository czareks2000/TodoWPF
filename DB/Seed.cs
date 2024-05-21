using Task = Todo.MVVM.Model.Task;
using TaskStatus = Todo.MVVM.Model.Enums.TaskStatus;
using Todo.MVVM.Model;
using Todo.MVVM.Model.Enums;

namespace Todo.DB
{
    public class Seed
    {
        public static void SeedData(DataContext context)
        {
            if (context.Tasks.Any())
                return;

            var categories = new List<Category>()
            {
                new Category { Name = "Home" },
                new Category { Name = "Work" },
                new Category { Name = "School" },
                new Category { Name = "Shopping" }
            };

            context.Categories.AddRange(categories);
            context.SaveChanges();

            var tasks = new List<Task>
            {
                new Task
                {
                    Name = "Task 1",
                    Description = "Description of Task 1",
                    Priority = TaskPriority.High,
                    CreatedAt = DateTime.UtcNow,
                    Deadline = DateTime.UtcNow.AddDays(7),
                    Categories = new List<TaskCategory>
                    {
                        new TaskCategory { Category = categories[0] },
                        new TaskCategory { Category = categories[3] }
                    },
                    SubTasks = new List<SubTask>
                    {
                        new SubTask { Name="SubTask 1" },
                        new SubTask { Name="SubTask 2" },
                        new SubTask { Name="SubTask 3" },
                        new SubTask { Name="SubTask 4", Status = TaskStatus.Done },
                    }
                },
                new Task
                {
                    Name = "Task 2",
                    Description = "Description of Task 2",
                    Priority = TaskPriority.Medium,
                    CreatedAt = DateTime.UtcNow,
                    Deadline = DateTime.UtcNow.AddDays(5),
                    Status = TaskStatus.Done,
                    Categories = new List<TaskCategory>
                    {
                        new TaskCategory { Category = categories[1] }
                    }
                },
                new Task
                {
                    Name = "Task 3",
                    Description = "Description of Task 3",
                    Priority = TaskPriority.Low,
                    CreatedAt = DateTime.UtcNow,
                    Deadline = DateTime.UtcNow.AddDays(10),
                    Categories = new List<TaskCategory>
                    {
                        new TaskCategory { Category = categories[2] }
                    }
                }
            };

            context.Tasks.AddRange(tasks);
            context.SaveChanges();
        }
    }
}
