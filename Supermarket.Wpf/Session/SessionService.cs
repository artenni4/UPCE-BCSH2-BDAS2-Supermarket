using Supermarket.Domain.Auth.LoggedEmployees;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Supermarket.Wpf.Session
{
    internal class SessionService : ILoggedUserService
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
