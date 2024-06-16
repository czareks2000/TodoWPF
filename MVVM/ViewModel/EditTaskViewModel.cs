using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using Todo.Core;
using Todo.DB;
using Todo.MVVM.Model;
using Todo.MVVM.Model.Enums;
using Task = Todo.MVVM.Model.Task;

namespace Todo.MVVM.ViewModel
{
    public class EditTaskViewModel : ObservableObject
    {
        private DataContext _dataContext;
        private Task _selectedTask;
        public Task SelectedTask
        {
            get => _selectedTask;
            set
            {
                _selectedTask = value;
                OnPropertyChanged();
            }
        }
        private ObservableCollection<Category> _categories;
        public ObservableCollection<Category> Categories
        {
            get { return _categories; }
            set { _categories = value; OnPropertyChanged(nameof(Categories)); }
        }

        public Array TaskPriorities => Enum.GetValues(typeof(TaskPriority));
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

        public ICommand DeleteTaskCommand { get; private set; }
        public ICommand SaveTaskCommand { get; private set; }
        public EditTaskViewModel()
        {
            _dataContext = new DataContext();
            Categories = new ObservableCollection<Category>([.. _dataContext.Categories]);
            Subtasks = new ObservableCollection<string>();
            SelectedCategories = new ObservableCollection<Category>();
            Deadline = DateTime.UtcNow;
            DeleteTaskCommand = new RelayCommand(DeleteTask);
            SaveTaskCommand = new RelayCommand(e => SaveTask());
        }
        private void SaveTask()
        {
            if (SelectedTask != null)
            {
               
                TaskName = SelectedTask.Name;
                TaskDescription = SelectedTask.Description;
                SelectedPriority = SelectedTask.Priority;

                if (string.IsNullOrEmpty(TaskName))
                {
                    MessageBox.Show("Enter a name.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;

                }
               SelectedTask.Deadline = Deadline;
                //Deadline = SelectedTask.Deadline;
                //Deadline = DateTime.SpecifyKind(SelectedTask.Deadline, DateTimeKind.Utc);

                // Aktualizacja podzadań
                foreach (var name in Subtasks)
                {
                    SelectedTask.SubTasks.Add(new SubTask { Name = name });
                }

                // Aktualizacja kategorii
                SelectedTask.Categories.Clear();
                foreach (var category in SelectedCategories)
                {
                    var existingCategory = _dataContext.Categories.FirstOrDefault(c => c.Id == category.Id);
                    if (existingCategory != null)
                    {
                        SelectedTask.Categories.Add(new TaskCategory { Category = existingCategory });
                    }
                }

                _dataContext.Tasks.Update(SelectedTask);
                _dataContext.SaveChanges();

                Mediator.Instance.Notify("UpdateEditTask", SelectedTask);
                Mediator.Instance.Notify("UpdateTasksList", SelectedTask);
                
                MessageBoxResult result = MessageBox.Show(
                    "You have successfully edited the task",
                    "Notification",
                    MessageBoxButton.OK
                );
            }



        }
        private void DeleteTask(object obj)
        {
            MessageBoxResult result = MessageBox
                .Show(
                    "Do you want to delete this task?",
                    "Confirm",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Question
                );
            if (result == MessageBoxResult.Yes)
            {
                _dataContext.Tasks.Remove(SelectedTask);
                _dataContext.SaveChanges();

                Mediator.Instance.Notify("UpdateTasksList", null);
            }

        }
    }
}
