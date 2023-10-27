using Supermarket.Domain.ProductCategories;

namespace Supermarket.Core.CashBoxes;

public class CashBoxProductCategory
{
    public required int Id { get; init; }
    public required string Name { get; init; }

    public static CashBoxProductCategory FromProductCategory(ProductCategory productCategory) => new()
    {
        Id = productCategory.Id,
        Name = productCategory.Name,
    };
}