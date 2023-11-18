using Dapper;
using Oracle.ManagedDataAccess.Client;
using Supermarket.Core.Domain.Common.Paging;
using Supermarket.Core.Domain.StoragePlaces;
using Supermarket.Core.Domain.Supermarkets;
using Supermarket.Core.UseCases.GoodsKeeping;
using Supermarket.Infrastructure.ProductCategories;

namespace Supermarket.Infrastructure.StoragePlaces;

internal class StoragePlaceRepository : CrudRepositoryBase<StoragePlace, int, DbStoragePlace>, IStoragePlaceRepository
{
    public StoragePlaceRepository(OracleConnection oracleConnection) : base(oracleConnection)
    {

    }

    public async Task MoveProduct(int storagePlaceId, MovingProduct movingProduct)
    {
        var parameters = new DynamicParameters().AddParameter("old_sklad_id", storagePlaceId).AddParameter("new_sklad_id", movingProduct.NewStoragePlaceId).AddParameter("var_zbozi_id", movingProduct.ProductId).AddParameter("var_kusy", movingProduct.Count);
        //const string sql = @"BEGIN
        //                        premistit_zbozi(
        //                            old_sklad_id => :misto_ulozeni_id,
        //                            new_sklad_id => :nove_misto_ulozeni_id,
        //                            var_zbozi_id => :zbozi_id,
        //                            var_kusy => :kusy);
        //                    END";

        await _oracleConnection.ExecuteAsync("premistit_zbozi", parameters, commandType:System.Data.CommandType.StoredProcedure);
    }

    public async Task<PagedResult<StoragePlace>> GetSupermarketStoragePlaces(int supermarketId, RecordsRange recordsRange)
    {
        var parameters = new DynamicParameters().AddParameter("supermarket_id", supermarketId);
        const string sql = @"SELECT mu.*
                            FROM MISTA_ULOZENI mu
                            JOIN SUPERMARKETY s ON mu.supermarket_id = s.supermarket_id
                            WHERE s.supermarket_id = :supermarket_id";
        var orderByColumns = DbStoragePlace.IdentityColumns.Select(ic => $"mu.{ic}");
        var result = await GetPagedResult<DbStoragePlace>(recordsRange, sql, orderByColumns, parameters);

        return result.Select(dbStoragePlace => dbStoragePlace.ToDomainEntity());
    }
}