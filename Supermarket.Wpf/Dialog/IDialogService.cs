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
    Task<DialogResult<TResult>> ShowAsync<TDialog, TResult, TParameters>(TParameters parameters) where TDialog : class, IDialogViewModel<TResult, TParameters>;
    Task<DialogResult<TResult>> ShowAsync<TDialog, TResult>() where TDialog : class, IDialogViewModel<TResult>;
    Task<DialogResult> ShowAsync<TDialog>() where TDialog : class, IDialogViewModel;
    void Hide();
}