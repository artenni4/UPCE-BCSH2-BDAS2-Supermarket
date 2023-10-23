using Supermarket.Domain.Products.Categories;

namespace Supermarket.Core.GoodsKeeping;

public class GoodsKeepingProductCategory
{
    public required int Id { get; init; }
    public required string Name { get; init; }

    public static GoodsKeepingProductCategory FromProductCategory(ProductCategory productCategory) => new()
    {
        Id = productCategory.Id,
        Name = productCategory.Name,
    };
}