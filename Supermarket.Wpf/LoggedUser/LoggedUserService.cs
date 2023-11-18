using Supermarket.Core.Domain.Auth.LoggedEmployees;
using System.Diagnostics.CodeAnalysis;

namespace Supermarket.Wpf.LoggedUser
{
    internal class LoggedUserService : ILoggedUserService
    {
        private EmployeeData? _employeeData;
        private IReadOnlySet<SupermarketEmployeeRole>? _roles;
        private bool _adminHasSupermarketId;
        
        public bool IsUserSet { get; private set; }

        public bool IsEmployee([NotNullWhen(true)] out EmployeeData? employeeData)
        {
            CheckUserIsSet();
            if (_employeeData is not null)
            {
                employeeData = _employeeData;
                return true;
            }

            employeeData = null;
            return false;
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
                if (IsAdmin(out _) && _adminHasSupermarketId == false)
                {
                    throw new InvalidOperationException($"Admin does not have {SupermarketId} set yet");
                }
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
            [NotNullWhen(true)] out IReadOnlySet<SupermarketEmployeeRole>? roles)
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

        public void SetAdmin(LoggedAdmin loggedAdmin)
        {
            _employeeData = EmployeeData.FromLoggedEmployee(loggedAdmin);
            IsUserSet = true;
            _adminHasSupermarketId = false;
            UserLoggedIn?.Invoke(this, EventArgs.Empty);
        }

        public void SetAdminSupermarket(int supermarketId)
        {
            if (IsAdmin(out _) == false)
            {
                throw new InvalidOperationException("Current user is not admin");
            }
            SupermarketId = supermarketId;
            _adminHasSupermarketId = true;
        }

        public void SetCustomer(int supermarketId)
        {
            SupermarketId = supermarketId;
            IsUserSet = true;
            UserLoggedIn?.Invoke(this, EventArgs.Empty);
        }

        public void UnsetUser()
        {
            _employeeData = null;
            _roles = null;
            _adminHasSupermarketId = false;
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
