using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Supermarket.Core.Domain.Auth.LoggedEmployees;

namespace Supermarket.Wpf.LoggedUser;

public class LoggedUserServiceFake : ILoggedUserService
{
    public bool IsEmployee { get; }
    public bool IsCustomer { get; }
    public int SupermarketId { get; }
    public bool IsAdmin([NotNullWhen(true)] out EmployeeData? loggedAdmin)
    {
        throw new NotImplementedException();
    }

    public bool IsSupermarketEmployee([NotNullWhen(true)] out EmployeeData? loggedSupermarketEmployee,[NotNullWhen(true)]  out IReadOnlyList<SupermarketEmployeeRole>? roles)
    {
        throw new NotImplementedException();
    }

    public event EventHandler? UserLoggedIn;
    public event EventHandler? UserLoggedOut;
    public void SetSupermarketEmployee(LoggedSupermarketEmployee loggedSupermarketEmployee)
    {
        throw new NotImplementedException();
    }

    public void SetAdmin(LoggedAdmin loggedAdmin, int supermarketId)
    {
        throw new NotImplementedException();
    }

    public void SetCustomer(int supermarketId)
    {
        throw new NotImplementedException();
    }

    public void UnsetUser()
    {
        throw new NotImplementedException();
    }
}