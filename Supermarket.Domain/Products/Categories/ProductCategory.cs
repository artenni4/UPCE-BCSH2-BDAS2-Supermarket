using Supermarket.Domain.Common;

namespace Supermarket.Domain.Products.Categories
{
    public class ProductCategory : IEntity<int>
    {
        public required int Id { get; init; }
        public required string Name { get; init; }
        public required string? Description { get; init; }
    }
}
