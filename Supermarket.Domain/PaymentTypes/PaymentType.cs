using Supermarket.Domain.Common;

namespace Supermarket.Domain.PaymentTypes
{
    public class PaymentType : IEntity<int>
    {
        public required int Id { get; set; }
        public required string Name { get; set; }
    }
}
