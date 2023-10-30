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
}