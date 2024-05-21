using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Todo.MVVM.Model;
using Todo.MVVM.ViewModel;

namespace Todo.MVVM.View
{
    /// <summary>
    /// Interaction logic for AddTaskView.xaml
    /// </summary>
    public partial class AddTaskView : UserControl
    {
        public AddTaskView()
        {
            InitializeComponent();
        }

        private void AddSubtask_Click(object sender, RoutedEventArgs e)
        {
            var viewModel = DataContext as AddTaskViewModel;
            if (viewModel != null && !string.IsNullOrWhiteSpace(txtSubtask.Text))
            {
                viewModel.Subtasks.Add(txtSubtask.Text);
                txtSubtask.Clear();
            }
        }

        private void SubtaskTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (sender is TextBox textBox && DataContext is AddTaskViewModel viewModel)
            {
                if (string.IsNullOrWhiteSpace(textBox.Text))
                {
                    string subtaskToRemove = textBox.DataContext as string;
                    if (subtaskToRemove != null)
                    {
                        viewModel.Subtasks.Remove(subtaskToRemove);
                    }
                }
            }
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            var viewModel = DataContext as AddTaskViewModel;
            var checkBox = sender as CheckBox;
            if (checkBox != null && checkBox.DataContext is Category category)
            {
                if (!viewModel.SelectedCategories.Contains(category))
                {
                    viewModel.SelectedCategories.Add(category);
                }
            }
        }

        private void CheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            var viewModel = DataContext as AddTaskViewModel;
            var checkBox = sender as CheckBox;
            if (checkBox != null && checkBox.DataContext is Category category)
            {
                if (viewModel.SelectedCategories.Contains(category))
                {
                    viewModel.SelectedCategories.Remove(category);
                }
            }
        }
    }
}
