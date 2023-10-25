using System.Text;
using Dapper;
using Oracle.ManagedDataAccess.Client;
using Supermarket.Domain.Common;
using Supermarket.Domain.Common.Paging;
using Supermarket.Domain.Products;
using Supermarket.Infrastructure.Common;

namespace Supermarket.Infrastructure.Products
{
    internal class ProductRepository : CrudRepositoryBase<Product, int, ProductRepository.DbProduct, PagingQueryObject>, IProductRepository
    {
        public ProductRepository(OracleConnection oracleConnection) : base(oracleConnection)
        {
        }
        
        public class DbProduct
        {
            public required int zbozi_id { get; init; }
            public required int druh_zbozi_id { get; init; }
            public required int merna_jednotka_id { get; init; }
            public required int naVahu { get; init; }
            public required string nazev { get; init; }
            public required decimal cena { get; init; }
            public required decimal hmotnost { get; init; }
            public required string? carovyKod { get; init; }
            public required string? popis { get; init; }
        }

        protected override string TableName => "ZBOZI";
        protected override IReadOnlyList<string> IdentityColumns { get; } = new[] { nameof(DbProduct.zbozi_id) };

        protected override Product MapToEntity(DbProduct dbEntity) => new()
        {
            Id = dbEntity.zbozi_id,
            ProductCategoryId = dbEntity.druh_zbozi_id,
            MeasureUnit = dbEntity.merna_jednotka_id switch
            {
                1 => MeasureUnit.Gram,
                2 => MeasureUnit.Kilogram,
                3 => MeasureUnit.Litre,
                _ => throw new DatabaseException($"Measure unit [{dbEntity.merna_jednotka_id}] is not known")
            },
            ByWeight = dbEntity.naVahu != 0,
            Name = dbEntity.nazev,
            Price = dbEntity.cena,
            Weight = dbEntity.hmotnost,
            Barcode = dbEntity.carovyKod,
            Description = dbEntity.popis
        };

        protected override DbProduct MapToDbEntity(Product entity) => new()
        {
            zbozi_id = entity.Id,
            druh_zbozi_id = entity.ProductCategoryId,
            merna_jednotka_id = GetMeasureUnitId(entity.MeasureUnit),
            carovyKod = entity.Barcode,
            cena = entity.Price,
            hmotnost = entity.Weight,
            naVahu = entity.ByWeight ? 1 : 0,
            nazev = entity.Name,
            popis = entity.Description
        };

        private static int GetMeasureUnitId(MeasureUnit measureUnit)
        {
            if (measureUnit == MeasureUnit.Gram)
            {
                return 1;
            }
            
            if (measureUnit == MeasureUnit.Kilogram)
            {
                return 2;
            }

            if (measureUnit == MeasureUnit.Litre)
            {
                return 3;
            }

            throw new DatabaseException($"Mapping for measure unit [{measureUnit}] is not implemented");
        }

        protected override DynamicParameters GetIdentityValues(int id) => GetSimpleIdentityValue(id);

        protected override int ExtractIdentity(DynamicParameters dynamicParameters) =>
            ExtractSimpleIdentity(dynamicParameters);
    }
}
