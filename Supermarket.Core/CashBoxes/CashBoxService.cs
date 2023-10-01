using Supermarket.Core.Employees;
using Supermarket.Core.Employees.LoggedEmployees;
using Supermarket.Core.Employees.Roles;
using Supermarket.Core.Products;
using Supermarket.Core.Products.Categories;

namespace Supermarket.Core.CashBoxes
{
    public class CashBoxService : ICashBoxService
    {
        private readonly IProductCategoryRepository _productCategoryRepository;
        private readonly IProductRepository _productRepository;
        private readonly IEmployeeService _employeeService;

        public CashBoxService(IProductCategoryRepository productCategoryRepository, IProductRepository productRepository, IEmployeeService employeeService)
        {
            _productCategoryRepository = productCategoryRepository;
            _productRepository = productRepository;
            _employeeService = employeeService;
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
            throw new NotImplementedException();
        }

        public async Task<AssistantLoginResult> AssistantLoginAsync(LoginData loginData)
        {
            try
            {
                var employee = await _employeeService.LoginEmployeeAsync(loginData);
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
