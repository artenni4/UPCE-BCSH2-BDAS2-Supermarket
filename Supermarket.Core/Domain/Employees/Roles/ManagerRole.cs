namespace Supermarket.Core.Domain.Employees.Roles
{
    public record ManagerRole(int SupermarketId) : IEmployeeRole;
}
