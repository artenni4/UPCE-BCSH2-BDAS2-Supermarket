using Supermarket.Domain.Common;

namespace Supermarket.Domain.SoldProducts
{
    public class SoldProduct : IEntity<int>
    {
        public required int Id { get; set; }
        public required decimal Pieces { get; set; }
        public required int SupermarketId { get; set; }
        public required int ProductId { get; set; }
    }
}
