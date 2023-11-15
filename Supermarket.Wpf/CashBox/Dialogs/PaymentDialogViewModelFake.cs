using Supermarket.Wpf.Dialog;

namespace Supermarket.Wpf.CashBox.Dialogs;

public class PaymentDialogViewModelFake : PaymentDialogViewModel
{
    public PaymentDialogViewModelFake() : base(new DialogServiceFake(), new CashBoxServiceFake())
    {
    }
}