using Supermarket.Domain.Common.Paging;

namespace Supermarket.Domain.Common
{
    /// <summary>
    /// Marker for query objects.
    /// Represents some arguments for ordering, filtering, paging the source of data
    /// </summary>
    public interface IQueryObject
    {
        RecordsRange RecordsRange { get; }
    }
}
