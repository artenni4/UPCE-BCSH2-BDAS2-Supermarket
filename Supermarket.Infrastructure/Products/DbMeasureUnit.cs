using Supermarket.Core.Domain.Common;
using Supermarket.Core.Domain.Products;

namespace Supermarket.Infrastructure.Products;

public static class DbMeasureUnit
{
    public static MeasureUnit GetMeasureUnit(int measureUnitId) => measureUnitId switch
    {
        1 => MeasureUnit.Kilogram,
        2 => MeasureUnit.Gram,
        3 => MeasureUnit.Litre,
        4 => MeasureUnit.Millilitre,
        5 => MeasureUnit.Piece,
        6 => MeasureUnit.Meter,
        _ => throw new RepositoryInconsistencyException($"Measure unit [{measureUnitId}] is not known")
    };
    
    public static int GetMeasureUnitId(MeasureUnit measureUnit)
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

        throw new RepositoryInconsistencyException($"Mapping for measure unit [{measureUnit}] is not implemented");
    }
}