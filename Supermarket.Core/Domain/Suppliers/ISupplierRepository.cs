using Supermarket.Core.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Supermarket.Core.Domain.Suppliers
{
    public interface ISupplierRepository : ICrudRepository<Supplier, int>
    {

    }
}
