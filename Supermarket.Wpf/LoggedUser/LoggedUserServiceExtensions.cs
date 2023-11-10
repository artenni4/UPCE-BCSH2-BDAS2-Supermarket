using Supermarket.Core.Domain.Auth.LoggedEmployees;
using System.Diagnostics.CodeAnalysis;

namespace Supermarket.Wpf.LoggedUser
{
    internal static class LoggedUserServiceExtensions
    {
        public static bool IsLoggedEmployee(this ILoggedUserService service, [NotNullWhen(true)] out ILoggedEmployee? loggedEmployee)
        {
            if (service.LoggedEmployee is null)
            {
                loggedEmployee = null;
                return false;
            }

            loggedEmployee = service.LoggedEmployee;
            return true;
        }

        public static bool IsLoggedCustomer(this ILoggedUserService service)
        {
            return service.LoggedEmployee is null;
        }

        public static bool IsAdmin(this ILoggedEmployee loggedEmployee, [NotNullWhen(true)] out LoggedAdmin? loggedAdmin)
        {
            loggedAdmin = loggedEmployee as LoggedAdmin;
            if (loggedAdmin is null)
            {
                return false;
            }

            return true;
        }

        public static bool IsSupermarketEmployee(this ILoggedEmployee loggedEmployee, [NotNullWhen(true)] out LoggedSupermarketEmployee? loggedSupermarketEmployee)
        {
            loggedSupermarketEmployee = loggedEmployee as LoggedSupermarketEmployee;
            if (loggedSupermarketEmployee is null)
            {
                return false;
            }

            return true;
        }
    }
}
