namespace Supermarket.Wpf.Navigation;

public class NavigationServiceFake : INavigationService
{
    public ApplicationView? CurrentView { get; }
    public event EventHandler<NavigationEventArgs>? NavigationSucceeded;
    public Task NavigateToAsync(ApplicationView applicationView)
    {
        throw new NotImplementedException();
    }

    public Task BackAsync()
    {
        throw new NotImplementedException();
    }
}