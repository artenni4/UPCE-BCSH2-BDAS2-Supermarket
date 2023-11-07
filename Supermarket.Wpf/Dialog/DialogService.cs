using System;
using System.Threading.Tasks;
using Supermarket.Wpf.Common;
using Supermarket.Wpf.ViewModelResolvers;

namespace Supermarket.Wpf.Dialog;

public class DialogService : IDialogService
{
    private readonly IViewModelResolver _viewModelResolver;

    private static readonly object IsShowingLock = new();
    private bool _isShowing;
    
    public DialogService(IViewModelResolver viewModelResolver)
    {
        _viewModelResolver = viewModelResolver;
    }
    
    public event EventHandler<DialogViewModelEventArgs>? DialogShown;
    public event EventHandler? DialogHidden;
    public IViewModel? CurrentDialog { get; private set; }
    public async Task<TResult> TryShowAsync<TDialog, TResult>() where TDialog : class, IDialogViewModel<TResult>
    {
        CheckShowingDialogAlready();
        var viewModel = await ShowDialog<TDialog>();

        var taskCompletionSource = new TaskCompletionSource<TResult>();
        viewModel.ResultReceived += (_, result) => taskCompletionSource.SetResult(result);
        return await taskCompletionSource.Task;
    }

    public async Task TryShowAsync<TDialog>() where TDialog : class, IDialogViewModel
    {
        CheckShowingDialogAlready();
        var viewModel = await ShowDialog<TDialog>();
        
        var taskCompletionSource = new TaskCompletionSource();
        viewModel.ResultReceived += (_, _) => taskCompletionSource.SetResult();
        await taskCompletionSource.Task;
    }

    private void CheckShowingDialogAlready()
    {
        lock (IsShowingLock)
        {
            if (_isShowing)
            {
                throw new InvalidOperationException($"Other dialog is already displayed");
            }

            _isShowing = true;
        }
    }

    private async Task<TDialog> ShowDialog<TDialog>() where TDialog : class, IViewModel
    {
        var viewModel = await _viewModelResolver.Resolve<TDialog>();
        CurrentDialog = viewModel;
        DialogShown?.Invoke(this, new DialogViewModelEventArgs { ViewModel = viewModel });

        return viewModel;
    }
    
    public void Hide()
    {
        CurrentDialog = null;
        DialogHidden?.Invoke(this, EventArgs.Empty);
        lock (IsShowingLock)
        {
            _isShowing = false;
        }
    }
}