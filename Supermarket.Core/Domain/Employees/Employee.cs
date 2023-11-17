using Supermarket.Core.Domain.Common;

namespace Supermarket.Core.Domain.Employees
{
    public class Employee : IEntity<int>
    {
        public required int Id { get; init; }
        public required string Login { get; init; }
        public required byte[] PasswordHash { get; init; }
        public required byte[] PasswordHashSalt { get; init; }
        public required string Name { get; init; }
        public required string Surname { get; init; }
        public required DateTimeOffset StartedWorking { get; init; }
        public required int? ManagerId { get; init; }
        public required int? SupermarketId { get; init; }
        public string? PersonalNumber { get; init; }
    }
}
