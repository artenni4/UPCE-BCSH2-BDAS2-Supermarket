using Supermarket.Core.Domain.Auth;
using Supermarket.Core.Domain.Auth.LoggedEmployees;
using Supermarket.Core.Domain.Common.Paging;

namespace Supermarket.Core.UseCases.CashBox
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
        Task AddSaleAsync(int cashBoxId, CashBoxPayment cashBoxPayment, IReadOnlyList<CashBoxSoldProduct> soldProducts);

        /// <summary>
        /// Tries to authenticate assistant to the cash box
        /// </summary>
        /// <exception cref="InvalidCredentialsException">in case of bad login or password</exception>
        /// <exception cref="PermissionDeniedException">in case when employee does not have appropriate role</exception>
        Task<LoggedSupermarketEmployee> AssistantLoginAsync(LoginData loginData, int cashBoxId);

        /// <summary>
        /// Checks whether coupon is valid
        /// </summary>
        /// <param name="soldProducts">sold products</param>
        /// <param name="couponCode">code of the coupon</param>
        /// <param name="usedCoupons">coupons that are already used</param>
        /// <exception cref="InvalidCouponException">coupon is not valid</exception>
        /// <exception cref="CouponExceedsCostException">coupon exceeds cost of products</exception>
        Task<Coupon> CheckCouponAsync(string couponCode, IReadOnlyList<CashBoxSoldProduct> soldProducts, IReadOnlyList<Coupon> usedCoupons);
        
        /// <summary>
        /// Gets list of cash boxes in given supermarket
        /// </summary>
        Task<PagedResult<SupermarketCashBox>> GetCashBoxesAsync(int supermarketId, RecordsRange recordsRange);
    }
}
