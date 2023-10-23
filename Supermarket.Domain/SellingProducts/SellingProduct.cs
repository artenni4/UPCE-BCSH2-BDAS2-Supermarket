using Supermarket.Domain.Common;

namespace Supermarket.Domain.SellingProducts
{
    public class SellingProduct : IEntity<int>
    {
        public required int Id { get; set; }
        public required int SupermarketId { get; set; }
    }
}
