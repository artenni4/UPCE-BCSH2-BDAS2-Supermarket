using Dapper;
using Supermarket.Core.UseCases.Admin;

namespace Supermarket.Infrastructure.Supermarkets
{
    internal class DbAdminSupermarket : IDbEntity<AdminMenuSupermarket, int, DbAdminSupermarket>
    {
        public required int supermarket_id { get; init; }
        public required string adresa { get; init; }
        public required int region_id { get; init; }
        public required string region_nazev { get; init; }

        public static string TableName => "SUPERMARKETY";
        public static IReadOnlySet<string> IdentityColumns { get; } = new HashSet<string>
        {
            nameof(supermarket_id)
        };

        public AdminMenuSupermarket ToDomainEntity() => new()
        {
            Id = supermarket_id,
            Address = adresa,
            RegionId = region_id,
            RegionName = region_nazev
        };

        public static DbAdminSupermarket ToDbEntity(AdminMenuSupermarket entity) => new()
        {
            supermarket_id = entity.Id,
            adresa = entity.Address,
            region_id = entity.RegionId,
            region_nazev = entity.RegionName
        };

        public static DynamicParameters GetEntityIdParameters(int id) =>
            new DynamicParameters().AddParameter(nameof(supermarket_id), id);

        public DynamicParameters GetInsertingValues() => this.GetPropertiesExceptIdentity();
    }
}
