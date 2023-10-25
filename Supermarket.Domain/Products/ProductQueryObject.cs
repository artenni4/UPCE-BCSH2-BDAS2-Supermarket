using Supermarket.Domain.Common;
using Supermarket.Domain.Common.Paging;

namespace Supermarket.Domain.Products
{
    public class ProductQueryObject : IQueryObject
    {
        public required RecordsRange RecordsRange { get; init; }
        public required int SupermarketId { get; init; }
        public required int? ProductCategoryId { get; init; }
        public required string? SearchText { get; init; }
    }
}
