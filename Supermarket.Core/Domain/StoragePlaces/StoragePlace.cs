using Supermarket.Core.Domain.Common;

namespace Supermarket.Core.Domain.StoragePlaces
{
    public class StoragePlace : IEntity<int>
    {
        public required int Id { get; init; }
        public required string Code { get; set; }
        public string? Location { get; set; }
        public required int SupermarketId { get; set; }
        public required StoragePlaceType Type { get; set; }
    }
}
