using Supermarket.Core.Domain.Common;

namespace Supermarket.Core.Domain.SellingProducts
{
    public class SellingProduct : IEntity<SellingProductId>
    {
        public required SellingProductId Id { get; init; }
    }
}
