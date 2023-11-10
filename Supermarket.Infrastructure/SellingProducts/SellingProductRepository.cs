using Dapper;
using Oracle.ManagedDataAccess.Client;
using Supermarket.Core.Domain.Common.Paging;
using Supermarket.Core.Domain.ProductCategories;
using Supermarket.Core.Domain.Products;
using Supermarket.Core.Domain.SellingProducts;
using Supermarket.Infrastructure.ProductCategories;
using Supermarket.Infrastructure.Products;

namespace Supermarket.Infrastructure.SellingProducts;

internal class SellingProductRepository : CrudRepositoryBase<SellingProduct, SellingProductId, DbSellingProduct>, ISellingProductRepository
{
    public SellingProductRepository(OracleConnection oracleConnection) : base(oracleConnection)
    {
    }

    public async Task<PagedResult<Product>> GetSupermarketProducts(int supermarketId, RecordsRange recordsRange, int categoryId, string? searchText)
    {
        var parameters = new DynamicParameters()
            .AddParameter("supermarket_id", supermarketId)
            .AddParameter("druh_zbozi_id", categoryId)
            .AddParameter("hledani", searchText);
        
        const string sql = @"SELECT z.* FROM ZBOZI z
                     JOIN PRODAVANE_ZBOZI pz ON (z.zbozi_id = pz.zbozi_id)
                     WHERE pz.supermarket_id = :supermarket_id AND z.druh_zbozi_id = :druh_zbozi_id AND z.nazev LIKE '%' || :hledani || '%'";

        var orderByColumns = DbProduct.IdentityColumns
            .Select(ic => $"z.{ic}");
        
        var result = await GetPagedResult<DbProduct>(recordsRange, sql, orderByColumns, parameters);

        return result.Select(dbProduct => dbProduct.ToDomainEntity());
    }

    public async Task<PagedResult<ProductCategory>> GetSupermarketProductCategories(int supermarketId, RecordsRange recordsRange)
    {
        var parameters = new DynamicParameters()
            .AddParameter("supermarket_id", supermarketId);
        
        const string sql = @"SELECT DISTINCT dz.*
                            FROM DRUHY_ZBOZI dz
                            JOIN ZBOZI z ON dz.DRUH_ZBOZI_ID = z.DRUH_ZBOZI_ID
                            JOIN PRODAVANE_ZBOZI pz ON z.ZBOZI_ID = pz.ZBOZI_ID
                            WHERE pz.SUPERMARKET_ID = :supermarket_id";

        var orderByColumns = DbProductCategory.IdentityColumns
            .Select(ic => $"dz.{ic}");
        
        var result = await GetPagedResult<DbProductCategory>(recordsRange, sql, orderByColumns, parameters);

        return result.Select(dbProductCategory => dbProductCategory.ToDomainEntity());
    }
}