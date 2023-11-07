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
    
    private void CheckShowingDialogAlready()
    {
        lock (IsShowingLock)
        {
            if (_isShowing)
            {
                throw new InvalidOperationException("Other dialog is already displayed");
            }

            _isShowing = true;
        }
    }

    public async Task<TResult> ShowAsync<TDialog, TResult, TParameters>(TParameters parameters) where TDialog : class, IDialogViewModel<TResult, TParameters>
    {
        CheckShowingDialogAlready();
        var viewModel = await _viewModelResolver.Resolve<TDialog>();
        viewModel.SetParameters(parameters);
        CurrentDialog = viewModel;
        DialogShown?.Invoke(this, new DialogViewModelEventArgs { ViewModel = viewModel });

        var taskCompletionSource = new TaskCompletionSource<TResult>();
        viewModel.ResultReceived += (_, result) => taskCompletionSource.SetResult(result);
        return await taskCompletionSource.Task;
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