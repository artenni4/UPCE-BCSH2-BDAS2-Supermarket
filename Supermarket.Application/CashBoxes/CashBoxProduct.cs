using Supermarket.Domain.Products;

namespace Supermarket.Core.CashBoxes;

public class CashBoxProduct
{
    public required int ProductId { get; init; }
    public required string Name { get; init; }
    public required bool IsByWeight { get; init; }

    public static CashBoxProduct FromProduct(Product product) => new()
    {
        ProductId = product.Id,
        Name = product.Name,
        IsByWeight = product.ByWeight
    };
}