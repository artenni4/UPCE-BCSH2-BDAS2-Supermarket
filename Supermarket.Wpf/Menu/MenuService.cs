using Supermarket.Core.UseCases.ApplicationMenu;
using Supermarket.Core.UseCases.Login;
using Supermarket.Wpf.Dialog;
using Supermarket.Wpf.LoggedUser;
using Supermarket.Wpf.Navigation;

namespace Supermarket.Wpf.Menu;

public class MenuService : IMenuService
{
    private readonly ILoggedUserService _loggedUserService;
    private readonly IDialogService _dialogService;
    private readonly INavigationService _navigationService;

    public MenuService(ILoggedUserService loggedUserService, IDialogService dialogService, INavigationService navigationService)
    {
        _loggedUserService = loggedUserService;
        _dialogService = dialogService;
        _navigationService = navigationService;
    }

    public async Task<bool> TryShowMenuAsync()
    {
        if (!_loggedUserService.IsUserSet || _loggedUserService.IsCustomer)
        {
            return false;
        }
        
        var result = await _dialogService.ShowAsync<MenuViewModel, MenuResult>();
        if (!result.IsOk(out var menuResult))
        {
            return false;
        }
        
        if (menuResult.IsNavigate(out var applicationView))
        {
            await _navigationService.NavigateToAsync(applicationView);
        }
        else if (menuResult.IsLogOut())
        {
            await _navigationService.NavigateToAsync(ApplicationView.Login);
            _loggedUserService.UnsetUser();
        }

        return true;
    }
}