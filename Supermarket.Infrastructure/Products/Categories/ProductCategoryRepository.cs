using Supermarket.Core.Common.Paging;
using Supermarket.Core.Products.Categories;
using Supermarket.Infrastructure.Common;

namespace Supermarket.Infrastructure.Products.Categories
{
    public class ProductCategoryRepository : CrudRepositoryBase<ProductCategory, int>, IProductCategoryRepository
    {
        public Task<PagedResult<ProductCategory>> GetPagedAsync(PagingQueryObject queryObject) => GetRecordsRangeAsync(queryObject.RecordsRange);
    }
}
