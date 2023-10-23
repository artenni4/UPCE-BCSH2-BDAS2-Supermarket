using Supermarket.Domain.Common.Paging;
using Supermarket.Domain.Common;

namespace Supermarket.Domain.StoredProducts
{
    public interface IStoredProductRepository : ICrudRepository<StoredProduct, int, PagingQueryObject>
    {
    }
}
