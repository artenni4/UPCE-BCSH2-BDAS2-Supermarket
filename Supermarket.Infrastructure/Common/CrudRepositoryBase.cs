using System.Data;
using System.Diagnostics;
using Dapper;
using Oracle.ManagedDataAccess.Client;
using Supermarket.Domain.Common;
using Supermarket.Domain.Common.Paging;

namespace Supermarket.Infrastructure.Common
{
    internal abstract class CrudRepositoryBase<TEntity, TId, TDbEntity>
        where TEntity : IEntity<TId>
        where TDbEntity : IDbEntity<TEntity, TId>
    {
        protected readonly OracleConnection _oracleConnection;
        
        /// <summary>
        /// Name of the SQL table
        /// </summary>
        protected abstract string TableName { get; }
        
        /// <summary>
        /// Primary keys in the table
        /// </summary>
        protected abstract IReadOnlyList<string> IdentityColumns { get; }
        
        /// <summary>
        /// Maps domain entity to db entity
        /// </summary>
        protected abstract TDbEntity MapToDbEntity(TEntity entity);
        
        /// <summary>
        /// Gets parameter list of identity values
        /// </summary>
        /// <param name="id">id of entity</param>
        protected abstract DynamicParameters GetIdentityValues(TId id);

        /// <summary>
        /// Helper method if <see cref="IdentityColumns"/> contains only one element
        /// </summary>
        protected DynamicParameters GetSimpleIdentityValue<TSimpleId>(TSimpleId id)
        {
            Debug.Assert(IdentityColumns.Count == 1, $"Identity of {TableName} is more than one column");
            
            var parameters = new DynamicParameters();
            parameters.Add(IdentityColumns[0], value: id);

            return parameters;
        }

        /// <summary>
        /// Gets identity parameters for returning inserted id into it
        /// </summary>
        /// <returns></returns>
        protected virtual DynamicParameters GetOutputIdentityParameters()
        {
            var parameters = new DynamicParameters();
            foreach (var identityColumn in IdentityColumns)
            {
                parameters.Add(identityColumn, direction: ParameterDirection.Output);
            }

            return parameters;
        }

        /// <summary>
        /// Gets insert values selector and parameters
        /// </summary>
        protected virtual DynamicParameters GetInsertingValues(TDbEntity entity) => new(entity);

        /// <summary>
        /// Extract <see cref="TId"/> from <see cref="DynamicParameters"/>
        /// </summary>
        protected abstract TId ExtractIdentity(DynamicParameters dynamicParameters);

        /// <summary>
        /// Helper method if <see cref="IdentityColumns"/> contains only one element
        /// </summary>
        protected TId ExtractSimpleIdentity(DynamicParameters dynamicParameters)
        {
            Debug.Assert(IdentityColumns.Count == 1, $"Identity of {TableName} is more than one column");
            
            return dynamicParameters.Get<TId>(IdentityColumns[0]);
        }
        
        protected CrudRepositoryBase(OracleConnection oracleConnection)
        {
            _oracleConnection = oracleConnection;
        }

        public async Task<PagedResult<TEntity>> GetPagedAsync(RecordsRange recordsRange)
        {
            var pagedItems = await GetPagedResult<TDbEntity>(recordsRange, $"{TableName}.*");
            return pagedItems.Select(MapToEntity);
        }

        protected async Task<PagedResult<TResult>> GetPagedResult<TResult>(
            RecordsRange recordsRange,
            string selectColumns,
            string? otherClauses = null,
            DynamicParameters? parameters = null)
        {
            var orderByIdentity = string.Join(", ", IdentityColumns);

            var pagingSql =
                $@"WITH NumberedResult AS 
                   (SELECT {selectColumns}, ROW_NUMBER() OVER (ORDER BY {orderByIdentity}) AS RowNumber 
                       FROM {TableName}
                       {otherClauses})
                   SELECT * FROM NumberedResult
                   WHERE RowNumber BETWEEN :StartRow AND :EndRow";

            var pagingParameters = GetPagingParameters(recordsRange);
            pagingParameters.AddDynamicParams(parameters);
            
            var pagedItems = await _oracleConnection.QueryAsync<TResult>(pagingSql, pagingParameters);
            
            var totalCountSql = $"SELECT COUNT(1) FROM {TableName} {otherClauses}";
            var totalCount = await _oracleConnection.ExecuteScalarAsync<int>(totalCountSql, pagingParameters);

            return new PagedResult<TResult>(pagedItems.ToArray(), recordsRange.PageNumber, totalCount);
        }

        public virtual async Task<TEntity?> GetByIdAsync(TId id)
        {
            var identity = GetIdentityValues(id);
            var identityCondition = GetIdentityCondition(identity);
            
            var sql = $"SELECT * FROM {TableName} WHERE {identityCondition}";
            var dbEntity = await _oracleConnection.QuerySingleAsync<TDbEntity>(sql, identity);

            return dbEntity.ToDomainEntity();
        }
        
        public virtual async Task<TId> AddAsync(TEntity entity)
        {
            var dbEntity = MapToDbEntity(entity);
            var identity = GetOutputIdentityParameters();
            var insertingValues = GetInsertingValues(dbEntity);

            var returningIdentity = string.Join(", ", IdentityColumns.Select(ic => $"{TableName}.{ic}"));
            var returningInto = string.Join(", ", identity.ParameterNames.Select(p => $":{p}"));
            
            var selector = string.Join(", ", insertingValues.ParameterNames);
            var parameters = string.Join(", ", insertingValues.ParameterNames.Select(v => ":" + v));
            
            var sql = $"INSERT INTO {TableName} ({selector}) VALUES {parameters} RETURNING {returningIdentity} INTO {returningInto}";

            insertingValues.AddDynamicParams(identity);
            await _oracleConnection.ExecuteAsync(sql, insertingValues);

            return ExtractIdentity(insertingValues);
        }

        public virtual async Task UpdateAsync(TEntity entity)
        {
            var dbEntity = MapToDbEntity(entity);
            var identity = GetIdentityValues(entity.Id);
            var insertingValues = GetInsertingValues(dbEntity);

            var updater = string.Join(", ", insertingValues.ParameterNames.Select(p => $"{p} = :{p}"));
            var identityCondition = GetIdentityCondition(identity);

            var sql = $"UPDATE {TableName} SET {updater} WHERE {identityCondition}";
            
            insertingValues.AddDynamicParams(identityCondition);
            await _oracleConnection.ExecuteAsync(sql, insertingValues);
        }

        public virtual async Task DeleteAsync(TId id)
        {
            var identity = GetIdentityValues(id);
            var identityCondition = GetIdentityCondition(identity);

            var sql = $"DELETE FROM {TableName} WHERE {identityCondition}";
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
            return string.Join(" AND ", IdentityColumns.Select(ic => $"{TableName}.{ic} = :{identity.Get<object>(ic)}"));
        } 
    }
}
