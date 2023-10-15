using Supermarket.Domain.Auth;
using Supermarket.Domain.Auth.LoggedEmployees;
using Supermarket.Domain.Employees.Roles;
using Supermarket.Domain.Products;
using Supermarket.Domain.Products.Categories;

namespace Supermarket.Core.CashBoxes
{
    internal class CashBoxService : ICashBoxService
    {
        private readonly IProductCategoryRepository _productCategoryRepository;
        private readonly IProductRepository _productRepository;
        private readonly IAuthDomainService _authDomainService;
        private readonly IUnitOfWork _unitOfWork;

        public CashBoxService(IProductCategoryRepository productCategoryRepository, IProductRepository productRepository, IAuthDomainService authDomainService, IUnitOfWork unitOfWork)
        {
            _productCategoryRepository = productCategoryRepository;
            _productRepository = productRepository;
            _authDomainService = authDomainService;
            _unitOfWork = unitOfWork;
        }

        public async Task<PagedResult<CashBoxProductCategory>> GetCategoriesAsync(int supermarketId, RecordsRange recordsRange)
        {
            var productCategories = await _productCategoryRepository.GetPagedAsync(new PagingQueryObject { RecordsRange = recordsRange });

            return productCategories.Select(CashBoxProductCategory.FromProductCategory);
        }

        public async Task<PagedResult<CashBoxProduct>> GetProductsAsync(int supermarketId, RecordsRange recordsRange, int productCategoryId, string? searchText)
        {
            var products = await _productRepository.GetPagedAsync(new ProductQueryObject
            {
                RecordsRange = recordsRange,
                SupermarketId = supermarketId,
                ProductCategoryId = productCategoryId,
                SearchText = searchText,
            });

            return products.Select(CashBoxProduct.FromProduct);
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
