using System.Windows;
using System.Windows.Data;

namespace Supermarket.Wpf.Common.Converters;

public class NullToVisibilityConverter : IValueConverter
{
    public object Convert(object? value, Type targetType, object? parameter, System.Globalization.CultureInfo culture)
    {
        var invisibility = parameter is string defaultInvisibility
            ? (Visibility)Enum.Parse(typeof(Visibility), defaultInvisibility)
            : Visibility.Collapsed;
        return value == null ? invisibility : Visibility.Visible;
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, System.Globalization.CultureInfo culture)
    {
        return DependencyProperty.UnsetValue;
    }
}