using System;
using System.Globalization;
using Microsoft.Maui.Controls;

namespace TrainingDiary2.Converters
{
    public class DurationCalculator : IValueConverter
    {
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value is TimeSpan duration)
            {
                if (duration.TotalSeconds < 0)
                    duration = TimeSpan.Zero;

                return $"{(int)duration.TotalHours} hour(s) {duration.Minutes} minute(s)";
            }
            return "No data";
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            return Binding.DoNothing;
        }
    }
}