using Supermarket.Core.Domain.Common.Paging;
using Supermarket.Core.Domain.Employees;
using Supermarket.Core.Domain.Products;
using Supermarket.Core.Domain.SellingProducts;
using Supermarket.Core.Domain.StoredProducts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Supermarket.Core.UseCases.ManagerMenu
{
    public class ManagerMenuService : IManagerMenuService
    {
        private readonly ISellingProductRepository _sellingProductRepository;
        private readonly IProductRepository _productRepository;
        private readonly IStoredProductRepository _storedProductRepository;
        private readonly IEmployeeRepository _employeeRepository;

        public ManagerMenuService(ISellingProductRepository sellingProductRepository, IStoredProductRepository storedProductRepository, IProductRepository productRepository, IEmployeeRepository employeeRepository)
        {
            _sellingProductRepository = sellingProductRepository;
            _productRepository = productRepository;
            _storedProductRepository = storedProductRepository;
            _employeeRepository = employeeRepository;
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
            await _storedProductRepository.DeleteAsync(id);
            await _sellingProductRepository.DeleteAsync(new SellingProductId { ProductId = id.ProductId, SupermarketId = id.SupermarketId });
        }

        public async Task AddProductToSupermarket(SellingProductId id)
        {
            await _sellingProductRepository.AddAsync(new SellingProduct { Id = id});
        }
        #endregion

        #region Employees
        public async Task<PagedResult<ManagerMenuEmployee>> GetManagerEmployees(int supermarketId, RecordsRange recordsRange)
        {
            return await _employeeRepository.GetSupermarketEmployees(supermarketId, recordsRange);
        }

        public async Task<Employee?> GetEmployeeToEdit(int employeeId)
        {
            return await _employeeRepository.GetByIdAsync(employeeId);
        }
        #endregion
    }
}
