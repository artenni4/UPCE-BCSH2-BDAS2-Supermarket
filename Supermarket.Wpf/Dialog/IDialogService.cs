namespace Supermarket.Wpf.Dialog;

public interface IDialogService
{
    event EventHandler<DialogViewModelEventArgs> DialogShown;
    event EventHandler DialogHidden;
    IEnumerable<IViewModel> DisplayedDialogs { get; }
    Task<DialogResult<TResult>> ShowAsync<TDialog, TResult, TParameters>(TParameters parameters) where TDialog : class, IDialogViewModel<TResult, TParameters>;
    Task<DialogResult<TResult>> ShowAsync<TDialog, TResult>() where TDialog : class, IDialogViewModel<TResult>;
    Task<DialogResult> ShowAsync<TDialog>() where TDialog : class, IDialogViewModel;
    void Hide();
}