using Supermarket.Core.Domain.Products;

namespace Supermarket.Core.UseCases.CashBox;

public class CashBoxProduct
{
    public required int ProductId { get; init; }
    public required string Name { get; init; }
    public required bool IsByWeight { get; init; }
    public required string MeasureUnit { get; init; }
    public required decimal Price { get; init; }

    public static CashBoxProduct FromProduct(Product product) => new()
    {
        ProductId = product.Id,
        Name = product.Name,
        IsByWeight = product.ByWeight,
        MeasureUnit = product.MeasureUnit.Abbreviation,
        Price = product.Price
    };
}