using Supermarket.Core.UseCases.CashBoxes;

namespace Supermarket.Wpf.Cashbox;

public class SelectedProductModel
{
    public required CashBoxProduct CashBoxProduct { get; init; }
    public required decimal Count { get; init; }
    public decimal OverallPrice => Count * CashBoxProduct.Price;
}