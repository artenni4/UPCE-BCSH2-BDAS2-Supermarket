using Supermarket.Domain.Auth.LoggedEmployees;

namespace Supermarket.Wpf.LoggedUser
{
    internal class LoggedUserService : ILoggedUserService
    {
        public ILoggedEmployee? LoggedEmployee { get; private set; }

        public void ResetLoggedEmployee()
        {
            LoggedEmployee = null;
        }

        public void SetLoggedEmployee(ILoggedEmployee loggedEmployee)
        {
            LoggedEmployee = loggedEmployee;
        }
    }
}
