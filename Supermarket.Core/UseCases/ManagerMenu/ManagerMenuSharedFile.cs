
using Supermarket.Core.Domain.Common;

namespace Supermarket.Core.UseCases.ManagerMenu
{
    public class ManagerMenuSharedFile : IEntity<int>
    {
        public required int Id { get; init; }
        public required string Name { get; init; }
        public required string Extenstion { get; init; }
        public string FullName => $"{Name}{Extenstion}";
        public required DateTime CreatedDate { get; init; }
        public required DateTime? ModifiedDate { get; init; }
        public required int SupermarketId { get; init; }
        public required int EmployeeId { get; init; }
        public required string EmployeeName { get; init; }
    }
}
