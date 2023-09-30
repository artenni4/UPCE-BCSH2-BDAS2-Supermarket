using Supermarket.Core.Common.Paging;

namespace Supermarket.Core.Common
{
    internal abstract class CrudServiceBase<TEntity, TId, TQueryObject>
        where TEntity : IEntity<TId>
        where TQueryObject : IQueryObject
    {
        protected readonly ICrudRepository<TEntity, TId, TQueryObject> _crudRepository;
        protected readonly IUnitOfWork _unitOfWork;

        public CrudServiceBase(ICrudRepository<TEntity, TId, TQueryObject> crudRepository, IUnitOfWork unitOfWork)
        {
            _crudRepository = crudRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<PagedResult<TEntity>> GetPagedAsync(TQueryObject queryObject)
        {
            return await _crudRepository.GetPagedAsync(queryObject);
        }

        public async Task<TEntity?> GetByIdAsync(TId id)
        {
            return await _crudRepository.GetByIdAsync(id);
        }

        public async Task<TId> AddAsync(TEntity entity)
        {
            var id = await _crudRepository.AddAsync(entity);
            await _unitOfWork.SaveChangesAsync();
            return id;
        }

        public async Task UpdateAsync(TEntity entity)
        {
            await _crudRepository.UpdateAsync(entity);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task DeleteAsync(TId id)
        {
            await _crudRepository.DeleteAsync(id);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
