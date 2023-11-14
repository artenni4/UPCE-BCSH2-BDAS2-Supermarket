using Supermarket.Wpf.ViewModelResolvers;

namespace Supermarket.Wpf.Dialog;

public class DialogService : IDialogService
{
    private readonly IViewModelResolver _viewModelResolver;

    private static readonly object IsShowingLock = new();
    private bool _isShowing;
    private Action? _tryCancelDialog;
    
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

    public async Task<DialogResult<TResult>> ShowAsync<TDialog, TResult, TParameters>(TParameters parameters) where TDialog : class, IDialogViewModel<TResult, TParameters>
    {
        CheckShowingDialogAlready();
        var viewModel = await _viewModelResolver.Resolve<TDialog>();
        viewModel.SetParameters(parameters);
        ShowDialog(viewModel);

        var taskCompletionSource = new TaskCompletionSource<DialogResult<TResult>>();
        viewModel.ResultReceived += (_, result) =>
        {
            taskCompletionSource.SetResult(result);
            Hide();
        };
        _tryCancelDialog = () =>
        {
            taskCompletionSource.TrySetResult(DialogResult<TResult>.Cancel());
        };
        return await taskCompletionSource.Task;
    }

    public async Task<DialogResult<TResult>> ShowAsync<TDialog, TResult>() where TDialog : class, IDialogViewModel<TResult>
    {
        CheckShowingDialogAlready();
        var viewModel = await _viewModelResolver.Resolve<TDialog>();
        ShowDialog(viewModel);

        var taskCompletionSource = new TaskCompletionSource<DialogResult<TResult>>();
        viewModel.ResultReceived += (_, result) =>
        {
            taskCompletionSource.SetResult(result);
            Hide();
        };
        _tryCancelDialog = () =>
        {
            taskCompletionSource.TrySetResult(DialogResult<TResult>.Cancel());
        };
        return await taskCompletionSource.Task;
    }

    public async Task<DialogResult> ShowAsync<TDialog>() where TDialog : class, IDialogViewModel
    {
        CheckShowingDialogAlready();
        var viewModel = await _viewModelResolver.Resolve<TDialog>();
        ShowDialog(viewModel);

        var taskCompletionSource = new TaskCompletionSource<DialogResult>();
        viewModel.ResultReceived += (_, result) =>
        {
            taskCompletionSource.SetResult(result);
            Hide();
        };
        _tryCancelDialog = () =>
        {
            taskCompletionSource.TrySetResult(DialogResult.Cancel());
        };
        return await taskCompletionSource.Task;
    }

    public void Hide()
    {
        _tryCancelDialog?.Invoke();
        CurrentDialog = null;
        DialogHidden?.Invoke(this, EventArgs.Empty);
        lock (IsShowingLock)
        {
            _isShowing = false;
        }
    }

    private void ShowDialog(IViewModel viewModel)
    {
        CurrentDialog = viewModel;
        DialogShown?.Invoke(this, new DialogViewModelEventArgs { ViewModel = viewModel });
    }
}