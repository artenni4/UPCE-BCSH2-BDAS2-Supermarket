using Supermarket.Core.Domain.Common;
using Supermarket.Core.Domain.Common.Paging;
using Supermarket.Core.UseCases.GoodsKeeping;

namespace Supermarket.Core.Domain.StoredProducts
{
    public interface IStoredProductRepository : ICrudRepository<StoredProduct, StoredProductId>
    {
        Task<PagedResult<GoodsKeepingStoredProduct>> GetSupermarketStoredProducts(int supermarketId, RecordsRange recordsRange);
        Task DeleteProductFormStorage(int storagePlaceId, int productId, decimal count);
    }
}
