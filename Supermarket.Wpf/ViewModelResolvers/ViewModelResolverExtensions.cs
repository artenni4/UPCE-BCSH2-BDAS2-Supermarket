using System;
using System.Threading.Tasks;

namespace Supermarket.Wpf.ViewModelResolvers;

public static class ViewModelResolverExtensions
{
    public static async Task<object> Resolve<TViewModel>(this IViewModelResolver viewModelResolver)
        where TViewModel : class => await viewModelResolver.Resolve(typeof(TViewModel));
}