using Supermarket.Core.Domain.Common.Paging;
using Supermarket.Core.Domain.Suppliers;

namespace Supermarket.Core.UseCases.Admin
{
    public class AdminMenuService : IAdminMenuService
    {
        private readonly ISupplierRepository _supplierRepository;

        public AdminMenuService(ISupplierRepository supplierRepository)
        {
            _supplierRepository = supplierRepository;
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
            await _supplierRepository.AddAsync(supplier);
        }

        public async Task EditSupplier(Supplier supplier)
        {
            await _supplierRepository.UpdateAsync(supplier);
        }

        public async Task DeleteSupplier(int supplierId)
        {
            await _supplierRepository.DeleteAsync(supplierId);
        }

    }
    #endregion
}
