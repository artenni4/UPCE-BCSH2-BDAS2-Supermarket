using System;
using System.Threading.Tasks;
using Supermarket.Wpf.Common;

namespace Supermarket.Wpf.Dialog;

public class DialogServiceFake : IDialogService
{
    public event EventHandler<DialogViewModelEventArgs>? DialogShown;
    public event EventHandler? DialogHidden;
    public IViewModel? CurrentDialog { get; }
    public Task<TResult> ShowAsync<TDialog, TResult, TParameters>(TParameters parameters) where TDialog : class, IDialogViewModel<TResult, TParameters>
    {
        throw new NotImplementedException();
    }

    public void Hide()
    {
        throw new NotImplementedException();
    }
}