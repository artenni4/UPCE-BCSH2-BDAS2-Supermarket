namespace Supermarket.Wpf.Common;

public static class AsyncActivatedExtensions
{
    public static async Task ActivateIfNeeded(this IViewModel viewModel)
    {
        if (viewModel is IAsyncActivated asyncActivated)
        {
            await asyncActivated.ActivateAsync();
        }
    }
}