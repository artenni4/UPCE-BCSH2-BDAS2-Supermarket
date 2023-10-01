using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Supermarket.Core.Products
{
    public interface IProductRepository : ICrudRepository<Product, int, ProductQueryObject>
    {
    }
}
