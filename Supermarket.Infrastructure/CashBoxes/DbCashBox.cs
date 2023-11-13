using Dapper;
using Supermarket.Core.Domain.CashBoxes;

namespace Supermarket.Infrastructure.CashBoxes;

public class DbCashBox : IDbEntity<CashBox, int, DbCashBox>
{
    public static string TableName => "POKLADNY";
    
    public required int pokladna_id { get; init; }
    public required int supermarket_id { get; init; }
    public required string nazev { get; init; }
    public required string kod { get; init; }
    public required string? poznamky { get; init; }

    public static IReadOnlyList<string> IdentityColumns { get; } = new[]
    {
        nameof(pokladna_id)
    };

    public CashBox ToDomainEntity() => new()
    {
        Id = pokladna_id,
        SupermarketId = supermarket_id,
        Name = nazev,
        Code = kod,
        Notes = poznamky
    };

    public static DbCashBox ToDbEntity(CashBox entity) => new()
    {
        pokladna_id = entity.Id,
        supermarket_id = entity.SupermarketId,
        nazev = entity.Name,
        kod = entity.Code,
        poznamky = entity.Notes
    };

    public static DynamicParameters GetEntityIdParameters(int id) =>
        new DynamicParameters().AddParameter(nameof(pokladna_id), id);
}