using Supermarket.Core.Domain.Common.Paging;
using Supermarket.Core.Domain.SellingProducts;
using Supermarket.Core.Domain.StoragePlaces;
using Supermarket.Core.Domain.StoredProducts;
using Supermarket.Core.Domain.Suppliers;
using Cashbox = Supermarket.Core.Domain.CashBoxes.CashBox;

namespace Supermarket.Core.UseCases.ManagerMenu
{
    public interface IManagerMenuService
    {
        Task<PagedResult<ManagerMenuProduct>> GetSupermarketProducts(int supermarketId, RecordsRange recordsRange);
        Task<PagedResult<ManagerMenuAddProduct>> GetManagerProductsToAdd(int supermarketId, RecordsRange recordsRange);
        Task RemoveProductFromSupermarket(StoredProductId id);
        Task AddProductToSupermarket(SellingProductId id);
        Task<PagedResult<ManagerMenuEmployee>> GetManagerEmployees(int supermarketId, RecordsRange recordsRange);
        Task<ManagerMenuEmployeeDetail?> GetEmployeeToEdit(int employeeId);
        Task DeleteEmployee(int employeeId);
        Task<PagedResult<StoragePlace>> GetStoragePlaces(int supermarketId, RecordsRange recordsRange);
        Task<StoragePlace?> GetStorageToEdit(int storageId);
        Task AddStorage(StoragePlace storagePlace);
        Task EditStorage(StoragePlace storagePlace);
        Task DeleteStorage(int id);
        Task<PagedResult<ManagerMenuSale>> GetSupermarketSales(int supermarket, DateTime dateFrom, DateTime dateTo, RecordsRange recordsRange);
        Task<PagedResult<ManagerMenuCashbox>> GetSupermarketCashboxes(int supermarketId, RecordsRange recordsRange);
        Task<Cashbox?> GetCashboxToEdit(int cashboxId);
        Task DeleteCashbox(int cashboxId);
        Task AddCashbox(Cashbox cashbox);
        Task EditCashbox(Cashbox cashbox);
    }
}
