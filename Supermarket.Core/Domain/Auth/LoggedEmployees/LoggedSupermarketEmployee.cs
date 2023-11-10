using Supermarket.Core.Domain.Employees.Roles;

namespace Supermarket.Core.Domain.Auth.LoggedEmployees
{
    public record LoggedSupermarketEmployee(int Id, string Name, string Surname, IReadOnlyList<IEmployeeRole> Roles) : ILoggedEmployee;
}
