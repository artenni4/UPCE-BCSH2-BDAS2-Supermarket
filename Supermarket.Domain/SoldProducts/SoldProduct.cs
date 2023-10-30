using Supermarket.Domain.Common;

namespace Supermarket.Domain.SoldProducts;

public class SoldProduct : IEntity<SoldProductId>
{
    public required SoldProductId Id { get; init; }
    public required decimal Pieces { get; init; }
}