using Supermarket.Wpf.LoggedUser;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Supermarket.Wpf.Common.Converters
{
    internal class RoleToVisibilityConverter : IValueConverter
    {
        public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value is not ILoggedUserService loggedUserService)
            {
                throw new ArgumentException("Value is not ILoggedUserService.");
            }
            
            if (loggedUserService.IsSupermarketEmployee(out _, out var roles) && !roles.Any() && parameter is null)
            {
                return Visibility.Visible;
            }
            
            if (parameter is string parameterString)
            {
                var parameters = parameterString
                    .Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                    .Select(Enum.Parse<EmployeeRole>)
                    .ToArray();

                if (loggedUserService.IsAdmin(out _) && parameters.Contains(EmployeeRole.Admin) ||
                    loggedUserService.IsCashier() && parameters.Contains(EmployeeRole.Cashier) ||
                    loggedUserService.IsGoodsKeeper() && parameters.Contains(EmployeeRole.GoodsKeeper) ||
                    loggedUserService.IsManager() && parameters.Contains(EmployeeRole.Manager))
                {
                    return Visibility.Visible;
                }
            }

            return Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
