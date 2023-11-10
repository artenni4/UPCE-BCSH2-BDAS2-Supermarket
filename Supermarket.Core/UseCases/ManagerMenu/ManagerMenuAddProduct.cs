using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Supermarket.Core.UseCases.ManagerMenu
{
    public class ManagerMenuAddProduct
    {
        public required int ProductId { get; init; }
        public required string ProductName { get; init; }
        public required bool IsInSupermarket { get; init; }
        public required int SupplierId { get; init; }
        public required string SupplierName { get; init; }
        public required decimal Price { get; init; }
    }
}
