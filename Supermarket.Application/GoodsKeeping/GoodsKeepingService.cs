using Supermarket.Core.CashBoxes;
using Supermarket.Domain.SellingProducts;
using Supermarket.Domain.StoragePlaces;
using Supermarket.Domain.StoredProducts;
using Supermarket.Domain.Supermarkets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Supermarket.Core.GoodsKeeping
{
    public class GoodsKeepingService : IGoodsKeepingService
    {
        private readonly IStoragePlaceRepository _storagePlaceRepository;
        private readonly ISellingProductRepository _sellingProductRepository;
        private readonly ISupermarketRepository _supermarketRepository;
        private readonly IStoredProductRepository _storedProductRepository;

        public GoodsKeepingService(IStoragePlaceRepository storagePlaceRepository, ISellingProductRepository sellingProductRepository, ISupermarketRepository supermarketRepository, IStoredProductRepository storedProductRepository)
        {
            _storagePlaceRepository = storagePlaceRepository;
            _sellingProductRepository = sellingProductRepository;
            _supermarketRepository = supermarketRepository;
            _storedProductRepository = storedProductRepository;
        }

        public async Task DeleteProductStorageAsync(int storagePlaceId, int productId, decimal count)
        {
            await _storedProductRepository.DeleteProductFormStorage(storagePlaceId, productId, count);
        }

        public async Task<PagedResult<GoodsKeepingProductCategory>> GetCategoriesAsync(int supermarketId, RecordsRange recordsRange)
        {
            var result = await _sellingProductRepository.GetSupermarketProductCategories(supermarketId, recordsRange);

            return result.Select(GoodsKeepingProductCategory.FromProductCategory);
        }

        public async Task<PagedResult<GoodsKeepingProduct>> GetProductsAsync(int supermarketId, RecordsRange recordsRange, int productCategoryId, string? searchText)
        {
            var result = await _sellingProductRepository.GetSupermarketProducts(supermarketId, recordsRange, productCategoryId, searchText);

            return result.Select(GoodsKeepingProduct.FromProduct);
        }

        public async Task<PagedResult<GoodsKeepingStoragePlace>> GetStoragePlacesAsync(int supermarketId, RecordsRange recordsRange)
        {
            var result = await _storagePlaceRepository.GetSupermarketStoragePlaces(supermarketId, recordsRange);

            return result.Select(GoodsKeepingStoragePlace.FromStoragePlace);
        }

        public async Task<PagedResult<GoodsKeepingStoredProduct>> GetStoredProducts(int supermarketId, RecordsRange recordsRange)
        {
            var result = await _storedProductRepository.GetSupermarketStoredProducts(supermarketId, recordsRange);

            return result;
        }

        public async Task<PagedResult<SupplyWarehouse>> GetWarehousesAsync(int supermarketId, RecordsRange recordsRange)
        {
            var result = await _supermarketRepository.GetSupermarketWarehouses(supermarketId, recordsRange);

            return result.Select(SupplyWarehouse.FromStoragePlace);
        }

        public async Task MoveProductAsync(int storagePlaceId, MovingProduct movingProduct)
        {
            var id = new StoredProductId { StoragePlaceId = storagePlaceId, SupermarketId = movingProduct.SupermarketId, ProductId = movingProduct.ProductId };
            var newId = new StoredProductId { StoragePlaceId = movingProduct.NewStoragePlaceId, SupermarketId = movingProduct.SupermarketId, ProductId = movingProduct.ProductId };

            var storedProduct = await _storedProductRepository.GetByIdAsync(id);
            var storedFoundProduct = await _storedProductRepository.GetByIdAsync(newId);

            if (storedFoundProduct != null) // this product is already stored in NewStoragePlace
            {
                var newStoredProduct = new StoredProduct { Id = newId, Count = storedFoundProduct.Count + movingProduct.Count };
                await _storedProductRepository.UpdateAsync(newStoredProduct);
            }
            else
            {
                var newStoredProduct = new StoredProduct { Id = newId, Count = movingProduct.Count };
                await _storedProductRepository.AddAsync(newStoredProduct);
            }

            if (storedProduct != null) // change chosen product's Count
            {
                if (storedProduct.Count > movingProduct.Count)
                {
                    var oldProduct = new StoredProduct { Id = storedProduct.Id, Count = storedProduct.Count - movingProduct.Count };
                    await _storedProductRepository.UpdateAsync(oldProduct);
                }
                else
                {
                    await _storedProductRepository.DeleteAsync(storedProduct.Id);
                }
            }

        }

        public async Task SupplyProductsToWarehouseAsync(int warehouseId, IReadOnlyList<SuppliedProduct> suppliedProducts)
        {
            var warehouse = await _storagePlaceRepository.GetByIdAsync(warehouseId) ?? throw new Exception();
            foreach(var product in suppliedProducts)
            {
                var id = new StoredProductId(warehouseId, warehouse.SupermarketId, product.ProductId);
                var storedProduct = await _storedProductRepository.GetByIdAsync(id);
                if (storedProduct != null)
                {
                    var newStoredProduct = new StoredProduct { Id = id, Count = storedProduct.Count + product.Count };
                    await _storedProductRepository.UpdateAsync(newStoredProduct);
                }
                else
                    await _storedProductRepository.AddAsync(new StoredProduct { Id = id, Count = product.Count });
            }
        }
    }
}
