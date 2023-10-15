using Supermarket.Domain.Common;
using Supermarket.Domain.Common.Paging;

namespace Supermarket.Domain.Supermarkets;

public interface ISupermarketRepository : ICrudRepository<Supermarket, int, PagingQueryObject>
{
    
}