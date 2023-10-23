using Supermarket.Domain.Common;

namespace Supermarket.Domain.Sales
{
    public class Sale : IEntity<int>
    {
        public required int Id { get; set; }
        public required DateTime Date { get; set; }
        public required int CashBoxId { get; set; }
        public required int PaymentTypeId { get; set; }
    }
}
