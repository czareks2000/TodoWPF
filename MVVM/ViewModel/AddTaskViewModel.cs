using System.Collections.ObjectModel;
using System.Windows.Input;
using Todo.Core;
using Todo.DB;
using Todo.MVVM.Model;
using Todo.MVVM.Model.Enums;
using Task = Todo.MVVM.Model.Task;

namespace Todo.MVVM.ViewModel
{
    public class AddTaskViewModel : ObservableObject
    {
        private DataContext _dataContext;

        private ObservableCollection<Category> _categories;
        public ObservableCollection<Category> Categories
        {
            get { return _categories; } 
            set { _categories = value; OnPropertyChanged(nameof(Categories)); }
        }

        public Array TaskPriorities => Enum.GetValues(typeof(TaskPriority));

        // pola formularza
        private string _taskName;
        public string TaskName
        {
            get { return _taskName; }
            set { _taskName = value; OnPropertyChanged(nameof(TaskName)); }
        }

        private string _taskDescription;
        public string TaskDescription
        {
            get { return _taskDescription; }
            set { _taskDescription = value; OnPropertyChanged(nameof(TaskDescription)); }
        }

        private TaskPriority _selectedPriority;
        public TaskPriority SelectedPriority
        {
            get { return _selectedPriority; }
            set { _selectedPriority = value; OnPropertyChanged(nameof(SelectedPriority)); }
        }

        private ObservableCollection<Category> _selectedCategories;
        public ObservableCollection<Category> SelectedCategories
        {
            get { return _selectedCategories; }
            set { _selectedCategories = value; OnPropertyChanged(nameof(SelectedCategories)); }
        }

        private DateTime _deadline;
        public DateTime Deadline
        {
            get { return _deadline; }
            set { _deadline = value; OnPropertyChanged(nameof(Deadline)); }
        }

        private ObservableCollection<string> _subtasks;
        public ObservableCollection<string> Subtasks
        {
            get { return _subtasks; }
            set { _subtasks = value; OnPropertyChanged(nameof(Subtasks)); }
        }

        public ICommand SaveTaskCommand { get; }

        public AddTaskViewModel()
        {
            _dataContext = new DataContext();

            Categories = new ObservableCollection<Category>([.. _dataContext.Categories]);
            Subtasks = new ObservableCollection<string>();

            SelectedCategories = new ObservableCollection<Category>();
            Deadline = DateTime.UtcNow;

            SaveTaskCommand = new RelayCommand(e => SaveTask());
        }

        private void SaveTask()
        {
            var newTask = new Task
            {
                Name = TaskName,
                Description = TaskDescription,
                Priority = SelectedPriority,
                Deadline = DateTime.SpecifyKind(Deadline, DateTimeKind.Utc)
            };

            var newSubTasks = new List<SubTask>();

            foreach (var name in Subtasks)
            {
                newSubTasks.Add(new SubTask{ Name = name });
            }

            newTask.SubTasks = newSubTasks;

            var newTaskCategories = new List<TaskCategory>();
            foreach (var category in SelectedCategories)
            {
                newTaskCategories.Add(new TaskCategory { Category = category });
            }

            newTask.Categories = newTaskCategories;

            _dataContext.Tasks.Add(newTask);

            _dataContext.SaveChanges();

            Mediator.Instance.Notify("UpdateTasks", newTask);

            ResetForm();
        }

        private void ResetForm()
        {
            TaskName = string.Empty;
            TaskDescription = string.Empty;
            SelectedPriority = TaskPriority.Low;
            SelectedCategories.Clear();
            Deadline = DateTime.UtcNow;
            Subtasks.Clear();
        }
    }
}
