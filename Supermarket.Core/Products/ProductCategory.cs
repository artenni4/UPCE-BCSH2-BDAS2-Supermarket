using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Supermarket.Core.Products
{
    public class ProductCategory : IEntity<int>
    {
        public required int Id { get; init; }
        public required string Name { get; init; }
        public required string? Description { get; init; }
    }
}
