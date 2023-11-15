using Dapper;
using Supermarket.Core.Domain.Common;
using Supermarket.Core.Domain.Payments;

namespace Supermarket.Infrastructure.Payments;

internal class DbPayment : IDbEntity<Payment, PaymentId, DbPayment>
{
    public required int prodej_id { get; init; }
    public required int typ_placeni_id { get; init; }
    public required decimal castka { get; init; }
    
    public static string TableName => "PLATBA";

    public static IReadOnlySet<string> IdentityColumns { get; } = new HashSet<string>
    {
        nameof(prodej_id),
        nameof(typ_placeni_id),
    };

    public Payment ToDomainEntity() => new Payment
    {
        Id = new PaymentId(prodej_id, ToPaymentType(typ_placeni_id)),
        Amount = castka
    };

    private static PaymentType ToPaymentType(int paymentTypeId) => paymentTypeId switch
    {
        1 => PaymentType.Hotovost,
        2 => PaymentType.Karta,
        3 => PaymentType.Kupon,
        _ => throw new RepositoryInconsistencyException("Payment type not supported")
    };

    private static int ToPaymentTypeId(PaymentType paymentType) => paymentType switch
    {
        PaymentType.Hotovost => 1,
        PaymentType.Karta => 2,
        PaymentType.Kupon => 3,
        _ => throw new ArgumentException(nameof(paymentType))
    };

    public static DbPayment ToDbEntity(Payment entity) => new DbPayment
    {
        prodej_id = entity.Id.SaleId,
        typ_placeni_id = ToPaymentTypeId(entity.Id.PaymentType),
        castka = entity.Amount
    };

    public static DynamicParameters GetEntityIdParameters(PaymentId id) => new DynamicParameters()
        .AddParameter(nameof(prodej_id), id.SaleId)
        .AddParameter(nameof(typ_placeni_id), ToPaymentTypeId(id.PaymentType));

    public DynamicParameters GetInsertingValues() => this.GetAllProperties();
}