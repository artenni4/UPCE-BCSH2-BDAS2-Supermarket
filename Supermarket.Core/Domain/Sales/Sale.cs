using Supermarket.Core.Domain.Common;
using Supermarket.Core.Domain.PaymentTypes;

namespace Supermarket.Core.Domain.Sales
{
    public class Sale : IEntity<int>
    {
        public required int Id { get; set; }
        public required DateTimeOffset Date { get; set; }
        public required int CashBoxId { get; set; }
        public required PaymentType PaymentType { get; set; }
    }
}
