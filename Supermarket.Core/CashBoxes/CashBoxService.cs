using Supermarket.Core.Products;
using Supermarket.Core.Products.Categories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Supermarket.Core.CashBoxes
{
    public class CashBoxService : ICashBoxService
    {
        private readonly IProductCategoryRepository _productCategoryRepository;
        private readonly IProductRepository _productRepository;

        public CashBoxService(IProductCategoryRepository productCategoryRepository, IProductRepository productRepository)
        {
            _productCategoryRepository = productCategoryRepository;
            _productRepository = productRepository;
        }

        public async Task<PagedResult<ProductCategory>> GetAllCategoriesAsync(RecordsRange recordsRange)
        {
            return await _productCategoryRepository.GetPagedAsync(new PagingQueryObject { RecordsRange = recordsRange });
        }

        public async Task<PagedResult<Product>> GetCashBoxProductsPage(RecordsRange recordsRange, int supermarketId, int productCategoryId, string? searchText)
        {
            return await _productRepository.GetPagedAsync(new ProductQueryObject
            {
                RecordsRange = recordsRange,
                SupermarketId = supermarketId,
                ProductCategoryId = productCategoryId,
                SearchText = searchText,
            });
        }

        public Task AddSaleAsync(int cashBoxId, IReadOnlyList<SoldProductDto> soldProducts)
        {
            throw new NotImplementedException();
        }
    }
}
