using Supermarket.Domain.Auth.LoggedEmployees;
using Supermarket.Domain.Employees.Roles;
using Supermarket.Wpf.LoggedUser;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace Supermarket.Wpf.Common
{
    internal class RoleToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is ILoggedEmployee loggedEmployee && parameter is string parameterString)
            {
                var parameters = parameterString
                    .Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                    .Select(Enum.Parse<EmployeeRole>)
                    .ToArray();

                if (loggedEmployee.IsAdmin(out _) && parameters.Contains(EmployeeRole.Admin))
                {
                    return Visibility.Visible;
                }

                if (loggedEmployee.IsSupermarketEmployee(out var supermarketEmployee))
                {
                    foreach (var role in supermarketEmployee.Roles)
                    {
                        if (role is CashierRole && parameters.Contains(EmployeeRole.Cashier) ||
                            role is GoodsKeeperRole && parameters.Contains(EmployeeRole.GoodsKeeper) ||
                            role is ManagerRole && parameters.Contains(EmployeeRole.Manager))
                        {
                            return Visibility.Visible;
                        }
                    }
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
