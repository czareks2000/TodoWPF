using LiveCharts;
using LiveCharts.Wpf;
using Todo.Core;
using Todo.DB;

namespace Todo.MVVM.ViewModel
{
    public class StatsViewModel : ObservableObject
    {
        private DataContext _dataContext;

        private SeriesCollection _taskCountSeries;
        public SeriesCollection TaskCountSeries
        {
            get { return _taskCountSeries; }
            set
            {
                _taskCountSeries = value;
                OnPropertyChanged();
            }
        }

        private SeriesCollection _completionSeries;
        public SeriesCollection CompletionSeries
        {
            get { return _completionSeries; }
            set
            {
                _completionSeries = value;
                OnPropertyChanged();
            }
        }

        public StatsViewModel()
        {
            _dataContext = new DataContext();

            CaluculateValues();

            Mediator.Instance.Register("RefreshStats", RefreshStats);
        }

        private void RefreshStats(object obj)
        {
            CaluculateValues();
        }

        private void CaluculateValues()
        {
            TaskCountSeries = CalculateCompletedTaskCountByCategorySeries();
            CompletionSeries = CalculateCompletionSeries();
        }

        private SeriesCollection CalculateCompletedTaskCountByCategorySeries()
        {
            var categoryCounts = _dataContext.Categories
                .Select(c => new
                {
                    CategoryName = c.Name,
                    CompletedTasksCount = c.Tasks.Count(t => t.Task.Status == Model.Enums.TaskStatus.Done)
                })
                .ToList();

            var taskCountSeries = new SeriesCollection();

            foreach (var count in categoryCounts)
            {
                taskCountSeries.Add(new ColumnSeries
                {
                    Title = count.CategoryName,
                    Values = new ChartValues<int> { count.CompletedTasksCount },
                    DataLabels = true
                });
            }

            return taskCountSeries;
        }

        private SeriesCollection CalculateCompletionSeries()
        {
            int totalCount = _dataContext.Tasks.Count();

            int inProgress = (int)Math.Round((double)_dataContext.Tasks.Count(t => t.Status == Model.Enums.TaskStatus.InProgress) / totalCount * 100);
            int done = (int)Math.Round((double)_dataContext.Tasks.Count(t => t.Status == Model.Enums.TaskStatus.Done) / totalCount * 100);

            return new SeriesCollection
            {
                new PieSeries
                {
                    Title = "Done",
                    Values = new ChartValues<int> { done },
                    DataLabels = true
                },
                new PieSeries
                {
                    Title = "InProgress",
                    Values = new ChartValues<int> { inProgress },
                    DataLabels = true
                }
            };
        }
    }
}
