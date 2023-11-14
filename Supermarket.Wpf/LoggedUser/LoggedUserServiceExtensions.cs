using Supermarket.Core.Domain.Auth.LoggedEmployees;

namespace Supermarket.Wpf.LoggedUser
{
    internal static class LoggedUserServiceExtensions
    {
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
