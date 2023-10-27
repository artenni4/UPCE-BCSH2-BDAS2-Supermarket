using Supermarket.Domain.Common;

namespace Supermarket.Domain.ProductCategories
{
    public interface IProductCategoryRepository : ICrudRepository<ProductCategory, int>
    {
    }
}
