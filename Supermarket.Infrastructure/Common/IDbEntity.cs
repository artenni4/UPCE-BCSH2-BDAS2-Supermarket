using Dapper;
using Supermarket.Domain.Common;

namespace Supermarket.Infrastructure.Common;

/// <summary>
/// Represents record in database table
/// </summary>
/// <typeparam name="TEntity">domain entity</typeparam>
/// <typeparam name="TId">id of domain entity</typeparam>
/// <typeparam name="TSelf">type of the implementation</typeparam>
public interface IDbEntity<TEntity, TId, TSelf> 
    where TEntity : IEntity<TId>
    where TSelf : IDbEntity<TEntity, TId, TSelf>
{
    /// <summary>
    /// Name of the SQL table
    /// </summary>
    static abstract string TableName { get; }
    
    /// <summary>
    /// Primary keys in the table.
    /// Contains names of properties in the type
    /// </summary>
    static abstract IReadOnlyList<string> IdentityColumns { get; }
    
    /// <summary>
    /// Maps instance of db entity to domain entity
    /// </summary>
    TEntity ToDomainEntity();

    /// <summary>
    /// Maps domain entity to db entity
    /// </summary>
    static abstract TSelf MapToDbEntity(TEntity entity);

    /// <summary>
    /// Gets parameter list of identity values
    /// </summary>
    /// <param name="id">id of entity</param>
    static abstract DynamicParameters GetEntityIdParameters(TId id);

    /// <summary>
    /// Gets identity parameters for returning inserted id into it
    /// </summary>
    static abstract DynamicParameters GetOutputIdentityParameters();
    
    /// <summary>
    /// Extract <see cref="TId"/> from <see cref="DynamicParameters"/>
    /// </summary>
    static abstract TId ExtractIdentity(DynamicParameters dynamicParameters);
    
    /// <summary>
    /// Gets insert values selector and parameters
    /// </summary>
    DynamicParameters GetInsertingValues() => new(this);
}