using Supermarket.Core.Domain.Common;
using Supermarket.Core.Domain.Common.Paging;
using Supermarket.Core.UseCases.ManagerMenu;

namespace Supermarket.Core.Domain.Sales
{
    public interface ISaleRepository : ICrudRepository<Sale, int>
    {
        Task<PagedResult<ManagerMenuSale>> GetSupermarketSales(int supermarketId, DateTime dateFrom, DateTime dateTo, RecordsRange recordsRange);
        Task<int> AddAndGetIdAsync(Sale sale);
    }
}
