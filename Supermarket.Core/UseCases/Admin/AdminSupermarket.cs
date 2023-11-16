using Supermarket.Core.Domain.Common;

namespace Supermarket.Core.UseCases.Admin
{
    public class AdminSupermarket : IEntity<int>
    {
        public required int Id { get; set; }
        public required string Address { get; set; }
        public required int RegionId { get; set; }
        public required string RegionName { get; set;}
    }
}
