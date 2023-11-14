namespace Supermarket.Wpf.Dialog;

public static class DialogServiceExtensions
{
    public static async Task<DialogResult> ShowAsync<TDialog, TParameters>(this IDialogService service, TParameters parameters)
        where TDialog : class, IDialogViewModel<EmptyResult, TParameters>
    {
        var result = await service.ShowAsync<TDialog, EmptyResult, TParameters>(parameters);
        if (result.IsOk(out _))
        {
            return DialogResult.Ok();
        }

        if (result.IsCancelled())
        {
            return DialogResult.Cancel();
        }

        throw new NotSupportedException($"{result} has result type that is not supported. Extend this method");
    }
}