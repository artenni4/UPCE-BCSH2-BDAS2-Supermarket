using Dapper;
using Supermarket.Core.Domain.Common;
using Supermarket.Core.Domain.Products;

namespace Supermarket.Infrastructure.Products;

internal class DbProduct : IDbEntity<Product, int, DbProduct>
{
    public required int zbozi_id { get; init; }
    public required int druh_zbozi_id { get; init; }
    public required int merna_jednotka_id { get; init; }
    public required int naVahu { get; init; }
    public required string nazev { get; init; }
    public required decimal cena { get; init; }
    public required decimal hmotnost { get; init; }
    public required string? carovyKod { get; init; }
    public required string? popis { get; init; }
    public required int dodavatel_id { get; init; }

    public static string TableName => "ZBOZI";

    public static IReadOnlySet<string> IdentityColumns { get; } = new HashSet<string>
    {
        nameof(zbozi_id)
    };

    public Product ToDomainEntity() => new()
    {
        Id = zbozi_id,
        ProductCategoryId = druh_zbozi_id,
        MeasureUnit = DbMeasureUnit.GetMeasureUnit(merna_jednotka_id),
        ByWeight = naVahu != 0,
        Name = nazev,
        Price = cena,
        Weight = hmotnost,
        Barcode = carovyKod,
        Description = popis,
        SupplierId = dodavatel_id
    };

    public static DbProduct ToDbEntity(Product entity) => new()
    {
        zbozi_id = entity.Id,
        druh_zbozi_id = entity.ProductCategoryId,
        merna_jednotka_id = DbMeasureUnit.GetMeasureUnitId(entity.MeasureUnit),
        carovyKod = entity.Barcode,
        cena = entity.Price,
        hmotnost = entity.Weight,
        naVahu = entity.ByWeight ? 1 : 0,
        nazev = entity.Name,
        popis = entity.Description,
        dodavatel_id = entity.SupplierId
    };

    public static DynamicParameters GetEntityIdParameters(int id) =>
        new DynamicParameters().AddParameter(nameof(zbozi_id), id);

    public DynamicParameters GetInsertingValues() => this.GetPropertiesExceptIdentity();

    
}