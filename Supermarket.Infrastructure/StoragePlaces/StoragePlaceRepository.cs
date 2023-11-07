using Dapper;
using Oracle.ManagedDataAccess.Client;
using Supermarket.Domain.Common.Paging;
using Supermarket.Domain.StoragePlaces;

namespace Supermarket.Infrastructure.StoragePlaces;

internal class StoragePlaceRepository : CrudRepositoryBase<StoragePlace, int, DbStoragePlace>, IStoragePlaceRepository
{
    public StoragePlaceRepository(OracleConnection oracleConnection) : base(oracleConnection)
    {

    }

    public async Task<PagedResult<StoragePlace>> GetSupermarketStoragePlaces(int supermarketId, RecordsRange recordsRange)
    {
        var parameters = new DynamicParameters().AddParameter("supermarket_id", supermarketId);
        const string sql = @"SELECT mu.*
                            FROM MISTA_ULOZENI mu
                            JOIN SUPERMARKETY s ON mu.supermarket_id = s.supermarket_id
                            WHERE s.supermarket_id = :supermarket_id;";
        var orderByColumns = DbStoragePlace.IdentityColumns.Select(ic => $"mu.{ic}");
        var result = await GetPagedResult<DbStoragePlace>(recordsRange, sql, orderByColumns, parameters);
        return result.Select(dbStoragePlace => dbStoragePlace.ToDomainEntity());

    }
}