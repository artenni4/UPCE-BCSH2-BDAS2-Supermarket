using Supermarket.Domain.Common;
using Supermarket.Domain.Common.Paging;

namespace Supermarket.Domain.CashBoxes;

public interface ICashBoxRepository : ICrudRepository<CashBox, int, PagingQueryObject>
{
    
}