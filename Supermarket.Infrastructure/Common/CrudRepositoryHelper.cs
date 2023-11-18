using Dapper;
using Supermarket.Core.Domain.Common.Paging;

namespace Supermarket.Infrastructure.Common;

public static class CrudRepositoryHelper
{
    public static DynamicParameters GetPagingParameters(this RecordsRange recordsRange)
    {
        var startRow = (recordsRange.PageNumber - 1) * recordsRange.PageSize;
        var rowsCount = recordsRange.PageSize;

        return new DynamicParameters(new
        {
            PagingOffset = startRow,
            PagingRowsCount = rowsCount
        });
    }
}