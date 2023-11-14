using Supermarket.Core.Domain.Common;
using Supermarket.Core.Domain.Employees.Roles;

namespace Supermarket.Core.Domain.Employees
{
    public class EmployeeRole : IEntity<int>
    {
        public required int Id { get; init; }
        public required IReadOnlyList<IEmployeeRole> Roles { get; init; }
        public required string Name { get; init; }
        public required string Surname { get; init; }
        public required byte[] PasswordHash { get; init; }
        public required byte[] PasswordHashSalt { get; init; }

    }
}
