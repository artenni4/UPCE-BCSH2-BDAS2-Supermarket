using Dapper;
using Oracle.ManagedDataAccess.Client;
using Supermarket.Domain.Common;
using Supermarket.Domain.Common.Paging;
using Supermarket.Domain.PaymentTypes;
using Supermarket.Domain.Sales;
using Supermarket.Infrastructure.Common;

namespace Supermarket.Infrastructure.Sales
{
    internal class SaleRepository : CrudRepositoryBase<Sale, int, SaleRepository.DbSale, PagingQueryObject>, ISaleRepository
    {
        public SaleRepository(OracleConnection oracleConnection) : base(oracleConnection)
        {
        }
        
        public class DbSale
        {
            public required int prodej_id { get; init; }
            public required DateTimeOffset datum { get; init; }
            public required int pokladna_id { get; init; }
            public required int typ_placeni_id { get; init; }
        }

        protected override string TableName => "PRODEJE";
        protected override IReadOnlyList<string> IdentityColumns { get; } = new[] { nameof(DbSale.prodej_id) };
        protected override Sale MapToEntity(DbSale dbEntity) => new()
        {
            Id = dbEntity.prodej_id,
            Date = dbEntity.datum,
            CashBoxId = dbEntity.pokladna_id,
            PaymentType = dbEntity.typ_placeni_id switch
            {
                1 => PaymentType.Card,
                2 => PaymentType.Cash,
                3 => PaymentType.Coupon,
                _ => throw new DatabaseException($"Payment type [{dbEntity.typ_placeni_id}] is not known")
            },
        };

        protected override DbSale MapToDbEntity(Sale entity) => new()
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

        protected override DynamicParameters GetIdentityValues(int id) => GetSimpleIdentityValue(id);

        protected override int ExtractIdentity(DynamicParameters dynamicParameters) => ExtractSimpleIdentity(dynamicParameters);
    }
}
