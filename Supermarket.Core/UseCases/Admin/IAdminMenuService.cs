using Supermarket.Core.Domain.Common.Paging;
using Supermarket.Core.Domain.Regions;
using Supermarket.Core.Domain.Suppliers;

namespace Supermarket.Core.UseCases.Admin
{
    public interface IAdminMenuService
    {
        Task<PagedResult<Supplier>> GetAllSuppliers(RecordsRange recordsRange);
        Task<Supplier?> GetSupplier(int supplierId);
        Task AddSupplier(Supplier supplier);
        Task EditSupplier(Supplier supplier);
        Task DeleteSupplier(int supplierId);
        Task<PagedResult<AdminSupermarket>> GetAllSupermarkets(RecordsRange recordsRange);
        Task<Domain.Supermarkets.Supermarket?> GetSupermarket(int supermarketId);
        Task AddSupermarket(Domain.Supermarkets.Supermarket supermarket);
        Task EditSupermarket(Domain.Supermarkets.Supermarket supermarket);
        Task DeleteSupermarket(int supermarketId);


        Task<PagedResult<Region>> GetAllRegions(RecordsRange recordsRange);
        Task<Region?> GetRegion(int id);
    }
}
