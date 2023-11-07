using System;
using System.Threading.Tasks;
using Supermarket.Wpf.Common;

namespace Supermarket.Wpf.Dialog;

public class DialogServiceFake : IDialogService
{
    public event EventHandler<DialogViewModelEventArgs>? DialogShown;
    public event EventHandler? DialogHidden;
    public IViewModel? CurrentDialog { get; }
    public Task<TDialog?> TryShowAsync<TDialog, TResult>() where TDialog : class, IDialogViewModel<TResult>
    {
        throw new NotImplementedException();
    }

    public Task<TDialog?> TryShowAsync<TDialog>() where TDialog : class, IDialogViewModel
    {
        throw new NotImplementedException();
    }

    public void Hide()
    {
        throw new NotImplementedException();
    }
}