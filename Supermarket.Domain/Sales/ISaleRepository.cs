using Supermarket.Domain.Common.Paging;
using Supermarket.Domain.Common;

namespace Supermarket.Domain.Sales
{
    public interface ISaleRepository : ICrudRepository<Sale, int, PagingQueryObject>
    {
    }
}
