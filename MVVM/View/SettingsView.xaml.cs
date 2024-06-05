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
using Todo.MVVM.ViewModel;

namespace Todo.MVVM.View
{
    /// <summary>
    /// Interaction logic for SettingsView.xaml
    /// </summary>
    public partial class SettingsView : UserControl
    {
        public SettingsView()
        {
            InitializeComponent();
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (sender is ComboBox comboBox)
            {
                var selectedLanguage = comboBox.SelectedItem as ComboBoxItem;
                if (selectedLanguage != null)
                {
                    var viewModel = DataContext as SettingsViewModel;
                    if (viewModel != null)
                    {
                        viewModel.SelectedLanguage = selectedLanguage.Content.ToString();
                    }
                }
            }
        }
    }
}
