using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Supermarket.Wpf.Common.Converters;

public class MultiOrBooleanToVisibilityConverter : IMultiValueConverter
{
    public bool[] TrueCondition { get; set; } = Array.Empty<bool>();
    
    public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
    {
        if (TrueCondition.Length != values.Length)
        {
            throw new ArgumentException($"Bad amount of parameters to {nameof(MultiOrBooleanToVisibilityConverter)}. Expected {TrueCondition.Length} got {values.Length}");
        }
        
        for (int i = 0; i < TrueCondition.Length; i++)
        {
            if (values[i] is bool value && value == TrueCondition[i])
            {
                return Visibility.Visible;
            }
        }

        return Visibility.Collapsed;
    }

    public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
    {
        throw new NotSupportedException();
    }
}