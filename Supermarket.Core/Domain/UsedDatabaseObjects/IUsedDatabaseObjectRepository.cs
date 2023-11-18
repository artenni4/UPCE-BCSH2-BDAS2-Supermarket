using Supermarket.Core.Domain.Common.Paging;

namespace Supermarket.Core.Domain.UsedDatabaseObjects;

public interface IUsedDatabaseObjectRepository
{
    Task<PagedResult<UsedDatabaseObject>> GetUsedDatabaseObjects(RecordsRange recordsRange);
}