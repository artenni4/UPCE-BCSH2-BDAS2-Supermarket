namespace Supermarket.Core.SelfCheckout
{
    public class SoldProduct
    {
        public required int ProductId { get; init; }
        public required decimal Count { get; set; }
    }
}
