using Supermarket.Core.Domain.Common;

namespace Supermarket.Core.Domain.Payments;

public interface IPaymentRepository : ICrudRepository<Payment, PaymentId>
{
    
}