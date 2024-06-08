using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using Todo.Core;

namespace Todo.MVVM.ViewModel
{
    public class SettingsViewModel : ObservableObject
    {
        private string _selectedLanguage;

        public SettingsViewModel()
        {
            AvailableLanguages = new ObservableCollection<string>
            {
                "Polski",
                "English"
            };

            _selectedLanguage = "English";
        }

        public ObservableCollection<string> AvailableLanguages { get; }

        public string SelectedLanguage
        {
            get => _selectedLanguage;
            set
            {
                if (_selectedLanguage != value)
                {
                    _selectedLanguage = value;
                    switch (_selectedLanguage)
                    {
                        case "English":
                            SetLanguage("Theme/LanguageEn.xaml");
                            break;
                        case "Polski":
                            SetLanguage("Theme/LanguagePl.xaml");
                            break;
                    }
                }
            }
        }
        private void SetLanguage(string resourcePath)
        {
            var dict = new ResourceDictionary { Source = new Uri(resourcePath, UriKind.Relative) };
            Application.Current.Resources.MergedDictionaries.Clear();
            Application.Current.Resources.MergedDictionaries.Add(dict);
        }
    }
}

