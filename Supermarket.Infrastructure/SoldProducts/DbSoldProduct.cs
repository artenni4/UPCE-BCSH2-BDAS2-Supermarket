using Dapper;
using Supermarket.Core.Domain.SoldProducts;

namespace Supermarket.Infrastructure.SoldProducts;

internal class DbSoldProduct : IDbEntity<SoldProduct, SoldProductId, DbSoldProduct>
{
    public required int prodej_id { get; init; }
    public required int supermarket_id { get; init; }
    public required int zbozi_id { get; init; }
    public required decimal kusy { get; init; }
    public required decimal celkova_cena { get; init; }
    
    public static string TableName => "PRODANE_ZBOZI";

    public static IReadOnlySet<string> IdentityColumns { get; } = new HashSet<string>
    {
        nameof(prodej_id),
        nameof(supermarket_id),
        nameof(zbozi_id)
    };

    public SoldProduct ToDomainEntity() => new SoldProduct
    {
        Id = new SoldProductId(prodej_id, supermarket_id, zbozi_id),
        Pieces = kusy,
        Price = celkova_cena,
    };

    public static DbSoldProduct ToDbEntity(SoldProduct entity) => new DbSoldProduct
    {
        zbozi_id = entity.Id.ProductId,
        supermarket_id = entity.Id.SupermarketId,
        prodej_id = entity.Id.SaleId,
        kusy = entity.Pieces,
        celkova_cena = entity.Price
    };

    public static DynamicParameters GetEntityIdParameters(SoldProductId id) => new DynamicParameters()
        .AddParameter(nameof(prodej_id), id.ProductId)
        .AddParameter(nameof(supermarket_id), id.SupermarketId)
        .AddParameter(nameof(zbozi_id), id.ProductId);

    public DynamicParameters GetInsertingValues() => this.GetAllProperties();
}