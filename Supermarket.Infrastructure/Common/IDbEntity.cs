using Supermarket.Domain.Common;

namespace Supermarket.Infrastructure.Common;

public interface IDbEntity<TEntity, TId> where TEntity : IEntity<TId>
{ 
    /// <summary>
    /// Maps instance of db entity to domain entity
    /// </summary>
    TEntity ToDomainEntity();

    static abstract  FromDomainEntity(TEntity entity);
}