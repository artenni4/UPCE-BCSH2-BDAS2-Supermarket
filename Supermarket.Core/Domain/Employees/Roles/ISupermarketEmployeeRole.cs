namespace Supermarket.Core.Domain.Employees.Roles;

public interface ISupermarketEmployeeRole : IEmployeeRole
{
    int SupermarketId { get; }
}