using Oracle.ManagedDataAccess.Client;
using Supermarket.Core.Domain.Payments;

namespace Supermarket.Infrastructure.Payments;

internal class PaymentRepository : CrudRepositoryBase<Payment, PaymentId, DbPayment>, IPaymentRepository
{
    public PaymentRepository(OracleConnection oracleConnection) : base(oracleConnection)
    {
    }
}