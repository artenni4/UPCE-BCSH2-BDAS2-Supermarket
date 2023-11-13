using Supermarket.Core.UseCases.CashBox;

namespace Supermarket.Wpf.CashBox;

public class SelectedProductModel
{
    public required CashBoxProduct CashBoxProduct { get; init; }
    public required decimal Count { get; init; }
    public decimal OverallPrice => Count * CashBoxProduct.Price;
}