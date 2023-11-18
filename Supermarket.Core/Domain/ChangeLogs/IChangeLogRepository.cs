using Supermarket.Core.Domain.Common.Paging;

namespace Supermarket.Core.Domain.ChangeLogs;

public interface IChangeLogRepository
{
    Task<PagedResult<ChangeLog>> GetChangeLogs(RecordsRange recordsRange);
}