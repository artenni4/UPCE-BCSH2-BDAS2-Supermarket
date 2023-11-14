using System.Globalization;
using System.Windows.Data;

namespace Supermarket.Wpf.Common.Converters
{
    public class StoragePlaceIdToBooleanConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is int storagePlaceId)
            {
                return storagePlaceId == 0;
            }

            return false;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

}
