using Supermarket.Core.Domain.UsedDatabaseObjects;

namespace Supermarket.Infrastructure.UsedDatabaseObjects;

public class DbUsedDatabaseObjectRepository
{
    public required string object_name { get; init; }
    public required string object_type { get; init; }

    public UsedDatabaseObject ToDomainEntity() => new UsedDatabaseObject()
    {
        ObjectName = object_name,
        ObjectType = object_type
    };
}