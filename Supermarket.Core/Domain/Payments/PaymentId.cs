namespace Supermarket.Core.Domain.Payments;

public record struct PaymentId(int SaleId, PaymentType PaymentType);