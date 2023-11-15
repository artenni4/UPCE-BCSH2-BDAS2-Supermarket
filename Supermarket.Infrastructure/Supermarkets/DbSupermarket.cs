using Dapper;

namespace Supermarket.Infrastructure.Supermarkets;

internal class DbSupermarket : IDbEntity<Core.Domain.Supermarkets.Supermarket, int, DbSupermarket>
{
    public required int supermarket_id { get; init; }
    public required string adresa { get; init; }
    public required int region_id { get; init; }
    
    public static string TableName => "SUPERMARKETY";
    public static IReadOnlySet<string> IdentityColumns { get; } = new HashSet<string>
    {
        nameof(supermarket_id)
    };

    public Core.Domain.Supermarkets.Supermarket ToDomainEntity() => new()
    {
        Id = supermarket_id,
        Address = adresa,
        RegionId = region_id
    };

    public static DbSupermarket ToDbEntity(Core.Domain.Supermarkets.Supermarket entity) => new()
    {
        supermarket_id = entity.Id,
        adresa = entity.Address,
        region_id = entity.RegionId
    };

    public static DynamicParameters GetEntityIdParameters(int id) => 
        new DynamicParameters().AddParameter(nameof(supermarket_id), id);

    public DynamicParameters GetInsertingValues()
    {
        throw new NotImplementedException();
    }
}