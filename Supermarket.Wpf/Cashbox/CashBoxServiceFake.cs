using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Supermarket.Core.CashBoxes;
using Supermarket.Domain.Auth;
using Supermarket.Domain.Common.Paging;

namespace Supermarket.Wpf.Cashbox;

public class CashBoxServiceFake : ICashBoxService
{
    public Task<PagedResult<CashBoxProduct>> GetProductsAsync(int supermarketId, RecordsRange recordsRange, int productCategoryId, string? searchText)
    {
        var products = new CashBoxProduct[]
        {
            new() { ProductId = 1, Name = "AAA", IsByWeight = true },
            new() { ProductId = 2, Name = "BBB", IsByWeight = true },
            new() { ProductId = 3, Name = "CCCCCCCC CCCC", IsByWeight = true },
        };
        
        return Task.FromResult(new PagedResult<CashBoxProduct>(products, 1, products.Length, products.Length));
    }

    public Task<PagedResult<CashBoxProductCategory>> GetCategoriesAsync(int supermarketId, RecordsRange recordsRange)
    {
        var categories = new CashBoxProductCategory[]
        {
            new() { CategoryId = 1, Name = "AAA" },
            new() { CategoryId = 2, Name = "BBB" },
            new() { CategoryId = 3, Name = "CCCCCCCC CCCC" },
        };
        return Task.FromResult(new PagedResult<CashBoxProductCategory>(categories, 1, categories.Length, categories.Length));
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