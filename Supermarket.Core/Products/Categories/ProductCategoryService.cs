using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Supermarket.Core.Products.Categories
{
    public class ProductCategoryService : IProductCategoryService
    {
        private readonly IProductCategoryRepository _productCategoryRepository;

        public ProductCategoryService(IProductCategoryRepository productCategoryRepository)
        {
            _productCategoryRepository = productCategoryRepository;
        }

        public async Task<PagedResult<ProductCategory>> GetAllCategoriesAsync(RecordsRange recordsRange)
        {
            return await _productCategoryRepository.GetPagedAsync(new PagingQueryObject { RecordsRange = recordsRange });
        }
    }
}
