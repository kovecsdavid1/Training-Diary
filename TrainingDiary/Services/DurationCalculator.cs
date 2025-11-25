using System;
using System.Globalization;
using Microsoft.Maui.Controls;
using TrainingDiary.Models;

namespace TrainingDiary.Services;

public class DurationCalculator : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is Training training)
        {
            TimeSpan duration = training.EndTime - training.StartTime;
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
