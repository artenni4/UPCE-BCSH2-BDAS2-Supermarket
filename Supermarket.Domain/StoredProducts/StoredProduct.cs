using Supermarket.Domain.Common;

namespace Supermarket.Domain.StoredProducts
{
    public class StoredProduct : IEntity<StoredProductId>
    {
        public required StoredProductId Id { get; init; }
        public required decimal Count { get; init; }
    }
}
