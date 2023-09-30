using Supermarket.Core.Common;

namespace Supermarket.Infrastructure.Common
{
    internal abstract class CrudRepositoryBase<TEntity, TId> where TEntity : IEntity<TId>
    {
        // TODO add basic CRUD operations for entities

        protected Task<TEntity> GetByIdAsync(TId id)
        {
            return null!;
        }

        protected Task<TId> AddAsync(TEntity entity)
        {
            return null!;
        }

        protected Task UpdateAsync(TEntity entity)
        {
            return null!;
        }

        protected Task DeleteAsync(TId id)
        {
            return null!;
        }
    }
}
