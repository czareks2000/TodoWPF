using System.Globalization;
using System.Windows.Data;
using TaskStatus = Todo.MVVM.Model.Enums.TaskStatus;

namespace Todo.MVVM.View.Converters
{
    public class TaskStatusToBoolConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is TaskStatus status)
            {
                return status == TaskStatus.Done;
            }
            return false;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool isChecked)
            {
                return isChecked ? TaskStatus.Done : TaskStatus.InProgress;
            }
            return TaskStatus.InProgress;
        }
    }
}
