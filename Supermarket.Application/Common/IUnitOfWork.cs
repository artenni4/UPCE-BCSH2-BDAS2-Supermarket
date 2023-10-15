namespace Supermarket.Core.Common
{
    /// <summary>
    /// Provides methods for persisting a unit of work done using repositories
    /// </summary>
    public interface IUnitOfWork
    {
        /// <summary>
        /// Stars new transaction that can be committed or rolled back
        /// </summary>
        /// <returns></returns>
        Task<ITransaction> BeginTransactionAsync();
    }
}
