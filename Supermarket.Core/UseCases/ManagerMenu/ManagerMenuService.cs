using Supermarket.Core.Domain.Auth;
using Supermarket.Core.Domain.Auth.LoggedEmployees;
using Supermarket.Core.Domain.CashBoxes;
using Supermarket.Core.Domain.Common;
using Supermarket.Core.Domain.Common.Paging;
using Supermarket.Core.Domain.Employees;
using Supermarket.Core.Domain.Employees.Roles;
using Supermarket.Core.Domain.Products;
using Supermarket.Core.Domain.Sales;
using Supermarket.Core.Domain.SellingProducts;
using Supermarket.Core.Domain.StoragePlaces;
using Supermarket.Core.Domain.StoredProducts;
using Cashbox = Supermarket.Core.Domain.CashBoxes.CashBox;

namespace Supermarket.Core.UseCases.ManagerMenu
{
    internal class ManagerMenuService : IManagerMenuService
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

        public async Task AddEmployee(ManagerAddEmployee managerAddEmployee)
        {
            var salt = PasswordHashing.GenerateSalt();
            var passwordHash = PasswordHashing.GenerateSaltedHash(managerAddEmployee.Password, salt);

            await _employeeRepository.AddAsync(new EmployeeRole
            {
                Id = managerAddEmployee.Id,
                Login = managerAddEmployee.Login,
                Name = managerAddEmployee.Name,
                Surname = managerAddEmployee.Surname,
                HireDate = managerAddEmployee.HireDate,
                RoleInfo = new SupermarketEmployee(managerAddEmployee.SupermarketId, managerAddEmployee.ManagerId, managerAddEmployee.Roles),
                PasswordHash = passwordHash,
                PasswordHashSalt = salt
            });
        }

        public async Task<PagedResult<ManagerMenuEmployee>> GetSupermarketEmployees(int employeeId, int supermarketId, RecordsRange recordsRange)
        {
            var employee = await _employeeRepository.GetRoleByIdAsync(employeeId);
            if (employee is null)
            {
                throw new ApplicationInconsistencyException($"Employee {employeeId} was not found");
            }
            
            if (employee.RoleInfo is Domain.Employees.Roles.Admin)
            {
                return await _employeeRepository.GetSupermarketEmployeesForAdmin(supermarketId, recordsRange);
            }
            return await _employeeRepository.GetSupermarketEmployeesForManager(employeeId, recordsRange);
        }

        public async Task<ManagerMenuEmployeeDetail> GetEmployeeToEdit(int employeeId)
        {
            var employee = await _employeeRepository.GetEmployeeDetail(employeeId);
            if (employee == null)
            {
                throw new ApplicationInconsistencyException("Employee was not found");
            }

            return employee;
        }

        public async Task EditEmployee(ManagerEditEmployee managerEditEmployee)
        {
            var oldEmployee = await _employeeRepository.GetByIdAsync(managerEditEmployee.Id);
            if (oldEmployee is null)
            {
                throw new ApplicationInconsistencyException($"Employee {managerEditEmployee.Id} not found");
            }

            if (oldEmployee.SupermarketId.HasValue is false)
            {
                throw new ApplicationInconsistencyException("Edited employee by manager must have supermarket id");
            }
            
            var password = managerEditEmployee.NewPassword is null 
                ? oldEmployee.PasswordHash
                : PasswordHashing.GenerateSaltedHash(managerEditEmployee.NewPassword, oldEmployee.PasswordHashSalt);
            
            await _employeeRepository.UpdateAsync(new EmployeeRole
            {
                Id = managerEditEmployee.Id,
                Login = managerEditEmployee.Login,
                Name = managerEditEmployee.Name,
                Surname = managerEditEmployee.Surname,
                HireDate = managerEditEmployee.HireDate,
                RoleInfo = new SupermarketEmployee(oldEmployee.SupermarketId.Value, managerEditEmployee.ManagerId, managerEditEmployee.Roles),
                PasswordHash = password,
                PasswordHashSalt = oldEmployee.PasswordHashSalt
            });
        }

        public async Task<PagedResult<PossibleManagerForEmployee>> GetPossibleManagers(int employeeId, int supermarketId, RecordsRange recordsRange)
        {
            var employee = await _employeeRepository.GetRoleByIdAsync(employeeId);
            if (employee is null)
            {
                throw new ApplicationInconsistencyException($"Employee {employeeId} was not found");
            }
            
            if (employee.RoleInfo is Domain.Employees.Roles.Admin)
            {
                return await _employeeRepository.GetPossibleManagersForAdmin(supermarketId, recordsRange);
            }
            return await _employeeRepository.GetPossibleManagersForManager(employeeId, recordsRange);
        }

        public async Task DeleteEmployee(int employeeId)
        {
            try
            {
                await _employeeRepository.DeleteAsync(employeeId);
            }
            catch (RepositoryOperationFailedException e)
            {
                throw new OperationCannotBeExecutedException(e);
            }
        }
        #endregion

        #region StoragePlaces
        public async Task<PagedResult<StoragePlace>> GetStoragePlaces(int supermarketId, RecordsRange recordsRange)
        {
            return await _storagePlaceRepository.GetSupermarketStoragePlaces(supermarketId, recordsRange);
        }

        public async Task<StoragePlace> GetStorageToEdit(int storageId)
        {
            var storagePlace = await _storagePlaceRepository.GetByIdAsync(storageId);
            if (storagePlace == null)
            {
                throw new ApplicationInconsistencyException("Storage place was not found");
            }

            return storagePlace;
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
            try
            {
                await _storagePlaceRepository.DeleteAsync(id);
            }
            catch (RepositoryOperationFailedException e)
            {
                throw new OperationCannotBeExecutedException(e);
            }
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

