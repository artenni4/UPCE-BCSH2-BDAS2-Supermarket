using System.Threading.Tasks;

namespace Supermarket.Wpf.Dialog;

public static class DialogServiceExtensions
{
    public static async Task<TResult> ShowForResultAsync<TDialog, TResult, TParameters>(this IDialogService dialogService,
        TParameters parameters)
        where TDialog : class, IDialogViewModel<TResult, TParameters>
    {
        var result = await dialogService.ShowAsync<TDialog, TResult, TParameters>(parameters);
        dialogService.Hide();
        return result;
    }
}