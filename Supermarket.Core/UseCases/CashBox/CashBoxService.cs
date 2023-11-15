using Supermarket.Core.Domain.Auth;
using Supermarket.Core.Domain.Auth.LoggedEmployees;
using Supermarket.Core.Domain.CashBoxes;
using Supermarket.Core.Domain.Common.Paging;
using Supermarket.Core.Domain.Payments;
using Supermarket.Core.Domain.Sales;
using Supermarket.Core.Domain.SellingProducts;
using Supermarket.Core.Domain.SoldProducts;

namespace Supermarket.Core.UseCases.CashBox
{
    internal class CashBoxService : ICashBoxService
    {
        private readonly ISellingProductRepository _sellingProductRepository;
        private readonly ISaleRepository _saleRepository;
        private readonly ISoldProductRepository _soldProductRepository;
        private readonly IPaymentRepository _paymentRepository;
        private readonly IAuthDomainService _authDomainService;
        private readonly ICashBoxRepository _cashBoxRepository;

        private readonly IDictionary<string, Coupon> _coupons = new Dictionary<string, Coupon>()
        {
            ["BDAS2-300"] = new Coupon { Code = "BDAS2-300", Discount = 300 },
            ["BDAS2-50"] = new Coupon { Code = "BDAS2-50", Discount = 50 },
            ["BCSH2-300"] = new Coupon { Code = "BCSH2-300", Discount = 300 },
            ["BCSH2-50"] = new Coupon { Code = "BCSH2-50", Discount = 50 },
        };

        public CashBoxService(
            ISellingProductRepository sellingProductRepository,
            IAuthDomainService authDomainService,
            ICashBoxRepository cashBoxRepository,
            ISaleRepository saleRepository,
            ISoldProductRepository soldProductRepository,
            IPaymentRepository paymentRepository)
        {
            _sellingProductRepository = sellingProductRepository;
            _authDomainService = authDomainService;
            _cashBoxRepository = cashBoxRepository;
            _saleRepository = saleRepository;
            _soldProductRepository = soldProductRepository;
            _paymentRepository = paymentRepository;
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

        public Task AddSaleAsync(int cashBoxId, CashBoxPaymentType cashBoxPaymentType, IReadOnlyList<CashBoxSoldProduct> soldProducts, IReadOnlyList<Coupon> coupons)
        {
            // TODO return id of new record, test transactions
            _saleRepository.AddAsync(new Sale
            {
                Id = 0,
                CashBoxId = cashBoxId,
                Date = DateTimeOffset.Now,
            });
            throw new NotImplementedException();
        }

        public async Task<LoggedSupermarketEmployee> AssistantLoginAsync(LoginData loginData, int cashBoxId)
        {
            var employee = await _authDomainService.AuthEmployeeAsync(loginData);
            if (employee is LoggedSupermarketEmployee supermarketEmployee && supermarketEmployee.Roles.Contains(SupermarketEmployeeRole.Cashier))
            {
                return supermarketEmployee;
            }

            throw new PermissionDeniedException();
        }

        public Task<Coupon> CheckCouponAsync(string couponCode)
        {
            if (_coupons.TryGetValue(couponCode, out var coupon))
            {
                return Task.FromResult(coupon);
            }

            throw new InvalidCouponException();
        }

        public async Task<PagedResult<SupermarketCashBox>> GetCashBoxesAsync(int supermarketId, RecordsRange recordsRange)
        {
            var cashBoxes = await _cashBoxRepository.GetBySupermarketId(supermarketId, recordsRange);
            return cashBoxes.Select(c => new SupermarketCashBox
            {
                Id = c.Id,
                Code = c.Code
            });
        }
    }
}
