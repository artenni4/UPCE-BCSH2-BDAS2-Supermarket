using Supermarket.Core.Domain.Common;

namespace Supermarket.Core.UseCases.Admin
{
    public class AdminProduct : IEntity<int>
    {
        public required int Id { get; set; }
        public required string Name { get; set; }
        public required int CategoryId { get; set; }
        public required string CategoryName { get; set; }
        public required bool IsByWeight { get; set; }
        public required decimal Price { get; set; }
        public required string Code { get; set; }
        public required int SupplierId { get; set; }
        public required string SupplierName { get; set; }
        public required string Description { get; set; }
        public required string PersonalNumber { get; set; }
    }
}
