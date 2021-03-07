using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace AutomatedCar.Converters
{
    public class BoolToBackgroundConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) => (bool)value ? Brushes.Green : Brushes.Transparent;

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => Binding.DoNothing;
    }
}
