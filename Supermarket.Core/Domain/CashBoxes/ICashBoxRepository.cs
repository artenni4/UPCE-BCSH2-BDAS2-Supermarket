using Supermarket.Core.Domain.Common;
using Supermarket.Core.Domain.Common.Paging;

namespace Supermarket.Core.Domain.CashBoxes;

public interface ICashBoxRepository : ICrudRepository<CashBox, int>
{
    Task<PagedResult<CashBox>> GetBySupermarketId(int supermarketId, RecordsRange recordsRange);
}