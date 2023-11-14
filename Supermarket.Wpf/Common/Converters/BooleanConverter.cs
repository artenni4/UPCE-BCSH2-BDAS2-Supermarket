using System.Globalization;
using System.Windows.Data;

namespace Supermarket.Wpf.Common.Converters;

public abstract class BooleanConverter<TValue> : IValueConverter
{
    public BooleanConverter(TValue? trueValue, TValue? falseValue)
    {
        True = trueValue;
        False = falseValue;
    }

    public TValue? True { get; set; }
    public TValue? False { get; set; }
    
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        return value is true ? True : False;
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        return value is TValue tValue && EqualityComparer<TValue>.Default.Equals(tValue, True);
    }
}