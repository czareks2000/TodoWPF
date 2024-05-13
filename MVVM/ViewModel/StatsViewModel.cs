using LiveCharts;
using LiveCharts.Wpf;
using Todo.Core;

namespace Todo.MVVM.ViewModel
{
    public class StatsViewModel : ObservableObject
    {

        public SeriesCollection TaskCountSeries { get; set; }
        public SeriesCollection CompletionSeries { get; set; }

        public StatsViewModel()
        {
            // Definicja danych próbkowych dla TaskCountSeries
            TaskCountSeries = new SeriesCollection
            {
                new ColumnSeries
                {
                    Title = "Kategoria",
                    Values = new ChartValues<int> { 10, 20, 15 } // Przykładowe liczby zadań dla różnych kategorii
                }
            };

            // Definicja danych próbkowych dla CompletionSeries
            CompletionSeries = new SeriesCollection
            {
                new PieSeries
                {
                    Title = "Ukończone",
                    Values = new ChartValues<double> { 70 }, // Przykładowy procent wykonanych zadań
                    DataLabels = true
                },
                new PieSeries
                {
                    Title = "Nieukończone",
                    Values = new ChartValues<double> { 30 }, // Przykładowy procent niezakończonych zadań
                    DataLabels = true
                }
            };
        }
    }
}
