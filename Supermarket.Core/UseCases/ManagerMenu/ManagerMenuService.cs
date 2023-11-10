using Supermarket.Core.Domain.Common.Paging;
using Supermarket.Core.Domain.Products;
using Supermarket.Core.Domain.SellingProducts;
using Supermarket.Core.Domain.StoredProducts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Supermarket.Core.UseCases.ManagerMenu
{
    public class ManagerMenuService : IManagerMenuService
    {
        private readonly ISellingProductRepository _sellingProductRepository;
        private readonly IProductRepository _productRepository;

        public ManagerMenuService(ISellingProductRepository sellingProductRepository, IStoredProductRepository storedProductRepository, IProductRepository productRepository)
        {
            _sellingProductRepository = sellingProductRepository;
            _productRepository = productRepository;
        }

        public async Task<PagedResult<ManagerMenuProduct>> GetSupermarketProducts(int supermarketId, RecordsRange recordsRange)
        {
            return await _sellingProductRepository.GetAllSupermarketProducts(supermarketId, recordsRange);
        }

        public async Task<PagedResult<ManagerMenuAddProduct>> GetManagerProductsToAdd(int supermarketId, RecordsRange recordsRange)
        {
            var result = await _productRepository.GetManagerProductsToAdd(supermarketId, recordsRange);

            return result;
        }



    }
}
