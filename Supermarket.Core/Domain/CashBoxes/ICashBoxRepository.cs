using Supermarket.Core.Domain.Common;
using Supermarket.Core.Domain.Common.Paging;
using Supermarket.Core.UseCases.ManagerMenu;

namespace Supermarket.Core.Domain.CashBoxes;

public interface ICashBoxRepository : ICrudRepository<CashBox, int>
{
    Task<PagedResult<CashBox>> GetBySupermarketId(int supermarketId, RecordsRange recordsRange);
    Task<PagedResult<ManagerMenuCashbox>> GetSupermarketCashboxes(int supermarketId, RecordsRange recordsRange);
    Task<ManagerMenuCashbox?> GetCashboxToEdit(int cashboxId);
}