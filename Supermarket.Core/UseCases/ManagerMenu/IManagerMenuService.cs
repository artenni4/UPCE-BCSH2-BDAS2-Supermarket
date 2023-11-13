using Supermarket.Core.Domain.Common.Paging;
using Supermarket.Core.Domain.Employees;
using Supermarket.Core.Domain.SellingProducts;
using Supermarket.Core.Domain.StoredProducts;

namespace Supermarket.Core.UseCases.ManagerMenu
{
    public interface IManagerMenuService
    {
        Task<PagedResult<ManagerMenuProduct>> GetSupermarketProducts(int supermarketId, RecordsRange recordsRange);
        Task<PagedResult<ManagerMenuAddProduct>> GetManagerProductsToAdd(int supermarketId, RecordsRange recordsRange);
        void RemoveProductFromSupermarket(StoredProductId id);
        void AddProductToSupermarket(SellingProductId id);
        Task<PagedResult<ManagerMenuEmployee>> GetManagerEmployees(int supermarketId, RecordsRange recordsRange);
        Task<Employee?> GetEmployeeToEdit(int employeeId);
    }
}
