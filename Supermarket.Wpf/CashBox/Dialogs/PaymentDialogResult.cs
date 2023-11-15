using Supermarket.Core.UseCases.CashBox;

namespace Supermarket.Wpf.CashBox.Dialogs;

public record PaymentDialogResult(
    CashBoxPaymentType PaymentType,
    decimal Total,
    IReadOnlyList<Coupon> UsedCoupons)
{
    public CashBoxPayment ToCashBoxPayment() => new (PaymentType, Total, UsedCoupons);
}