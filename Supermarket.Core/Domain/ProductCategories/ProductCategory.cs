using Supermarket.Core.Domain.Common;

namespace Supermarket.Core.Domain.ProductCategories
{
    public class ProductCategory : IEntity<int>
    {
        public required int Id { get; init; }
        public required string Name { get; init; }
        public required string? Description { get; init; }
    }
}
