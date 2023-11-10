using Supermarket.Core.Domain.Common;

namespace Supermarket.Core.Domain.Regions;

public class Region : IEntity<int>
{
    public required int Id { get; init; }
    public required string Name { get; init; }
}