using Dapper;
using Oracle.ManagedDataAccess.Client;
using Supermarket.Domain.Common.Paging;
using Supermarket.Domain.ProductCategories;
using Supermarket.Domain.Products;
using Supermarket.Domain.SellingProducts;
using Supermarket.Infrastructure.Common;
using Supermarket.Infrastructure.Products;

namespace Supermarket.Infrastructure.SellingProducts;

internal class SellingProductRepository : CrudRepositoryBase<SellingProduct, SellingProductId, DbSellingProduct>, ISellingProductRepository
{
    public SellingProductRepository(OracleConnection oracleConnection) : base(oracleConnection)
    {
    }

    protected override string TableName => "PRODAVANE_ZBOZI";

    protected override IReadOnlyList<string> IdentityColumns { get; } = new[]
    {
        nameof(DbSellingProduct.zbozi_id),
        nameof(DbSellingProduct.supermarket_id)
    };
        
    protected override SellingProduct MapToEntity(DbSellingProduct dbEntity)
    {
        throw new NotImplementedException();
    }

    protected override DbSellingProduct MapToDbEntity(SellingProduct entity)
    {
        throw new NotImplementedException();
    }

    protected override DynamicParameters GetIdentityValues(SellingProductId id)
    {
        throw new NotImplementedException();
    }

    protected override SellingProductId ExtractIdentity(DynamicParameters dynamicParameters)
    {
        throw new NotImplementedException();
    }

    public async Task<PagedResult<Product>> GetSupermarketProducts(int supermarketId, RecordsRange recordsRange, int categoryId, string? searchText)
    {
        var parameters = new DynamicParameters();
        parameters.Add("supermarket_id", supermarketId);

        var result = await GetPagedResult<DbProduct>(recordsRange,
            selectColumns: "ZBOZI.*",
            otherClauses: @"JOIN ZBOZI USING zbozi_id
                            WHERE PRODAVANE_ZBOZI.supermarket_id = :supermarket_id",
            parameters: parameters);

        return result.Select(MapToEntity);
    }

    public Task<PagedResult<ProductCategory>> GetSupermarketProductCategories(int supermarketId, RecordsRange recordsRange)
    {
        throw new NotImplementedException();
    }
}