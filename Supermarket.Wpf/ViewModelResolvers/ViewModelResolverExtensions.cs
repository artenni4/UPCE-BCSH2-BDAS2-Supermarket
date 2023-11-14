namespace Supermarket.Wpf.ViewModelResolvers;

public static class ViewModelResolverExtensions
{
    public static async Task<TViewModel> Resolve<TViewModel>(this IViewModelResolver viewModelResolver)
        where TViewModel : class, IViewModel
    {
        return await viewModelResolver.Resolve(typeof(TViewModel)) as TViewModel ??
               throw new InvalidOperationException();
    }
}