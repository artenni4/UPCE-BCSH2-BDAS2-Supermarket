using Supermarket.Wpf.Dialog;

namespace Supermarket.Wpf.CashBox.Dialogs;

public class PaymentDialogViewModel : NotifyPropertyChangedBase, IDialogViewModel<PaymentDialogResult, decimal>
{
    
    
    public void SetParameters(decimal parameters)
    {
        throw new NotImplementedException();
    }

    public event EventHandler<DialogResult<PaymentDialogResult>>? ResultReceived;
}