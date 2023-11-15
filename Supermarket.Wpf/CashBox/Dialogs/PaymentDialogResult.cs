using Supermarket.Core.UseCases.CashBox;

namespace Supermarket.Wpf.CashBox.Dialogs;

public record PaymentDialogResult(CashBoxPaymentType CashBoxPaymentType, Coupon[] UsedCoupons);