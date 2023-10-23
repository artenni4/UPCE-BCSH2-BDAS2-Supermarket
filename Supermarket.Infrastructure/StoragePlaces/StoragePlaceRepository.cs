using Oracle.ManagedDataAccess.Client;
using Supermarket.Domain.Common.Paging;
using Supermarket.Domain.StoragePlaces;
using Supermarket.Infrastructure.Common;
using StoragePlace = Supermarket.Domain.StoragePlaces.StoragePlace;

namespace Supermarket.Infrastructure.StoragePlaces
{
    internal class StoragePlaceRepository : CrudRepositoryBase<StoragePlace, int>, IStoragePlaceRepository
    {
        public StoragePlaceRepository(OracleConnection oracleConnection) : base(oracleConnection)
        {
        }

        public Task<PagedResult<StoragePlace>> GetPagedAsync(PagingQueryObject queryObject) => GetRecordsRangeAsync(queryObject.RecordsRange);
    }
}
