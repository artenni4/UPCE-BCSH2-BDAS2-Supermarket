namespace Supermarket.Wpf.Dialog;

public class DialogServiceFake : IDialogService
{
    public event EventHandler<DialogViewModelEventArgs>? DialogShown;
    public event EventHandler? DialogHidden;
    public IEnumerable<IViewModel> DisplayedDialogs { get; } = Enumerable.Empty<IViewModel>();

    public Task<DialogResult<TResult>> ShowAsync<TDialog, TResult, TParameters>(TParameters parameters) where TDialog : class, IDialogViewModel<TResult, TParameters>
    {
        throw new NotImplementedException();
    }

    public Task<DialogResult<TResult>> ShowAsync<TDialog, TResult>() where TDialog : class, IDialogViewModel<TResult>
    {
        throw new NotImplementedException();
    }

    public Task<DialogResult> ShowAsync<TDialog>() where TDialog : class, IDialogViewModel
    {
        throw new NotImplementedException();
    }

    public void Hide()
    {
        throw new NotImplementedException();
    }
}