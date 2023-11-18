using Supermarket.Core.Domain.Common;
using Supermarket.Core.Domain.Employees;
using Supermarket.Core.Domain.Employees.Roles;

namespace Supermarket.Core.UseCases.Admin
{
    public class AdminEmployeeDetail : IEntity<int>
    {
        public required int Id { get; init; }
        public required string Name { get; init; }
        public required string Surname { get; init; }
        public required string Login { get; init; }
        public required DateTime HireDate { get; init; }
        public required string? PersonalNumber { get; init; }
        public required IEmployeeRoleInfo RoleInfo { get; init; }

        public static AdminEmployeeDetail FromEmployeeRole(EmployeeRole employeeRole) => new AdminEmployeeDetail()
        {
            Id = employeeRole.Id,
            Login = employeeRole.Login,
            Name = employeeRole.Name,
            Surname = employeeRole.Surname,
            HireDate = employeeRole.HireDate,
            PersonalNumber = employeeRole.PersonalNumber,
            RoleInfo = employeeRole.RoleInfo,
        };
    }
}
