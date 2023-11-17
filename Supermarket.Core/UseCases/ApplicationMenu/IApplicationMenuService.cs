using Supermarket.Core.Domain.Common.Paging;

namespace Supermarket.Core.UseCases.ApplicationMenu;

public interface IApplicationMenuService
{
    Task<PagedResult<AdminSupermarket>> GetSupermarketsForAdminAsync(RecordsRange recordsRange);
}