using Supermarket.Core.Common;
using Supermarket.Core.Common.Paging;

namespace Supermarket.Infrastructure.Common
{
    public abstract class CrudRepositoryBase<TEntity, TId, TQueryObject> : ICrudRepository<TEntity, TId, TQueryObject>
        where TEntity : IEntity<TId>
        where TQueryObject : IQueryObject
    {
        // TODO add basic CRUD operations for entities

        public abstract Task<PagedResult<TEntity>> GetPagedAsync(TQueryObject queryObject);

        public Task<PagedResult<TEntity>> GetRecordsRangeAsync(RecordsRange queryObject)
        {
            throw new NotImplementedException();
        }

        public virtual Task<TEntity?> GetByIdAsync(TId id)
        {
            throw new NotImplementedException();
        }

        public virtual Task<TId> AddAsync(TEntity entity)
        {
            throw new NotImplementedException();
        }

        public virtual Task UpdateAsync(TEntity entity)
        {
            throw new NotImplementedException();
        }

        public virtual Task DeleteAsync(TId id)
        {
            throw new NotImplementedException();
        }
    }
}
