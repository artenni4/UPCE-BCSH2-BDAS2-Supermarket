using Supermarket.Domain.Auth;
using Supermarket.Domain.Common.Paging;
using Supermarket.Domain.Products;
using Supermarket.Domain.Products.Categories;

namespace Supermarket.Core.SelfCheckout
{
    /// <summary>
    /// Service for cash box screen
    /// </summary>
    public interface ISelfCheckoutService
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
        Task AddSaleAsync(int cashBoxId, IReadOnlyList<SoldProduct> soldProducts, IReadOnlyList<Coupon> coupons);

        /// <summary>
        /// Tries to authenticate assitant to the cash box
        /// </summary>
        /// <exception cref="InvalidCredentialsException">in case of bad login or password</exception>
        /// <exception cref="PermissionDeniedException">in case when employee does not have appropriate role</exception>
        Task<AssistantLogin> AssistantLoginAsync(LoginData loginData, int cashBoxId);

        /// <summary>
        /// Checks whether coupon is valid
        /// </summary>
        /// <param name="couponCode">code of the coupon</param>
        /// <exception cref="InvalidCouponException">in case when coupon is not valid</exception>
        Task<Coupon> CheckCoupon(string couponCode);
    }
}
