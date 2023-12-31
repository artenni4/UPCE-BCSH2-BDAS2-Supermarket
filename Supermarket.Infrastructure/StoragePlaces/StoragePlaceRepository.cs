﻿using Dapper;
using Oracle.ManagedDataAccess.Client;
using Supermarket.Core.Domain.Common.Paging;
using Supermarket.Core.Domain.StoragePlaces;
using Supermarket.Core.UseCases.GoodsKeeping;

namespace Supermarket.Infrastructure.StoragePlaces;

internal class StoragePlaceRepository : CrudRepositoryBase<StoragePlace, int, DbStoragePlace>, IStoragePlaceRepository
{
    public StoragePlaceRepository(OracleConnection oracleConnection) : base(oracleConnection)
    {

    }

    public async Task MoveProduct(int storagePlaceId, MovingProduct movingProduct)
    {
        var parameters = new DynamicParameters().AddParameter("old_sklad_id", storagePlaceId).AddParameter("new_sklad_id", movingProduct.NewStoragePlaceId).AddParameter("var_zbozi_id", movingProduct.ProductId).AddParameter("var_kusy", movingProduct.Count);
        await _oracleConnection.ExecuteAsync("premistit_zbozi", parameters, commandType:System.Data.CommandType.StoredProcedure);
    }

    public async Task SupplyProductsToWarehouse(int warehouseId, int productId, int supermarketId, decimal count)
    {
        var parameters = new DynamicParameters().AddParameter("sklad_id", warehouseId).AddParameter("pr_zbozi_id", productId).AddParameter("pr_supermarket_id", supermarketId).AddParameter("pr_kusy", count);
        await _oracleConnection.ExecuteAsync("prijezd_zbozi", parameters, commandType: System.Data.CommandType.StoredProcedure);
    }

    public async Task MoveProductAndDelete(int id, int newPlaceId)
    {
        var parameters = new DynamicParameters().AddParameter("p_misto_ulozeni_id", id).AddParameter("p_new_misto_ulozeni_id", newPlaceId);
        await _oracleConnection.ExecuteAsync("move_and_delete", parameters, commandType: System.Data.CommandType.StoredProcedure);
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