using Supermarket.Core.Domain.Auth;
using Supermarket.Core.Domain.ChangeLogs;
using Supermarket.Core.Domain.Common;
using Supermarket.Core.Domain.Common.Paging;
using Supermarket.Core.Domain.Employees;
using Supermarket.Core.Domain.Employees.Roles;
using Supermarket.Core.Domain.ProductCategories;
using Supermarket.Core.Domain.Products;
using Supermarket.Core.Domain.Regions;
using Supermarket.Core.Domain.Supermarkets;
using Supermarket.Core.Domain.Suppliers;
using Supermarket.Core.Domain.UsedDatabaseObjects;
using Supermarket.Core.UseCases.Common;
using Supermarket.Core.UseCases.ManagerMenu;

namespace Supermarket.Core.UseCases.Admin
{
    internal class AdminMenuService : IAdminMenuService
    {
        private readonly ISupplierRepository _supplierRepository;
        private readonly ISupermarketRepository _supermarketRepository;
        private readonly IRegionRepository _regionRepository;
        private readonly IProductCategoryRepository _productCategoryRepository;
        private readonly IProductRepository _productRepository;
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IChangeLogRepository _changeLogRepository;
        private readonly IUsedDatabaseObjectRepository _usedDatabaseObjectRepository;
        private readonly IUnitOfWork _unitOfWork;

        public AdminMenuService(ISupplierRepository supplierRepository,
            ISupermarketRepository supermarketRepository,
            IRegionRepository regionRepository,
            IProductCategoryRepository productCategoryRepository,
            IProductRepository productRepository,
            IEmployeeRepository employeeRepository,
            IChangeLogRepository changeLogRepository,
            IUsedDatabaseObjectRepository usedDatabaseObjectRepository,
            IUnitOfWork unitOfWork)
        {
            _supplierRepository = supplierRepository;
            _supermarketRepository = supermarketRepository;
            _regionRepository = regionRepository;
            _productCategoryRepository = productCategoryRepository;
            _productRepository = productRepository;
            _employeeRepository = employeeRepository;
            _changeLogRepository = changeLogRepository;
            _usedDatabaseObjectRepository = usedDatabaseObjectRepository;
            _unitOfWork = unitOfWork;
        }

        #region Suppliers
        public async Task<PagedResult<Supplier>> GetAllSuppliers(RecordsRange recordsRange)
        {
            return await _supplierRepository.GetPagedAsync(recordsRange);
        }

        public async Task<Supplier?> GetSupplier(int supplierId)
        {
            return await _supplierRepository.GetByIdAsync(supplierId);
        }

        public async Task AddSupplier(Supplier supplier)
        {
            await using var transaction = await _unitOfWork.BeginTransactionAsync();
            await _supplierRepository.AddAsync(supplier);
            await transaction.CommitAsync();
        }

        public async Task EditSupplier(Supplier supplier)
        {
            await using var transaction = await _unitOfWork.BeginTransactionAsync();
            await _supplierRepository.UpdateAsync(supplier);
            await transaction.CommitAsync();
        }

        public async Task DeleteSupplier(int supplierId)
        {
            try
            {
                await using var transaction = await _unitOfWork.BeginTransactionAsync();
                await _supplierRepository.DeleteAsync(supplierId);
                await transaction.CommitAsync();
            }
            catch (RepositoryOperationFailedException e)
            {
                throw new OperationCannotBeExecutedException(e);
            }
        }
        #endregion


        #region Supermarkets
        public async Task<PagedResult<AdminMenuSupermarket>> GetAllSupermarkets(RecordsRange recordsRange)
        {
            return await _supermarketRepository.GetAdminSupermarkets(recordsRange);
        }

        public async Task<Domain.Supermarkets.Supermarket?> GetSupermarket(int supermarketId)
        {
            return await _supermarketRepository.GetByIdAsync(supermarketId);
        }

        public async Task AddSupermarket(Domain.Supermarkets.Supermarket supermarket)
        {
            await using var transaction = await _unitOfWork.BeginTransactionAsync();
            await _supermarketRepository.AddAsync(supermarket);
            await transaction.CommitAsync();
        }

        public async Task EditSupermarket(Domain.Supermarkets.Supermarket supermarket)
        {
            await using var transaction = await _unitOfWork.BeginTransactionAsync();
            await _supermarketRepository.UpdateAsync(supermarket);
            await transaction.CommitAsync();
        }

        public async Task DeleteSupermarket(int supermarketId)
        {
            try
            {
                await using var transaction = await _unitOfWork.BeginTransactionAsync();
                await _supermarketRepository.DeleteAsync(supermarketId);
                await transaction.CommitAsync();
            }
            catch (RepositoryOperationFailedException e)
            {
                throw new OperationCannotBeExecutedException(e);
            }
        }
        #endregion

        #region Categories
        public async Task<PagedResult<ProductCategory>> GetAllCategories(RecordsRange recordsRange)
        {
            return await _productCategoryRepository.GetPagedAsync(recordsRange);
        }

        public async Task<ProductCategory?> GetCategory(int id)
        {
            return await _productCategoryRepository.GetByIdAsync(id);
        }

        public async Task AddCategory(ProductCategory category)
        {
            await using var transaction = await _unitOfWork.BeginTransactionAsync();
            await _productCategoryRepository.AddAsync(category);
            await transaction.CommitAsync();
        }

        public async Task EditCategory(ProductCategory category)
        {
            await using var transaction = await _unitOfWork.BeginTransactionAsync();
            await _productCategoryRepository.UpdateAsync(category);
            await transaction.CommitAsync();
        }

        public async Task DeleteCategory(int categoryId)
        {
            try
            {
                await using var transaction = await _unitOfWork.BeginTransactionAsync();
                await _productCategoryRepository.DeleteAsync(categoryId);
                await transaction.CommitAsync();
            }
            catch (RepositoryOperationFailedException e)
            {
                throw new OperationCannotBeExecutedException(e);
            }
        }
        #endregion

        #region Regions
        public async Task<PagedResult<Region>> GetAllRegions(RecordsRange recordsRange)
        {
            return await _regionRepository.GetPagedAsync(recordsRange);
        }

        public async Task<Region?> GetRegion(int id)
        {
            return await _regionRepository.GetByIdAsync(id);
        }

        public async Task AddRegion(Region region)
        {
            await using var transaction = await _unitOfWork.BeginTransactionAsync();
            await _regionRepository.AddAsync(region);
            await transaction.CommitAsync();
        }

        public async Task EditRegion(Region region)
        {
            await using var transaction = await _unitOfWork.BeginTransactionAsync();
            await _regionRepository.UpdateAsync(region);
            await transaction.CommitAsync();
        }

        public async Task DeleteRegion(int regionId)
        {
            try
            {
                await using var transaction = await _unitOfWork.BeginTransactionAsync();
                await _regionRepository.DeleteAsync(regionId);
                await transaction.CommitAsync();
            }
            catch (RepositoryOperationFailedException e)
            {
                throw new OperationCannotBeExecutedException(e);
            }
        }
        #endregion

        #region Products
        public async Task<PagedResult<AdminProduct>> GetAdminProducts(RecordsRange recordsRange)
        {
            return await _productRepository.GetAdminProducts(recordsRange);
        }

        public async Task<Product?> GetProduct(int productId)
        {
            return await _productRepository.GetByIdAsync(productId);
        }

        public async Task AddProduct(Product product)
        {
            await using var transaction = await _unitOfWork.BeginTransactionAsync();
            await _productRepository.AddAsync(product);
            await transaction.CommitAsync();
        }

        public async Task EditProduct(Product product)
        {
            await using var transaction = await _unitOfWork.BeginTransactionAsync();
            await _productRepository.UpdateAsync(product);
            await transaction.CommitAsync();
        }

        public async Task DeleteProduct(int productId)
        {
            try
            {
                await using var transaction = await _unitOfWork.BeginTransactionAsync();
                await _productRepository.DeleteAsync(productId);
                await transaction.CommitAsync();
            }
            catch (RepositoryOperationFailedException e)
            {
                throw new OperationCannotBeExecutedException(e);
            }
        }
        #endregion

        #region Employees
        public async Task<PagedResult<AdminEmployee>> GetAllEmployees(RecordsRange recordsRange)
        {
            return await _employeeRepository.GetAdminMenuEmployees(recordsRange);
        }

        public async Task DeleteEmployee(int employeeId)
        {
            try
            {
                await using var transaction = await _unitOfWork.BeginTransactionAsync();
                await _employeeRepository.DeleteAsync(employeeId);
                await transaction.CommitAsync();
            }
            catch (RepositoryOperationFailedException e)
            {
                throw new OperationCannotBeExecutedException(e);
            }
        }

        public async Task<PagedResult<PossibleManagerForEmployee>> GetPossibleManagers(int supermarketId, RecordsRange recordsRange)
        {
            return await _employeeRepository.GetPossibleManagersForAdmin(supermarketId, recordsRange);
        }

        public async Task<PagedResult<ChangeLog>> GetChangeLogs(RecordsRange recordsRange)
        {
            return await _changeLogRepository.GetChangeLogs(recordsRange);
        }

        public async Task<PagedResult<UsedDatabaseObject>> GetUsedDatabaseObjects(RecordsRange recordsRange)
        {
            return await _usedDatabaseObjectRepository.GetUsedDatabaseObjects(recordsRange);
        }

        public async Task<AdminEmployeeDetail> GetEmployeeToEdit(int employeeId)
        {
            var employee = await _employeeRepository.GetRoleByIdAsync(employeeId);
            if (employee == null)
            {
                throw new ApplicationInconsistencyException("Employee was not found");
            }

            return AdminEmployeeDetail.FromEmployeeRole(employee);
        }

        public async Task EditEmployee(AdminEditEmployee editEmployee)
        {
            var oldEmployee = await _employeeRepository.GetByIdAsync(editEmployee.Id);
            if (oldEmployee is null)
            {
                throw new ApplicationInconsistencyException($"Employee {editEmployee.Id} not found");
            }

            var password = editEmployee.NewPassword is null
                ? oldEmployee.PasswordHash
                : PasswordHashing.GenerateSaltedHash(editEmployee.NewPassword, oldEmployee.PasswordHashSalt);

            await using var transaction = await _unitOfWork.BeginTransactionAsync();
            await _employeeRepository.UpdateAsync(new EmployeeRole
            {
                Id = editEmployee.Id,
                Login = editEmployee.Login,
                Name = editEmployee.Name,
                Surname = editEmployee.Surname,
                HireDate = editEmployee.HireDate,
                RoleInfo = editEmployee.RoleInfo,
                PasswordHash = password,
                PasswordHashSalt = oldEmployee.PasswordHashSalt,
                PersonalNumber = oldEmployee.PersonalNumber
            });
            await transaction.CommitAsync();
        }

        public async Task AddEmployee(AdminAddEmployee employee)
        {
            var salt = PasswordHashing.GenerateSalt();
            var passwordHash = PasswordHashing.GenerateSaltedHash(employee.Password, salt);

            await using var transaction = await _unitOfWork.BeginTransactionAsync();
            await _employeeRepository.AddAsync(new EmployeeRole
            {
                Id = employee.Id,
                Login = employee.Login,
                Name = employee.Name,
                Surname = employee.Surname,
                HireDate = employee.HireDate,
                PersonalNumber = employee.PersonalNumber,
                RoleInfo = employee.RoleInfo,
                PasswordHash = passwordHash,
                PasswordHashSalt = salt
            });
            await transaction.CommitAsync();
        }

        #endregion

    }
}
