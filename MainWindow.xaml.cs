using System.Windows;
using Todo.DB;
using Todo.MVVM.ViewModel;

namespace Todo
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly DataContext _context = new DataContext();

        public MainWindow()
        {
            InitializeComponent();
            //Seed.SeedData(_context);
        }
    }
}