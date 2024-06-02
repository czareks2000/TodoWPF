using System.Windows.Controls;
using Todo.MVVM.ViewModel;
namespace Todo.MVVM.View
{
    /// <summary>
    /// Interaction logic for TaskListView.xaml
    /// </summary>
    public partial class TaskListView : UserControl
    {
        public TaskListView()
        {
            InitializeComponent();
        }

        private void TaskList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var viewModel = DataContext as TaskListViewModel;
            viewModel.ShowDetails(TaskList.SelectedItem);
        }

        private void ResetFilters(object sender, System.Windows.RoutedEventArgs e)
        {
            CategoryFilter.SelectedIndex = -1;
            StatusFilter.SelectedIndex = -1;
            PriorityFilter.SelectedIndex = -1;
            //DeadlineFilter.SelectedDate = null;
            SearchTextBox.Text = string.Empty;
        }
    }
}
