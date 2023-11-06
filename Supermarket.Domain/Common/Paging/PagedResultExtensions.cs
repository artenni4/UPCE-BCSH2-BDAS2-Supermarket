namespace Supermarket.Domain.Common.Paging;

public static class PagedResultExtensions
{
    /// <summary>
    /// Maps all objects in paged result using provided <see cref="selector"/>
    /// and creates new instance of <see cref="PagedResult{TItem}"/>
    /// </summary>
    /// <param name="source">source paging</param>
    /// <param name="selector">mapping function</param>
    public static PagedResult<TResult> Select<TResult, TSource>(this PagedResult<TSource> source, Func<TSource, TResult> selector)
    {
        var items = source.Items.Select(selector).ToArray();
        return new PagedResult<TResult>(items, source.PageNumber, source.PageSize, source.TotalItems);
    }
}