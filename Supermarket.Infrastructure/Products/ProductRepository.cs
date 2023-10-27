using Dapper;
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

    protected override string TableName => "ZBOZI";
    protected override IReadOnlyList<string> IdentityColumns { get; } = new[] { nameof(DbProduct.zbozi_id) };

    protected override Product MapToEntity(DbProduct dbEntity) => 

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
        if (measureUnit == MeasureUnit.Kilogram)
        {
            return 1;
        }
            
        if (measureUnit == MeasureUnit.Gram)
        {
            return 2;
        }

        if (measureUnit == MeasureUnit.Litre)
        {
            return 3;
        }
            
        if (measureUnit == MeasureUnit.Millilitre)
        {
            return 4;
        }
            
        if (measureUnit == MeasureUnit.Piece)
        {
            return 5;
        }

        if (measureUnit == MeasureUnit.Meter)
        {
            return 6;
        }

        throw new DatabaseException($"Mapping for measure unit [{measureUnit}] is not implemented");
    }

    protected override DynamicParameters GetIdentityValues(int id) => GetSimpleIdentityValue(id);

    protected override int ExtractIdentity(DynamicParameters dynamicParameters) =>
        ExtractSimpleIdentity(dynamicParameters);
}