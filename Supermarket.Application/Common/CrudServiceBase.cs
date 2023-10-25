namespace Supermarket.Core.Common
{
    internal abstract class CrudServiceBase<TEntity, TId, TQueryObject>
        where TEntity : IEntity<TId>
        where TQueryObject : IQueryObject
    {
        protected readonly ICrudRepository<TEntity, TId, TQueryObject> _crudRepository;
        protected readonly IUnitOfWork _unitOfWork;

        protected CrudServiceBase(ICrudRepository<TEntity, TId, TQueryObject> crudRepository, IUnitOfWork unitOfWork)
        {
            _crudRepository = crudRepository;
            _unitOfWork = unitOfWork;
        }

        public virtual async Task<PagedResult<TEntity>> GetPagedAsync(TQueryObject queryObject)
        {
            return await _crudRepository.GetPagedAsync(queryObject);
        }

        public virtual async Task<TEntity?> GetByIdAsync(TId id)
        {
            return await _crudRepository.GetByIdAsync(id);
        }

        public virtual async Task<TId> AddAsync(TEntity entity)
        {
            await using var transaction = await _unitOfWork.BeginTransactionAsync();
            var id = await _crudRepository.AddAsync(entity);
            await transaction.CommitAsync();
            return id;
        }

        public virtual async Task UpdateAsync(TEntity entity)
        {
            await using var transaction = await _unitOfWork.BeginTransactionAsync();
            await _crudRepository.UpdateAsync(entity);
            await transaction.CommitAsync();
        }

        public virtual async Task DeleteAsync(TId id)
        {
            await using var transaction = await _unitOfWork.BeginTransactionAsync();
            await _crudRepository.DeleteAsync(id);
            await transaction.CommitAsync();
        }
    }
}
