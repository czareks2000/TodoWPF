using System.Collections.ObjectModel;
using System.Windows.Input;
using Todo.Core;
using Todo.DB;
using Todo.MVVM.Model.Enums;
using Todo.MVVM.Model;
using Task = Todo.MVVM.Model.Task;
using TaskStatus = Todo.MVVM.Model.Enums.TaskStatus;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel;
using System.Windows.Data;
using System.Threading.Tasks;

namespace Todo.MVVM.ViewModel
{
    public class TaskListViewModel : ObservableObject
    {
        private MainViewModel _mainViewModel;
        private DataContext _dataContext;
        private ICollectionView _tasksView;

        public ICollectionView TasksView => _tasksView;

        private ObservableCollection<Task> _tasks;
        public ObservableCollection<Task> Tasks
        {
            get { return _tasks; }
            private set
            {
                if (_tasks != null)
                {
                    foreach (var task in _tasks)
                    {
                        task.PropertyChanged -= OnTaskPropertyChanged;
                    }
                }

                _tasks = value;

                if (_tasks != null)
                {
                    foreach (var task in _tasks)
                    {
                        task.PropertyChanged += OnTaskPropertyChanged;
                    }
                }

                _tasksView = CollectionViewSource.GetDefaultView(_tasks);
                _tasksView.Filter = FilterTasks;
                OnPropertyChanged(nameof(Tasks));
                OnPropertyChanged(nameof(TasksView));
            }
        }

        private ObservableCollection<Category> _categories;
        public ObservableCollection<Category> Categories
        {
            get { return _categories; }
            set { _categories = value; OnPropertyChanged(nameof(Categories)); }
        }

        public Array TaskPriorities => Enum.GetValues(typeof(TaskPriority));
        public Array TaskStatuses => Enum.GetValues(typeof(TaskStatus));

        // Filtry
        private string _searchBoxFilter;
        public string SearchBoxFilter
        {
            get { return _searchBoxFilter; }
            set
            {
                _searchBoxFilter = value;
                OnPropertyChanged(nameof(SearchBoxFilter));
                ApplyFilter();
            }
        }

        private Category _selectedCategoryFilter;
        public Category SelectedCategoryFilter
        {
            get { return _selectedCategoryFilter; }
            set
            {
                _selectedCategoryFilter = value;
                OnPropertyChanged(nameof(SelectedCategoryFilter));
                ApplyFilter();
            }
        }

        private TaskStatus? _selectedStatusFilter;
        public TaskStatus? SelectedStatusFilter
        {
            get { return _selectedStatusFilter; }
            set
            {
                _selectedStatusFilter = value;
                OnPropertyChanged(nameof(SelectedStatusFilter));
                ApplyFilter();
            }
        }

        private TaskPriority? _selectedPriorityFilter;
        public TaskPriority? SelectedPriorityFilter
        {
            get { return _selectedPriorityFilter; }
            set
            {
                _selectedPriorityFilter = value;
                OnPropertyChanged(nameof(SelectedPriorityFilter));
                ApplyFilter();
            }
        }

        private DateTime? _selectedDeadlineFilter;
        public DateTime? SelectedDeadlineFilter
        {
            get { return _selectedDeadlineFilter; }
            set
            {
                _selectedDeadlineFilter = value;
                OnPropertyChanged(nameof(SelectedDeadlineFilter));
                ApplyFilter();
            }
        }
    
        private void SortDirection()
        {
            SortAscending = !SortAscending;
        }
        // Komendy
        public ICommand ShowEditTaskCommand { get; private set; }
        public ICommand ResetFiltersCommand { get; private set; }

        public ICommand SortCommand { get; private set; }
        public TaskListViewModel(MainViewModel mainViewModel)
        {
            _mainViewModel = mainViewModel;
            _dataContext = new DataContext();

            // Inicjalizacja komend
            ShowEditTaskCommand = new RelayCommand(ShowEditTask);
            SortCommand = new RelayCommand(_=>SortDirection());

            // Inicjalizacja kolekcji
            LoadTasks();
            Categories = new ObservableCollection<Category>([.. _dataContext.Categories]);

            Mediator.Instance.Register("UpdateTasksList", UpdateTasksList);

            ResetFiltersCommand = new RelayCommand(ResetFilters);

            ResetFilters(null);
        }

     

        //sorotowanie po dacie
        private bool _sortAscending = true;
        public bool SortAscending
        {
            get => _sortAscending;
            set
            {
                if (_sortAscending != value)
                {
                    _sortAscending = value;
                    OnPropertyChanged(nameof(SortAscending));
                    ApplySorting();
                }
            }
        }
        private void ApplySorting()
        {
            if (_tasksView != null)
            {
                _tasksView.SortDescriptions.Clear();

                // Sortowanie po dacie 
                var direction = SortAscending ? ListSortDirection.Ascending : ListSortDirection.Descending;
                _tasksView.SortDescriptions.Add(new SortDescription("Deadline", direction));

                _tasksView.Refresh();
            }
        }

        private void LoadTasks()
        {
            Tasks = new ObservableCollection<Task>([.. _dataContext.Tasks
                .Include(t => t.SubTasks)
                .Include(t => t.Categories)
                    .ThenInclude(c => c.Category)]);
            ApplySorting(); // Zastosuj sortowanie 

        }

        private void UpdateTasksList(object obj)
        {
            LoadTasks();
            Mediator.Instance.Notify("RefreshStats", null);
        }

        public void ShowDetails(object task)
        {
            Mediator.Instance.Notify("ShowDetails", task);
        }

        private void OnTaskPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(Task.Status))
            {
                var taskSender = sender as Task;

                var task = _dataContext.Tasks.FirstOrDefault(t => t.Id == taskSender.Id);

                task.Status = taskSender.Status;

                _dataContext.Tasks.Update(task);
                _dataContext.SaveChanges();

                Mediator.Instance.Notify("RefreshStats", null);
            }
        }

        // Metoda wywoływana po naciśnięciu przycisku "Edit Task"
        private void ShowEditTask(object obj)
        {
            _mainViewModel.EditTaskViewCommand.Execute(null);
        }

        private bool FilterTasks(object obj)
        {
            if (obj is not Task task) return false;

            if (!string.IsNullOrEmpty(SearchBoxFilter))
            {
                if (!task.Name.Contains(SearchBoxFilter, StringComparison.OrdinalIgnoreCase) &&
                    !task.Description.Contains(SearchBoxFilter, StringComparison.OrdinalIgnoreCase))
                {
                    return false;
                }
            }

            if (SelectedCategoryFilter != null &&
                (task.Categories == null || !task.Categories.Any(c => c.CategoryId == SelectedCategoryFilter.Id)))
                return false;

            if (SelectedStatusFilter != null && task.Status != SelectedStatusFilter)
                return false;

            if (SelectedPriorityFilter != null && task.Priority != SelectedPriorityFilter)
                return false;

            if (SelectedDeadlineFilter != null && task.Deadline.Date != SelectedDeadlineFilter.Value.Date)
                return false;

            return true;
        }

        private void ApplyFilter()
        {
            _tasksView?.Refresh();
        }

        private void ResetFilters(object obj)
        {
            _searchBoxFilter = "";
            _selectedCategoryFilter = null;
            _selectedPriorityFilter = null;
            _selectedStatusFilter = null;
            _selectedDeadlineFilter = null;
            ApplyFilter();
        }
    }
}
