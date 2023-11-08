using Dapper;
using Oracle.ManagedDataAccess.Client;
using Supermarket.Core.GoodsKeeping;
using Supermarket.Domain.Common.Paging;
using Supermarket.Domain.StoragePlaces;
using Supermarket.Domain.Supermarkets;
using Supermarket.Infrastructure.StoragePlaces;

namespace Supermarket.Infrastructure.Supermarkets;

internal class SupermarketRepository : CrudRepositoryBase<Domain.Supermarkets.Supermarket, int, DbSupermarket>, ISupermarketRepository
{
    public SupermarketRepository(OracleConnection oracleConnection) : base(oracleConnection)
    {
    }

    public async Task<PagedResult<StoragePlace>> GetSupermarketWarehouses(int supermarketId, RecordsRange recordsRange)
    {
        var parameters = new DynamicParameters().AddParameter("supermarket_id", supermarketId);
        const string sql = @"SELECT mu.*
                            FROM MISTA_ULOZENI mu
                            JOIN SUPERMARKETY s ON mu.supermarket_id = s.supermarket_id
                            WHERE s.supermarket_id = :supermarket_id AND mu.misto_ulozeni_typ = 'SKLAD'";
        var orderByColumns = DbStoragePlace.IdentityColumns.Select(ic => $"mu.{ic}");
        var result = await GetPagedResult<DbStoragePlace>(recordsRange, sql, orderByColumns, parameters);

        return result.Select(dbStoragePlace => dbStoragePlace.ToDomainEntity());
    }
}