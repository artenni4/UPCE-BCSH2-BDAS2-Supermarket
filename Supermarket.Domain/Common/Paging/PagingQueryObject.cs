namespace Supermarket.Domain.Common.Paging
{   
    /// <summary>
    /// Query object that specifies only records range
    /// </summary>
    public class PagingQueryObject : IQueryObject
    {
        public required RecordsRange RecordsRange { get; init; }
    }
}
