﻿using Dapper;
using Oracle.ManagedDataAccess.Client;
using Supermarket.Domain.Common.Paging;
using Supermarket.Domain.ProductCategories;
using Supermarket.Domain.Products;
using Supermarket.Domain.SellingProducts;
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
            .AddParameter("supermarket_id", supermarketId);
        
        const string sql = @"SELECT z.* FROM ZBOZI z
                     JOIN PRODAVANE_ZBOZI pz ON (z.zbozi_id = pz.zbozi_id)
                     WHERE pz.supermarket_id = :supermarket_id";

        var orderByColumns = DbProduct.IdentityColumns
            .Select(ic => $"z.{ic}");
        
        var result = await GetPagedResult<DbProduct>(recordsRange, sql, orderByColumns, parameters);

        return result.Select(dbProduct => dbProduct.ToDomainEntity());
    }

    public async Task<PagedResult<ProductCategory>> GetSupermarketProductCategories(int supermarketId, RecordsRange recordsRange)
    {
        var parameters = new DynamicParameters()
            .AddParameter("supermarket_id", supermarketId);
        
        const string sql = @"SELECT dz.* FROM DRUHY_ZBOZI dz
                     JOIN PRODAVANE_ZBOZI pz USING (zbozi_id) 
                     WHERE pz.supermarket_id = :supermarket_id";

        var orderByColumns = DbProductCategory.IdentityColumns
            .Select(ic => $"dz.{ic}");
        
        var result = await GetPagedResult<DbProductCategory>(recordsRange, sql, orderByColumns, parameters);

        return result.Select(dbProductCategory => dbProductCategory.ToDomainEntity());
    }
}