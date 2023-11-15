using Supermarket.Wpf.ViewModelResolvers;

namespace Supermarket.Wpf.Dialog;

public class DialogService : IDialogService
{
    private readonly IViewModelResolver _viewModelResolver;
    private Action? _tryCancelDialog;
    
    public DialogService(IViewModelResolver viewModelResolver)
    {
        _viewModelResolver = viewModelResolver;
    }
    
    public event EventHandler<DialogViewModelEventArgs>? DialogShown;
    public event EventHandler? DialogHidden;

    private readonly Stack<IViewModel> _dialogStack = new();
    public IEnumerable<IViewModel> DisplayedDialogs => _dialogStack;
    
    public async Task<DialogResult<TResult>> ShowAsync<TDialog, TResult, TParameters>(TParameters parameters) where TDialog : class, IDialogViewModel<TResult, TParameters>
    {
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
        _dialogStack.Pop();
        DialogHidden?.Invoke(this, EventArgs.Empty);
    }

    private void ShowDialog(IViewModel viewModel)
    {
        _dialogStack.Push(viewModel);
        DialogShown?.Invoke(this, new DialogViewModelEventArgs { ViewModel = viewModel });
    }
}