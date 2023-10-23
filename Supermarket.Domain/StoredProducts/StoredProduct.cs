using Supermarket.Domain.Common;

namespace Supermarket.Domain.StoredProducts
{
    public class StoredProduct : IEntity<int>
    {
        public required int Id { get; set; }
        public required int StoragePlaceId { get; set; }
        public required int SupermarketId { get; set; }
        public required int ProductId { get; set;}
    }
}
