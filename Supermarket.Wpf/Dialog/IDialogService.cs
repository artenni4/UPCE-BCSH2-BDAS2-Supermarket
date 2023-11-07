using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Supermarket.Wpf.Common;

namespace Supermarket.Wpf.Dialog;

public interface IDialogService
{
    event EventHandler<DialogViewModelEventArgs> DialogShown;
    event EventHandler DialogHidden;
    IViewModel? CurrentDialog { get; } 
    Task<TDialog?> TryShowAsync<TDialog, TResult>() where TDialog : class, IDialogViewModel<TResult>;
    Task<TDialog?> TryShowAsync<TDialog>() where TDialog : class, IDialogViewModel;
    void Hide();
}