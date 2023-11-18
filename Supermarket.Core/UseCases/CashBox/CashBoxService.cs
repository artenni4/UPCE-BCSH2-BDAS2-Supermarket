using Supermarket.Core.Domain.Auth;
using Supermarket.Core.Domain.Auth.LoggedEmployees;
using Supermarket.Core.Domain.CashBoxes;
using Supermarket.Core.Domain.Common;
using Supermarket.Core.Domain.Common.Paging;
using Supermarket.Core.Domain.Payments;
using Supermarket.Core.Domain.Sales;
using Supermarket.Core.Domain.SellingProducts;
using Supermarket.Core.Domain.SoldProducts;
using Supermarket.Core.Domain.StoredProducts;
using Supermarket.Core.UseCases.Common;

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
        private readonly IStoredProductRepository _storedProductRepository;
        private readonly IUnitOfWork _unitOfWork;

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
            IPaymentRepository paymentRepository,
            IStoredProductRepository storedProductRepository,
            IUnitOfWork unitOfWork)
        {
            _sellingProductRepository = sellingProductRepository;
            _authDomainService = authDomainService;
            _cashBoxRepository = cashBoxRepository;
            _saleRepository = saleRepository;
            _soldProductRepository = soldProductRepository;
            _paymentRepository = paymentRepository;
            _storedProductRepository = storedProductRepository;
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

        public async Task AddSaleAsync(int cashBoxId, CashBoxPayment cashBoxPayment, IReadOnlyList<CashBoxSoldProduct> soldProducts)
        {
            var cashBox = await _cashBoxRepository.GetByIdAsync(cashBoxId);
            if (cashBox is null)
            {
                throw new ApplicationInconsistencyException("Cash box must exist when adding sale to it");
            }
            
            await using var transaction = await _unitOfWork.BeginTransactionAsync();
            var saleId = await _saleRepository.AddAndGetIdAsync(new Sale
            {
                Id = 0,
                CashBoxId = cashBoxId,
                Date = DateTimeOffset.Now,
            });

            foreach (var soldProduct in soldProducts)
            {
                await _soldProductRepository.AddAsync(new SoldProduct
                {
                    Id = new SoldProductId(saleId, cashBox.SupermarketId, soldProduct.ProductId),
                    Pieces = soldProduct.Count,
                    Price = soldProduct.Price
                });
            }

            foreach (var coupon in cashBoxPayment.Coupons)
            {
                await _paymentRepository.AddAsync(new Payment
                {
                    Id = new PaymentId(saleId, PaymentType.Kupon),
                    Amount = coupon.Discount
                });
            }

            var paymentType = cashBoxPayment.PaymentType switch
            {
                CashBoxPaymentType.Cash => PaymentType.Hotovost,
                CashBoxPaymentType.Card => PaymentType.Karta,
                _ => throw new ArgumentException(nameof(cashBoxPayment.PaymentType)),
            };
            await _paymentRepository.AddAsync(new Payment
            {
                Id = new PaymentId(saleId, paymentType),
                Amount = cashBoxPayment.Total
            });
            await transaction.CommitAsync();
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
