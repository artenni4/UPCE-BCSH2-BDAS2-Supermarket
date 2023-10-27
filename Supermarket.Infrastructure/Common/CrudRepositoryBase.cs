using Dapper;
using Oracle.ManagedDataAccess.Client;
using Supermarket.Domain.Common;
using Supermarket.Domain.Common.Paging;

namespace Supermarket.Infrastructure.Common
{
    internal abstract class CrudRepositoryBase<TEntity, TId, TDbEntity>
        where TEntity : IEntity<TId>
        where TDbEntity : IDbEntity<TEntity, TId, TDbEntity>
    {
        protected readonly OracleConnection _oracleConnection;
        
        protected CrudRepositoryBase(OracleConnection oracleConnection)
        {
            _oracleConnection = oracleConnection;
        }

        public async Task<PagedResult<TEntity>> GetPagedAsync(RecordsRange recordsRange)
        {
            var pagedItems = await GetPagedResult<TDbEntity>(recordsRange, $"{TDbEntity.TableName}.*");
            return pagedItems.Select(i => i.ToDomainEntity());
        }

        protected async Task<PagedResult<TResult>> GetPagedResult<TResult>(
            RecordsRange recordsRange,
            string selectColumns,
            string? otherClauses = null,
            DynamicParameters? parameters = null)
        {
            var orderByIdentity = string.Join(", ", TDbEntity.IdentityColumns);

            var pagingSql =
                $@"WITH NumberedResult AS 
                   (SELECT {selectColumns}, 
                       ROW_NUMBER() OVER (ORDER BY {TDbEntity.TableName}.{orderByIdentity}) AS RowNumber 
                       FROM {TDbEntity.TableName}
                       {otherClauses})
                   SELECT * FROM NumberedResult
                   WHERE RowNumber BETWEEN :StartRow AND :EndRow";

            var pagingParameters = GetPagingParameters(recordsRange);
            pagingParameters.AddDynamicParams(parameters);
            
            var pagedItems = await _oracleConnection.QueryAsync<TResult>(pagingSql, pagingParameters);
            
            var totalCountSql = $"SELECT COUNT(1) FROM {TDbEntity.TableName} {otherClauses}";
            var totalCount = await _oracleConnection.ExecuteScalarAsync<int>(totalCountSql, pagingParameters);

            return new PagedResult<TResult>(pagedItems.ToArray(), recordsRange.PageNumber, totalCount);
        }

        public virtual async Task<TEntity?> GetByIdAsync(TId id)
        {
            var identity = TDbEntity.GetEntityIdParameters(id);
            var identityCondition = GetIdentityCondition(identity);
            
            var sql = $"SELECT * FROM {TDbEntity.TableName} WHERE {identityCondition}";
            var dbEntity = await _oracleConnection.QuerySingleAsync<TDbEntity>(sql, identity);

            return dbEntity.ToDomainEntity();
        }
        
        public virtual async Task<TId> AddAsync(TEntity entity)
        {
            var dbEntity = TDbEntity.MapToDbEntity(entity);
            var identity = TDbEntity.GetOutputIdentityParameters();
            var insertingValues = dbEntity.GetInsertingValues();

            var returningIdentity = string.Join(", ", TDbEntity.IdentityColumns.Select(ic => $"{TDbEntity.TableName}.{ic}"));
            var returningInto = string.Join(", ", identity.ParameterNames.Select(p => $":{p}"));
            
            var selector = string.Join(", ", insertingValues.ParameterNames);
            var parameters = string.Join(", ", insertingValues.ParameterNames.Select(v => ":" + v));
            
            var sql = $"INSERT INTO {TDbEntity.TableName} ({selector}) VALUES {parameters} RETURNING {returningIdentity} INTO {returningInto}";

            insertingValues.AddDynamicParams(identity);
            await _oracleConnection.ExecuteAsync(sql, insertingValues);

            return TDbEntity.ExtractIdentity(insertingValues);
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

        protected static DynamicParameters GetPagingParameters(RecordsRange recordsRange)
        {
            var startRow = (recordsRange.PageNumber - 1) * recordsRange.PageSize + 1;
            var endRow = recordsRange.PageNumber * recordsRange.PageSize;

            return new DynamicParameters(new
            {
                StartRow = startRow,
                EndRow = endRow
            });
        }
        
        protected string GetIdentityCondition(DynamicParameters identity)
        {
            return string.Join(" AND ", TDbEntity.IdentityColumns
                .Select(ic => $"{TDbEntity.TableName}.{ic} = :{identity.Get<object>(ic)}"));
        }
    }
}
