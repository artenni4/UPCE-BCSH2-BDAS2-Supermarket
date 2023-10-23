using Supermarket.Domain.Common.Paging;
using Supermarket.Domain.Common;

namespace Supermarket.Domain.SoldProducts
{
    public interface ISoldProductRepository : ICrudRepository<SoldProduct, int, PagingQueryObject>
    {
    }
}
