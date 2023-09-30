namespace Supermarket.Core.Products
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;

        public ProductService(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<PagedResult<Product>> GetCashBoxProductsPage(int supermarketId, int productCategoryId, RecordsRange recordsRange)
        {
            return await _productRepository.GetPagedAsync(new ProductQueryObject
            {
                RecordsRange = recordsRange,
                SupermarketId = supermarketId,
                ProductCategoryId = productCategoryId,
            });
        }
    }
}
