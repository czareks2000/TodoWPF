using Todo.Core;
using Task = Todo.MVVM.Model.Task;

namespace Todo.MVVM.ViewModel
{
    public class MainViewModel : ObservableObject
    {
        public object LeftView { get; set; }

        private object _rightView;

        public object RightView
        {
            get { return _rightView; }
            set
            {
                _rightView = value;
                OnPropertyChanged();
            }
        }

        public RelayCommand AddTaskViewCommand { get; set; }
        public RelayCommand EditTaskViewCommand { get; set; }
        public RelayCommand StatsViewCommand { get; set; }
        public RelayCommand SettingsViewCommand { get; set; }

        public TaskListViewModel TaskListVM { get; set; }

        public AddTaskViewModel AddTaskVM { get; set; }
        public EditTaskViewModel EditTaskVM { get; set; }
        public DetailsViewModel DetailsVM { get; set; }
        public StatsViewModel StatsVM { get; set; }
        public SettingsViewModel SettingsVM { get; set; }

        public MainViewModel()
        {
            TaskListVM = new TaskListViewModel(this);

            LeftView = TaskListVM;

            AddTaskVM = new AddTaskViewModel();
            EditTaskVM = new EditTaskViewModel();
            DetailsVM = new DetailsViewModel();
            StatsVM = new StatsViewModel();
            SettingsVM = new SettingsViewModel();

            RightView = AddTaskVM;

            Mediator.Instance.Register("ShowDetails", task => ShowDetails((Task)task));

            AddTaskViewCommand = new RelayCommand(o =>
            {
                RightView = AddTaskVM;
            });

            EditTaskViewCommand = new RelayCommand(o =>
            {
                RightView = EditTaskVM;
            });

            StatsViewCommand = new RelayCommand(o =>
            {
                RightView = StatsVM;
            });

            SettingsViewCommand = new RelayCommand(o =>
            {
                RightView = SettingsVM;
            });
        }

        private void ShowDetails(Task task)
        {
            DetailsVM.SelectedTask = task;
            RightView = DetailsVM;
        }
    }
}
