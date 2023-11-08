using Dapper;
using Oracle.ManagedDataAccess.Client;
using Supermarket.Domain.Common;
using Supermarket.Domain.Common.Paging;

namespace Supermarket.Infrastructure.Common
{
    internal abstract class CrudRepositoryBase<TEntity, TId, TDbEntity>
        where TEntity : class, IEntity<TId>
        where TDbEntity : IDbEntity<TEntity, TId, TDbEntity>
    {
        protected readonly OracleConnection _oracleConnection;
        
        protected CrudRepositoryBase(OracleConnection oracleConnection)
        {
            _oracleConnection = oracleConnection;
        }

        public async Task<PagedResult<TEntity>> GetPagedAsync(RecordsRange recordsRange)
        {
            var sql = $"SELECT * FROM {TDbEntity.TableName}";
            var pagedItems =
                await GetPagedResult<TDbEntity>(recordsRange, sql, TDbEntity.IdentityColumns, new DynamicParameters());
            
            return pagedItems.Select(i => i.ToDomainEntity());
        }

        protected async Task<PagedResult<TResult>> GetPagedResult<TResult>(
            RecordsRange recordsRange,
            string innerSql,
            IEnumerable<string> orderByColumns,
            DynamicParameters parameters)
        {
            var orderBy = string.Join(", ", orderByColumns);
            var pagingSql = $"{innerSql} ORDER BY {orderBy} OFFSET :PagingOffset ROWS FETCH NEXT :PagingRowsCount ROWS ONLY";

            var pagingParameters = GetPagingParameters(recordsRange);
            pagingParameters.AddDynamicParams(parameters);
            
            var pagedItems = await _oracleConnection.QueryAsync<TResult>(pagingSql, pagingParameters);
            
            var totalCountSql = $"SELECT COUNT(*) FROM ({innerSql})";
            var totalCount = await _oracleConnection.ExecuteScalarAsync<int>(totalCountSql, pagingParameters);

            return new PagedResult<TResult>(pagedItems.ToArray(), recordsRange.PageNumber, recordsRange.PageSize, totalCount);
        }

        public virtual async Task<TEntity?> GetByIdAsync(TId id)
        {
            var identity = TDbEntity.GetEntityIdParameters(id);
            var identityCondition = GetIdentityCondition(identity);
            
            var sql = $"SELECT * FROM {TDbEntity.TableName} WHERE {identityCondition}";
            var dbEntity = await _oracleConnection.QuerySingleOrDefaultAsync<TDbEntity>(sql, identity);

            return dbEntity?.ToDomainEntity();
        }
        
        public virtual async Task AddAsync(TEntity entity)
        {
            var dbEntity = TDbEntity.MapToDbEntity(entity);
            var insertingValues = dbEntity.GetInsertingValues();
            
            var selector = string.Join(", ", insertingValues.ParameterNames);
            var parameters = string.Join(", ", insertingValues.ParameterNames.Select(v => ":" + v));
            
            var sql = $"INSERT INTO {TDbEntity.TableName} ({selector}) VALUES ({parameters})";

            await _oracleConnection.ExecuteAsync(sql, insertingValues);
        }

        public virtual async Task UpdateAsync(TEntity entity)
        {
            var dbEntity = TDbEntity.MapToDbEntity(entity);
            var identity = TDbEntity.GetEntityIdParameters(entity.Id);
            var insertingValues = dbEntity.GetInsertingValues();

            var updater = string.Join(", ", insertingValues.ParameterNames.Select(p => $"{p} = :{p}"));
            var identityCondition = GetIdentityCondition(identity);

            var sql = $"UPDATE {TDbEntity.TableName} SET {updater} WHERE {identityCondition}";
            
            insertingValues.AddDynamicParams(identityCondition);
            await _oracleConnection.ExecuteAsync(sql, insertingValues);
        }

        public virtual async Task DeleteAsync(TId id)
        {
            var identity = TDbEntity.GetEntityIdParameters(id);
            var identityCondition = GetIdentityCondition(identity);

            var sql = $"DELETE FROM {TDbEntity.TableName} WHERE {identityCondition}";
            await _oracleConnection.ExecuteAsync(sql, identity);
        }

        private static DynamicParameters GetPagingParameters(RecordsRange recordsRange)
        {
            var startRow = (recordsRange.PageNumber - 1) * recordsRange.PageSize;
            var rowsCount = recordsRange.PageSize;

            return new DynamicParameters(new
            {
                PagingOffset = startRow,
                PagingRowsCount = rowsCount
            });
        }

        protected string GetIdentityCondition(DynamicParameters identity)
        {
            return string.Join(" AND ", TDbEntity.IdentityColumns
                .Select(ic => $"{TDbEntity.TableName}.{ic} = :{ic}"));
        }
    }
}
