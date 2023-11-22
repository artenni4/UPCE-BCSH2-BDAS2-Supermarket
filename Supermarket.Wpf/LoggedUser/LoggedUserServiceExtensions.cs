using System.Diagnostics.CodeAnalysis;
using Supermarket.Core.Domain.Auth.LoggedEmployees;

namespace Supermarket.Wpf.LoggedUser
{
    internal static class LoggedUserServiceExtensions
    {
        public static bool HasEmployeeId(this ILoggedUserService loggedUserService, [NotNullWhen(true)] out int? employeeId)
        {
            if (loggedUserService.IsAdmin(out var loggedAdmin))
            {
                employeeId = loggedAdmin.Id;
                return true;
            }
            
            if (loggedUserService.IsSupermarketEmployee(out var loggedSupermarketEmployee, out _))
            {
                employeeId = loggedSupermarketEmployee.Id;
                return true;
            }

            employeeId = null;
            return false;
        }

        public static bool IsCashier(this ILoggedUserService service)
        {
            return service.IsSupermarketEmployee(out _, out var roles) &&
                   roles.Contains(SupermarketEmployeeRole.Cashier);
        }
        
        public static bool IsGoodsKeeper(this ILoggedUserService service)
        {
            return service.IsSupermarketEmployee(out _, out var roles) &&
                   roles.Contains(SupermarketEmployeeRole.GoodsKeeper);
        }
        
        public static bool IsManager(this ILoggedUserService service)
        {
            return service.IsSupermarketEmployee(out _, out var roles) &&
                   roles.Contains(SupermarketEmployeeRole.Manager);
        }
    }
}
