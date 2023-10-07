namespace Supermarket.Core.Common
{
    public interface ITransaction : IAsyncDisposable
    {
        /// <summary>
        /// Commits changes made in transaction
        /// </summary>
        /// <returns></returns>
        Task CommitAsync();
    }
}
