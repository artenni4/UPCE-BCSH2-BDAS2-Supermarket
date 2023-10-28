using Dapper;
using Supermarket.Domain.Regions;

namespace Supermarket.Infrastructure.Regions;

internal class DbRegion : IDbEntity<Region, int, DbRegion>
{
    public required int region_id { get; init; }
    public required string nazev { get; init; }
    
    
    public static string TableName => "REGIONY";
    
    public static IReadOnlyList<string> IdentityColumns { get; } = new[]
    {
        nameof(region_id)
    };
    
    public Region ToDomainEntity() => new()
    {
        Id = region_id,
        Name = nazev
    };
    public static DbRegion MapToDbEntity(Region entity) => new()
    {
        region_id = entity.Id,
        nazev = entity.Name
    };

    public static DynamicParameters GetEntityIdParameters(int id)
    {
        throw new NotImplementedException();
    }

    public static DynamicParameters GetOutputIdentityParameters()
    {
        throw new NotImplementedException();
    }

    public static int ExtractIdentity(DynamicParameters dynamicParameters)
    {
        throw new NotImplementedException();
    }
}