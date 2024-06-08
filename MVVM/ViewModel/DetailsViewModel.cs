﻿using System.Windows;
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
                OnPropertyChanged(nameof(IsTaskEditable));

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
            }
        }

        private void CheckIfAllSubTasksComplete(object obj){
            if(SelectedTask != null && SelectedTask.SubTasks.All(st=>st.Status==Model.Enums.TaskStatus.Done ))
            { MarkAsCompleted(null); }
           
        }
        public void SubTaskChecked(object sender)
        {
            CheckIfAllSubTasksComplete(null);
        }

        public void SubTaskUnchecked(object sender)
        {
            CheckIfAllSubTasksComplete(null);
        }
    }
}
