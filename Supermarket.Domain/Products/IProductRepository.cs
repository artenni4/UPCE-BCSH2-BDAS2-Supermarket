using Supermarket.Domain.Common;

namespace Supermarket.Domain.Products
{
    public interface IProductRepository : ICrudRepository<Product, int>
    {
    }
}
