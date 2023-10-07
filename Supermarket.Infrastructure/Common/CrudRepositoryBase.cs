using Oracle.ManagedDataAccess.Client;
using Supermarket.Domain.Common;
using Supermarket.Domain.Common.Paging;

namespace Supermarket.Infrastructure.Common
{
    internal abstract class CrudRepositoryBase<TEntity, TId>
        where TEntity : IEntity<TId>
    {
        protected readonly OracleConnection _oracleConnection;

        protected CrudRepositoryBase(OracleConnection oracleConnection)
        {
            _oracleConnection = oracleConnection;
        }

        protected Task<PagedResult<TEntity>> GetRecordsRangeAsync(RecordsRange queryObject)
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
