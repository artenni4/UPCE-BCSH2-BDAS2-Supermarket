using Supermarket.Domain.Employees.Roles;

namespace Supermarket.Domain.Auth.LoggedEmployees
{
    public record LoggedSupermarketEmployee(int Id, string Name, string Surname, IReadOnlyList<IEmployeeRole> Roles) : ILoggedEmployee;
}
