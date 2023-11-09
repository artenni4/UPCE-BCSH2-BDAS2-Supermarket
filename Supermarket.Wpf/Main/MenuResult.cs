using Supermarket.Wpf.Navigation;

namespace Supermarket.Wpf.Main;

public class MenuResult
{
    private readonly ApplicationView? _applicationView;

    private MenuResult(ApplicationView? applicationView)
    {
        _applicationView = applicationView;
    }

    public static MenuResult Navigate(ApplicationView applicationView) => new(applicationView);
    public static MenuResult LogOut() => new(applicationView: null);

    public bool IsNavigate(out ApplicationView applicationView)
    {
        if (_applicationView.HasValue)
        {
            applicationView = _applicationView.Value;
            return true;
        }

        applicationView = default;
        return false;
    }

    public bool IsLogOut() => _applicationView.HasValue == false;
}