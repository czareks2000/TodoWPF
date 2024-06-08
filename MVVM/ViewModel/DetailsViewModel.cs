﻿﻿using System.Windows;
using System.Windows.Input;
using Todo.Core;
using Todo.DB;
using Todo.MVVM.Model;
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
                    OnPropertyChanged(nameof(IsTaskEditable));
                
               /* if (_selectedTask == null)
                    Mediator.Instance.Notify("ChangeView", null);*/
            }
        }
        public bool IsTaskEditable => SelectedTask != null && SelectedTask.Status != Model.Enums.TaskStatus.Done;

        public ICommand DeleteTaskCommand { get; private set; }
        public ICommand MarkAsCompletedCommand { get; private set; }

        public DetailsViewModel() 
        {
            _dataContext = new DataContext();

            DeleteTaskCommand = new RelayCommand(DeleteTask);
            MarkAsCompletedCommand = new RelayCommand(MarkAsCompleted, obj=>IsTaskEditable);

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

        private void MarkAsCompleted(object obj)
        {
            if(SelectedTask != null)
            {
                SelectedTask.Status=Model.Enums.TaskStatus.Done;
                _dataContext.SaveChanges();
                OnPropertyChanged(nameof(SelectedTask));
                OnPropertyChanged(nameof(IsTaskEditable));
                Mediator.Instance.Notify("UpdateTasksList", null);
                Mediator.Instance.Notify("ChangeViewToAddTask", null);

            }
        }

        private void CheckIfAllSubTasksComplete(object obj){
            if(SelectedTask != null && SelectedTask.SubTasks.All(st=>st.Status==Model.Enums.TaskStatus.Done ))
            { MarkAsCompleted(null); }
           
        }
        public void SubTaskChecked(object sender)
        {
            if (sender is SubTask subTask)
            {
                subTask.Status = Model.Enums.TaskStatus.Done;
                var subTaskToUpdate = _dataContext.SubTasks.FirstOrDefault(s => s.Id == subTask.Id);
                if (subTaskToUpdate != null)
                {
                    subTaskToUpdate.Status = Model.Enums.TaskStatus.Done;
                    _dataContext.SaveChanges();
                }
            }
            CheckIfAllSubTasksComplete(null);
        }

        public void SubTaskUnchecked(object sender)
        {
            if (sender is SubTask subTask)
            {
                subTask.Status = Model.Enums.TaskStatus.InProgress;
                var subTaskToUpdate = _dataContext.SubTasks.FirstOrDefault(s => s.Id == subTask.Id);
                if (subTaskToUpdate != null)
                {
                    subTaskToUpdate.Status = Model.Enums.TaskStatus.InProgress;
                    _dataContext.SaveChanges();
                }
            }
            CheckIfAllSubTasksComplete(null);
        }
    }
}
