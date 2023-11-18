using Supermarket.Core.Domain.Auth.LoggedEmployees;
using Supermarket.Core.Domain.Employees.Roles;

namespace Supermarket.Core.UseCases.Admin
{
    public class AdminEmployeeData
    {
        public required int Id { get; init; }
        public required string Name { get; init; }
        public required string Surname { get; init; }
        public required string Login { get; init; }
        public required DateTime HireDate { get; init; }
        public required IEmployeeRoleInfo RoleInfo { get; init; }
        public required string PersonalNumber { get; init; }
    }
}
