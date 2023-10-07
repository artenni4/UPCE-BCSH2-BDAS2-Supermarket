using Supermarket.Domain.Employees.Roles;

namespace Supermarket.Domain.Auth.LoggedEmployees
{
    public record LoggedSupermarketEmployee(int Id, IReadOnlyList<IEmployeeRole> Roles) : ILoggedEmployee;
}
