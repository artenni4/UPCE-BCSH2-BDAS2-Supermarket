using Supermarket.Core.Domain.Auth.LoggedEmployees;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Supermarket.Wpf.LoggedUser
{
    internal class LoggedUserService : ILoggedUserService
    {
        private EmployeeData? _employeeData;
        private IReadOnlyList<SupermarketEmployeeRole>? _roles;

        public bool IsEmployee => _employeeData is not null;
        public bool IsCustomer => _employeeData is null;
        public int SupermarketId { get; private set; }
        public event EventHandler? UserLoggedIn;
        public event EventHandler? UserLoggedOut;
        
        public bool IsAdmin([NotNullWhen(true)] out EmployeeData? loggedAdmin)
        {
            if (_employeeData is not null && _roles is null)
            {
                loggedAdmin = _employeeData;
                return true;
            }

            loggedAdmin = null;
            return false;
        }

        public bool IsSupermarketEmployee(
            [NotNullWhen(true)] out EmployeeData? loggedSupermarketEmployee,
            [NotNullWhen(true)] out IReadOnlyList<SupermarketEmployeeRole>? roles)
        {
            if (_employeeData is not null && _roles is not null)
            {
                loggedSupermarketEmployee = _employeeData;
                roles = _roles;
                return true;
            }

            loggedSupermarketEmployee = null;
            roles = null;
            return false;
        }

        public void SetSupermarketEmployee(LoggedSupermarketEmployee loggedSupermarketEmployee)
        {
            _employeeData = EmployeeData.FromLoggedEmployee(loggedSupermarketEmployee);
            _roles = loggedSupermarketEmployee.Roles;
            SupermarketId = loggedSupermarketEmployee.SupermarketId;
            
            UserLoggedIn?.Invoke(this, EventArgs.Empty);
        }

        public void SetAdmin(LoggedAdmin loggedAdmin, int supermarketId)
        {
            _employeeData = EmployeeData.FromLoggedEmployee(loggedAdmin);
            SupermarketId = supermarketId;
            
            UserLoggedIn?.Invoke(this, EventArgs.Empty);
        }

        public void SetCustomer(int supermarketId)
        {
            SupermarketId = supermarketId;
            
            UserLoggedIn?.Invoke(this, EventArgs.Empty);
        }

        public void UnsetUser()
        {
            UserLoggedOut?.Invoke(this, EventArgs.Empty);
        }
    }
}
