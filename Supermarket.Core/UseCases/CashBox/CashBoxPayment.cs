namespace Supermarket.Core.UseCases.CashBox;

public record CashBoxPayment(CashBoxPaymentType PaymentType, decimal Total, IReadOnlyList<Coupon> Coupons);
