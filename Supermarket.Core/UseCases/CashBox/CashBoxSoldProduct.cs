namespace Supermarket.Core.UseCases.CashBox
{
    public class CashBoxSoldProduct
    {
        public required int ProductId { get; init; }
        public required decimal Count { get; init; }
        public required decimal Price { get; init; }
    }
}
