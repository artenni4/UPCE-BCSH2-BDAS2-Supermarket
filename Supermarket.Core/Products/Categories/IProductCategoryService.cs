using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Supermarket.Core.Products.Categories
{
    public interface IProductCategoryService
    {
        Task<PagedResult<ProductCategory>> GetAllCategoriesAsync(RecordsRange recordsRange);
    }
}
