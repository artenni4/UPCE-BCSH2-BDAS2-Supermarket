﻿using Dapper;
using Supermarket.Domain.Common;
using Supermarket.Domain.Products;
using Supermarket.Infrastructure.Common;

namespace Supermarket.Infrastructure.Products;

internal class DbProduct : IDbEntity<Product, int, DbProduct>
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

    public static string TableName => "ZBOZI";

    public static IReadOnlyList<string> IdentityColumns { get; } = new[]
    {
        nameof(zbozi_id)
    };

    public Product ToDomainEntity() => new()
    {
        Id = zbozi_id,
        ProductCategoryId = druh_zbozi_id,
        MeasureUnit = merna_jednotka_id switch
        {
            1 => MeasureUnit.Kilogram,
            2 => MeasureUnit.Gram,
            3 => MeasureUnit.Litre,
            4 => MeasureUnit.Millilitre,
            5 => MeasureUnit.Piece,
            6 => MeasureUnit.Meter,
            _ => throw new DatabaseException($"Measure unit [{merna_jednotka_id}] is not known")
        },
        ByWeight = naVahu != 0,
        Name = nazev,
        Price = cena,
        Weight = hmotnost,
        Barcode = carovyKod,
        Description = popis
    };

    public static DbProduct MapToDbEntity(Product entity) => new()
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

    public static DynamicParameters GetEntityIdParameters(int id) =>
        new DynamicParameters().AddParameter(nameof(zbozi_id), id);

    public static DynamicParameters GetOutputIdentityParameters() =>
        new DynamicParameters().AddOutputParameter(nameof(zbozi_id));

    public static int ExtractIdentity(DynamicParameters dynamicParameters) =>
        dynamicParameters.Get<int>(nameof(zbozi_id));

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
}