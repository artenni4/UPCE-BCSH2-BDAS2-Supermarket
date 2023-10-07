using Oracle.ManagedDataAccess.Client;
using Supermarket.Core.Common;

namespace Supermarket.Infrastructure.Common
{
    internal sealed class Transaction : ITransaction
    {
        private readonly OracleTransaction _transaction;
        private bool _commited = false;

        public Transaction(OracleTransaction transaction)
        {
            _transaction = transaction;
        }

        public async Task CommitAsync()
        {
            await _transaction.CommitAsync();
            _commited = true;
        }

        public async ValueTask DisposeAsync()
        {
            if (_commited is false)
            {
                await _transaction.RollbackAsync();
            }
            await _transaction.DisposeAsync();
        }
    }
}
