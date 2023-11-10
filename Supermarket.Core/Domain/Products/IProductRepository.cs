using Supermarket.Core.Domain.Common;

namespace Supermarket.Core.Domain.Products
{
    public interface IProductRepository : ICrudRepository<Product, int>
    {
    }
}
