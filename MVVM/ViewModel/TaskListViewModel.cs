using System.Collections.ObjectModel;
using System.Windows.Input;
using Todo.Core;
using Todo.DB;
using Todo.MVVM.Model.Enums;
using Todo.MVVM.Model;
using Task = Todo.MVVM.Model.Task;
using TaskStatus = Todo.MVVM.Model.Enums.TaskStatus;

namespace Todo.MVVM.ViewModel
{
    public class TaskListViewModel : ObservableObject
    {
        private MainViewModel _mainViewModel;
        private DataContext _dataContext;

        // Tworzenie kolekcji na potrzeby ListView
        private ObservableCollection<Task> _tasks;
        public ObservableCollection<Task> Tasks
        {
            get { return _tasks; }
            private set { _tasks = value; OnPropertyChanged(nameof(Tasks)); }
        }

        private ObservableCollection<Category> _categories;
        public ObservableCollection<Category> Categories
        {
            get { return _categories; }
            set { _categories = value; OnPropertyChanged(nameof(Categories)); }
        }

        public Array TaskPriorities => Enum.GetValues(typeof(TaskPriority));
        public Array TaskStatuses => Enum.GetValues(typeof(TaskStatus));

        // Komendy
        public ICommand ShowDetailsCommand { get; private set; }
        public ICommand ShowEditTaskCommand { get; private set; }

        public TaskListViewModel(MainViewModel mainViewModel)
        {
            _mainViewModel = mainViewModel;
            _dataContext = new DataContext();

            // Inicjalizacja komend
            ShowDetailsCommand = new RelayCommand(ShowDetails);
            ShowEditTaskCommand = new RelayCommand(ShowEditTask);

            // Inicjalizacja kolekcji
            Tasks = new ObservableCollection<Task>([.. _dataContext.Tasks]);
            Categories = new ObservableCollection<Category>([.. _dataContext.Categories]);
        }

        // Metoda wywoływana po naciśnięciu przycisku "Details"
        private void ShowDetails(object obj)
        {
            _mainViewModel.DetailsViewCommand.Execute(null);
        }

        // Metoda wywoływana po naciśnięciu przycisku "Edit Task"
        private void ShowEditTask(object obj)
        {
            _mainViewModel.EditTaskViewCommand.Execute(null);
        }
    }
}
