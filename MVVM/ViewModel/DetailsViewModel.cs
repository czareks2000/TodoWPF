using System.Windows;
using System.Windows.Input;
using Todo.Core;
using Todo.DB;
using Task = Todo.MVVM.Model.Task;

namespace Todo.MVVM.ViewModel
{
    public class DetailsViewModel : ObservableObject
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

        public ICommand DeleteTaskCommand { get; private set; }

        public DetailsViewModel() 
        {
            _dataContext = new DataContext();

            DeleteTaskCommand = new RelayCommand(DeleteTask);
        }


        private void DeleteTask(object obj)
        {
            MessageBoxResult result = MessageBox
                .Show(
                    "Czy na pewno chcesz usunąć to zadanie?", 
                    "Potwierdzenie", 
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
