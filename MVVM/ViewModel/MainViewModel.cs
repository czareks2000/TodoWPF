using Todo.Core;

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
        public RelayCommand DetailsViewCommand { get; set; }
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

            AddTaskViewCommand = new RelayCommand(o =>
            {
                RightView = AddTaskVM;
            });

            EditTaskViewCommand = new RelayCommand(o =>
            {
                RightView = EditTaskVM;
            });

            DetailsViewCommand = new RelayCommand(o =>
            {
                RightView = DetailsVM;
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
    }
}
