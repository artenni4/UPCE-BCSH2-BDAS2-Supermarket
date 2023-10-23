using Supermarket.Domain.Common.Paging;
using Supermarket.Domain.Common;

namespace Supermarket.Domain.SellingProducts
{
    public interface ISellingProductRepository : ICrudRepository<SellingProduct, int, PagingQueryObject>
    {
    }
}
