using Supermarket.Core.Domain.Common;

namespace Supermarket.Core.Domain.SoldProducts;

public class SoldProduct : IEntity<SoldProductId>
{
    public required SoldProductId Id { get; init; }
    public required decimal Pieces { get; init; }
    public required decimal Price { get; init; }
}