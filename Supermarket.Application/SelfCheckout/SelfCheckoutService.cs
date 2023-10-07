using Supermarket.Domain.Auth;
using Supermarket.Domain.Auth.LoggedEmployees;
using Supermarket.Domain.Common.Paging;
using Supermarket.Domain.Employees.Roles;
using Supermarket.Domain.Products;
using Supermarket.Domain.Products.Categories;

namespace Supermarket.Core.SelfCheckout
{
    internal class SelfCheckoutService : ISelfCheckoutService
    {
        private readonly IProductCategoryRepository _productCategoryRepository;
        private readonly IProductRepository _productRepository;
        private readonly IAuthDomainService _authDomainService;
        private readonly IUnitOfWork _unitOfWork;

        public SelfCheckoutService(IProductCategoryRepository productCategoryRepository, IProductRepository productRepository, IAuthDomainService authDomainService, IUnitOfWork unitOfWork)
        {
            _productCategoryRepository = productCategoryRepository;
            _productRepository = productRepository;
            _authDomainService = authDomainService;
            _unitOfWork = unitOfWork;
        }

        public async Task<PagedResult<ProductCategory>> GetAllCategoriesAsync(RecordsRange recordsRange)
        {
            return await _productCategoryRepository.GetPagedAsync(new PagingQueryObject { RecordsRange = recordsRange });
        }

        public async Task<PagedResult<Product>> GetCashBoxProductsPage(RecordsRange recordsRange, int supermarketId, int productCategoryId, string? searchText)
        {
            return await _productRepository.GetPagedAsync(new ProductQueryObject
            {
                RecordsRange = recordsRange,
                SupermarketId = supermarketId,
                ProductCategoryId = productCategoryId,
                SearchText = searchText,
            });
        }

        public Task AddSaleAsync(int cashBoxId, IReadOnlyList<SoldProduct> soldProducts, IReadOnlyList<Coupon> coupons)
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

        public Task<Coupon> CheckCoupon(string couponCode)
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
    }
}
