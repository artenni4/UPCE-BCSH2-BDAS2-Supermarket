namespace Supermarket.Core.CashBoxes
{
    public class CashBoxSoldProduct
    {
        public required int ProductId { get; init; }
        public required decimal Count { get; set; }
    }
}
