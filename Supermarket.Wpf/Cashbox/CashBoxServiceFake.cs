﻿using System;
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
        return Task.FromResult(new PagedResult<CashBoxProduct>(Array.Empty<CashBoxProduct>(), 1, 0));
    }

    public Task<PagedResult<CashBoxProductCategory>> GetCategoriesAsync(int supermarketId, RecordsRange recordsRange)
    {
        var categories = new CashBoxProductCategory[]
        {
            new() { Id = 1, Name = "AAA" },
            new() { Id = 2, Name = "BBB" },
        };
        return Task.FromResult(new PagedResult<CashBoxProductCategory>(categories, 1, categories.Length));
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