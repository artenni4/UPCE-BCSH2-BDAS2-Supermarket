using Supermarket.Domain.Common;
using Supermarket.Domain.Common.Paging;

namespace Supermarket.Domain.Products.Categories
{
    public interface IProductCategoryRepository : ICrudRepository<ProductCategory, int, PagingQueryObject>
    {
    }
}
