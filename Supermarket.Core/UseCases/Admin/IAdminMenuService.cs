using Supermarket.Core.Domain.Common.Paging;
using Supermarket.Core.Domain.Suppliers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Supermarket.Core.UseCases.Admin
{
    public interface IAdminMenuService
    {
        Task<PagedResult<Supplier>> GetAllSuppliers(RecordsRange recordsRange);
        Task<Supplier?> GetSupplier(int supplierId);
        Task AddSupplier(Supplier supplier);
        Task EditSupplier(Supplier supplier);
        Task DeleteSupplier(int supplierId);
    }
}
