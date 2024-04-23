using System.Windows;
using Todo.DB;

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
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            Seed.SeedData(_context);

            listBox.Items.Add(_context.Categories.First().Name);
        }
    }
}