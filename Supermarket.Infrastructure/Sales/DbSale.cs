using Dapper;
using Supermarket.Domain.Common;
using Supermarket.Domain.PaymentTypes;
using Supermarket.Domain.Sales;

namespace Supermarket.Infrastructure.Sales;

internal class DbSale : IDbEntity<Sale, int, DbSale>
{
    public required int prodej_id { get; init; }
    public required DateTimeOffset datum { get; init; }
    public required int pokladna_id { get; init; }
    public required int typ_placeni_id { get; init; }
    
    
    public static string TableName => "PRODEJE";
    
    public static IReadOnlyList<string> IdentityColumns { get; } = new[]
    {
        nameof(prodej_id)
    };

    public Sale ToDomainEntity() => new()
    {
        Id = prodej_id,
        Date = datum,
        CashBoxId = pokladna_id,
        PaymentType = typ_placeni_id switch
        {
            1 => PaymentType.Card,
            2 => PaymentType.Cash,
            3 => PaymentType.Coupon,
            _ => throw new DatabaseException($"Payment type [{typ_placeni_id}] is not known")
        },
    };

    public static DynamicParameters GetEntityIdParameters(int id) =>
        new DynamicParameters().AddParameter(nameof(prodej_id), id);

    public static DynamicParameters GetOutputIdentityParameters() =>
        new DynamicParameters().AddOutputParameter(nameof(prodej_id));

    public static int ExtractIdentity(DynamicParameters dynamicParameters) =>
        dynamicParameters.Get<int>(nameof(prodej_id));

    public static DbSale MapToDbEntity(Sale entity) => new()
    {
        prodej_id = entity.Id,
        datum = entity.Date,
        pokladna_id = entity.CashBoxId,
        typ_placeni_id = GetPaymentTypeId(entity.PaymentType)
    };

    private static int GetPaymentTypeId(PaymentType paymentType)
    {
        if (paymentType == PaymentType.Card) return 1;
        if (paymentType == PaymentType.Cash) return 2;
        if (paymentType == PaymentType.Coupon) return 3;

        throw new DatabaseException($"Mapping for payment type [{paymentType}] is not implemented");
    }
}