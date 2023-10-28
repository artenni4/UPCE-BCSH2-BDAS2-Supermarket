﻿using Dapper;
using Oracle.ManagedDataAccess.Client;
using Supermarket.Domain.Common;
using Supermarket.Domain.Products;
using Supermarket.Infrastructure.Common;

namespace Supermarket.Infrastructure.Products;

internal class ProductRepository : CrudRepositoryBase<Product, int, DbProduct>, IProductRepository
{
    public ProductRepository(OracleConnection oracleConnection) : base(oracleConnection)
    {
    }
}