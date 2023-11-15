using Supermarket.Core.Domain.CashBoxes;
using Supermarket.Core.Domain.Common;
using Supermarket.Core.Domain.Common.Paging;
using Supermarket.Core.Domain.Employees;
using Supermarket.Core.Domain.Products;
using Supermarket.Core.Domain.Sales;
using Supermarket.Core.Domain.SellingProducts;
using Supermarket.Core.Domain.StoragePlaces;
using Supermarket.Core.Domain.StoredProducts;
using Cashbox = Supermarket.Core.Domain.CashBoxes.CashBox;

namespace Supermarket.Core.UseCases.ManagerMenu
{
    public class ManagerMenuService : IManagerMenuService
    {
        private readonly ISellingProductRepository _sellingProductRepository;
        private readonly IProductRepository _productRepository;
        private readonly IStoredProductRepository _storedProductRepository;
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IStoragePlaceRepository _storagePlaceRepository;
        private readonly ISaleRepository _saleRepository;
        private readonly ICashBoxRepository _cashBoxRepository;

        public ManagerMenuService(ISellingProductRepository sellingProductRepository, IStoredProductRepository storedProductRepository, IProductRepository productRepository, IEmployeeRepository employeeRepository, IStoragePlaceRepository storagePlaceRepository, ISaleRepository saleRepository, ICashBoxRepository cashBoxRepository)
        {
            _sellingProductRepository = sellingProductRepository;
            _productRepository = productRepository;
            _storedProductRepository = storedProductRepository;
            _employeeRepository = employeeRepository;
            _storagePlaceRepository = storagePlaceRepository;
            _saleRepository = saleRepository;
            _cashBoxRepository = cashBoxRepository;
        }

        #region SupermarketProducts
        public async Task<PagedResult<ManagerMenuProduct>> GetSupermarketProducts(int supermarketId, RecordsRange recordsRange)
        {
            return await _sellingProductRepository.GetAllSupermarketProducts(supermarketId, recordsRange);
        }
        #endregion

        #region AddProducts
        public async Task<PagedResult<ManagerMenuAddProduct>> GetManagerProductsToAdd(int supermarketId, RecordsRange recordsRange)
        {
            var result = await _productRepository.GetManagerProductsToAdd(supermarketId, recordsRange);

            return result;
        }

        public async Task RemoveProductFromSupermarket(StoredProductId id)
        {
            await _sellingProductRepository.UpdateAsync(new SellingProduct { Id = new SellingProductId { ProductId = id.ProductId, SupermarketId = id.SupermarketId }, IsActive = false });
        }

        public async Task AddProductToSupermarket(SellingProductId id)
        {
            var product = await _sellingProductRepository.GetByIdAsync(id);
            if (product == null)
            {
                await _sellingProductRepository.AddAsync(new SellingProduct { Id = id, IsActive = true });
            }
            else
            {
                await _sellingProductRepository.UpdateAsync(new SellingProduct { Id = id, IsActive = true });
            }
        }
        #endregion

        #region Employees
        public async Task<PagedResult<ManagerMenuEmployee>> GetManagerEmployees(int supermarketId, RecordsRange recordsRange)
        {
            return await _employeeRepository.GetSupermarketEmployees(supermarketId, recordsRange);
        }

        public async Task<ManagerMenuEmployeeDetail?> GetEmployeeToEdit(int employeeId)
        {
            var employee = await _employeeRepository.GetEmployeeDetail(employeeId);

            if (employee == null)
            {
                throw new InconsistencyException("Zaměstnanec nebyl nalezn");
            }

            return employee;
        }

        public async Task DeleteEmployee(int employeeId)
        {
            await _employeeRepository.DeleteAsync(employeeId);
        }
        #endregion

        #region StoragePlaces
        public async Task<PagedResult<StoragePlace>> GetStoragePlaces(int supermarketId, RecordsRange recordsRange)
        {
            return await _storagePlaceRepository.GetSupermarketStoragePlaces(supermarketId, recordsRange);
        }

        public async Task<StoragePlace?> GetStorageToEdit(int storageId)
        {
            return await _storagePlaceRepository.GetByIdAsync(storageId);
        }

        public async Task AddStorage(StoragePlace storagePlace)
        {
            await _storagePlaceRepository.AddAsync(storagePlace);
        }

        public async Task EditStorage(StoragePlace storagePlace)
        {
            await _storagePlaceRepository.UpdateAsync(storagePlace);
        }

        public async Task DeleteStorage(int id)
        {
            await _storagePlaceRepository.DeleteAsync(id);
        }

        #endregion

        public async Task<PagedResult<ManagerMenuSale>> GetSupermarketSales(int supermarketId, DateTime dateFrom, DateTime dateTo, RecordsRange recordsRange)
        {
            return await _saleRepository.GetSupermarketSales(supermarketId, dateFrom, dateTo, recordsRange);
        }

        #region SupermarketCashboxes

        public async Task<PagedResult<ManagerMenuCashbox>> GetSupermarketCashboxes(int supermarketId, RecordsRange recordsRange)
        {
            return await _cashBoxRepository.GetSupermarketCashboxes(supermarketId, recordsRange);
        }

        public async Task<Cashbox?> GetCashboxToEdit(int cashboxId)
        {
            return await _cashBoxRepository.GetByIdAsync(cashboxId);
        }

        public async Task DeleteCashbox(int cashboxId)
        {
            await _cashBoxRepository.DeleteAsync(cashboxId);
        }

        public async Task AddCashbox(Cashbox cashbox)
        {
            await _cashBoxRepository.AddAsync(cashbox);
        }

        public async Task EditCashbox(Cashbox cashbox)
        {
            await _cashBoxRepository.UpdateAsync(cashbox);
        }

        #endregion
    }
}
