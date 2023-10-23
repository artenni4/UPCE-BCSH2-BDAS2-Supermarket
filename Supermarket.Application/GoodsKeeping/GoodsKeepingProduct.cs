using Supermarket.Domain.Products;

namespace Supermarket.Core.GoodsKeeping;

public class GoodsKeepingProduct
{
    public required int ProductId { get; init; }
    public required string Name { get; init; }

    public static GoodsKeepingProduct FromProduct(Product product) => new()
    {
        ProductId = product.Id,
        Name = product.Name,
    };
}