namespace Supermarket.Core.UseCases.Common
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
