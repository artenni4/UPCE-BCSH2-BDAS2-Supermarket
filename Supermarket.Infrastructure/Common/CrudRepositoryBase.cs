using System.Data;
using System.Diagnostics;
using System.Text;
using Dapper;
using Oracle.ManagedDataAccess.Client;
using Supermarket.Domain.Common;
using Supermarket.Domain.Common.Paging;

namespace Supermarket.Infrastructure.Common
{
    internal abstract class CrudRepositoryBase<TEntity, TId, TDbEntity, TQueryObject>
        where TEntity : IEntity<TId>
        where TQueryObject : IQueryObject
    {
        protected readonly OracleConnection _oracleConnection;
        
        /// <summary>
        /// Name of the SQL table
        /// </summary>
        protected abstract string TableName { get; }
        
        protected abstract IReadOnlyList<string> IdentityColumns { get; }

        /// <summary>
        /// Maps instance of db entity to domain entity
        /// </summary>
        protected abstract TEntity MapToEntity(TDbEntity dbEntity);

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

        /// <summary>
        /// Gets sql where condition and parameters that is specific to current <see cref="TQueryObject"/>
        /// </summary>
        /// <param name="queryObject">query object</param>
        /// <param name="whereConditionBuilder">sql where condition builder</param>
        /// <returns>output parameters for query</returns>
        protected virtual DynamicParameters GetCustomPagingCondition(TQueryObject queryObject, StringBuilder whereConditionBuilder)
        {
            return new DynamicParameters();
        }
        
        protected CrudRepositoryBase(OracleConnection oracleConnection)
        {
            _oracleConnection = oracleConnection;
        }

        public async Task<PagedResult<TEntity>> GetPagedAsync(TQueryObject queryObject)
        {
            var orderByIdentity = string.Join(", ", IdentityColumns);

            var whereConditionBuilder = new StringBuilder();
            var whereParameters = GetCustomPagingCondition(queryObject, whereConditionBuilder);
            var whereCondition = whereConditionBuilder.ToString();
            
            var customWhere = string.IsNullOrEmpty(whereCondition) ? null : $"WHERE {whereCondition}";

            var pagingSql =
                $@"WITH NumberedResult AS 
                   (SELECT t.*, ROW_NUMBER() OVER (ORDER BY {orderByIdentity}) AS RowNumber 
                       FROM {TableName} t
                       {customWhere})
                   SELECT * FROM NumberedResult
                   WHERE RowNumber BETWEEN :StartRow AND :EndRow";

            var pagingParameters = GetPagingParameters(queryObject.RecordsRange);
            pagingParameters.AddDynamicParams(whereParameters);

            var pagedItems = await _oracleConnection.QueryAsync<TDbEntity>(pagingSql, pagingParameters);
            var entities = pagedItems.Select(MapToEntity).ToArray();
            
            var totalCountSql = $"SELECT COUNT(*) FROM {TableName} {customWhere}";
            var totalCount = await _oracleConnection.ExecuteScalarAsync<int>(totalCountSql, whereParameters);

            return new PagedResult<TEntity>(entities, queryObject.RecordsRange.PageNumber, totalCount);
        }

        public virtual async Task<TEntity?> GetByIdAsync(TId id)
        {
            var identity = GetIdentityValues(id);
            var identityCondition = GetIdentityCondition(identity);
            
            var sql = $"SELECT * FROM {TableName} WHERE {identityCondition}";
            var dbEntity = await _oracleConnection.QuerySingleAsync<TDbEntity>(sql, identity);

            return MapToEntity(dbEntity);
        }
        
        public virtual async Task<TId> AddAsync(TEntity entity)
        {
            var dbEntity = MapToDbEntity(entity);
            var identity = GetOutputIdentityParameters();
            var insertingValues = GetInsertingValues(dbEntity);

            var returningIdentity = string.Join(", ", identity.ParameterNames);
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
        
        protected static string GetIdentityCondition(DynamicParameters identity)
        {
            return string.Join(" AND ", identity.ParameterNames.Select(p => $"{p} = :{p}"));
        } 
    }
}
