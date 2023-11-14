using Supermarket.Core.Domain.Auth.LoggedEmployees;
using System.Diagnostics.CodeAnalysis;

namespace Supermarket.Wpf.LoggedUser
{
    internal class LoggedUserService : ILoggedUserService
    {
        private EmployeeData? _employeeData;
        private IReadOnlyList<SupermarketEmployeeRole>? _roles;

        public bool IsUserSet { get; private set; }

        public bool IsEmployee
        {
            get
            {
                CheckUserIsSet();
                return _employeeData is not null;
            }
        }

        public bool IsCustomer
        {
            get
            {
                CheckUserIsSet();
                return _employeeData is null;
            }
        }

        private int _supermarketId;
        public int SupermarketId
        {
            get
            {
                CheckUserIsSet();
                return _supermarketId;
            }
            private set => _supermarketId = value;
        }

        public event EventHandler? UserLoggedIn;
        public event EventHandler? UserLoggedOut;
        
        public bool IsAdmin([NotNullWhen(true)] out EmployeeData? loggedAdmin)
        {
            CheckUserIsSet();
            
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
            CheckUserIsSet();
            
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
            IsUserSet = true;
            UserLoggedIn?.Invoke(this, EventArgs.Empty);
        }

        public void SetAdmin(LoggedAdmin loggedAdmin, int supermarketId)
        {
            _employeeData = EmployeeData.FromLoggedEmployee(loggedAdmin);
            SupermarketId = supermarketId;
            IsUserSet = true;
            UserLoggedIn?.Invoke(this, EventArgs.Empty);
        }

        public void SetCustomer(int supermarketId)
        {
            SupermarketId = supermarketId;
            IsUserSet = true;
            UserLoggedIn?.Invoke(this, EventArgs.Empty);
        }

        public void UnsetUser()
        {
            IsUserSet = false;
            UserLoggedOut?.Invoke(this, EventArgs.Empty);
        }

        private void CheckUserIsSet()
        {
            if (IsUserSet == false)
            {
                throw new InvalidOperationException("User is not set");
            }
        }
    }
}
