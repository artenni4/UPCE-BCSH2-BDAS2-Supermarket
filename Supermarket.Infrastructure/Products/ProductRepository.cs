﻿using Oracle.ManagedDataAccess.Client;
using Supermarket.Core.Common.Paging;
using Supermarket.Core.Products;
using Supermarket.Infrastructure.Common;

namespace Supermarket.Infrastructure.Products
{
    public class ProductRepository : CrudRepositoryBase<Product, int>, IProductRepository
    {
        public ProductRepository(OracleConnection oracleConnection) : base(oracleConnection)
        {
        }

        public Task<PagedResult<Product>> GetPagedAsync(ProductQueryObject queryObject)
        {
            throw new NotImplementedException();
        }
    }
}
