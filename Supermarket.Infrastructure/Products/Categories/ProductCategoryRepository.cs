using Oracle.ManagedDataAccess.Client;
using Supermarket.Domain.Common.Paging;
using Supermarket.Domain.Products.Categories;
using Supermarket.Infrastructure.Common;

namespace Supermarket.Infrastructure.Products.Categories
{
    internal class ProductCategoryRepository : CrudRepositoryBase<ProductCategory, int>, IProductCategoryRepository
    {
        public ProductCategoryRepository(OracleConnection oracleConnection) : base(oracleConnection)
        {
        }

        public Task<PagedResult<ProductCategory>> GetPagedAsync(PagingQueryObject queryObject) => GetRecordsRangeAsync(queryObject.RecordsRange);
    }
}
