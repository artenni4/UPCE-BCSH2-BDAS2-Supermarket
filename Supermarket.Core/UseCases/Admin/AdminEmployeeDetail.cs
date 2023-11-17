using Supermarket.Core.Domain.Common;

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
        public required int? ManagerId { get; init; }
        public required bool IsCashier { get; init; }
        public required bool IsGoodsKeeper { get; init; }
        public required bool IsManager { get; init; }
        public required bool IsAdmin { get; init;}
        public required string PersonalNumber { get; init; }
    }
}
