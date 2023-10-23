using Supermarket.Domain.Common.Paging;
using Supermarket.Domain.Common;

namespace Supermarket.Domain.StoragePlaces
{
    public interface IStoragePlaceRepository : ICrudRepository<StoragePlace, int, PagingQueryObject>
    {
    }
}
