﻿using Supermarket.Core.Domain.Products;

namespace Supermarket.Core.UseCases.GoodsKeeping;

public class GoodsKeepingProduct
{
    public required int ProductId { get; init; }
    public required string Name { get; init; }
    public required bool IsByWeight { get; init; }
    public required string MeasureUnit { get; init; }
    public required decimal Weight { get; init; }
    public required decimal Price { get; init; }

    public static GoodsKeepingProduct FromProduct(Product product) => new()
    {
        ProductId = product.Id,
        Name = product.Name,
        IsByWeight = product.ByWeight,
        MeasureUnit = product.MeasureUnit.Abbreviation,
        Weight = product.Weight,
        Price = product.Price
    };
}