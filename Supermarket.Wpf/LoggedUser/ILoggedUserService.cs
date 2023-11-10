using Supermarket.Core.Domain.Auth.LoggedEmployees;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Supermarket.Wpf.LoggedUser
{
    public interface ILoggedUserService
    {
        /// <summary>
        /// Logged employee instance
        /// </summary>
        ILoggedEmployee? LoggedEmployee { get; }
        
        /// <summary>
        /// Raised when employee is logged in
        /// </summary>
        event EventHandler<LoggedEmployeeArgs> EmployeeLoggedIn;

        /// <summary>
        /// Raised when employee is logged out
        /// </summary>
        event EventHandler EmployeeLoggedOut;

        /// <summary>
        /// Caches logged employee
        /// </summary>
        void SetLoggedEmployee(ILoggedEmployee loggedEmployee);

        /// <summary>
        /// Clears logged employee
        /// </summary>
        void ResetLoggedEmployee();
    }
}
