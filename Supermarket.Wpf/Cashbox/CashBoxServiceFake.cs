using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Supermarket.Core.Domain.Auth;
using Supermarket.Core.Domain.Common.Paging;
using Supermarket.Core.UseCases.CashBoxes;

namespace Supermarket.Wpf.Cashbox;

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

    public Task AddSaleAsync(int cashBoxId, IReadOnlyList<CashBoxSoldProduct> soldProducts, IReadOnlyList<Coupon> coupons)
    {
        throw new NotImplementedException();
    }

    public Task<AssistantLogin> AssistantLoginAsync(LoginData loginData, int cashBoxId)
    {
        throw new NotImplementedException();
    }

    public Task<Coupon> CheckCouponAsync(string couponCode)
    {
        throw new NotImplementedException();
    }

    public Task<PagedResult<SupermarketCashBox>> GetCashBoxesAsync(int supermarketId)
    {
        throw new NotImplementedException();
    }
}