using Supermarket.Core.Domain.Common;
using Supermarket.Core.Domain.Common.Paging;

namespace Supermarket.Core.Domain.StoragePlaces
{
    public interface IStoragePlaceRepository : ICrudRepository<StoragePlace, int>
    {
        Task<PagedResult<StoragePlace>> GetSupermarketStoragePlaces(int supermarketId, RecordsRange recordsRange);
    }
}
