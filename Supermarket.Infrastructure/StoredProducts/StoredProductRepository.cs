using Dapper;
using Oracle.ManagedDataAccess.Client;
using Supermarket.Domain.Common.Paging;
using Supermarket.Domain.StoredProducts;
using Supermarket.Domain.Supermarkets;

namespace Supermarket.Infrastructure.StoredProducts;

internal class StoredProductRepository : CrudRepositoryBase<StoredProduct, StoredProductId, DbStoredProduct>, IStoredProductRepository
{
    public StoredProductRepository(OracleConnection oracleConnection) : base(oracleConnection)
    {
    }

    public async Task<PagedResult<GoodsKeepingStoredProduct>> GetSupermarketStoredProducts(int supermarketId, RecordsRange recordsRange)
    {
        var parameters = new DynamicParameters()
            .AddParameter("supermarket_id", supermarketId);

        const string sql = @"SELECT mu.misto_ulozeni_id as misto_ulozeni_id, mu.kod as kod, uz.kusy as kusy, uz.zbozi_id as zbozi_id, z.nazev as nazev
                            FROM MISTA_ULOZENI mu
                            JOIN ULOZENI_ZBOZI uz ON mu.misto_ulozeni_id = uz.misto_ulozeni_id
                            JOIN ZBOZI z ON z.zbozi_id = uz.zbozi_id
                            WHERE mu.supermarket_id = :supermarket_id";

        var orderByColumns = DbStoredProduct.IdentityColumns
            .Select(ic => $"uz.{ic}");

        var result = await GetPagedResult<DbGoodsKeepingStoredProduct>(recordsRange, sql, orderByColumns, parameters);

        return result.Select(dbProduct => dbProduct.ToDomainEntity());
    }

    public async Task DeleteProductFormStorage(int storagePlaceId, int productId, decimal count)
    {
        var parameters = new DynamicParameters()
            .AddParameter("misto_ulozeni_id", storagePlaceId).AddParameter("zbozi_id", productId).AddParameter("kusy", count);

        const string sqlUpdate = @"UPDATE ULOZENI_ZBOZI
                            SET kusy = kusy - :kusy
                            WHERE misto_ulozeni_id = :misto_ulozeni_id AND zbozi_id = :zbozi_id";

        const string sqlDelete = @"DELETE FROM ULOZENI_ZBOZI
                            WHERE misto_ulozeni_id = :misto_ulozeni_id AND zbozi_id = :zbozi_id AND kusy <= 0";

        await _oracleConnection.ExecuteAsync(sqlUpdate, parameters);
        await _oracleConnection.ExecuteAsync(sqlDelete, parameters);
    }

    private class DbGoodsKeepingStoredProduct
    {
        public required int zbozi_id { get; set; }
        public required string nazev { get; set; }
        public required decimal kusy { get; set; }
        public required string kod { get; set; }
        public required int misto_ulozeni_id { get; set; }

        public GoodsKeepingStoredProduct ToDomainEntity() => new GoodsKeepingStoredProduct
        {
            ProductId = zbozi_id,
            ProductName = nazev,
            Count = kusy,
            StoragePlaceCode = kod,
            StoragePlaceId = misto_ulozeni_id
        };
    }
}