using Supermarket.Core.Domain.Auth;
using Supermarket.Core.Domain.Auth.LoggedEmployees;
using Supermarket.Core.UseCases.CashBox;

namespace Supermarket.Wpf.CashBox;

public class CashBoxServiceFake : ICashBoxService
{
    public Task<PagedResult<CashBoxProduct>> GetProductsAsync(int supermarketId, RecordsRange recordsRange, int productCategoryId, string? searchText)
    {
        return Task.FromResult(PagedResult<CashBoxProduct>.Empty());
    }

    public Task<PagedResult<CashBoxProductCategory>> GetCategoriesAsync(int supermarketId, RecordsRange recordsRange)
    {
        return Task.FromResult(PagedResult<CashBoxProductCategory>.Empty());
    }

    public Task AddSaleAsync(int cashBoxId, CashBoxPaymentType cashBoxPaymentType, IReadOnlyList<CashBoxSoldProduct> soldProducts, IReadOnlyList<Coupon> coupons)
    {
        throw new NotImplementedException();
    }

    public Task<LoggedSupermarketEmployee> AssistantLoginAsync(LoginData loginData, int cashBoxId)
    {
        throw new NotImplementedException();
    }

    public Task<Coupon> CheckCouponAsync(string couponCode)
    {
        throw new NotImplementedException();
    }

    public Task<PagedResult<SupermarketCashBox>> GetCashBoxesAsync(int supermarketId, RecordsRange recordsRange)
    {
        throw new NotImplementedException();
    }
}