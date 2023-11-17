using Supermarket.Core.Domain.Auth.LoggedEmployees;
using System.Diagnostics.CodeAnalysis;

namespace Supermarket.Wpf.LoggedUser
{
    public interface ILoggedUserService
    {
        /// <summary>
        /// Checks whether some user was remembered in the service.
        /// If returns false, getters in the service will throw exception
        /// </summary>
        bool IsUserSet { get; }

        /// <summary>
        /// Checks whether current user is employee
        /// </summary>
        bool IsEmployee([NotNullWhen(true)] out EmployeeData? employeeData);
        
        /// <summary>
        /// Checks whether current user is customer
        /// </summary>
        bool IsCustomer { get; }
        
        /// <summary>
        /// Current id of supermarket for logged user
        /// </summary>
        int SupermarketId { get; }
        
        /// <summary>
        /// Returns admin data if logged in
        /// </summary>
        bool IsAdmin([NotNullWhen(true)] out EmployeeData? loggedAdmin);

        /// <summary>
        /// Returns supermarket employee data if logged in
        /// </summary>
        bool IsSupermarketEmployee(
            [NotNullWhen(true)] out EmployeeData? loggedSupermarketEmployee,
            [NotNullWhen(true)] out IReadOnlySet<SupermarketEmployeeRole>? roles);
        
        /// <summary>
        /// Raised when user is set
        /// </summary>
        event EventHandler UserLoggedIn;

        /// <summary>
        /// Raised when employee is logged out
        /// </summary>
        event EventHandler UserLoggedOut;

        /// <summary>
        /// Remembers logged in supermarket employee
        /// </summary>
        void SetSupermarketEmployee(LoggedSupermarketEmployee loggedSupermarketEmployee);
    
        /// <summary>
        /// Remembers logged in admin
        /// </summary>
        void SetAdmin(LoggedAdmin loggedAdmin);
        
        /// <summary>
        /// Sets supermarket id for admin.
        /// Throws exception if current user is not admin
        /// </summary>
        /// <param name="supermarketId">id of supermarket</param>
        /// <exception cref="InvalidOperationException">current user is not admin</exception>
        void SetAdminSupermarket(int supermarketId);
        
        /// <summary>
        /// Remembers logged in customer
        /// </summary>
        void SetCustomer(int supermarketId);
        
        /// <summary>
        /// Clears current user
        /// </summary>
        void UnsetUser();
    }
}
