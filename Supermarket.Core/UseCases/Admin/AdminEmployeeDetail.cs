using Supermarket.Core.Domain.Common;
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
        public required int? SupermarketId { get; init; }
        public required string PersonalNumber { get; init; }
        public required IEmployeeRoleInfo RoleInfo { get; init; }
    }
}
