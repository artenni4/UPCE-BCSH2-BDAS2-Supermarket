using Supermarket.Core.UseCases.CashBox;

namespace Supermarket.Wpf.CashBox.Dialogs;

public record PaymentDialogResult(PaymentType PaymentType, Coupon[] UsedCoupons);