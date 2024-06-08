using System.Windows;
using System.Windows.Controls;

namespace Todo.MVVM.View
{
    /// <summary>
    /// Interaction logic for DetailsView.xaml
    /// </summary>
    public partial class DetailsView : UserControl
    {
        public DetailsView()
        {
            InitializeComponent();
        }
        private void SubTaskChecked(object sender, RoutedEventArgs e)
        {
            // Pobierz kontekst danych subtaska, który został zaznaczony
            var checkBox = sender as CheckBox;
            if (checkBox?.DataContext is Model.SubTask subTask)
            {
                // Przekazanie zdarzenia do ViewModelu
                (DataContext as ViewModel.DetailsViewModel)?.SubTaskChecked(subTask);
            }
        }

        private void SubTaskUnchecked(object sender, RoutedEventArgs e)
        {
            // Pobierz kontekst danych subtaska, który został odznaczony
            var checkBox = sender as CheckBox;
            if (checkBox?.DataContext is Model.SubTask subTask)
            {
                // Przekazanie zdarzenia do ViewModelu
                (DataContext as ViewModel.DetailsViewModel)?.SubTaskUnchecked(subTask);
            }
        }
    }
}
