using Supermarket.Core.Domain.Auth.LoggedEmployees;

namespace Supermarket.Wpf.LoggedUser;

public record EmployeeData(int Id, string Name, string Surname)
{
    public static EmployeeData FromLoggedEmployee(ILoggedEmployee loggedEmployee) =>
        new(loggedEmployee.Id, loggedEmployee.Name, loggedEmployee.Surname);
}