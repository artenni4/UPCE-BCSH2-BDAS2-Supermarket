using Supermarket.Core.Employees;
using Supermarket.Core.Products;
using Supermarket.Core.Products.Categories;

namespace Supermarket.Core.CashBoxes
{
    interface ICashBoxService
    {
        /// <summary>
        /// Returns a page with accessible products for a given supermarket
        /// </summary>
        Task<PagedResult<Product>> GetCashBoxProductsPage(RecordsRange recordsRange, int supermarketId, int productCategoryId, string? searchText);

        /// <summary>
        /// Returns categories for display bar in cash box
        /// </summary>
        /// <param name="recordsRange"></param>
        /// <returns></returns>
        Task<PagedResult<ProductCategory>> GetAllCategoriesAsync(RecordsRange recordsRange);

        /// <summary>
        /// Adds sale for the cash box
        /// </summary>
        /// <returns></returns>
        Task AddSaleAsync(int cashBoxId, IReadOnlyList<SoldProductDto> soldProducts);

        /// <summary>
        /// Tries to authenticate assitant to the cash box
        /// </summary>
        Task<AssistantLoginResult> AssistantLoginAsync(LoginData loginData);
    }
}
