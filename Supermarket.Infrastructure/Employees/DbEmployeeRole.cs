using Supermarket.Core.Domain.Auth.LoggedEmployees;
using Supermarket.Core.Domain.Employees.Roles;

namespace Supermarket.Infrastructure.Employees;

public class DbEmployeeRole
{
    public required int zamestnanec_id { get; init; }
    public required int role_id { get; init; }
    
    public static DbEmployeeRole[] ToDbEmployeeRoles(int employeeId, IEmployeeRoleInfo employeeRoleInfo)
    {
        if (employeeRoleInfo is Admin)
        {
            return new[]
            {
                new DbEmployeeRole
                {
                    role_id = 4,
                    zamestnanec_id = employeeId,
                }
            };
        }

        if (employeeRoleInfo is SupermarketEmployee supermarketEmployee)
        {
            return supermarketEmployee.Roles.Select(r =>
            {
                var roleId = r switch
                {
                    SupermarketEmployeeRole.Cashier => 1,
                    SupermarketEmployeeRole.Manager => 2,
                    SupermarketEmployeeRole.GoodsKeeper => 3,
                    _ => throw new ArgumentException(nameof(employeeRoleInfo))
                };

                return new DbEmployeeRole
                {
                    role_id = roleId,
                    zamestnanec_id = employeeId
                };
            }).ToArray();
        }

        throw new NotSupportedException($"{employeeRoleInfo} is not supported");
    }
}