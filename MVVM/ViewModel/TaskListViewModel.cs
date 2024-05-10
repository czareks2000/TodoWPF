using System.Collections.ObjectModel;
using System.Windows.Input;
using Todo.Core;
using Todo.DB;
using Task = Todo.MVVM.Model.Task;

namespace Todo.MVVM.ViewModel
{
    public class TaskListViewModel : ObservableObject
    {
        private MainViewModel _mainViewModel;
        private DataContext _dataContext;

        // Tworzenie kolekcji na potrzeby ListView
        public ObservableCollection<Task> Tasks { get; set; }

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
