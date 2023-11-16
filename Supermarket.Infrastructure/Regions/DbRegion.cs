using Dapper;
using Supermarket.Core.Domain.Regions;

namespace Supermarket.Infrastructure.Regions;

internal class DbRegion : IDbEntity<Region, int, DbRegion>
{
    public required int region_id { get; init; }
    public required string nazev { get; init; }


    public static string TableName => "REGIONY";

    public static IReadOnlySet<string> IdentityColumns { get; } = new HashSet<string>
    {
        nameof(region_id)
    };

    public Region ToDomainEntity() => new()
    {
        Id = region_id,
        Name = nazev
    };

    public static DbRegion ToDbEntity(Region entity) => new()
    {
        region_id = entity.Id,
        nazev = entity.Name
    };

    public static DynamicParameters GetEntityIdParameters(int id) =>
        new DynamicParameters().AddParameter(nameof(region_id), id);

    public DynamicParameters GetInsertingValues() => this.GetPropertiesExceptIdentity();
}