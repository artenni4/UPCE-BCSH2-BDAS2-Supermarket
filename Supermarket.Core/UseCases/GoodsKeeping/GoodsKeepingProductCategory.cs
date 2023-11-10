using Supermarket.Core.Domain.ProductCategories;

namespace Supermarket.Core.UseCases.GoodsKeeping;

public class GoodsKeepingProductCategory
{
    public required int CategoryId { get; init; }
    public required string Name { get; init; }

    public static GoodsKeepingProductCategory FromProductCategory(ProductCategory productCategory) => new()
    {
        CategoryId = productCategory.Id,
        Name = productCategory.Name,
    };
}