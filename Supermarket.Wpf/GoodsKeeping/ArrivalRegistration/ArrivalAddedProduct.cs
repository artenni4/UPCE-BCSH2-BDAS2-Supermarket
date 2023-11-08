using Supermarket.Domain.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Supermarket.Wpf.GoodsKeeping.ArrivalRegistration
{
    public class ArrivalAddedProduct
    {
        public required int ProductId { get; init; }
        public required string Name { get; init; }
        public required string MeasureUnit { get; init; }
        public decimal Weight { get; init; }
        public required decimal Price { get; init; }
    }
}
