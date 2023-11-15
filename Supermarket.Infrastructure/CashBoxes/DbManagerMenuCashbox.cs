using Dapper;
using Supermarket.Core.UseCases.ManagerMenu;

namespace Supermarket.Infrastructure.CashBoxes
{

    public class DbManagerMenuCashbox : IDbEntity<ManagerMenuCashbox, int, DbManagerMenuCashbox>
    {
        public static string TableName => "POKLADNY";

        public required int pokladna_id { get; init; }
        public required int supermarket_id { get; init; }
        public required string nazev { get; init; }
        public required string kod { get; init; }
        public required string? poznamky { get; init; }

        public static IReadOnlySet<string> IdentityColumns { get; } = new HashSet<string>
        {
            nameof(pokladna_id)
        };

        public ManagerMenuCashbox ToDomainEntity() => new()
        {
            Id = pokladna_id,
            SupermarketId = supermarket_id,
            Name = nazev,
            Code = kod,
            Notes = poznamky
        };

        public static DbManagerMenuCashbox ToDbEntity(ManagerMenuCashbox entity) => new()
        {
            pokladna_id = entity.Id,
            supermarket_id = entity.SupermarketId,
            nazev = entity.Name,
            kod = entity.Code,
            poznamky = entity.Notes
        };

        public static DynamicParameters GetEntityIdParameters(int id) =>
            new DynamicParameters().AddParameter(nameof(pokladna_id), id);

        public DynamicParameters GetInsertingValues() => this.GetPropertiesExceptIdentity();
    }
}
