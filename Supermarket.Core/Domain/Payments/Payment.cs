using Supermarket.Core.Domain.Common;

namespace Supermarket.Core.Domain.Payments;

public class Payment : IEntity<PaymentId>
{
    public required PaymentId Id { get; init; }
    public required decimal Amount { get; init; }
}