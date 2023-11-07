using System;
using Supermarket.Wpf.Common;

namespace Supermarket.Wpf.Navigation;

public class NavigationServiceFake : INavigationService
{
    public ApplicationView? CurrentView { get; }
    public event EventHandler<NavigationEventArgs>? NavigationSucceeded;
    public void NavigateTo(ApplicationView applicationView)
    {
        
    }
}