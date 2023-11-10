using Supermarket.Core.Domain.Auth;
using Supermarket.Core.Domain.Auth.LoggedEmployees;
using Supermarket.Core.Domain.Common.Paging;
using Supermarket.Core.Domain.Employees.Roles;
using Supermarket.Core.Domain.SellingProducts;
using Supermarket.Core.UseCases.Common;

namespace Supermarket.Core.UseCases.CashBoxes
{
    internal class CashBoxService : ICashBoxService
    {
        private readonly ISellingProductRepository _sellingProductRepository;
        private readonly IAuthDomainService _authDomainService;
        private readonly IUnitOfWork _unitOfWork;

        public CashBoxService(ISellingProductRepository sellingProductRepository, IAuthDomainService authDomainService, IUnitOfWork unitOfWork)
        {
            _sellingProductRepository = sellingProductRepository;
            _authDomainService = authDomainService;
            _unitOfWork = unitOfWork;
        }

        public async Task<PagedResult<CashBoxProductCategory>> GetCategoriesAsync(int supermarketId, RecordsRange recordsRange)
        {
            var result = await _sellingProductRepository.GetSupermarketProductCategories(supermarketId, recordsRange);

            return result.Select(CashBoxProductCategory.FromProductCategory);
        }

        public async Task<PagedResult<CashBoxProduct>> GetProductsAsync(int supermarketId, RecordsRange recordsRange, int productCategoryId, string? searchText)
        {
            var result = await _sellingProductRepository.GetSupermarketProducts(supermarketId, recordsRange, productCategoryId, searchText);

            return result.Select(CashBoxProduct.FromProduct);
        }

        public Task AddSaleAsync(int cashBoxId, IReadOnlyList<CashBoxSoldProduct> soldProducts, IReadOnlyList<Coupon> coupons)
        {
            // do not forget use unit of work and transactions
            throw new NotImplementedException();
        }

        public async Task<AssistantLogin> AssistantLoginAsync(LoginData loginData, int cashBoxId)
        {
            // TODO check if cash box is in correct supermarket
            var employee = await _authDomainService.AuthEmployeeAsync(loginData);
            if (employee is LoggedSupermarketEmployee supermarketEmployee && supermarketEmployee.Roles.Any(r => r is CashierRole))
            {
                return new AssistantLogin
                {
                    Employee = employee
                };
            }

            throw new PermissionDeniedException();
        }

        public Task<Coupon> CheckCouponAsync(string couponCode)
        {
            if (couponCode == "BDAS2+BCSH2=LOVE")
            {
                var coupon = new Coupon
                {
                    Code = couponCode,
                    Discount = 100
                };

                return Task.FromResult(coupon);
            }

            throw new InvalidCouponException();
        }

        public Task<PagedResult<SupermarketCashBox>> GetCashBoxesAsync(int supermarketId)
        {
            throw new NotImplementedException();
        }
    }
}
