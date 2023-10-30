using Supermarket.Domain.Auth.LoggedEmployees;
using System;

namespace Supermarket.Wpf.LoggedUser
{
    internal class LoggedUserService : ILoggedUserService
    {
        public ILoggedEmployee? LoggedEmployee { get; private set; }

        public event EventHandler<LoggedEmployeeArgs>? EmployeeLoggedIn;
        public event EventHandler? EmployeeLoggedOut;

        public void ResetLoggedEmployee()
        {
            LoggedEmployee = null;
            EmployeeLoggedOut?.Invoke(this, EventArgs.Empty);
        }

        public void SetLoggedEmployee(ILoggedEmployee loggedEmployee)
        {
            LoggedEmployee = loggedEmployee;
            EmployeeLoggedIn?.Invoke(this, new LoggedEmployeeArgs { LoggedEmployee = LoggedEmployee });
        }
    }
}
