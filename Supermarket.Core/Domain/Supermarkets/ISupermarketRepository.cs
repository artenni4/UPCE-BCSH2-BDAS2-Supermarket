using Supermarket.Core.Domain.Common;
using Supermarket.Core.Domain.Common.Paging;
using Supermarket.Core.Domain.StoragePlaces;
using Supermarket.Core.UseCases.Admin;

namespace Supermarket.Core.Domain.Supermarkets;

public interface ISupermarketRepository : ICrudRepository<Supermarket, int>
{
    Task<PagedResult<StoragePlace>> GetSupermarketWarehouses(int supermarketId, RecordsRange recordsRange);
    Task<PagedResult<AdminMenuSupermarket>> GetAdminSupermarkets(RecordsRange recordsRange);
}