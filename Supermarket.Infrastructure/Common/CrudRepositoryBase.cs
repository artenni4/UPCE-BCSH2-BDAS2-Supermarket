using Oracle.ManagedDataAccess.Client;
using Supermarket.Core.Common;
using Supermarket.Core.Common.Paging;

namespace Supermarket.Infrastructure.Common
{
    public abstract class CrudRepositoryBase<TEntity, TId>
        where TEntity : IEntity<TId>
    {
        protected readonly OracleConnection _oracleConnection;

        protected CrudRepositoryBase(OracleConnection oracleConnection)
        {
            _oracleConnection = oracleConnection;
        }

        public virtual Task<PagedResult<TEntity>> GetRecordsRangeAsync(RecordsRange queryObject)
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
