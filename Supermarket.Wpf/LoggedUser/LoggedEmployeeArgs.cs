using Supermarket.Core.Domain.Auth.LoggedEmployees;
using System;

namespace Supermarket.Wpf.LoggedUser
{
    public class LoggedEmployeeArgs : EventArgs
    {
        public required ILoggedEmployee LoggedEmployee { get; init; }
    }
}
