using System;
using System.Threading.Tasks;
using Supermarket.Wpf.Common;

namespace Supermarket.Wpf.Navigation;

public class NavigationServiceFake : INavigationService
{
    public ApplicationView? CurrentView { get; }
    public event EventHandler<NavigationEventArgs>? NavigationSucceeded;
    public Task NavigateToAsync(ApplicationView applicationView)
    {
        throw new NotImplementedException();
    }
}