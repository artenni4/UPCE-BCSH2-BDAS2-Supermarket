using Supermarket.Core.Domain.Common.Paging;
using Supermarket.Core.Domain.SellingProducts;
using Supermarket.Core.Domain.StoredProducts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Supermarket.Core.UseCases.ManagerMenu
{
    public interface IManagerMenuService
    {
        Task<PagedResult<ManagerMenuProduct>> GetSupermarketProducts(int supermarketId, RecordsRange recordsRange);
        Task<PagedResult<ManagerMenuAddProduct>> GetManagerProductsToAdd(int supermarketId, RecordsRange recordsRange);
        void RemoveProductFromSupermarket(StoredProductId id);
        void AddProductToSupermarket(SellingProductId id);
        
    }
}
