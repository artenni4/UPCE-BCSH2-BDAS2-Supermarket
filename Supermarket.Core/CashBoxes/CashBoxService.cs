using Supermarket.Core.Auth;
using Supermarket.Core.Auth.LoggedEmployees;
using Supermarket.Core.Employees.Roles;
using Supermarket.Core.Products;
using Supermarket.Core.Products.Categories;

namespace Supermarket.Core.CashBoxes
{
    public class CashBoxService : ICashBoxService
    {
        private readonly IProductCategoryRepository _productCategoryRepository;
        private readonly IProductRepository _productRepository;
        private readonly IAuthService _authService;
        private readonly IUnitOfWork _unitOfWork;

        public CashBoxService(IProductCategoryRepository productCategoryRepository, IProductRepository productRepository, IAuthService authService, IUnitOfWork unitOfWork)
        {
            _productCategoryRepository = productCategoryRepository;
            _productRepository = productRepository;
            _authService = authService;
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

        public Task AddSaleAsync(int cashBoxId, IReadOnlyList<SoldProductDto> soldProducts)
        {
            // do not forget use unit of work and transactions
            throw new NotImplementedException();
        }

        public async Task<AssistantLoginResult> AssistantLoginAsync(LoginData loginData, int cashBoxId)
        {
            // TODO check if cash box is in correct supermarket
            try
            {
                var employee = await _authService.AuthEmployeeAsync(loginData);
                if (employee is LoggedSupermarketEmployee supermarketEmployee && supermarketEmployee.Roles.Any(r => r is CashierRole))
                {
                    return AssistantLoginResult.Success();
                }

                return AssistantLoginResult.Fail(AssistantLoginFail.PermissionDenied);
            }
            catch (InvalidCredentialsException)
            {
                return AssistantLoginResult.Fail(AssistantLoginFail.InvalidCredentials);
            }
        }
    }
}
