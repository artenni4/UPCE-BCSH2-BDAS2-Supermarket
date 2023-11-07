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
    public async Task<TDialog?> TryShowAsync<TDialog, TResult>() where TDialog : class, IDialogViewModel<TResult>
    {
        lock (IsShowingLock)
        {
            if (_isShowing)
            {
                return null;
            }

            _isShowing = true;
        }
        
        var viewModel = await _viewModelResolver.Resolve<TDialog>();
        CurrentDialog = viewModel;
        DialogShown?.Invoke(this, new DialogViewModelEventArgs { ViewModel = viewModel });

        return viewModel;
    }

    public async Task<TDialog?> TryShowAsync<TDialog>() where TDialog : class, IDialogViewModel
    {
        lock (IsShowingLock)
        {
            if (_isShowing)
            {
                return null;
            }

            _isShowing = true;
        }
        
        var viewModel = await _viewModelResolver.Resolve<TDialog>();
        CurrentDialog = viewModel;
        DialogShown?.Invoke(this, new DialogViewModelEventArgs { ViewModel = viewModel });

        return viewModel;
    }

    public void Hide()
    {
        DialogHidden?.Invoke(this, EventArgs.Empty);
        lock (IsShowingLock)
        {
            _isShowing = false;
        }
    }
}