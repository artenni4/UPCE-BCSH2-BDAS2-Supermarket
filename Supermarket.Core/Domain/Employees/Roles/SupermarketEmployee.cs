using Supermarket.Core.Domain.Auth.LoggedEmployees;

namespace Supermarket.Core.Domain.Employees.Roles;

public record SupermarketEmployee(int SupermarketId, int? ManagerId, IReadOnlySet<SupermarketEmployeeRole> Roles) : IEmployeeRoleInfo;
