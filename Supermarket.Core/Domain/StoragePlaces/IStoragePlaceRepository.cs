using Supermarket.Core.Domain.Common;
using Supermarket.Core.Domain.Common.Paging;
using Supermarket.Core.UseCases.GoodsKeeping;

namespace Supermarket.Core.Domain.StoragePlaces
{
    public interface IStoragePlaceRepository : ICrudRepository<StoragePlace, int>
    {
        Task<PagedResult<StoragePlace>> GetSupermarketStoragePlaces(int supermarketId, RecordsRange recordsRange);
        Task MoveProduct(int storagePlaceId, MovingProduct movingProduct);
        Task SupplyProductsToWarehouse(int warehouseId, int productId, int supermarketId, decimal count);
        Task MoveProductAndDelete(int id, int newPlaceId);
    }
}
