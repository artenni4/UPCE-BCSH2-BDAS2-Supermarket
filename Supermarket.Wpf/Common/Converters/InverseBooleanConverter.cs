﻿using System.Globalization;
using System.Windows.Data;

namespace Supermarket.Wpf.Common.Converters;

public class InverseBooleanConverter : IValueConverter
{
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is bool boolValue)
        {
            return !boolValue;
        }

        throw new ArgumentException("Value is not boolean.");
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is bool boolValue)
        {
            return !boolValue;
        }

        throw new ArgumentException("Value is not boolean.");
    }
}
