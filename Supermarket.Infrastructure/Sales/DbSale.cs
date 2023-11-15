using Dapper;
using Supermarket.Core.Domain.Sales;

namespace Supermarket.Infrastructure.Sales;

internal class DbSale : IDbEntity<Sale, int, DbSale>
{
    public required int prodej_id { get; init; }
    public required DateTimeOffset datum { get; init; }
    public required int pokladna_id { get; init; }
    
    public static string TableName => "PRODEJE";
    
    public static IReadOnlySet<string> IdentityColumns { get; } = new HashSet<string>
    {
        nameof(prodej_id)
    };

    public Sale ToDomainEntity() => new()
    {
        Id = prodej_id,
        Date = datum,
        CashBoxId = pokladna_id
    };

    public static DynamicParameters GetEntityIdParameters(int id) =>
        new DynamicParameters().AddParameter(nameof(prodej_id), id);

    public static DbSale ToDbEntity(Sale entity) => new()
    {
        prodej_id = entity.Id,
        datum = entity.Date,
        pokladna_id = entity.CashBoxId,
    };

    public DynamicParameters GetInsertingValues() => new DynamicParameters()
        .AddParameter(nameof(datum), datum)
        .AddParameter(nameof(pokladna_id), pokladna_id);
}