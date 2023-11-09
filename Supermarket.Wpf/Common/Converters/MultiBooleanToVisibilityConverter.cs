using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Supermarket.Wpf.Common.Converters;

public class MultiBooleanToVisibilityConverter : IMultiValueConverter
{
    public bool[] TrueCondition { get; set; } = Array.Empty<bool>();
    
    public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
    {
        if (TrueCondition.Length != values.Length)
        {
            return Visibility.Collapsed;
        }
        
        for (int i = 0; i < TrueCondition.Length; i++)
        {
            if (values[i] is not bool value || value != TrueCondition[i])
            {
                return Visibility.Collapsed;
            }
        }

        return Visibility.Visible;
    }

    public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
    {
        throw new NotSupportedException();
    }
}