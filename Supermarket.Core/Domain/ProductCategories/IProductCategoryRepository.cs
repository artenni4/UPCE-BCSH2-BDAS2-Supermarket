using Supermarket.Core.Domain.Common;

namespace Supermarket.Core.Domain.ProductCategories
{
    public interface IProductCategoryRepository : ICrudRepository<ProductCategory, int>
    {
    }
}
