using System.Data;
using Dapper;

namespace Supermarket.Infrastructure.Common;

public static class DynamicParametersExtensions
{
    public static DynamicParameters AddParameter(this DynamicParameters dynamicParameters, string name,
        object? parameter)
    {
        dynamicParameters.Add(name, parameter);
        return dynamicParameters;
    }
}