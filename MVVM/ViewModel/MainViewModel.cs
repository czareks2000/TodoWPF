using Microsoft.EntityFrameworkCore;
using Microsoft.Win32;
using PdfSharp.Drawing;
using PdfSharp.Pdf;
using System.Windows;
using System.Windows.Threading;
using Todo.Core;
using Todo.DB;
using Task = Todo.MVVM.Model.Task;
using TaskStatus = Todo.MVVM.Model.Enums.TaskStatus;

namespace Todo.MVVM.ViewModel
{
    public class MainViewModel : ObservableObject
    {
        private DataContext _dataContext;

        public object LeftView { get; set; }

        private object _rightView;

        public object RightView
        {
            get { return _rightView; }
            set
            {
                _rightView = value;
                OnPropertyChanged();
            }
        }

        public RelayCommand AddTaskViewCommand { get; set; }
        public RelayCommand EditTaskViewCommand { get; set; }
        public RelayCommand StatsViewCommand { get; set; }
        public RelayCommand SettingsViewCommand { get; set; }

        public RelayCommand ExportCommand { get; set; }

        public TaskListViewModel TaskListVM { get; set; }

        public AddTaskViewModel AddTaskVM { get; set; }
        public EditTaskViewModel EditTaskVM { get; set; }
        public DetailsViewModel DetailsVM { get; set; }
        public StatsViewModel StatsVM { get; set; }
        public SettingsViewModel SettingsVM { get; set; }

        public MainViewModel()
        {
            _dataContext = new DataContext();

            TaskListVM = new TaskListViewModel(this);

            LeftView = TaskListVM;

            AddTaskVM = new AddTaskViewModel();
            EditTaskVM = new EditTaskViewModel();
            DetailsVM = new DetailsViewModel();
            StatsVM = new StatsViewModel();
            SettingsVM = new SettingsViewModel();

            RightView = AddTaskVM;

            Mediator.Instance.Register("ShowDetails", task => ShowDetails((Task)task));


            AddTaskViewCommand = new RelayCommand(o =>
            {
                RightView = AddTaskVM;
            });

            EditTaskViewCommand = new RelayCommand(o =>
            {
                RightView = EditTaskVM;
            });

            StatsViewCommand = new RelayCommand(o =>
            {
                RightView = StatsVM;
            });

            SettingsViewCommand = new RelayCommand(o =>
            {
                RightView = SettingsVM;
            });

            ExportCommand = new RelayCommand(ExportData);
        }

        private void ShowDetails(Task task)
        {
            if (task == null)
            {
                RightView = AddTaskVM;
                return;
            }
            DetailsVM.SelectedTask = task;
            RightView = DetailsVM;
        }


        private void ExportData(object obj)
        {
            // Show save file dialog
            var saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "PDF files (*.pdf)|*.pdf|All files (*.*)|*.*";
            saveFileDialog.FilterIndex = 1;
            saveFileDialog.RestoreDirectory = true;

            if (saveFileDialog.ShowDialog() == true)
            {
                // Export data to PDF
                using (PdfDocument document = new PdfDocument())
                {
                    PdfPage page = document.AddPage();
                    XGraphics gfx = XGraphics.FromPdfPage(page);
                    XFont titleFont = new XFont("Times New Roman", 16, XFontStyleEx.Bold);
                    XFont normalFont = new XFont("Times New Roman", 12);

                    var tasks = _dataContext.Tasks
                        .Include(t => t.Categories)
                            .ThenInclude(c => c.Category)
                        .Include(t => t.SubTasks)
                        .ToList();
                    double yPos = 10;
                    double maxYPos = 200;

                    foreach (var task in tasks)
                    {
                        if (yPos + 40 > maxYPos)
                        {
                            page = document.AddPage();
                            gfx = XGraphics.FromPdfPage(page);
                            yPos = 10;
                        }

                        gfx.DrawString($"Task: {task.Name} ({(task.Status == TaskStatus.Done ? "Done" : "InProgress")})", titleFont, XBrushes.Black, new XRect(50, MmToPixel(yPos), page.Width, page.Height), XStringFormats.TopLeft);
                        yPos += 5; 

                        gfx.DrawString($"Priorytet: {task.Priority}", normalFont, XBrushes.Black, new XRect(50, MmToPixel(yPos), page.Width, page.Height), XStringFormats.TopLeft);
                        yPos += 5; 

                        gfx.DrawString($"Deadline: {task.Deadline:dd/MM/yyyy}", normalFont, XBrushes.Black, new XRect(50, MmToPixel(yPos), page.Width, page.Height), XStringFormats.TopLeft);
                        yPos += 5; 

                        gfx.DrawString($"Kategorie:", normalFont, XBrushes.Black, new XRect(50, MmToPixel(yPos), page.Width, page.Height), XStringFormats.TopLeft);
                        yPos += 5; 

                        foreach (var category in task.Categories)
                        {
                            gfx.DrawString($"- {category.Category.Name}", normalFont, XBrushes.Black, new XRect(60, MmToPixel(yPos), page.Width, page.Height), XStringFormats.TopLeft);
                            yPos += 5;
                        }

                        gfx.DrawString($"Podzadania:", normalFont, XBrushes.Black, new XRect(50, MmToPixel(yPos), page.Width, page.Height), XStringFormats.TopLeft);
                        yPos += 5;

                        foreach (var subtask in task.SubTasks)
                        {
                            gfx.DrawString($"- {subtask.Name} ({(subtask.Status == TaskStatus.Done ? "Done" : "InProgress")})", normalFont, XBrushes.Black, new XRect(60, MmToPixel(yPos), page.Width, page.Height), XStringFormats.TopLeft);
                            yPos += 5; 
                        }

                        yPos += 10; 
                        gfx.DrawLine(XPens.Black, 50, MmToPixel(yPos), page.Width - 50, MmToPixel(yPos));
                        yPos += 10;
                    }

                    // Save the document in a separate thread
                    Thread saveThread = new Thread(() =>
                    {
                        document.Save(saveFileDialog.FileName);

                        // Show completion message using Dispatcher
                        Dispatcher.CurrentDispatcher.Invoke(() =>
                        {
                            MessageBox.Show("Pomyślnie exportowano plik.", "Export zakończony", MessageBoxButton.OK, MessageBoxImage.Information);
                        });
                    });

                    saveThread.SetApartmentState(ApartmentState.STA);
                    saveThread.Start();
                }
            }
        }

        private int MmToPixel(double mm, double dpi = 96.0)
        {
            return (int)(mm / 25.4 * dpi);
        }

    }
}
