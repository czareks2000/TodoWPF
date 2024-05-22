using Todo.Core;
using Task = Todo.MVVM.Model.Task;

namespace Todo.MVVM.ViewModel
{
    public class DetailsViewModel : ObservableObject
    {
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

        public DetailsViewModel() 
        {
            
        }
    }
}
