using System;
using Supermarket.Core.Domain.Auth.LoggedEmployees;

namespace Supermarket.Wpf.LoggedUser;

public class LoggedUserServiceFake : ILoggedUserService
{
    public ILoggedEmployee? LoggedEmployee { get; }
    public event EventHandler<LoggedEmployeeArgs>? EmployeeLoggedIn;
    public event EventHandler? EmployeeLoggedOut;
    public void SetLoggedEmployee(ILoggedEmployee loggedEmployee)
    {
    }

    public void ResetLoggedEmployee()
    {
    }
}