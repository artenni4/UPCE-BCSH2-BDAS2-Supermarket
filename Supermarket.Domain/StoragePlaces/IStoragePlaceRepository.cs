using Supermarket.Domain.Common;
using Supermarket.Domain.Common.Paging;

namespace Supermarket.Domain.StoragePlaces
{
    public interface IStoragePlaceRepository : ICrudRepository<StoragePlace, int>
    {
        Task<PagedResult<StoragePlace>> GetSupermarketStoragePlaces(int supermarketId, RecordsRange recordsRange);
    }
}
