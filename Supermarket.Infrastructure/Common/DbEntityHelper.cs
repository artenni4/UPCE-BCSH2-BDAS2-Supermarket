using System.Collections.Immutable;
using System.Reflection;
using Dapper;
using Supermarket.Core.Domain.Common;

namespace Supermarket.Infrastructure.Common;

public static class DbEntityHelper
{
    public static DynamicParameters GetAllProperties<TEntity, TId, TSelf>(this IDbEntity<TEntity, TId, TSelf> dbEntity)
        where TEntity : IEntity<TId>
        where TSelf : IDbEntity<TEntity, TId, TSelf>
    {
        return GetPropertiesExcept(dbEntity, ImmutableHashSet<string>.Empty);
    }

    public static DynamicParameters GetPropertiesExceptIdentity<TEntity, TId, TSelf>(this IDbEntity<TEntity, TId, TSelf> dbEntity)
        where TEntity : IEntity<TId>
        where TSelf : IDbEntity<TEntity, TId, TSelf>
    {
        return GetPropertiesExcept(dbEntity, TSelf.IdentityColumns);
    }

    public static DynamicParameters GetPropertiesExcept(object obj, IReadOnlySet<string> propertiesName)
    {
        var parameters = new DynamicParameters();
        foreach (var property in obj.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public))
        {
            if (propertiesName.Contains(property.Name))
            {
                continue;
            }
            
            parameters.Add(property.Name, property.GetValue(obj));
        }

        return parameters;
    }
}