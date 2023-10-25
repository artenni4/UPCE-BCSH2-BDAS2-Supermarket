using Supermarket.Domain.Common;
using Supermarket.Domain.Common.Paging;

namespace Supermarket.Domain.Products
{
    public interface IProductRepository : ICrudRepository<Product, int, PagingQueryObject>
    {
    }
}
