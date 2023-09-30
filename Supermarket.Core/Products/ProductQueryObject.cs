using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Supermarket.Core.Products
{
    public class ProductQueryObject : IQueryObject
    {
        public required RecordsRange RecordsRange { get; init; }
        public required int? SupermarketId { get; init; }
        public required int? ProductCategoryId { get; init; }
    }
}
