using Supermarket.Core.Domain.Common.Paging;
using Supermarket.Core.Domain.Supermarkets;

namespace Supermarket.Core.UseCases.ApplicationMenu;

public class ApplicationMenuService : IApplicationMenuService
{
    private readonly ISupermarketRepository _supermarketRepository;

    public ApplicationMenuService(ISupermarketRepository supermarketRepository)
    {
        _supermarketRepository = supermarketRepository;
    }

    public async Task<PagedResult<AdminSupermarket>> GetSupermarketsForAdminAsync(RecordsRange recordsRange)
    {
        var supermarkets = await _supermarketRepository.GetPagedAsync(recordsRange);
        return supermarkets.Select(s => new AdminSupermarket
        {
            Id = s.Id,
            Address = s.Address
        });
    }
}