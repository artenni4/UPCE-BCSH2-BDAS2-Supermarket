using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Supermarket.Core.UseCases.ManagerMenu
{
    public class ManagerMenuProduct
    {
        public required int ProductId { get; init; }
        public required string ProductName { get; init; }
        public required decimal Count { get; init; }
        public required int SupplierId { get; init; }
        public required string SupplierName { get; init; }
        public required decimal Price { get; init; }
    }
}
