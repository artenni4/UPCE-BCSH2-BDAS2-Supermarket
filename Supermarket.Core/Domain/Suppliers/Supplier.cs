using Supermarket.Core.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Supermarket.Core.Domain.Suppliers
{
    public class Supplier : IEntity<int>
    {
        public required int Id { get; set; }
        public required string Name { get; set; }
    }
}
