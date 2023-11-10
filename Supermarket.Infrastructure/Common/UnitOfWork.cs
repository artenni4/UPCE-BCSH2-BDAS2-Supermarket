using Oracle.ManagedDataAccess.Client;
using Supermarket.Core.UseCases.Common;

namespace Supermarket.Infrastructure.Common
{
    public sealed class UnitOfWork : IUnitOfWork
    {
        private readonly OracleConnection _dbConnection;

        public UnitOfWork(OracleConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }

        public Task<ITransaction> BeginTransactionAsync()
        {
            var dbTransaction = _dbConnection.BeginTransaction();
            return Task.FromResult(new Transaction(dbTransaction) as ITransaction);
        }
    }
}
