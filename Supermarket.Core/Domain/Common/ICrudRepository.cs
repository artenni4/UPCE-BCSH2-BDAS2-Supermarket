using Supermarket.Core.Domain.Common.Paging;

namespace Supermarket.Core.Domain.Common
{
    /// <summary>
    /// Represents repository with all common CRUD methods
    /// </summary>
    public interface ICrudRepository<TEntity, TId>
        where TEntity : IEntity<TId>
    {
        Task<PagedResult<TEntity>> GetPagedAsync(RecordsRange recordsRange);
        Task<TEntity?> GetByIdAsync(TId id);
        Task AddAsync(TEntity entity);
        Task UpdateAsync(TEntity entity);
        Task DeleteAsync(TId id);
    }
}
