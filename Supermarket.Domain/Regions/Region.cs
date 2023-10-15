using Supermarket.Domain.Common;

namespace Supermarket.Domain.Regions;

public class Region : IEntity<int>
{
    public required int Id { get; init; }
    public required string Name { get; init; }
}