using Supermarket.Core.Common;
using Supermarket.Core.Common.Paging;

namespace Supermarket.Infrastructure.Common
{
    public abstract class CrudRepositoryBase<TEntity, TId, TQueryObject> : ICrudRepository<TEntity, TId, TQueryObject>
        where TEntity : IEntity<TId>
        where TQueryObject : IQueryObject
    {
        // TODO add basic CRUD operations for entities

        public Task<PagedResult<TEntity>> GetPagedAsync(TQueryObject queryObject)
        {
            throw new NotImplementedException();
        }

        public Task<TEntity?> GetByIdAsync(TId id)
        {
            throw new NotImplementedException();
        }

        public Task<TId> AddAsync(TEntity entity)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(TEntity entity)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(TId id)
        {
            throw new NotImplementedException();
        }
    }
}
