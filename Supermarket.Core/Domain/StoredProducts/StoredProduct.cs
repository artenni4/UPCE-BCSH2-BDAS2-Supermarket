using Supermarket.Core.Domain.Common;

namespace Supermarket.Core.Domain.StoredProducts
{
    public class StoredProduct : IEntity<StoredProductId>
    {
        public required StoredProductId Id { get; init; }
        public required decimal Count { get; init; }
    }
}
