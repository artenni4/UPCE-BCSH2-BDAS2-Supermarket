using Supermarket.Domain.Common;

namespace Supermarket.Domain.SellingProducts
{
    public class SellingProduct : IEntity<SellingProductId>
    {
        public required SellingProductId Id { get; init; }
    }
}
