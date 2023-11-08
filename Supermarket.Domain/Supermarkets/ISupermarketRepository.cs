using Supermarket.Domain.Common;
using Supermarket.Domain.Common.Paging;
using Supermarket.Domain.StoragePlaces;

namespace Supermarket.Domain.Supermarkets;

public interface ISupermarketRepository : ICrudRepository<Supermarket, int>
{
    Task<PagedResult<StoragePlace>> GetSupermarketWarehouses(int supermarketId, RecordsRange recordsRange);
}