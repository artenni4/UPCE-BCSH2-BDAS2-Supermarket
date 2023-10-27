using Dapper;
using Oracle.ManagedDataAccess.Client;
using Supermarket.Domain.Regions;
using Supermarket.Infrastructure.Common;

namespace Supermarket.Infrastructure.Regions;

internal class RegionRepository : CrudRepositoryBase<Region, int, DbRegion>, IRegionRepository
{
    public RegionRepository(OracleConnection oracleConnection) : base(oracleConnection)
    {
    }

    protected override string TableName => "REGIONY";
    protected override IReadOnlyList<string> IdentityColumns { get; } = new[] { nameof(DbRegion.region_id) };

    protected override Region MapToEntity(DbRegion dbEntity) => new()
    {
        Id = dbEntity.region_id,
        Name = dbEntity.nazev
    };

    protected override DbRegion MapToDbEntity(Region entity) => new()
    {
        region_id = entity.Id,
        nazev = entity.Name
    };

    protected override DynamicParameters GetIdentityValues(int id) => GetSimpleIdentityValue(id);

    protected override int ExtractIdentity(DynamicParameters dynamicParameters) =>
        ExtractSimpleIdentity(dynamicParameters);
}