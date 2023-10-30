using Oracle.ManagedDataAccess.Client;
using Supermarket.Domain.StoragePlaces;

namespace Supermarket.Infrastructure.StoragePlaces;

internal class StoragePlaceRepository : CrudRepositoryBase<StoragePlace, int, DbStoragePlace>, IStoragePlaceRepository
{
    public StoragePlaceRepository(OracleConnection oracleConnection) : base(oracleConnection)
    {
    }
}