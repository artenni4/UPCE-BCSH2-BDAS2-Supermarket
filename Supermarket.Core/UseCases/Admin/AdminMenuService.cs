using Supermarket.Core.Domain.Common;
using Supermarket.Core.Domain.Common.Paging;
using Supermarket.Core.Domain.Regions;
using Supermarket.Core.Domain.Supermarkets;
using Supermarket.Core.Domain.Suppliers;

namespace Supermarket.Core.UseCases.Admin
{
    public class AdminMenuService : IAdminMenuService
    {
        private readonly ISupplierRepository _supplierRepository;
        private readonly ISupermarketRepository _supermarketRepository;
        private readonly IRegionRepository _regionRepository;

        public AdminMenuService(ISupplierRepository supplierRepository, ISupermarketRepository supermarketRepository, IRegionRepository regionRepository)
        {
            _supplierRepository = supplierRepository;
            _supermarketRepository = supermarketRepository;
            _regionRepository = regionRepository;
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
            try
            {
                await _supplierRepository.DeleteAsync(supplierId);
            }
            catch (RepositoryOperationFailedException e)
            {
                throw new OperationCannotBeExecutedException(e);
            }
        }
        #endregion


        #region Supermarkets
        public async Task<PagedResult<AdminSupermarket>> GetAllSupermarkets(RecordsRange recordsRange)
        {
            return await _supermarketRepository.GetAdminSupermarkets(recordsRange);
        }

        public async Task<Domain.Supermarkets.Supermarket?> GetSupermarket(int supermarketId)
        {
            return await _supermarketRepository.GetByIdAsync(supermarketId);
        }

        public async Task AddSupermarket(Domain.Supermarkets.Supermarket supermarket)
        {
            await _supermarketRepository.AddAsync(supermarket);
        }

        public async Task EditSupermarket(Domain.Supermarkets.Supermarket supermarket)
        {
            await _supermarketRepository.UpdateAsync(supermarket);
        }

        public async Task DeleteSupermarket(int supermarketId)
        {
            try
            {
                await _supermarketRepository.DeleteAsync(supermarketId);
            }
            catch (RepositoryOperationFailedException e)
            {
                throw new OperationCannotBeExecutedException(e);
            }
        }
        #endregion


        public async Task<PagedResult<Region>> GetAllRegions(RecordsRange recordsRange)
        {
            return await _regionRepository.GetPagedAsync(recordsRange);
        }

        public async Task<Region?> GetRegion(int id)
        {
            return await _regionRepository.GetByIdAsync(id);
        }
    }
}
