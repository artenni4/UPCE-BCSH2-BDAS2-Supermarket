using System.Windows;
using System.Windows.Data;

namespace Supermarket.Wpf.Common.Converters;

public class BooleanToVisibilityConverter : BooleanConverter<Visibility>, IValueConverter
{
    public BooleanToVisibilityConverter() : base(Visibility.Visible, Visibility.Collapsed)
    {
    }
}