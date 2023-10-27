using Supermarket.Domain.Common;
using Supermarket.Domain.Products;
using Supermarket.Infrastructure.Common;

namespace Supermarket.Infrastructure.Products;

internal class DbProduct : IDbEntity<Product, int>
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
    
    public Product ToDomainEntity() => new()
    {
        Id = zbozi_id,
        ProductCategoryId = druh_zbozi_id,
        MeasureUnit = merna_jednotka_id switch
        {
            1 => MeasureUnit.Kilogram,
            2 => MeasureUnit.Gram,
            3 => MeasureUnit.Litre,
            4 => MeasureUnit.Millilitre,
            5 => MeasureUnit.Piece,
            6 => MeasureUnit.Meter,
            _ => throw new DatabaseException($"Measure unit [{merna_jednotka_id}] is not known")
        },
        ByWeight = naVahu != 0,
        Name = nazev,
        Price = cena,
        Weight = hmotnost,
        Barcode = carovyKod,
        Description = popis
    };
}