using Supermarket.Core.Domain.Common;
using Supermarket.Core.Domain.Common.Paging;
using Supermarket.Core.UseCases.ManagerMenu;

namespace Supermarket.Core.Domain.SharedFiles
{
    public interface ISharedFileRepository : ICrudRepository<SharedFile, int>
    {
        Task<PagedResult<ManagerMenuSharedFile>> GetAllAsync(int supermarketId, RecordsRange recordsRange, string? search);
        Task SaveSharedFile(SharedFile file, byte[] data);
        Task<byte[]?> DownloadSharedFile(int fileId);
    }
}
