using Supermarket.Core.Domain.Common.Paging;
using Supermarket.Core.Domain.SellingProducts;
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

        public ManagerMenuService(ISellingProductRepository sellingProductRepository)
        {
            _sellingProductRepository = sellingProductRepository;
        }

        public async Task<PagedResult<ManagerMenuProduct>> GetSupermarketProducts(int supermarketId, RecordsRange recordsRange)
        {
            return await _sellingProductRepository.GetAllSupermarketProducts(supermarketId, recordsRange);
        }
    }
}
