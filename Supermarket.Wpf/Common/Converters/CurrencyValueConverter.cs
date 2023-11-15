using System.Globalization;
using System.Windows.Data;

namespace Supermarket.Wpf.Common.Converters;

public class CurrencyValueConverter : IValueConverter
{
    public bool PrependMinus { get; set; }
    
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is decimal decimalValue)
        {
            var formatted = $"{decimalValue} Kč";
            
            return PrependMinus
                ? $"-{formatted}"
                : formatted;
        }

        return value ?? string.Empty;
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}