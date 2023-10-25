using Supermarket.Domain.Common;
using Supermarket.Domain.PaymentTypes;

namespace Supermarket.Domain.Sales
{
    public class Sale : IEntity<int>
    {
        public required int Id { get; set; }
        public required DateTimeOffset Date { get; set; }
        public required int CashBoxId { get; set; }
        public required PaymentType PaymentType { get; set; }
    }
}
