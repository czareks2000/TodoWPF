using System.Windows.Input;
using Todo.Core;

namespace Todo.MVVM.ViewModel
{
    public class TaskListViewModel : ObservableObject
    {
        private MainViewModel _mainViewModel;

        // Komendy
        public ICommand ShowDetailsCommand { get; private set; }
        public ICommand ShowEditTaskCommand { get; private set; }

        public TaskListViewModel(MainViewModel mainViewModel)
        {
            _mainViewModel = mainViewModel;

            // Inicjalizacja komend
            ShowDetailsCommand = new RelayCommand(ShowDetails);
            ShowEditTaskCommand = new RelayCommand(ShowEditTask);
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
