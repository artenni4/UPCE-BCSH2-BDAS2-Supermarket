using Supermarket.Core.UseCases.ApplicationMenu;

namespace Supermarket.Wpf.Menu;

public interface IMenuService
{
    Task<bool> TryShowMenuAsync();
}