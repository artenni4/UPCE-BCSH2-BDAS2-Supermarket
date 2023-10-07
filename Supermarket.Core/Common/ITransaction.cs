using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
