namespace Supermarket.Wpf.Dialog;

public class DialogViewModelEventArgs : EventArgs
{
    public required IViewModel ViewModel { get; init; }
}