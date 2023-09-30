using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Supermarket.Core.Common.Paging;

namespace Supermarket.Core.Common
{
    /// <summary>
    /// Represents repository with all common CRUD methods
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    /// <typeparam name="TId"></typeparam>
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
