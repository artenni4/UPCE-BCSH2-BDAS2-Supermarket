namespace Supermarket.Domain.Employees.Roles
{
    public record ManagerRole(int SupermarketId) : IEmployeeRole;
}
