using Supermarket.Domain.Common;
using Supermarket.Domain.Common.Paging;

namespace Supermarket.Domain.StoredProducts
{
    public interface IStoredProductRepository : ICrudRepository<StoredProduct, StoredProductId>
    {
        Task<PagedResult<GoodsKeepingStoredProduct>> GetSupermarketStoredProducts(int supermarketId, RecordsRange recordsRange);
    }
}
