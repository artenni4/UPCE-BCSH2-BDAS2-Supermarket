namespace Supermarket.Core.Common
{
    /// <summary>
    /// Provides methods for persisting a unit of work done using repositories
    /// </summary>
    public interface IUnitOfWork
    {
        /// <summary>
        /// Persists changes to database, commits transaction in case of SQL database
        /// </summary>
        /// <returns></returns>
        Task SaveChangesAsync();
    }
}
