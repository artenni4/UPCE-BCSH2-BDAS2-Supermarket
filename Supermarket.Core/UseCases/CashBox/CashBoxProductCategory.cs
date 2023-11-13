using Supermarket.Core.Domain.ProductCategories;

namespace Supermarket.Core.UseCases.CashBox;

public class CashBoxProductCategory
{
    public required int CategoryId { get; init; }
    public required string Name { get; init; }

    public static CashBoxProductCategory FromProductCategory(ProductCategory productCategory) => new()
    {
        CategoryId = productCategory.Id,
        Name = productCategory.Name,
    };
}