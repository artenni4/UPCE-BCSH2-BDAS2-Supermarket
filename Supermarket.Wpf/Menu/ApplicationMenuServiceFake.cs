using Supermarket.Core.UseCases.ApplicationMenu;

namespace Supermarket.Wpf.Menu;

public class ApplicationMenuServiceFake : IApplicationMenuService
{
    public Task<PagedResult<AdminSupermarket>> GetSupermarketsForAdminAsync(RecordsRange recordsRange)
    {
        throw new NotImplementedException();
    }
}