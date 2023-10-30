using Dapper;
using Oracle.ManagedDataAccess.Client;
using Supermarket.Domain.Common.Paging;
using Supermarket.Domain.Products.Categories;
using Supermarket.Infrastructure.Common;

namespace Supermarket.Infrastructure.Products.Categories
{
    internal class ProductCategoryRepository : CrudRepositoryBase<ProductCategory, int, ProductCategoryRepository.DbProductCategory, PagingQueryObject>, IProductCategoryRepository
    {
        public ProductCategoryRepository(OracleConnection oracleConnection) : base(oracleConnection)
        {
        }

        public class DbProductCategory
        {
            public required int druh_zbozi_id { get; init; }
            public required string nazev { get; init; }
            public string? popis { get; init; }
        }

        protected override string TableName => "DRUHY_ZBOZI";

        protected override IReadOnlyList<string> IdentityColumns { get; } =
            new[] { nameof(DbProductCategory.druh_zbozi_id) };

        protected override ProductCategory MapToEntity(DbProductCategory dbEntity) => new()
        {
            Id = dbEntity.druh_zbozi_id,
            Name = dbEntity.nazev,
            Description = dbEntity.popis
        };

        protected override DbProductCategory MapToDbEntity(ProductCategory entity) => new()
        {
            druh_zbozi_id = entity.Id,
            nazev = entity.Name,
            popis = entity.Description
        };

        protected override DynamicParameters GetIdentityValues(int id) => GetSimpleIdentityValue(id);

        protected override int ExtractIdentity(DynamicParameters dynamicParameters) =>
            ExtractSimpleIdentity(dynamicParameters);
    }
}
