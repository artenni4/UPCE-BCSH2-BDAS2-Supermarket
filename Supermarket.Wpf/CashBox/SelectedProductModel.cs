using Supermarket.Core.Domain.Products;
using Supermarket.Core.UseCases.CashBox;

namespace Supermarket.Wpf.CashBox;

public class SelectedProductModel
{
    public required int ProductId { get; init; }
    public required string ProductName { get; init; }
    public required decimal Price { get; init; }
    public required decimal Count { get; init; }
    public required MeasureUnit MeasureUnit { get; init; }
    public decimal OverallPrice => Count * Price;

    public static SelectedProductModel FromCashBoxProduct(CashBoxProduct product, decimal count) => new SelectedProductModel
    {
        ProductId = product.ProductId,
        Count = count,
        MeasureUnit = product.MeasureUnit,
        Price = product.Price,
        ProductName = product.Name
    };
}