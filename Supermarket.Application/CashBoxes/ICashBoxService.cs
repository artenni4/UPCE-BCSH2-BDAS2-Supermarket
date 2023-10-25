using Supermarket.Domain.Auth;

namespace Supermarket.Core.CashBoxes
{
    /// <summary>
    /// Service for cash box screen
    /// </summary>
    public interface ICashBoxService
    {
        /// <summary>
        /// Returns a page with accessible products for a given supermarket
        /// </summary>
        Task<PagedResult<CashBoxProduct>> GetProductsAsync(int supermarketId, RecordsRange recordsRange, int productCategoryId, string? searchText);

        /// <summary>
        /// Returns categories for display bar in cash box
        /// </summary>
        Task<PagedResult<CashBoxProductCategory>> GetCategoriesAsync(int supermarketId, RecordsRange recordsRange);

        /// <summary>
        /// Adds sale for the cash box
        /// </summary>
        Task AddSaleAsync(int cashBoxId, IReadOnlyList<CashBoxSoldProduct> soldProducts, IReadOnlyList<Coupon> coupons);

        /// <summary>
        /// Tries to authenticate assistant to the cash box
        /// </summary>
        /// <exception cref="InvalidCredentialsException">in case of bad login or password</exception>
        /// <exception cref="PermissionDeniedException">in case when employee does not have appropriate role</exception>
        Task<AssistantLogin> AssistantLoginAsync(LoginData loginData, int cashBoxId);

        /// <summary>
        /// Checks whether coupon is valid
        /// </summary>
        /// <param name="couponCode">code of the coupon</param>
        /// <exception cref="InvalidCouponException">in case when coupon is not valid</exception>
        Task<Coupon> CheckCouponAsync(string couponCode);
        
        /// <summary>
        /// Gets list of cash boxes in given supermarket
        /// </summary>
        Task<PagedResult<SupermarketCashBox>> GetCashBoxesAsync(int supermarketId);
    }
}
