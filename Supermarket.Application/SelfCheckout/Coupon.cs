namespace Supermarket.Core.SelfCheckout;

public class Coupon
{
    public required string Code { get; init; }
    public required decimal Discount { get; init; }
}