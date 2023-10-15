using Supermarket.Domain.Common;

namespace Supermarket.Domain.Supermarkets;

public class Supermarket : IEntity<int>
{
    public required int Id { get; init; }
    public required string Address { get; init; }
    public required int RegionId { get; init; }
}