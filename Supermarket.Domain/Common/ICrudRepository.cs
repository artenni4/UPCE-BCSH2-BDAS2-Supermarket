using Supermarket.Domain.Common.Paging;

namespace Supermarket.Domain.Common
{
    /// <summary>
    /// Represents repository with all common CRUD methods
    /// </summary>
    public interface ICrudRepository<TEntity, TId, TQueryObject>
        where TEntity : IEntity<TId>
        where TQueryObject : IQueryObject
    {
        Task<PagedResult<TEntity>> GetPagedAsync(TQueryObject queryObject);
        Task<TEntity?> GetByIdAsync(TId id);
        Task<TId> AddAsync(TEntity entity);
        Task UpdateAsync(TEntity entity);
        Task DeleteAsync(TId id);
    }
}
