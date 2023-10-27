using Dapper;
using Oracle.ManagedDataAccess.Client;
using Supermarket.Domain.ProductCategories;
using Supermarket.Infrastructure.Common;

namespace Supermarket.Infrastructure.ProductCategories;

internal class ProductCategoryRepository : CrudRepositoryBase<ProductCategory, int, DbProductCategory>, IProductCategoryRepository
{
    public ProductCategoryRepository(OracleConnection oracleConnection) : base(oracleConnection)
    {
    }

    protected override string TableName => "DRUHY_ZBOZI";

    protected override IReadOnlyList<string> IdentityColumns { get; } =
        new[] { nameof(DbProductCategory.druh_zbozi_id) };
        
    protected override ProductCategory MapToEntity(DbProductCategory dbEntity)
    {
        throw new NotImplementedException();
    }

    protected override DbProductCategory MapToDbEntity(ProductCategory entity)
    {
        throw new NotImplementedException();
    }

    protected override DynamicParameters GetIdentityValues(int id)
    {
        throw new NotImplementedException();
    }

    protected override int ExtractIdentity(DynamicParameters dynamicParameters)
    {
        throw new NotImplementedException();
    }
}