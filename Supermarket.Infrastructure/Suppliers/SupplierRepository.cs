using Oracle.ManagedDataAccess.Client;
using Supermarket.Core.Domain.Suppliers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Supermarket.Infrastructure.Suppliers
{
    internal class SupplierRepository : CrudRepositoryBase<Supplier, int, DbSupplier>, ISupplierRepository
    {
        public SupplierRepository(OracleConnection oracleConnection) : base(oracleConnection)
        {
        }

    }
}
