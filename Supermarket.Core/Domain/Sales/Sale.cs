using Supermarket.Core.Domain.Common;

namespace Supermarket.Core.Domain.Sales
{
    public class Sale : IEntity<int>
    {
        public required int Id { get; set; }
        public required DateTimeOffset Date { get; set; }
        public required int CashBoxId { get; set; }
    }
}
