using Supermarket.Domain.Auth.LoggedEmployees;
using System;

namespace Supermarket.Wpf.LoggedUser
{
    public class LoggedUserArgs : EventArgs
    {
        public required ILoggedEmployee LoggedEmployee { get; init; }
    }
}
