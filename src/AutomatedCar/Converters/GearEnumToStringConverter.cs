using AutomatedCar.Models.Enums;
using System;
using System.Globalization;
using System.Windows.Data;

namespace AutomatedCar.Converters
{
    public class GearEnumToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) => $"Gear: {value}";

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => Binding.DoNothing;
    }
}
