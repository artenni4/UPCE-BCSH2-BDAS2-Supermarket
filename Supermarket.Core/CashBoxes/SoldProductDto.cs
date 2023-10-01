using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Supermarket.Core.CashBoxes
{
    public class SoldProductDto
    {
        public required int ProductId { get; init; }
        public required decimal Count { get; set; }
    }
}
